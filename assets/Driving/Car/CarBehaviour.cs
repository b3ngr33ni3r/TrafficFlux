using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

	public int iterator = 1;
	
	public NodeConstructor nc;
	public Vector3 target_v;
	
	public float speed = 0.05f;
	public float max_speed = 0;
	
	public Vector2[] timeSpeeds;
	
	public int lane = 1;
	
	void Start () {
		//target_v = nc.GetNode(iterator).position;
		target_v=new Vector3(5,0,0);
	}
	
	public float lane_x = 0.0f;
	public float lane_z = 0.0f;
	float DistanceToGo=0;
	public bool changing_lanes = false;
	
	public float distance_traveled = 0f;
	
	public TextMesh score_board;
	
	void Update () {
		Vector3 target_total = target_v;
		target_total.x+=lane_x;
		target_total.z+=lane_z;
		// point yourself to the target vector
		Point(target_total);
		
		// move towards the target vector
		Vector3 translate_comp = new Vector3();
		translate_comp.x=0f;
		translate_comp.z=speed;
		
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			lane++;
			DistanceToGo-=2;
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			lane--;
			DistanceToGo+=2;
		}
		
		float moveDistance = 11f*Time.deltaTime;
		
		if(DistanceToGo>moveDistance || DistanceToGo<-moveDistance) {
			if(DistanceToGo < 0) {
				transform.Translate(-moveDistance, 0, 0);
				DistanceToGo+=moveDistance;
				lane_x+=moveDistance*Mathf.Sin(Mathf.Deg2Rad*transform.rotation.y);
				lane_z+=moveDistance*Mathf.Cos(Mathf.Deg2Rad*transform.rotation.y);
			} else if(DistanceToGo > 0) {
				transform.Translate(moveDistance, 0, 0);
				DistanceToGo-=moveDistance;
				lane_x-=moveDistance*Mathf.Sin(Mathf.Deg2Rad*transform.rotation.y);
				lane_z-=moveDistance*Mathf.Cos(Mathf.Deg2Rad*transform.rotation.y);
			}
		} else {
			Debug.Log("HERE, " + iterator);
			transform.Translate(DistanceToGo, 0, 0, Space.Self);
			DistanceToGo=0;	
			changing_lanes=false;
			lane_x=0;
			lane_z=0;
			target_v = nc.GetNode(iterator).GetPosition(lane);
		}
		
		
		if(Input.GetKey(KeyCode.W)) {
			transform.Translate(0, 0, 0.15f);
		} else if(Input.GetKey(KeyCode.S)) {
			transform.Translate(0, 0, -0.15f);
		} 
		transform.Translate(translate_comp);
		distance_traveled+=speed;
		score_board.text=""+Mathf.Ceil(distance_traveled);
		
		for(int i = 0; i < timeSpeeds.Length; i++) {
			if(Time.time < timeSpeeds[i].x) {
				max_speed = timeSpeeds[i].y*Time.deltaTime;
			}
		}
		if(speed < max_speed) {
			speed+=(max_speed-speed)*(0.25f*Time.deltaTime);
		} 
		if(speed > max_speed) {
			speed = max_speed;	
		}
		//if(Mathf.Ceil(distance_traveled)%2==0) {
		//	speed*=1.005f;
		//}
		
		if(Vector3.Distance(transform.position, target_total) < 1) {
			if(iterator<5)
				iterator++;	
			else
				nc.AdvanceQueue();
			target_v = nc.GetNode(iterator).GetPosition(lane);//.position;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="Obstacle") {
			//Application.LoadLevel(0);	
			speed*=0.25f;
		}
	}
	
	void Point(Vector3 v) {
		Vector3 targetRot = new Vector3(0f, GetAngle(transform.position, v), 0f);
		while(targetRot.y<0) {
			targetRot.y+=360;
		}
		while(targetRot.y>=360) {
			targetRot.y-=360;
		}
		var distance = Vector3.Distance(targetRot, transform.rotation.eulerAngles);
		while(distance<0) {
			distance+=360;
		}
		while(distance>=360) {
			distance-=360;
		}
		if(distance>1) {
			if(targetRot.y-transform.rotation.eulerAngles.y<0) {
				if(distance > 180) {
					AddEulerAngle(0f, (360-distance)*(speed/2), 0f);
				} else {
					AddEulerAngle(0f, (-distance)*(speed/2), 0f);
				}
			} else {
				if(distance > 180) {
					AddEulerAngle(0f, -(360-distance)*(speed/2), 0f);
				} else {
					AddEulerAngle(0f, (distance)*(speed/2), 0f);
				}
			}
		} else {
			SetEulerAngle(targetRot.x, targetRot.y, targetRot.z);
		}	
	}
	
	bool InThreshHold2(float x1, float y1, float x2, float y2, float threshhold) {
		if(x2-x1 < threshhold && y2-y1 < threshhold) {
			return true;
		}
		if(x1-x2 < threshhold && y1-y2 < threshhold) {
			return true;
		}
		return false;
	}
	
	float GetAngle(Vector3 vect1, Vector3 vect2) {
		var rotation = Mathf.Rad2Deg*Mathf.Atan2(vect2.x-vect1.x, vect2.z-vect1.z);
		return rotation;
	}
	
	void AddEulerAngle(float x, float y, float z) {
		Quaternion q = transform.rotation;
		Vector3 v = q.eulerAngles;
		v.x+=x;
		v.y+=y;
		v.z+=z;
		q.eulerAngles=v;
		transform.rotation=q;
	}
	void SetEulerAngle(float x, float y, float z) {
		Quaternion q = transform.rotation;
		Vector3 v = q.eulerAngles;
		v.x=x;
		v.y=y;
		v.z=z;
		q.eulerAngles=v;
		transform.rotation=q;
	}
	
	IEnumerable Translation(Vector3 startPos, Vector3 endPos, float time) {
		Debug.Log("T");
    	float t = 0.0f;
    	while (t < 1.0) {
    	    t += Time.deltaTime * (1/time);
    	    transform.localPosition = Vector3.Lerp(startPos, endPos, t);
    	    float dist = Vector3.Distance(transform.localPosition, endPos);
			if(dist < .001) {
				transform.localPosition = endPos;
			} else {
    	    	yield return null; 
    	    }
    	}
	}
	IEnumerable Translation(Vector3 distance, float time) {
		Debug.Log("T");
    	float t = 0.0f;
    	while (t < 1.0) {
    	    t += Time.deltaTime * (1/time);
			transform.Translate(Vector3.Lerp(new Vector3(0,0,0), distance, t),Space.Self);
    	    //transform.localPosition = Vector3.Lerp(startPos, endPos, t);
    	    float dist = Vector3.Distance(transform.localPosition, transform.localPosition+distance);
			if(dist < .001) {
				changing_lanes=false;
				//transform.localPosition = endPos;
			} else {
    	    	yield return null; 
    	    }
    	}
	}
}
