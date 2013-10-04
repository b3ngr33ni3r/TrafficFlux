using UnityEngine;
using System.Collections;

public class NodeConstructor : MonoBehaviour {
	
	public Vector3 start_point;
	public int length;
	
	public NodeQueue Q;
	
	public int lane_width = 3;
	
	public Vector2 obstacleRange = new Vector2(3,4);
	
	Pool roadPool;
	Pool obstaclePool;
	
	public bool done = false;

	void Start () {
		roadPool = new Pool(10*lane_width, road, "roads");
		obstaclePool = new Pool(21, obstacle, "obstacles");
		
		Q = new NodeQueue();
		Q.Enqueue(new Node(new Vector3(-5f,0f,0f)));
		Q.Enqueue(new Node(new Vector3(5f,0f,0f)));
		CreateRoad(new Vector3(-5f,0f,0f), new Vector3(5f,0f,0f), Mathf.Deg2Rad*90);
		while(Q.queue_length<length) {
			AddNewPath();
		}
		done = true;
		Debug.Log("DONE");
	}
	
	void Update () {
		
	}
	
	public Transform road;
	public Transform obstacle;
	
	void AddNewPath() {
		Node n1 = Q.GetInQueue(Q.queue_length-2);
		Node n2 = Q.GetInQueue(Q.queue_length-1);
		float rotation = Mathf.Atan2(n2.position.x-n1.position.x, n2.position.z-n1.position.z)+Random.Range(-0.025f, 0.025f);
		float centerX = n2.position.x+Random.Range(30,50)*Mathf.Sin(rotation);
		float centerZ = n2.position.z+Random.Range(30,50)*Mathf.Cos(rotation);
		Node new_node = new Node(new Vector3(centerX, 0f, centerZ));
		new_node.SetLanes(lane_width);
		for(int r = 0; r < lane_width; r++) {
			new_node.positions[r] = new Vector3(centerX-(2*Mathf.Cos(rotation)*r), 0f, centerZ+(2*Mathf.Sin(rotation)*r));
		}
		Q.Enqueue(new_node);
		
		Vector3 Vert1 = n2.position;
		Vector3 Vert2 = new_node.position;
		
		CreateRoad(Vert1, Vert2, rotation);
		CreateObstacle(Vert1, Vert2);
		
	}
	
	void CreateRoad(Vector3 Vert1, Vector3 Vert2, float rotation) {
		float road_rotation = Mathf.Rad2Deg*Mathf.Atan2(Vert2.x-Vert1.x, Vert2.z-Vert1.z);
		float size = Mathf.Sqrt((Vert2.x-Vert1.x)*(Vert2.x-Vert1.x)+(Vert2.z-Vert1.z)*(Vert2.z-Vert1.z));
		for(int r = 0; r < lane_width; r++) {
			float road_centerX = Vert1.x+(Vert2.x-Vert1.x)/2-(2*Mathf.Cos(rotation)*r);
			float road_centerZ = Vert1.z+(Vert2.z-Vert1.z)/2+(2*Mathf.Sin(rotation)*r);
			
			//Transform newRoad = (Transform) Instantiate(road, new Vector3(road_centerX, -0.5f, road_centerZ), Quaternion.identity);
			Transform newRoad = roadPool.RentObject();
			newRoad.position = new Vector3(road_centerX, -0.5f, road_centerZ);
			
			Vector3 localScale = newRoad.localScale;
			localScale.z=size; 
			newRoad.localScale = localScale;
				
			Quaternion quat_rotation = newRoad.rotation;
			Vector3 vect_rotation = quat_rotation.eulerAngles;
			vect_rotation.y=road_rotation;
			quat_rotation.eulerAngles=vect_rotation;
				
			newRoad.rotation = quat_rotation;
		}
	}
	
	void CreateObstacle(Vector3 Vert1, Vector3 Vert2) {
		float road_rotation = Mathf.Rad2Deg*Mathf.Atan2(Vert2.x-Vert1.x, Vert2.z-Vert1.z);
		int obstacle_count = Random.Range((int)obstacleRange.x,(int)obstacleRange.y);
		float size = Mathf.Sqrt((Vert2.x-Vert1.x)*(Vert2.x-Vert1.x)+(Vert2.z-Vert1.z)*(Vert2.z-Vert1.z));
		for(int i = 0; i < obstacle_count; i++) {
			int r = (int)Random.Range(0, lane_width);
			
			//Transform new_obstacle  = (Transform) Instantiate(obstacle, new Vector3(Vert1.x+(Vert2.x-Vert1.x)/2, 0f, Vert1.z+(Vert2.z-Vert1.z)/2), Quaternion.identity);
			
			Transform new_obstacle = obstaclePool.RentObject();
			new_obstacle.position = new Vector3(Vert1.x+(Vert2.x-Vert1.x)/2, 0f, Vert1.z+(Vert2.z-Vert1.z)/2);
			
			Quaternion quat_rotation = new_obstacle.rotation;
			Vector3 vect_rotation = quat_rotation.eulerAngles;
			vect_rotation.y=road_rotation;
			quat_rotation.eulerAngles=vect_rotation;

			new_obstacle.rotation = quat_rotation;
			new_obstacle.Translate(r*-2,0,size/obstacle_count*i-size/2, Space.Self); //+Random.Range(-1*size/obstacle_count,size/obstacle_count)
			new_obstacle.Rotate(0,90+Random.Range(-15,15),0, Space.World);
			//new_obstacle.Translate(Random.Range(-10,10),0,0, Space.Self);
		}
	}
	
	public Node GetNode(int position) {
		return Q.GetInQueue(position);	
	}
	
	public void AdvanceQueue() {
		Q.Dequeue();
		AddNewPath();
	}
}
