using UnityEngine;
using System.Collections;

public class RaycastTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
            100))
        {
            Debug.Log("Theres something happening here");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(0, 0, -10);
        }
	}
}
