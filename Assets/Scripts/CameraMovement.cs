using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _zoomSpeed;

    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;

    [SerializeField] private float _farthestPoint;
    [SerializeField] private float _nearestPoint;

    private Camera _camera;
    private float _xRot = 0;
    private float _yRot = 0;
    private float _cameraPos;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _cameraPos = _farthestPoint;
    }

    void Update()
    {
        _xRot -= Input.GetAxis("Vertical") * Time.deltaTime * _speed;
        _xRot = Mathf.Clamp(_xRot, _minX, _maxX);
        _yRot -= Input.GetAxis("Horizontal") * Time.deltaTime * _speed;
        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

        _cameraPos -= _zoomSpeed * Input.mouseScrollDelta.y;
        _cameraPos = Mathf.Clamp(_cameraPos, _nearestPoint, _farthestPoint);
        _camera.transform.localPosition = Vector3.forward * _cameraPos;
    }
}
