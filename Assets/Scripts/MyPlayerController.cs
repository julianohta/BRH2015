using UnityEngine;
using System.Collections;

public class MyPlayerController : MonoBehaviour {

	public float movementSpeed = 1.0f;
	public float mouseSens = 1.0f;
	public float maxVerticalRotation = 70f;
	public float gravity;
	public float jumpSpeed;
	public bool lockCursor;
	public LayerMask mask;
	public ParticleSystem psystem;

	Vector3 vel;
	float verticalRot = 0;

	CharacterController controller;

	// Use this for initialization
	void Start () {
		controller = transform.GetComponent<CharacterController>();

		if (lockCursor) {
			Screen.lockCursor = true;
		}
	}

	void FireParticles () {
		StartCoroutine ("particleCoroutine");
	}

	IEnumerator particleCoroutine () {
		psystem.Play ();

		yield return new WaitForSeconds (.1f);

		psystem.Stop ();
		//psystem.Clear ();
	}

	//Changes Gravity
	void UpdateGrav () {
		if (Input.GetAxis ("Fire1") == 1) {

			FireParticles ();

			Vector3 orientation = Camera.main.transform.rotation.ToEulerAngles();

			float xComponent = orientation.x;
			float yComponent = orientation.y;

			//-Y
			if (((Mathf.PI / 4f) <= xComponent) && (xComponent <= (3f / 4 * Mathf.PI))) {
				Physics.gravity = new Vector3 (0, -9.8f, 0);
				return;
			}
			//+Y
			if (xComponent >= 5f / 4 * Mathf.PI &&  xComponent <= Mathf.PI * 7f / 4) {
				Physics.gravity = new Vector3 (0, 9.8f, 0);
				return;
			}
			//+Z
			if (7f / 4 * Mathf.PI <= yComponent || Mathf.PI / 4 >= yComponent) {
				Physics.gravity = new Vector3 (0, 0, 9.8f);
				return;
			}
			//+X
			if (Mathf.PI / 4 <= yComponent && yComponent <= 3f / 4 * Mathf.PI) {
				Physics.gravity = new Vector3 (9.8f, 0, 0);
				return;
			}
			//-Z
			if (Mathf.PI * 3f / 4 <= yComponent && 5f / 4 * Mathf.PI >= yComponent) {
				Physics.gravity = new Vector3 (0, 0, -9.8f);
				return;
			}
			//-X
			Physics.gravity = new Vector3 (-9.8f, 0, 0);
		}
	}

	// Rotate player
	void UpdateRot () {
		float horizRot = Input.GetAxis ("Mouse X") * mouseSens;
		transform.Rotate(0, horizRot, 0);

		verticalRot -= Input.GetAxis ("Mouse Y") * mouseSens;

	    verticalRot = Mathf.Clamp (verticalRot, -maxVerticalRotation, maxVerticalRotation);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRot, 0, 0);
	}

	bool CheckAirborn () {
		RaycastHit hit;
		bool b = Physics.Raycast(transform.position, Vector3.up * -1, out hit, 2f, mask);

		return !b || (hit.distance > 1.1f);
	}

	void UpdatePos () {

		vel.z= Input.GetAxis ("Vertical") * movementSpeed;
		vel.x= Input.GetAxis ("Horizontal") * movementSpeed;

		if (CheckAirborn ()) {
			vel.y+= -10f * Time.deltaTime;
		}
		else {
			if (Input.GetButtonDown ("Jump")) {
				vel.y= 0;
				vel.y = jumpSpeed;
			}
		}

		vel = transform.rotation * vel;
		
		controller.Move (vel * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {

		//UpdateRot ();
		UpdatePos ();
		UpdateGrav ();

	}
}
