using UnityEngine;
using UnityEngine.InputSystem;

public class UniversalDevicesManager : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    private CameraMoveParent[] cameras;
    private Controlls controls;
    private InputAction[] mnkCameraMove, gamepadCameraMove;
    private DeviceType device = DeviceType.MnK;

    private void Awake()
    {
        controls = new Controlls();
        controls = settings.controlls;

        cameras = GetComponents<CameraMoveParent>();

        RebindType type = settings.rebinds.Type;
        switch (type)
        {
            case RebindType.Universal:
                mnkCameraMove = new InputAction[2] { controls.Universal.MnKHorizantal, controls.Universal.MnKVertical };
                gamepadCameraMove  = new InputAction[2] { controls.Universal.GpdHorizantal, controls.Universal.GpdVertical };
                break;
            case RebindType.Apex:
                mnkCameraMove = new InputAction[2] { controls.GamepadControl.MnKHorizantal, controls.GamepadControl.MnKVertical };
                gamepadCameraMove = new InputAction[2] { controls.GamepadControl.Horizontal, controls.GamepadControl.Vertical };
                break;
        }

        foreach (var cam in cameras)
        {
            cam.ChangeDevice(DeviceType.MnK);
            device = DeviceType.Gamepad;
        }
    }

    private void Update()
    {
        if (device == DeviceType.MnK)
        {
            foreach (var move in gamepadCameraMove)
            {
                if (move.ReadValue<float>() != 0f)
                {
                    float xRot = 0f;
                    float yRot = 0f;
                    foreach(var cam in cameras)
                    {
                        float[] rot =  cam.ChangeDevice(DeviceType.Gamepad);
                        device = DeviceType.Gamepad;
                        xRot += rot[0];
                        yRot += rot[1];
                    }
                    foreach (var cam in cameras)
                    {
                        cam.RotatePlayer(xRot, yRot);
                    }
                    return;
                }
            }
        }
        else if (device == DeviceType.Gamepad)
        {
            foreach (var move in mnkCameraMove)
            {
                if (move.ReadValue<float>() != 0f)
                {
                    float xRot = 0f;
                    float yRot = 0f;
                    foreach (var cam in cameras)
                    {
                        float[] rot = cam.ChangeDevice(DeviceType.MnK);
                        device = DeviceType.MnK;
                        xRot += rot[0];
                        yRot += rot[1];
                    }
                    foreach(var cam in cameras)
                    {
                        cam.RotatePlayer(xRot, yRot);
                    }
                    return;
                }
            }
        }
    }
}
