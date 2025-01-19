using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserSettings : MonoBehaviour
{
    public bool crouchButton = false, jumpButton = false, fireButton = false, AdsButton = false;
    private Controlls controlls;

    private void OnEnable()
    {
        //controlls.UserSettings.Enable();
    }
    private void OnDisable()
    {
        //controlls.UserSettings.Disable();
    }

    private void Awake()
    {
        controlls = new Controlls();
        /*controlls.UserSettings.ButtonEast.performed += ctx => BindButtonEast();
        controlls.UserSettings.ButtonWest.performed += ctx => BindButtonWest();
        controlls.UserSettings.ButtonSouth.performed += ctx => BindButtonSouth();
        controlls.UserSettings.ButtonNorth.performed += ctx => BindButtonNorth();
        controlls.UserSettings.Right.performed += ctx => BindRight();
        controlls.UserSettings.Left.performed += ctx => BindLeft();
        controlls.UserSettings.Up.performed += ctx => BindUp();
        controlls.UserSettings.Down.performed += ctx => BindDown();
        controlls.UserSettings.LeftStick.performed += ctx => BindLeftStick();
        controlls.UserSettings.RightStick.performed += ctx => BindRightStick();
        controlls.UserSettings.L1.performed += ctx => BindL1();
        controlls.UserSettings.L2.performed += ctx => BindL2();
        controlls.UserSettings.R1.performed += ctx => BindR1();
        controlls.UserSettings.R2.performed += ctx => BindR2();*/
    }
    //<Gamepad>/buttonEast
    private void BindButtonEast()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/buttonEast");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/buttonEast");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/buttonEast");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/buttonEast");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindButtonWest()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/buttonWest");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/buttonWest");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/buttonWest");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/buttonWest");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindButtonSouth()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/buttonSouth");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/buttonSouth");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/buttonSouth");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/buttonSouth");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindButtonNorth()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/buttonNorth");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/buttonNorth");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/buttonNorth");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/buttonNorth");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindRight()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/dpad/right");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/dpad/right");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/dpad/right");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/dpad/right");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindLeft()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/dpad/left");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/dpad/left");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/dpad/left");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/dpad/left");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }
    //<Gamepad>/dpad/up
    private void BindUp()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/dpad/up");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/dpad/up");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/dpad/up");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/dpad/up");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindDown()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/dpad/down");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/dpad/down");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/dpad/down");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/dpad/down");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindLeftStick()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/leftStickPress");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/leftStickPress");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/leftStickPress");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/leftStickPress");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }
    
    private void BindRightStick()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/rightStickPress");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/rightStickPress");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/rightStickPress");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/rightStickPress");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindL1()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/leftShoulder");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/leftShoulder");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/leftShoulder");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/leftShoulder");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindL2()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/leftTrigger");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/leftTrigger");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/leftTrigger");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/leftTrigger");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindR1()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/rightShoulder");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/rightShoulder");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/rightShoulder");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/rightShoulder");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }

    private void BindR2()
    {
        if (crouchButton)
        {
            PlayerPrefs.SetString("crouchButton", "<Gamepad>/rightTrigger");
        }
        else if (jumpButton)
        {
            PlayerPrefs.SetString("jumpButton", "<Gamepad>/rightTrigger");
        }
        else if (fireButton)
        {
            PlayerPrefs.SetString("fireButton", "<Gamepad>/rightTrigger");
        }
        else if (AdsButton)
        {
            PlayerPrefs.SetString("AdsButton", "<Gamepad>/rightTrigger");
        }
        crouchButton = false; jumpButton = false; fireButton = false; AdsButton = false;
    }
}
