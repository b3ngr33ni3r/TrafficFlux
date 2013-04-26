using UnityEngine;
using System.Collections;

public class Pool : MonoBehaviour {
	
	public Transform PoolPrefab;
	public int PoolLength;
	
	Transform[] Library;
	
	public Pool(int length, Transform prefab) {
		PoolPrefab=prefab;
		PoolLength = length;
		init();
	}
	
	void init () {
		Library = new Transform[PoolLength];
		for(int i = 0; i < PoolLength; i++) {
			Library[i] = (Transform) Instantiate(PoolPrefab);	
		}
	}
	
	int next = -1;
	
	void Update () {
	
	}
	
	public Transform RentObject() {
		if(next>=PoolLength-1) {
			next = -1;
		}
		next++;
		return Library[next];
	}
}
