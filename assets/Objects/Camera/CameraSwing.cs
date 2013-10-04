using UnityEngine;
using System.Collections;

public class CameraSwing : MonoBehaviour {
	
	public float speed = 0.0f;
	public Vector3 origin;
	public Vector3 axis;
	public Transform lookAt;

	void Start () {
		
	}
	
	void Update () {
		this.transform.LookAt(origin);
		this.transform.RotateAround(origin, axis, speed);
	}
}
