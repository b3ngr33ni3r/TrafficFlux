using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

public class RoadMaster : MonoBehaviour {
	
	public MasterMaster masterMaster;
	
	Pool road_pool;
	public Transform road;
	public Vector3 start_point;
	Vector3 spawn_point;
	public bool use_file;
	public Vector3 offset;
	public float scale;
	public string filename;
	public ArrayList segements;
	public string filepath;
    public bool isJSON;
    public int RoadPoolCount = 50;
	
	public RoadSegement head = null;

    public string[] type_exceptions = {};
    public string[] properties_exceptions = { };

	// Use this for initialization
	void Start () {
        road_pool = new Pool(RoadPoolCount, road, "Road");
		spawn_point = start_point;

		segements = new ArrayList();
		RoadSegement previous = null;
		if(use_file) {
            if (!isJSON)
            {
                using (StreamReader reader = new StreamReader(filepath + filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Do something with line
                        string[] parts = line.Split(',');
                        //Debug.Log(parts[0]+", "+parts[1]+", "+parts[2]);
                        int lanes = int.Parse(parts[0]);
                        float x = (float.Parse(parts[1]) + offset.x) * scale;
                        float y = (float.Parse(parts[2]) + offset.y) * scale;
                        float z = (float.Parse(parts[3]) + offset.z) * scale;
                        RoadSegement roadSeg = new RoadSegement();
                        roadSeg.x = x;
                        roadSeg.y = y;
                        roadSeg.z = z;
                        roadSeg.lanes = lanes;
                        roadSeg.oneway = false;
                        roadSeg.name = "";
                        segements.Add(roadSeg);

                        roadSeg.previousSegement = previous;
                        if (previous != null)
                        {
                            previous.nextSegement = roadSeg;
                            MakeRoad(previous, roadSeg);
                        }
                        previous = roadSeg;
                    }
                }
            }
            else
            {

                StartCoroutine(LoadFromJSON());

            }
		} else {
			int lanes = 2;
			
			float x = 0;
			float y = 0;
			float z = 0;
			float rot  = 0;
			for(int i = 0; i < 30; i++) {
				RoadSegement roadSeg = new RoadSegement();
				rot += Mathf.Deg2Rad*Random.Range(-15,15);
				float distance = Random.Range(30,50);
				roadSeg.x=x+=Mathf.Sin(rot)*distance;
				roadSeg.y=y+=0;
				roadSeg.z=z+=Mathf.Cos(rot)*distance;
				roadSeg.lanes = lanes;
				roadSeg.oneway = false;
                roadSeg.name = "";
				segements.Add(roadSeg);
					
				roadSeg.previousSegement=previous;
                if (previous != null)
                {
                    previous.nextSegement = roadSeg;
                    MakeRoad(previous, roadSeg);
                }
				previous=roadSeg;
			}
		}
		if(head == null && segements.Count > 0) {
			head = (RoadSegement) segements[0];	
			if(masterMaster.carMaster!=null)
				masterMaster.carMaster.StartCar();
		}
        masterMaster.carMaster.SetCarPosition(new Vector3(head.x, head.y, head.z));
		//for(int i = 0; i < segements.Count; i++) {
			//RoadSegement roadSeg1 = (RoadSegement)segements[i];
			//RoadSegement roadSeg2 = roadSeg1.nextSegement;
			//MakeRoad(roadSeg1, roadSeg2);
		//}
		
	}
    IEnumerator LoadFromJSON()
    {
        RoadSegement previous = null;
        using (StreamReader reader = new StreamReader(filepath + filename))
        {
            string geo = reader.ReadToEnd();
            reader.Close();
            JSONNode node = JSON.Parse(geo);
            //Debug.Log("1");
            foreach (JSONNode n in node["features"].Childs)
            {
                bool pass = false;
                for (int w = 0; w < type_exceptions.Length; w++)
                {
                    if (n["properties"]["highway"].Value.Equals(type_exceptions[w]))
                    {
                        pass = true;
                    }
                }
                for (int w = 0; w < properties_exceptions.Length; w++)
                {
                    if (n["properties"][properties_exceptions[w]] != null)
                    {
                        pass = true;
                    }
                }
                if (pass)
                    continue;


                int lanes = n["properties"]["lanes"].AsInt;
                if (lanes <= 0)
                {
                    lanes = 1;
                }
                string name = n["properties"]["name"].Value;

                if (n["geometry"]["type"].Value.ToString().ToLower().Equals("linestring"))
                {
                    for (int r = 0; r < n["geometry"]["coordinates"].Count; r++)
                    {
                        JSONNode coordTop = n["geometry"]["coordinates"][r];
                        for (int e = 0; e < coordTop.Count; e += 2)
                        {
                            //Debug.Log("f.p = "+float.Parse(coordTop[e]));
                            float x = (float.Parse(coordTop[e]) + offset.x) * scale;
                            float z = (float.Parse(coordTop[e + 1]) + offset.y) * scale;

                            float y = (0.0f + offset.z) * scale;
                            RoadSegement roadSeg = new RoadSegement();
                            roadSeg.x = x;
                            roadSeg.y = y;
                            roadSeg.z = z;
                            roadSeg.lanes = lanes;
                            roadSeg.oneway = false;
                            roadSeg.name = name;
                            segements.Add(roadSeg);
                            roadSeg.previousSegement = previous;
                            if (previous != null)
                            {
                                previous.nextSegement = roadSeg;
                                MakeRoad(previous, roadSeg);
                            }
                            previous = roadSeg;
                            //Debug.Log("Got one");
                        }
                    }
                    RoadSegement breakSeg = new RoadSegement();
                    breakSeg.Break = true;
                    segements.Add(breakSeg);
                    breakSeg.previousSegement = previous;
                    if (previous != null)
                        previous.nextSegement = breakSeg;
                    previous = breakSeg;

                }
                //Debug.Log("Yielding");
                yield return null;
            }
        }
        //Debug.Log("DONE");
    }
	// Update is called once per frame
	void Update () {
		
	}
    public float roadWidth = 4f;
	void MakeRoad(RoadSegement roadSeg1, RoadSegement roadSeg2) {
		for(int i = 0; i < roadSeg1.lanes; i++) {
            if (roadSeg1.Break == false && roadSeg2.Break == false && roadSeg2 != null)
            {
				Vector3 point1 = new Vector3(roadSeg1.x, roadSeg1.y, roadSeg1.z);
				Vector3 point2 = new Vector3(roadSeg2.x, roadSeg2.y, roadSeg2.z);
				Transform newRoad = road_pool.RentObject();
                newRoad.GetComponent<SoMuchWork>().name = roadSeg1.name;
                newRoad.GetComponent<SoMuchWork>().coord_x1 = point1.x;
                newRoad.GetComponent<SoMuchWork>().coord_y1 = point1.z;
                newRoad.GetComponent<SoMuchWork>().coord_x2 = point2.x;
                newRoad.GetComponent<SoMuchWork>().coord_y2 = point2.z;
                if ((point1.x - point2.x) == 0)
                    newRoad.GetComponent<SoMuchWork>().atanResult = 0;
                else
                    newRoad.GetComponent<SoMuchWork>().atanResult = 90+Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan((point2.z - point1.z) / (point2.x - point1.x)));
				newRoad.position = new Vector3(point1.x+(point2.x-point1.x)/2.0f, point1.y+(point2.y-point1.y)/2.0f, point1.z+(point2.z-point1.z)/2.0f);
				newRoad.localScale = new Vector3(roadWidth,0.025f,Vector3.Distance(point1,point2));
                //Debug.Log("point1 = " + point1 + ", point2 = " + point2);
                if ((point1.x - point2.x) == 0)
                    newRoad.Rotate(new Vector3(0,
                        0
                        , 0));
                else
                {
                    if ((point1.x > point2.x && point1.z > point2.z) ||
                        (point1.x < point2.x && point1.z < point2.z))
                    {
                        // 360 minus
                        Quaternion newRot = new Quaternion();
                        newRot.eulerAngles = new Vector3(0,
                            360 - newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                            , 0);
                        newRoad.rotation = newRot;
                    }
                    else
                    {
                        // just take what it is
                        Quaternion newRot = new Quaternion();
                        newRot.eulerAngles = new Vector3(0,
                            newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                            , 0);
                        newRoad.rotation = newRot;
                    }
                    //if (point1.x > point2.x)
                    //{
                    //    if (point1.z > point2.z)
                    //    {
                    //        Quaternion newRot = new Quaternion();
                    //        newRot.eulerAngles = new Vector3(0,
                    //            360 - newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                    //            , 0);
                    //        newRoad.rotation = newRot;
                    //    }
                    //    else
                    //    {
                    //        Quaternion newRot = new Quaternion();
                    //        newRot.eulerAngles = new Vector3(0,
                    //            newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                    //            , 0);
                    //        newRoad.rotation = newRot;
                    //    }
                    //}
                    //else
                    //{
                    //    if (point1.z < point2.z)
                    //    {
                    //        Quaternion newRot = new Quaternion();
                    //        newRot.eulerAngles = new Vector3(0,
                    //            360 - newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                    //            , 0);
                    //        newRoad.rotation = newRot;
                    //    }
                    //    else
                    //    {
                    //        Quaternion newRot = new Quaternion();
                    //        newRot.eulerAngles = new Vector3(0,
                    //            newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                    //            , 0);
                    //        newRoad.rotation = newRot;
                    //    }
                    //}
                    
                    //newRoad.Rotate(new Vector3(0,
                    //    Mathf.Rad2Deg * newRoad.GetComponent<SoMuchWork>().atanResult//Mathf.Atan((point1.z - point2.z) / (point1.x - point2.x))
                    //    , 0), Space.Self);
                    //newRoad.Translate(i * roadWidth, 0, 0);
                }
			}
		}
	}
	
	void GetNextRoadCoord() {
		
	}
}
