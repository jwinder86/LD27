using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class PigBehaviour : MonoBehaviour {
	
	private static Plane plane = new Plane(new Vector3(0f, 0f, 1f), Vector3.zero);
	private static Vector3 shoulderPos = new Vector3(0f, 1.5f, 0f);
	
	public float runSpeed;
	public float airSpeed;
	public float jumpSpeed;
	public float stunTime;
	
	public RocketBehaviour rocketPrefab;
	
	public FeetCollider feetCollider;
	
	public Transform crosshair;
	
	private bool ridingRocket;
	private bool onGround;
	private bool isDead;
	private float stunTimer;
	
	private RigidbodyConstraints initialConstraints;
	
	private RocketBehaviour rocket;
	private Vector3 screenCenter;
	
	private Animation animation;
	
	// Use this for initialization
	void Start () {
		screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		ridingRocket = false;
		isDead = false;
		stunTimer = 0f;
		
		initialConstraints = rigidbody.constraints;
		
		animation = GetComponentInChildren<Animation>();
		
		rocket = null;
	}
	
	void LateUpdate() {
		Vector3 mouseTarget = GetMouseTarget();
			if (mouseTarget != Vector3.zero) {
				crosshair.position = mouseTarget;
			} else {
				crosshair.position = transform.position + shoulderPos + new Vector3(5f, 0f, 0f);
			}
	}
	
	// Update is called once per frame
	void Update () {
		
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
					} else if (Input.GetAxis("Horizontal") > 0f) {
						rigidbody.velocity = new Vector3(runSpeed, rigidbody.velocity.y, 0f);
						animation.Play("RunAnimation", PlayMode.StopAll);
					} else {
						animation.Stop();
					}
					
				} else {
					if (Input.GetAxis("Horizontal") < 0f) {
						rigidbody.AddForce(new Vector3(-airSpeed, 0f, 0f), ForceMode.Acceleration);
						animation.Play("StunAnimation", PlayMode.StopAll);
					} else if (Input.GetAxis("Horizontal") > 0f) {
						rigidbody.AddForce(new Vector3(airSpeed * 2f, 0f, 0f), ForceMode.Acceleration);
						animation.Play("StunAnimation", PlayMode.StopAll);
					} else {
						animation.Play("StunAnimation", PlayMode.StopAll);	
					}
					
				}
				
				// jump
				if (Input.GetButtonDown("Jump") && feetCollider.OnGround()) {
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0f);
				}
				
				// fire rocket
				if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
					FireRocket(aimDir, Input.GetButtonDown("Fire2"));
				}
				
			} else {
				// jump
				if (Input.GetButtonDown("Jump")) {
					AbandonRocket();
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
				}
			}
		} else if (stunTimer > 0f) {
			stunTimer -= Time.deltaTime;
			
			if (stunTimer <= 0f) {
				Recover();
			}
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
			transform.localPosition = new Vector3(0f, 0.6f, -2f);
			transform.localRotation = Quaternion.Euler(new Vector3(80f, 0f, 0f));
			
			animation.Play("StunAnimation", PlayMode.StopAll);
		}
	}
	
	public void AbandonRocket() {
		ridingRocket = false;
		transform.parent = null;
		rigidbody.isKinematic = false;
		rigidbody.velocity = rocket.rigidbody.velocity;
		
		if (!isDead && stunTimer <= 0f) {
			transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		}
		
		rocket.SetControlRocket(false);
	}
	
	public void Stun() {
		stunTimer = stunTime;
		
		if (ridingRocket) {
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			AbandonRocket();
		} else {
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
		}
		
		animation.Play("StunAnimation", PlayMode.StopAll);
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		rigidbody.AddTorque(Random.rotation.eulerAngles, ForceMode.VelocityChange);
	}
	
	private void Recover() {
		stunTimer = 0f;
		
		if (!isDead) {
			rigidbody.constraints = initialConstraints;
			transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		}
	}
	
	public void Die() {
		if (!isDead) {
			isDead = true;
			
			if (ridingRocket) {
				transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
				AbandonRocket();
			} else {
				transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			}
			
			animation.Play("StunAnimation", PlayMode.StopAll);
			transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			rigidbody.AddTorque(Random.rotation.eulerAngles, ForceMode.VelocityChange);
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
}
