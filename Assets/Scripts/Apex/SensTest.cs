using UnityEngine;
using UnityEngine.InputSystem;


public class SensTest : MonoBehaviour
{
    private Controlls controlls;
    float xRot;
    float yRot;
    public Camera player;
    [SerializeField] private float _horizontal, _vertical;

    private void Awake()
    {
        controlls = new Controlls();
    }
    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }

    private void FixedUpdate()
    {
        GamePadMove();
    }
    private void GamePadMove()
    {
        xRot += controlls.GamepadControl.Horizontal.ReadValue<float>() * _horizontal;
        yRot += controlls.GamepadControl.Vertical.ReadValue<float>() * _vertical;

        player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }
}
