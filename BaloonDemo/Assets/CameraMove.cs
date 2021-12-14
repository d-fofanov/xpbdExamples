using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _rotationDistance = 10f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _panSpeedMouse = 0.03f;
    [SerializeField] private float _panSpeedKeys = 0.1f;
    [SerializeField] private float _scrollSpeed = 0.4f;
    [SerializeField] private float _minDistance = 3f;
    [SerializeField] private float _maxDistance = 100f;
    
    private Vector3 _lastMousePos;
    
    void Update()
    {
        var t = transform;
        
        var mouseDx = Input.mousePosition.x - _lastMousePos.x;
        var mouseDy = Input.mousePosition.y - _lastMousePos.y;
        var scroll = Input.mouseScrollDelta;
        var rightMouse = Input.GetMouseButton(1);
        var midMouse = Input.GetMouseButton(2);

        if (rightMouse && !midMouse)
        {
            var center = t.position + t.forward * _rotationDistance;

            t.RotateAround(center, Vector3.up, mouseDx * _rotationSpeed);
            t.RotateAround(center, t.right, -mouseDy * _rotationSpeed);
            t.position += _panSpeedKeys * (
                Input.GetAxis("Vertical") * t.forward +
                Input.GetAxis("Horizontal") * t.right + 
                Input.GetAxis("Orthogonal") * Vector3.up
            );
        }

        if (!rightMouse && midMouse)
        {
            t.position -= _panSpeedMouse * (t.up * mouseDy + t.right * mouseDx);
        }

        var prevDistance = _rotationDistance;
        _rotationDistance = Mathf.Clamp(_rotationDistance - scroll.y * _scrollSpeed, _minDistance, _maxDistance);
        t.position += -t.forward * (_rotationDistance - prevDistance);
        
        _lastMousePos = Input.mousePosition;
    }
}
