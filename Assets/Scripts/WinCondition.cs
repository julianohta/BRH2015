using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class WinCondition : MonoBehaviour {
	
	BoxCollider bc;
	public Transform t;

	// Use this for initialization
	void Start () {
		bc = this.GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.transform == t) {
			Debug.Log ("Win");
			StartCoroutine ("ResetScene");
		}
	}

	IEnumerator ResetScene () {
		yield return new WaitForSeconds (5f);

		
		Application.LoadLevel (0);
	}

	// Update is called once per frame
	void Update () {

	}
}