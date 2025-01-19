using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private GameObject handler;
    [SerializeField] private float senseMouse = 105f, gamepadSense = 30;
    private Slider slider;
    private Controlls controlls;

    private void Awake()
    {
        slider = GetComponent<Slider>();
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

    public void ChangePos(float value)
    {
        handler.transform.localPosition = new(0f, value, 0f);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            slider.value -= senseMouse * Input.mouseScrollDelta.y;
        }
        if(controlls.GamepadControl.MoveVer.ReadValue<float>() != 0f)
        {
            slider.value -= gamepadSense * controlls.GamepadControl.MoveVer.ReadValue<float>();
        }
    }
}
