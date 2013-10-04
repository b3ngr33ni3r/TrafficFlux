using UnityEngine;
using System.Collections;

public class CarMaster : MonoBehaviour {
	
	public MasterMaster masterMaster;
	public Camera mainCamera;
	public bool OnRails = false;
	Transform rail;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void StartCar() {
		RoadSegement start = (RoadSegement) masterMaster.roadMaster.head.nextSegement;
		if(start!=null) {
			//mainCamera.transform.position = new Vector3(start.x, start.y, start.z);
			//mainCamera.transform.Rotate(90,0,0);;
		}
	}
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="Rail" && rail!=collision.transform) {
			rail = collision.transform;
			/*transform.Rotate(
				rail.rotation.eulerAngles.x-transform.rotation.eulerAngles.x,
				rail.rotation.eulerAngles.y-transform.rotation.eulerAngles.y,
				rail.rotation.eulerAngles.z-transform.rotation.eulerAngles.z
				);*/
			
		}
	}
	void OnCollisionExit(Collision collision) {
		if(collision.gameObject.tag=="Rail") {
			//OnRails=false;
			//rail=null;
		}
	}
	// Update is called once per frame
	public float rotateSpeed = 0f;
	public float maxRotateSpeed = 0f;
	
	public float forwardSpeed = 0f;
	public float maxForwardSpeed = 0.75f;
	
	void Update () {
		
        /*
         *  fire a ray out infront of the car
         *  drive until it stops hitting an object, then turn back
         */

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
            10))
        {
            if (rail == null)
            {
                //Debug.Log("Theres something happening here");
            }
            else
            {
                //OnRails = true;
                //transform.Rotate(
                //rail.rotation.eulerAngles.x - transform.rotation.eulerAngles.x,
                //rail.rotation.eulerAngles.y - transform.rotation.eulerAngles.y,
                //rail.rotation.eulerAngles.z - transform.rotation.eulerAngles.z
                //);
            }
        }
        else
        {
            //Debug.Log("Nevermind");
            if (rail != null)
            {
                OnRails = true;
                /*transform.Rotate(
                rail.rotation.eulerAngles.x - transform.rotation.eulerAngles.x,
                rail.rotation.eulerAngles.y - transform.rotation.eulerAngles.y,
                rail.rotation.eulerAngles.z - transform.rotation.eulerAngles.z
                );*/
            }
        }
		
		if(Input.GetKeyDown(KeyCode.A)) {
			mergingLeft = true;
			mergingRight = false;
		//	transform.Rotate(0,-10,0);
		} else if(Input.GetKeyDown(KeyCode.D)) {
			mergingLeft = false;
			mergingRight = true;
		//	transform.Rotate(0,10,0);
		}
		if(!OnRails) {
		if(Input.GetKey(KeyCode.A)) {
			// go left	
			
			if(rotateSpeed>=0) {
				rotateSpeed=-0.1f;	
			}
			rotateSpeed-=Mathf.Sqrt(Mathf.Abs(rotateSpeed/5000));
			if(rotateSpeed < -maxRotateSpeed) {
				rotateSpeed = -maxRotateSpeed;	
			}
			transform.Rotate(0,rotateSpeed,0);
		} else if(Input.GetKey(KeyCode.D)) {
			// go right
			
			if(rotateSpeed<=0) {
				rotateSpeed=0.1f;	
			}
			rotateSpeed+=Mathf.Sqrt(Mathf.Abs(rotateSpeed/5000));
			if(rotateSpeed > maxRotateSpeed) {
				rotateSpeed = maxRotateSpeed;	
			}
			transform.Rotate(0,rotateSpeed,0);
			
		} else {
			// straigten out
			//mergingLeft = false;
			//mergingRight = false;
			if(rotateSpeed>0.05) {
				rotateSpeed-=Mathf.Sqrt(Mathf.Abs(rotateSpeed/500));
			} else if(rotateSpeed<-0.05) {
				rotateSpeed+=Mathf.Sqrt(Mathf.Abs(rotateSpeed/500));
			} else {
				rotateSpeed = 0;	
			}
			transform.Rotate(0,rotateSpeed,0);
		}
		} else {
			float dif = transform.rotation.eulerAngles.y - rail.rotation.eulerAngles.y;
            Debug.Log("dif = " + dif);
			if(dif > 2f) {
                transform.Rotate(0, -Mathf.Sqrt(Mathf.Abs(dif))*(forwardSpeed*10), 0);
			} else if (dif < -2f) {
                transform.Rotate(0, Mathf.Sqrt(Mathf.Abs(dif))*(forwardSpeed*10), 0);
			} else {
				transform.Rotate(
				rail.rotation.eulerAngles.x-transform.rotation.eulerAngles.x,
				rail.rotation.eulerAngles.y-transform.rotation.eulerAngles.y,
				rail.rotation.eulerAngles.z-transform.rotation.eulerAngles.z
				);
			}
			/*Debug.Log("this position = "+transform.position);
			Debug.Log("rail position = "+rail.collider.transform.position);rail.collider.
			Debug.Log("collide scale = "+rail.collider.transform.lossyScale);
			if(
				transform.position.x > rail.collider.transform.position.x-rail.collider.transform.lossyScale.z/2 && 
				transform.position.x < rail.collider.transform.position.x+rail.collider.transform.lossyScale.z/2 &&
				transform.position.z > rail.collider.transform.position.z-rail.collider.transform.lossyScale.x/2 && 
				transform.position.z < rail.collider.transform.position.z+rail.collider.transform.lossyScale.x/2) {
				Debug.Log("ENCLOSED");
				transform.Rotate(
				rail.rotation.eulerAngles.x-transform.rotation.eulerAngles.x,
				rail.rotation.eulerAngles.y-transform.rotation.eulerAngles.y,
				rail.rotation.eulerAngles.z-transform.rotation.eulerAngles.z
				);
			}*/
		}
		if(Input.GetKey(KeyCode.W)) {
			// go forward
			if(forwardSpeed<=0) {
				forwardSpeed=0.05f;	
			}
			forwardSpeed+=Mathf.Sqrt(Mathf.Abs(forwardSpeed/1000));
			if(forwardSpeed>maxForwardSpeed) {
				forwardSpeed = maxForwardSpeed;	
			}
			transform.Translate(0,0,forwardSpeed);
		} else if(Input.GetKey(KeyCode.S)) {
			// go  back
			//if(forwardSpeed>=0) {
				//forwardSpeed=-0.05f;	
			//}
			forwardSpeed-=Mathf.Sqrt(Mathf.Abs(forwardSpeed/1000));
			if(forwardSpeed<0) {
				forwardSpeed = 0;	
			}
			transform.Translate(0,0,forwardSpeed);
		} else {
			// slow down
			/*if(forwardSpeed>0.05) {
				forwardSpeed-=Mathf.Sqrt(Mathf.Abs(forwardSpeed/100000));
			} else if(forwardSpeed<-0.05) {
				forwardSpeed+=Mathf.Sqrt(Mathf.Abs(forwardSpeed/100000));
			} else {
				forwardSpeed = 0;	
			}*/
			transform.Translate(0,0,forwardSpeed);
		}
	}
    public void SetCarPosition(Vector3 position)
    {
        transform.position = position;
    }
	bool mergingLeft = false;
	bool mergingRight = false;
}