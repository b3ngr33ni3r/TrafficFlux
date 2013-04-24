using UnityEngine;
using System.Collections;


//attach to camera
public class CameraController : MonoBehaviour {

    public Transform player;
    
    // Use this for initialization
	void Start () {
	    //for now, just parent the camera to the player, and offset
        camera.transform.parent = player;
        camera.transform.localPosition = new Vector3(0,2,-2);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
