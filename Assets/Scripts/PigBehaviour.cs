using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class PigBehaviour : MonoBehaviour {
	
	public float runSpeed;
	public float airSpeed;
	public float jumpSpeed;
	public float stunTime;
	
	public RocketBehaviour rocketPrefab;
	
	public FeetCollider feetCollider;
	
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
	
	// Update is called once per frame
	void Update () {
		
		if (!isDead && stunTimer <= 0f) {
			Vector3 aimDir = (Input.mousePosition - screenCenter).normalized;
			Debug.DrawLine(transform.position, transform.position + aimDir * 4);
			
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
					float angle = Mathf.Rad2Deg * Mathf.Atan2(-aimDir.y, aimDir.x);
					FireRocket(angle, Input.GetButtonDown("Fire2"));
				}
				
			} else {
				// jump
				if (Input.GetButtonDown("Jump")) {
					AbandonRocket();
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + jumpSpeed, 0f);
				}
			}
		} else if (stunTimer > 0f) {
			stunTimer -= Time.deltaTime;
			
			if (stunTimer <= 0f) {
				Recover();
			}
		}
	}
	
	private void FireRocket(float angle, bool ride) {
		rocket = (RocketBehaviour) Instantiate(rocketPrefab, transform.position, Quaternion.Euler(new Vector3(angle, 90f, 0f)));
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
		
		rigidbody.AddTorque(Random.rotation.eulerAngles, ForceMode.VelocityChange);
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
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
			
			transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		}
	}
}
