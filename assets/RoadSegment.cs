using UnityEngine;
using System.Collections;

public class RoadSegment {

    private Vector3 position, center;
    private Quaternion rotation;
    private float scale;

    //construct a road segement
    public RoadSegment(Vector3 position, Quaternion rotation, Vector3 center, float scale = 0)
    {
        this.position = position;
        this.rotation = rotation;
        this.center = center;
        this.scale = scale;
    }

    //how is the road segment designed to be positioned in unity
    public Vector3 GetPosition()
    {
        return position;
    }

    //how is the road segment designed to be rotated in unity
    public Quaternion GetRotation()
    {
        return rotation;
    }

    //since unity uses center points for instantiation, this is useful
    public Vector3 GetCenterVertex()
    {
        return center;
    }

    //what scale is the road segment designed to use in unity
    public float GetScale()
    {
        return scale;
    }

}