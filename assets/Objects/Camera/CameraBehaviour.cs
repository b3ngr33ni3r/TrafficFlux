using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
	
	public Transform car;

	void Start () {
	
	}
	
	void Update () {
		
		
		
		//Vector3 ihatedoingthis = new Vector3(car.position.x, transform.position.y, car.position.z);
		//transform.position = ihatedoingthis;
		
		Quaternion ireallyhatedoingthis = new Quaternion();
		Vector3 thisisstupid = new Vector3(car.rotation.eulerAngles.x, car.rotation.eulerAngles.y, car.rotation.eulerAngles.z);
		ireallyhatedoingthis.eulerAngles = thisisstupid;
		transform.rotation = ireallyhatedoingthis;
		
		transform.Translate(
			car.position.x-transform.position.x,
			0,
			car.position.z-transform.position.z,Space.World);
		
		transform.Translate(0,0,0.5f,Space.Self);
		
		//transform.LookAt(car);
	}
}
