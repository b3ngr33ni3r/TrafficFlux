using UnityEngine;
using System.Collections;


//uses PoolManager to take RoadSegments and merge them into unity, using a specified visual as
//the road segment visual. This visual must be stored in the PoolManager, and is accessible
//via its string uid, called poolRoadName. spawnLimit is the amount of segments allowed
//to spawn per iteration of Update()
public class RoadController : MonoBehaviour {

    public string roadPoolName="RoadSegment";
    public int spawnLimit = 3;

    private int ySpawnPoint = 100;//for demoing constant creation

    private static ArrayList _roads;
	
   // private ArrayList _spawnedRoads;

    //call this to tell road segments to be drawn
	//note that road segments are given world points, so 
	//thats where they are drawn, not at DrawDistance, like
	//other things (ie billboards)
    public static void Add(RoadSegment road) {
        if (_roads == null)
            _roads = new ArrayList();
        ArrayList.Synchronized(_roads).Add(road);
    }
    
    // Use this for initialization
	void Start () {
        int x = 0;
            for (int y = 0; y < ySpawnPoint; y++)
                Add(new RoadSegment(new Vector3(x, 0, y), new Quaternion(), new Vector3()));
	}
	
	// Update is called once per frame
	void Update () {
        ArrayList sync = ArrayList.Synchronized(_roads);

        if (sync.Count > 0)
        {
            //if (_spawnedRoads == null)
            //    _spawnedRoads = new ArrayList();

            int spawnCount = 0;//only spawn spawnLimit segements each Update() this does that.
            foreach (RoadSegment road in sync) {
                if (spawnCount > spawnLimit)
                    break;
                GameObject o = PoolManager.Spawn(roadPoolName);
                if (o == null)//this should hit when the hard limit is reached
                    break;
                o.transform.position = road.GetPosition();
                o.transform.rotation = road.GetRotation();
               
               // _spawnedRoads.Add(o);
                spawnCount++;
            }
            sync.RemoveRange(0, spawnCount);
        }
	}

    void FixedUpdate()
    {
        Add(new RoadSegment(new Vector3(0, 0, ySpawnPoint++), new Quaternion(), new Vector3()));
    }

}
