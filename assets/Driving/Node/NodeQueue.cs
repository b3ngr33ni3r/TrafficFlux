using UnityEngine;
using System.Collections;

public class NodeQueue {
	
	Node first_node;
	Node last_node;
	
	public int queue_length = 0;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public void Enqueue(Node node) {
		if(queue_length>1) {
			last_node.nextNode = node;
			node.previousNode = last_node;
			last_node = node;
			queue_length++;
		} else if(queue_length==1) {
			last_node = node;
			first_node.nextNode=node;
			last_node.previousNode=first_node;
			queue_length++;
		} else if(queue_length==0) {
			first_node = node;
			last_node = node;
			queue_length++;
		}
	}
	
	public void Dequeue() {
		if(queue_length>2) {
			first_node = first_node.nextNode;
			queue_length--;
		} else if(queue_length==2) {
			last_node = first_node;
			queue_length--;
		} else if(queue_length==1) {
			last_node = null;
			first_node = null;
			queue_length--;
		}
	}
	
	public Node GetInQueue(int position) {
		Node node = first_node;
		for(int i = 0; i<position; i++) {
			node = node.nextNode;	
		}
		return node;
	}
	
	public void Print() {
		Node node = first_node;
		if(queue_length>1) {
			Debug.Log("Length = "+queue_length);
			while(node!=null) {
				Debug.Log(node.position);
				node = node.nextNode;
			}
		} else if(queue_length==1) {
			Debug.Log("Length = "+queue_length+", "+first_node.position);
		} else if(queue_length==0) {
			Debug.Log("Empty queue");
		}
	}
}
