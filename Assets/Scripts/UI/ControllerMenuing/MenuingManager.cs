using System.Collections.Generic;
using UnityEngine;

public class MenuingManager : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Transform[] canvasesHandler;
    [SerializeField] private UIRebindButtons buttonsHintsManager;

    public CanvasManager canvas;

    private Controlls controls;
    public delegate void MenuDelegate(int t = 0);
    private float fastTimer;
    private int DeviceType = -1; //0 - gamepad, 1 - mnk

    public event MenuDelegate show;
    public event MenuDelegate hide;
    public event MenuDelegate arUp;
    public event MenuDelegate arDwn;
    public event MenuDelegate arLft;
    public event MenuDelegate arRgt;
    public event MenuDelegate trngl;
    public event MenuDelegate crcl;
    public event MenuDelegate sqr;
    public event MenuDelegate crss;

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public int ClearEvents()
    {
        arUp = null;
        arDwn = null;
        arLft = null;
        arRgt = null;
        trngl = null;
        crcl = null;
        crss = null;
        sqr = null;

        return DeviceType;
    }

    private void Awake()
    {
        List<ChangeButtonHints> Hints = new();

        controls = new Controlls();
        controls.MenuControl.ArrowDown.performed += ctx => CheckForDeviceType(arDwn);
        controls.MenuControl.ArrowUp.performed += ctx => CheckForDeviceType(arUp);
        controls.MenuControl.ArrowLeft.performed += ctx => CheckForDeviceType(arLft);
        controls.MenuControl.ArrowRight.performed += ctx => CheckForDeviceType(arRgt);
        controls.MenuControl.Triangle.performed += ctx => CheckForDeviceType(trngl);
        controls.MenuControl.Circle.performed += ctx => CheckForDeviceType(crcl);
        controls.MenuControl.Cross.performed += ctx => CheckForDeviceType(crss);
        controls.MenuControl.Square.performed += ctx => CheckForDeviceType(sqr);


        List<CanvasManager> canvases = new List<CanvasManager>();
        foreach(var hand in canvasesHandler)
        {
            canvases.AddRange(hand.GetComponentsInChildren<CanvasManager>(true));
            Hints.AddRange(hand.GetComponentsInChildren<ChangeButtonHints>(true));
        }
        foreach(var can in canvases)
        {
            can.Setup(this);
        }

        buttonsHintsManager.ButtonHints = Hints.ToArray();
        DeviceType = settings.DeviceType;
    }

    private void Start()
    {
        if(DeviceType == 0)
        {
            show?.Invoke();
            Cursor.lockState = CursorLockMode.Locked;
            DeviceType = settings.DeviceType = 0;
            buttonsHintsManager.SetHintsVisible(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            buttonsHintsManager.SetHintsVisible(false);
            hide?.Invoke();
        }
    }

    private void Update()
    {
        if(DeviceType != 1)
        {
            if (controls.MenuControl.Mouse.ReadValue<Vector2>().x >= 30f ||
                controls.MenuControl.Mouse.ReadValue<Vector2>().x <= -30f||
                controls.MenuControl.Mouse.ReadValue<Vector2>().y >= 30f ||
                controls.MenuControl.Mouse.ReadValue<Vector2>().y <= -30f)
            {
                Cursor.lockState = CursorLockMode.None;
                buttonsHintsManager.SetHintsVisible(false);
                hide?.Invoke();
                DeviceType = settings.DeviceType = 1;
            }
        }
        else if(controls.MenuControl.ArrowLeft.ReadValue<float>() > 0 ||
           controls.MenuControl.ArrowRight.ReadValue<float>() > 0 ||
           controls.MenuControl.ArrowUp.ReadValue<float>() > 0 ||
           controls.MenuControl.ArrowDown.ReadValue<float>() > 0)
        {
            fastTimer += Time.deltaTime;
            if(fastTimer > 0.5f)
            {
                if(controls.MenuControl.ArrowRight.ReadValue<float>() > 0)
                    arRgt.Invoke(1);
                if (controls.MenuControl.ArrowLeft.ReadValue<float>() > 0)
                    arLft.Invoke(1);
                if (controls.MenuControl.ArrowUp.ReadValue<float>() > 0)
                    arUp.Invoke(1);
                if (controls.MenuControl.ArrowDown.ReadValue<float>() > 0)
                    arDwn.Invoke(1);
            }
        }
        else if (fastTimer > 0)
        {
            fastTimer = 0;
        }
    }

    private void CheckForDeviceType(MenuDelegate _event)
    {
        if(DeviceType != 0)
        {
            show?.Invoke();
            Cursor.lockState = CursorLockMode.Locked;
            DeviceType = settings.DeviceType = 0;
            buttonsHintsManager.SetHintsVisible(true);
        }
        else
        {
            _event?.Invoke();
        }
    }
}
