using UnityEngine;
using System.Collections;

public class Dolphin : MonoBehaviour {
	public Transform nose;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		nose.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
	}
	void OnCollisionStay(Collision collision) {
		nose.transform.position = new Vector3(collision.collider.transform.position.x, collision.collider.transform.position.y, collision.collider.transform.position.z);
	}
}
