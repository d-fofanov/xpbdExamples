using DefaultNamespace;
using UnityEngine;
using xpbdUnity;

[RequireComponent(typeof(XPBDBody))]
public class Baloon : MonoBehaviour
{
    [SerializeField] private OverallSettings _settings;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private XPBDJoint _rope;
    
    private XPBDBody _body;

    public OverallSettings Settings
    {
        get => _settings;
        set => _settings = value;
    }

    void Start()
    {
        _body = GetComponent<XPBDBody>();
    }

    void Update()
    {
        if (_lineRenderer == null || _rope == null)
            return;

        _lineRenderer.SetPosition(0, _rope.GlobalPos0);
        _lineRenderer.SetPosition(1, _rope.GlobalPos1);
    }

    void FixedUpdate()
    {
        if (_body == null || _body.Collider == null)
            return;

        var bouyancy = _body.Collider.Volume * _settings.AirDensity * _settings.Gravity;
        _body.AddForce(Vector3.up * bouyancy);
    }
}
