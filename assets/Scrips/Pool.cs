using UnityEngine;
using System.Collections;

public class Pool : MonoBehaviour {
	
	public Transform PoolPrefab;
	public int PoolLength;
	public string poolName;
	Vector3 pool_point = new Vector3(0,0,0);
	
	Transform[] Library;
	
	public Pool(int length, Transform prefab, string poolName) {
		PoolPrefab=prefab;
		PoolLength = length;
		this.poolName = poolName;
		init();
	}
	
	void init () {
		GameObject parent = new GameObject();
		parent.transform.position=pool_point;
		parent.name=poolName;
		Library = new Transform[PoolLength];
		for(int i = 0; i < PoolLength; i++) {
			Library[i] = (Transform) Instantiate(PoolPrefab);	
			Library[i].parent = parent.transform;
			Library[i].transform.position=pool_point;
			Library[i].gameObject.SetActive(false);
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
		Library[next].gameObject.SetActive(true);
        Library[next].gameObject.transform.rotation = Quaternion.identity;
		return Library[next];
	}
}
