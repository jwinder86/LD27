using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(AudioSource))]
public class PigBehaviour : MonoBehaviour {
	
	private static Plane plane = new Plane(new Vector3(0f, 0f, 1f), Vector3.zero);
	private static Vector3 shoulderPos = new Vector3(0f, 1.5f, 0f);
	private static float rotateTime = 0.4f;
	
	private static FollowTransform screen;
	
	public float runSpeed;
	public float airSpeed;
	public float jumpSpeed;
	public float stunTime;
	
	public RocketBehaviour rocketPrefab;
	
	public FeetCollider feetCollider;
	
	public Transform launcher;
	
	public Transform crosshair;
	
	public AudioClip deathSound;
	public AudioClip hitSound;
	
	public float rocketMax = 5;
	public float rocketRechargePerSecond = 0.5f;
	
	private bool ridingRocket;
	private bool onGround;
	private bool isDead;
	private float stunTimer;
	private float rocketCount;
	
	private RigidbodyConstraints initialConstraints;
	
	private RocketBehaviour rocket;
	private Vector3 screenCenter;
	
	private Animation animation;
	
	private BoredomClock boredomClock;
	private TimerBarBehaviour timerBar;
	
	private bool facingRight;
	
	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		
		screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		ridingRocket = false;
		isDead = false;
		stunTimer = 0f;
		rocketCount = rocketMax;
		
		initialConstraints = rigidbody.constraints;
		
		animation = GetComponentInChildren<Animation>();
		
		rocket = null;
		
		boredomClock = (BoredomClock) FindObjectOfType(typeof(BoredomClock));
		timerBar = (TimerBarBehaviour) FindObjectOfType(typeof(TimerBarBehaviour));
		
