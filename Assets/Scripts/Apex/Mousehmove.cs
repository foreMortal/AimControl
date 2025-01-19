using UnityEngine;
using UnityEngine.InputSystem;

public class Mousehmove : MonoBehaviour
{
    private  float xRot;
    private  float yRot;
    public Camera player;
    private Controlls controlls;

    [SerializeField] private Transform _target;
    [SerializeField] private float _angle, _aimAssist;
    private Vector3 _targetDir, _forward;

    //float sensativity = 5f;
    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }

    private void Awake()
    {
        controlls = new Controlls();
    }

    private void MouseMove()
    {
        Assist();
        if (_angle < 5f && _angle > 0)
        {
            xRot += controlls.GamepadControl.Newaction.ReadValue<float>() * 0.5f - _aimAssist;
        }
        else if (_angle > -5f && _angle < 0)
        {
            xRot += controlls.GamepadControl.Newaction.ReadValue<float>() * 0.5f + _aimAssist;
        }
        else 
        {
            xRot += controlls.GamepadControl.Newaction.ReadValue<float>() * 0.5f;
        }
        
        yRot += Input.GetAxis("Mouse Y");
        yRot = Mathf.Clamp(yRot, -90, 90);

        player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }

    private void Update()
    {
        MouseMove();
    }
    private void Assist()
    {
        _targetDir = _target.position - transform.position;
        _forward = transform.forward;
        _angle = Vector3.SignedAngle(_targetDir, _forward, Vector3.up);
    }
}
