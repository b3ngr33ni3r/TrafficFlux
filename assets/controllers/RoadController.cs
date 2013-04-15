using UnityEngine;
using System.Collections;


//uses PoolManager to take RoadSegments and merge them into unity, using a specified visual as
//the road segment visual. This visual must be stored in the PoolManager, and is accessible
//via its string uid, called poolRoadName. spawnLimit is the amount of segments allowed
//to spawn per iteration of Update()
public class RoadController : MonoBehaviour {

    public string poolRoadName="RoadSegment";
    public int spawnLimit = 3;

    private static ArrayList _roads;
   // private ArrayList _spawnedRoads;

    //call this to tell road segments to be drawn
    public static void Add(RoadSegment road) {
        if (_roads == null)
            _roads = new ArrayList();
        ArrayList.Synchronized(_roads).Add(road);
    }
    
    // Use this for initialization
	void Start () {
        Add(new RoadSegment(new Vector3(), new Quaternion(), new Vector3()));
	}
	
	// Update is called once per frame
	void Update () {
        ArrayList sync = ArrayList.Synchronized(_roads);
        if (sync.Count > 0)
        {
            //if (_spawnedRoads == null)
            //    _spawnedRoads = new ArrayList();

            int spawnCount = 0;
            foreach (RoadSegment road in sync) {
                if (spawnCount>spawnLimit)
                    break;
                GameObject o = PoolManager.Spawn(poolRoadName);
                o.transform.position = road.GetPosition();
                o.transform.rotation = road.GetRotation();
               
               // _spawnedRoads.Add(o);
                spawnCount++;
            }
            sync.RemoveRange(0, spawnCount);
        }
	}

}
