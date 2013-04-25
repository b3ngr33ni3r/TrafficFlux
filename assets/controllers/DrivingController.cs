using UnityEngine;
using System.Collections;

//ATTACH TO PLAYER OBJECT
public class DrivingController : MonoBehaviour {

    private static Vector3 staticPosition;
	private static float staticDrawDistance = 100; //a static for how far the dd is
    
	//get the instance position of the player. if this is unneeded, remove it cause its slowish
	public static Vector3 GetPosition() { return staticPosition; }
	
	//this is a point on the same x,y as the player (this obj) but
	//at a z that is at a set distance in front of it. ie, 100 in front
	//its calculated.
	public static Vector3 GetDrawDistance() { return staticPosition + new Vector3(0,0,staticDrawDistance); }
	
    public float speed = 5f;
	public float drawDistance = -1; //-1 will use default of 100

	// Use this for initialization
	void Start () {
        staticPosition = transform.position;
		if (drawDistance != -1)
		staticDrawDistance = drawDistance;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Time.deltaTime * speed);
        
        staticPosition = transform.position;

	}
}
