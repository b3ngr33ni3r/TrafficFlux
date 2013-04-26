using UnityEngine;
using System.Collections;

public class Node {
	
	public Vector3 position;
	public Node nextNode;
	public Node previousNode;
	public int lanes;
	public Vector3[] positions;
	
	public Node (Vector3 v) {
		position = v;
	}
	
	public void SetLanes(int lanes) {
		this.lanes=lanes;
		positions=new Vector3[lanes];
		
	}
	public void SetLane(int lane, Vector3 position) {
		if(lane < lanes && positions !=null) {
			positions[lane] = position;
		}
	}
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public Vector3 GetPosition(int lane) {
		if(lane < lanes) {
			return positions[lane];	
		}
		return new Vector3(0f,0f,0f);
	}
}