		facingRight = true;
	}
	
	void LateUpdate() {
		float aimAngle = 0f;
		Vector3 mouseTarget = GetMouseTarget();
		if (mouseTarget != Vector3.zero) {
			crosshair.position = mouseTarget;
			Vector3 aimDir = (mouseTarget - (transform.position + shoulderPos)).normalized;
			
			if (facingRight) {
				aimAngle = Mathf.Rad2Deg * Mathf.Atan2(aimDir.y, aimDir.x);
			} else {
				aimAngle = Mathf.Rad2Deg * Mathf.Atan2(aimDir.y, -aimDir.x);
			}
		} else {
			crosshair.position = transform.position + shoulderPos + new Vector3(5f, 0f, 0f);
		}
		
		if (ridingRocket || isDead || stunTimer > 0f) {
			launcher.renderer.enabled = false;
		} else {
			launcher.renderer.enabled = true;
			launcher.transform.localEulerAngles = new Vector3(0f, 90f, aimAngle);
		}
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.WakeUp();
		
		if (!isDead && stunTimer <= 0f) {
			Vector3 aimDir;
			Vector3 mouseTarget = GetMouseTarget();
			if (mouseTarget != Vector3.zero) {
				aimDir = (mouseTarget - (transform.position + shoulderPos)).normalized;
			} else {
				aimDir = new Vector3(1f, 0f, 0f);
			}
			
			if (!ridingRocket) {
				// move left and right
				if (feetCollider.OnGround()) {
					if (Input.GetAxis("Horizontal") < 0f) {
						rigidbody.velocity = new Vector3(-runSpeed, rigidbody.velocity.y, 0f);
						animation.Play("RunAnimation", PlayMode.StopAll);
						
						if (facingRight) {
							Rotate(false);
						}
						
					} else if (Input.GetAxis("Horizontal") > 0f) {
						rigidbody.velocity = new Vector3(runSpeed, rigidbody.velocity.y, 0f);
						animation.Play("RunAnimation", PlayMode.StopAll);
						
						if (!facingRight) {
							Rotate(true);
						}
						
					} else {
						animation.Play("StandAnimation", PlayMode.StopAll);
					}
					
				} else {
					if (Input.GetAxis("Horizontal") < 0f) {
						rigidbody.AddForce(new Vector3(-airSpeed, 0f, 0f), ForceMode.Acceleration);
						animation.Play("StandAnimation", PlayMode.StopAll);
						
						if (facingRight) {
							Rotate(false);
						}
						
					} else if (Input.GetAxis("Horizontal") > 0f) {
						rigidbody.AddForce(new Vector3(airSpeed * 2f, 0f, 0f), ForceMode.Acceleration);
						animation.Play("StandAnimation", PlayMode.StopAll);
						
						if (!facingRight) {
							Rotate(true);
						}
						
					} else {
						animation.Play("StandAnimation", PlayMode.StopAll);	
					}
					
				}
				
				// jump
				if (Input.GetButtonDown("Jump") && feetCollider.OnGround()) {
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0f);
				}
				
				// fire rocket
				if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && rocketCount >= 1f) {
					Debug.Log("RocketCount: " + rocketCount);
					rocketCount -= 1f;
					FireRocket(aimDir, Input.GetButtonDown("Fire2"));
				}
				
			} else {
				// jump
				if (Input.GetButtonDown("Jump")) {
					AbandonRocket();
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
				}
				
				if (rocket != null) {
					//Debug.Log(rocket.transform.forward);
					if (rocket.transform.forward.x > 0f && !facingRight) {
						Debug.Log("Rotating!");
						MountRocket(transform.localPosition, transform.localRotation, true);
					} else if (rocket.transform.forward.x < 0f && facingRight) {
						Debug.Log("Rotating!");
						MountRocket(transform.localPosition, transform.localRotation, false);
					}
				} else {
					AbandonRocket();
				}
			}
		} else if (stunTimer > 0f) {
			stunTimer -= Time.deltaTime;
			
			if (stunTimer <= 0f) {
				Recover();
			}
		}
		
		if (!isDead) {
			rocketCount = Mathf.Min(rocketCount + rocketRechargePerSecond * Time.deltaTime, rocketMax);
			timerBar.setRocketCount((int)Mathf.Floor(rocketCount));
		}
	}
	
	private void FireRocket(Vector3 aimDir, bool ride) {
		float angle = Mathf.Rad2Deg * Mathf.Atan2(-aimDir.y, aimDir.x);
		rocket = (RocketBehaviour) Instantiate(rocketPrefab, transform.position + shoulderPos + aimDir * 3, Quaternion.Euler(new Vector3(angle, 90f, 0f)));
		rocket.rigidbody.velocity = rigidbody.velocity;
		
		// ride rocket
		if (ride) {
			rocket.SetControlRocket(true);
			ridingRocket = true;
			transform.parent = rocket.transform;
			rigidbody.isKinematic = true;
			
			animation.Play("StandAnimation", PlayMode.StopAll);
			
			MountRocket(transform.localPosition, transform.localRotation, facingRight);
		}
	}
	
	public void AbandonRocket() {
		if (ridingRocket) {
			StopAllCoroutines();
			
			ridingRocket = false;
			transform.parent = null;
			rigidbody.isKinematic = false;
			rigidbody.velocity = rocket.rigidbody.velocity;
			
			if (!isDead && stunTimer <= 0f) {
				StartCoroutine(RecoverCoroutine(transform.localRotation));
			}
			
			rocket.SetControlRocket(false);
			rocket = null;
		}
	}
	
	public void Stun() {
		stunTimer = stunTime;
		
		Debug.Log("Hit Sound");
		audio.PlayOneShot(hitSound);
		
		if (ridingRocket) {
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			AbandonRocket();
		} else {
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
		}
		
		animation.Play("StandAnimation", PlayMode.StopAll);
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		rigidbody.AddTorque(Random.rotation.eulerAngles, ForceMode.VelocityChange);
		
		getScreen().HeavyShakeTime(stunTime);
	}
	
	private void Recover() {
		stunTimer = 0f;
		
		if (!isDead) {
			rigidbody.constraints = initialConstraints;
			transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
			
			StopAllCoroutines();
			StartCoroutine(RecoverCoroutine(transform.localRotation));
		}
	}
	
	public void Die() {
		if (!isDead && boredomClock.isGameRunning()) {
			isDead = true;
			audio.PlayOneShot(deathSound);
			if (ridingRocket) {
				transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
				AbandonRocket();
			} else {
				transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			}
			
			animation.Play("StandAnimation", PlayMode.StopAll);
			transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			rigidbody.AddTorque(Random.rotation.eulerAngles, ForceMode.VelocityChange);
			
			boredomClock.GameOver();
			
			getScreen().HeavyShakeTime(0.3f);
		}
	}
	
	public Vector3 GetMouseTarget() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		float dist = 0f;
		if (plane.Raycast(ray, out dist)) {
			return ray.GetPoint(dist);
		} else {
			return Vector3.zero;
		}
	}
	
	private void Rotate(bool faceRight) {
		StopAllCoroutines();
		StartCoroutine(RotateCoroutine(faceRight));
	}
	
	private IEnumerator RotateCoroutine(bool faceRight) {
		facingRight = faceRight;
		
		for (float t = 0; t <= rotateTime; t += Time.deltaTime) {
			if (faceRight) {			
				transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 270f, 0f)), Quaternion.Euler(new Vector3(0f, 90f, 0f)), t / rotateTime);
			} else {
				transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 90f, 0f)), Quaternion.Euler(new Vector3(0f, 270f, 0f)), t / rotateTime);
			}
			
			yield return null;
		}
		
		if (faceRight) {			
			transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		} else {
			transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
		}
	}
	
	private void MountRocket(Vector3 originalPosition, Quaternion originalRotation, bool faceRight) {
		StopAllCoroutines();
		StartCoroutine(MountRocketCoroutine(originalPosition, originalRotation, faceRight));
	}
	
	private IEnumerator MountRocketCoroutine(Vector3 originalPosition, Quaternion originalRotation, bool faceRight) {
		facingRight = faceRight;
		
		for (float t = 0; t <= rotateTime; t += Time.deltaTime) {
			
			if (faceRight) {
				transform.localPosition = Vector3.Lerp(originalPosition, new Vector3(0f, 0.4f, -2f), t / rotateTime); 
				transform.localRotation = Quaternion.Slerp(originalRotation, Quaternion.Euler(new Vector3(90f, 0f, 0f)), t / rotateTime);
			} else {
				transform.localPosition = Vector3.Lerp(originalPosition, new Vector3(0f, -0.4f, -2f), t / rotateTime); 
				transform.localRotation = Quaternion.Slerp(originalRotation, Quaternion.Euler(new Vector3(270f, 0f, 180f)), t / rotateTime);
			}
			
			yield return null;
		}
		
		if (faceRight) {
				transform.localPosition = new Vector3(0f, 0.4f, -2f); 
				transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			} else {
				transform.localPosition = new Vector3(0f, -0.4f, -2f); 
				transform.localRotation = Quaternion.Euler(new Vector3(270f, 0f, 180f));
			}
	}
	
	private IEnumerator RecoverCoroutine(Quaternion originalRotation) {
		for (float t = 0; t <= rotateTime; t += Time.deltaTime) {
			
			transform.localRotation = Quaternion.Slerp(originalRotation, Quaternion.Euler(new Vector3(0f, 90f, 0f)), t / rotateTime);
			
			yield return null;
		}
		
		transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
	}
	
	private FollowTransform getScreen() {
		if (screen == null) {
			screen = (FollowTransform) FindObjectOfType(typeof(FollowTransform));
		}
		
		return screen;
	}
}
