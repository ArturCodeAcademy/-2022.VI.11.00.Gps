using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Transform _mapMarkerSphereTransform;

    [Header("UI Settings")]
    [SerializeField] private Image _mapMarker;

    private RaycastHit _hit;
    private GeoCoord _newCoordinate;
    private Vector3 _worldPositionCoord;
    private Vector2 _coordLatLong;
    private Vector2 _tempMissionCoords = new Vector2(54.6872f, 25.2797f);
    private bool _canDisplayMarker;
    private bool _canChooseNewLocation = true;
    private GeoCoordToCart _coordTransformator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _coordTransformator = GeoCoordToCart.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canChooseNewLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit))
            {
                _coordLatLong = (ToGPSCoords(_hit.point, _coordTransformator.SphereScale) - new Vector2(90, 0)) * -1;
                _newCoordinate = new GeoCoord
                {
                    Latitude = _coordLatLong.x,
                    Longitude = _coordLatLong.y
                };
                CreateMarker();
            }
        }

        if (_canDisplayMarker)
        {
            DisplayMarker(_worldPositionCoord);
        }
    }

    private void CreateMarker()
    {
        _mapMarker.gameObject.SetActive(true);
        Vector3 spherePoint = _newCoordinate.ToVector3UnitSphere();
        _worldPositionCoord = _coordTransformator.transform.position + spherePoint * _coordTransformator.SphereScale;
        _mapMarkerSphereTransform.position = _worldPositionCoord;
        _canDisplayMarker = true;
    }

    private void DisplayMarker(Vector3 position)
    {
        _mapMarker.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    private Vector2 ToGPSCoords(Vector3 position, float sphereRadius)
    {
        float lat = Mathf.Acos(position.y / sphereRadius);
        float lon = Mathf.Atan2(position.x, position.z);
        lat *= Mathf.Rad2Deg;
        lon *= Mathf.Rad2Deg;
        return new Vector2(lat, lon);
    }

    public void SetCanConfirm(bool value)
    {
        _canChooseNewLocation = value;
    }
}
