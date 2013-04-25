using UnityEngine;
using System.Collections;

//uses poolmanager to spawn billboards
public class BillboardController : MonoBehaviour {
	
	public string billboardPoolName; //what the billboard is called in poolmanager
	public float timeBetween = 2; //time between each billboard spawn, if not using rndm
	public bool useRandom = false; //if true, time between is ignored
	private float deltaStore = 0; // stores our temp delta until its cleared and spawn occurs, repeated.
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		deltaStore += Time.deltaTime;
		if (deltaStore >= timeBetween) {
			deltaStore = 0;
			GameObject o = PoolManager.Spawn(billboardPoolName);
			if (o == null)//hard limit reached, or bad pool name
				return;
			Vector3 ppos = DrivingController.GetDrawDistance();//staticly set in DrivingController by unityEditor
			o.transform.position = new Vector3(ppos.x + 4,ppos.y,ppos.z);//this needs polish, obv
			o.transform.rotation = new Quaternion();
		}
			
	}
}
