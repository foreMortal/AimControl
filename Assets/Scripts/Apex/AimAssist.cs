using Unity.VisualScripting;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    [SerializeField] private Camera player;
    [SerializeField] private Transform _target;
    [SerializeField] private float _angle, _aimAssist;
    private Vector3 _targetDir, _forward;
    [SerializeField] Mousehmove _mousemove;
    [SerializeField] private float xRot;
    [SerializeField] private float yRot;

    private void Update()
    {
        
    }
    public void AimHelper(float _xRot, float _yRot)
    {
        _targetDir = _target.position - transform.position;
        _forward = transform.forward;
        _angle = Vector3.SignedAngle(_targetDir, _forward, Vector3.up);
        if (_angle <= 5f && _angle > 0)
        {
            xRot -= _aimAssist;
            player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
        }
        else if (_angle >= -5f && _angle < 0)
        {
            xRot += _aimAssist;
            player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
        }
    }
}
