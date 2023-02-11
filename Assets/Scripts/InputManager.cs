using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Transform _mapMarkerSphereTransform;

    [Header("UI Settings")]
    [SerializeField] private Image _mapMarker;
    [SerializeField] private TMP_Text _coordText;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private GameObject _missionCompletionPanel;
    [SerializeField] private Slider _pointsSlider;
    [SerializeField] private TMP_Text _pointsText;

    [Header("Score")]
    [SerializeField] private float _missionMaxPoints;
    [SerializeField] private float _missionMaxDistance;

    private float _guessDistance;
    private float _totalScore;
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
        _pointsSlider.maxValue = _missionMaxPoints;
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

    public void CompleteMission()
    {
        _canDisplayMarker = false;
        _mapMarker.gameObject.SetActive(false);
        _missionCompletionPanel.SetActive(true);
        _guessDistance = GetDistanceBetweenTwoPoints(_coordLatLong, _tempMissionCoords);
        _distanceText.text = $"Your guess was { _guessDistance} km away from location";
        _coordText.text = $"Selected location coordinates ({_coordLatLong.x}, {_coordLatLong.y})";           

        float calculatedScore = CalculatePointsBasedOnDistance();
        _pointsSlider.value = calculatedScore;
        _pointsText.text = $"{calculatedScore} POINTS";
        _totalScore += calculatedScore;
    }

    private float GetDistanceBetweenTwoPoints(Vector2 clickedLatLong, Vector2 actualLocationLatLong)
    {
        float lat1InRads = clickedLatLong.x * Mathf.PI / 180;
        float lat2InRads = actualLocationLatLong.x * Mathf.PI / 180;
        float latDiffInRads = (actualLocationLatLong.x - clickedLatLong.x) * Mathf.PI / 180;
        float longDiffInRads = (actualLocationLatLong.y - clickedLatLong.y) * Mathf.PI / 180;
        float a = Mathf.Sin(latDiffInRads / 2) * Mathf.Sin(latDiffInRads / 2) + Mathf.Cos(lat1InRads) * Mathf.Cos(lat2InRads) *
          Mathf.Sin(longDiffInRads / 2) * Mathf.Sin(longDiffInRads / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float distance = 6371000 * c;
        return distance / 1000;
    }

    private float CalculatePointsBasedOnDistance()
    {
        if (_guessDistance > _missionMaxDistance)
            return 0;
        return _missionMaxPoints - _guessDistance * _missionMaxPoints / _missionMaxDistance;
    }
}
