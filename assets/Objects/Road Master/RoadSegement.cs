using UnityEngine;
using System.Collections;

public class RoadSegement {
	public float x;
	public float y;
	public float z;
	public float lanes;
	public bool oneway;
    public bool Break = false;
    public string name;
	
	public RoadSegement nextSegement;
	public RoadSegement previousSegement;
}