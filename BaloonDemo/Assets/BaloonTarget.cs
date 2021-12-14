using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using xpbdUnity;

[RequireComponent(typeof(XPBDBody))]
public class BaloonTarget : MonoBehaviour
{
    [SerializeField] private OverallSettings _settings;
    [SerializeField] private XPBDBody _baloonPrefab;
    [SerializeField] private float _ropeLength;
    [SerializeField] private float _baloonSpawnShift = 1f;

    private Transform _transform;
    private XPBDBody _thisBody;

    void Start()
    {
        _transform = transform;
        _thisBody = GetComponent<XPBDBody>();
    }
    
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var localRay = new Ray(_transform.InverseTransformPoint(ray.origin),
            _transform.InverseTransformDirection(ray.direction));
        
        var size = _thisBody.Collider.AABBSize;
        var bounds = new Bounds(Vector3.zero, size);

        if (!bounds.IntersectRay(localRay, out var distance))
            return;

        var localPoint = localRay.GetPoint(distance);
        var baloonPos = ray.GetPoint(distance - _baloonSpawnShift);
        
        var newInstance = Instantiate(_baloonPrefab);
        newInstance.transform.position = baloonPos;

        var joint = newInstance.GetComponent<XPBDJoint>();
        joint.TargetBody = _thisBody;
        joint.Parameters.localPos1 = localPoint;
        joint.Parameters.distance = _ropeLength;

        var baloon = newInstance.GetComponent<Baloon>();
        baloon.Settings = _settings;
    }
}
