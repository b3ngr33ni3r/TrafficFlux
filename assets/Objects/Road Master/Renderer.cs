using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StreamReader reader = new StreamReader("lon.geojson");
		string geo = reader.ReadToEnd();
		reader.Close();
		JSONNode node = JSON.Parse(geo);
		foreach (JSONNode n in node["features"].Childs)
			if (n["geometry"]["type"].ToString().ToLower() == "linestring")
				coordRender(n["geometry"]["coordinates"]);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void coordRender(JSONNode array) {
		//you can do this
		float[] coords = new float[array.Count];
		int index = 0;
		foreach (JSONNode node in array.Childs)
			coords[index++] = float.Parse(node.Value);
	}
}
