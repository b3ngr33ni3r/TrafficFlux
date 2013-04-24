using UnityEngine;
using System.Collections;

public class DrivingController : MonoBehaviour {

    private static Vector3 staticPosition;
    public static Vector3 GetPosition() { return staticPosition; }

    public float speed = 5f;

	// Use this for initialization
	void Start () {
        staticPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Time.deltaTime * speed);
        
        staticPosition = transform.position;

	}
}
