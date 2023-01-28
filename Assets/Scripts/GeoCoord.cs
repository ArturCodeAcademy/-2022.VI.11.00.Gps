using System;
using UnityEngine;

[Serializable]
public class GeoCoord
{
    public string Label;
    public float Latitude;
    public float Longitude;
}

public static class GeoCoordExtentions
{
    public static Vector3 ToVector3UnitSphere(this GeoCoord coord)
    {
        return Quaternion.Euler(0, -coord.Longitude, 0) *
            (Quaternion.Euler(-coord.Latitude, 0, 0) * Vector3.forward);
    }
}
