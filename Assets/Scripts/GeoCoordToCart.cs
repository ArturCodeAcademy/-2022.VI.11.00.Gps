using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoCoordToCart : MonoBehaviour
{
    public static GeoCoordToCart Instance { get; private set; }

    public float SphereScale => _sphereScale;

    [SerializeField] private float _sphereScale = 10;
    [SerializeField] private LocationPoint _locationPointPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Vector3 origin = transform.position;
        foreach (GeoCoord coord in MissionController.Instance.Coords)
        {
            Vector3 spherePoint = coord.ToVector3UnitSphere();
            Vector3 worldPosPoint = origin + spherePoint * _sphereScale;
            InstantiatePoint(worldPosPoint, coord.Label);
        }
    }

    private void InstantiatePoint(Vector3 position, string city)
    {
        Instantiate(_locationPointPrefab, position, Quaternion.identity).Text = city;
    }
}
