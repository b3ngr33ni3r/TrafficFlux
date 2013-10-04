using UnityEngine;
using System.Collections;

public class SphereColliderMovementThing : MonoBehaviour {

    public int inside = 0;

	void Start () {
	    
	}

	void Update () {
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -0.1f, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0.1f, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, 0, 0.1f);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(0, 0, -0.1f);
        }

	}

    void OnTriggerStay(Collider other)
    {
        inside = other.gameObject.GetInstanceID();
    }
    void OnTriggerExit(Collider other)
    {
        inside = 0;
    }
}
