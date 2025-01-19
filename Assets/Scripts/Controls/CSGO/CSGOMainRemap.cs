using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CSGOMainRemap : MonoBehaviour
{
    private Controlls controlls;
    private UserSettingsRebindsData data = new();
    private CSRebinds rebinds = new();

    public UnityEvent<IRebindable> GiveData = new();

    [SerializeField] private LoadSettingsDataMainMenu loadData;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Transform[] slidersHandlers;
    [SerializeField] private Text[] MnKTextFiedls;

    private void Awake()
    {
        List<SettingsSlider> sliders = new();

        foreach (var handler in slidersHandlers)
        {
            sliders.AddRange(handler.GetComponentsInChildren<SettingsSlider>());
        }

        controlls = new Controlls();

        data = loadData.GetData();

        foreach (var slider in sliders)
        {
            //GiveData.AddListener(slider.TakeData);
            slider.SettingsChange.AddListener(ChangeSetting);
        }

        if (!settings.SettingsGained)
        {
            //if (data.CSRebinds.TryGetValue(AllSettingsKeys.CSGO_REBINDS, out CSRebinds rebinds))
                //this.rebinds.UploadRebinds(rebinds);

            //settings.rebinds = this.rebinds;
            settings.controlls = controlls;

            settings.SettingsGained = true;
        }
        else
        {
           // this.rebinds = (CSRebinds)settings.rebinds;
            controlls = settings.controlls;
        }

        SetUpSettings();

        settings.SetUpButtons();
        SetUpMnKTextFields();
    }

    private void OnEnable()
    {
        if (rebinds != null && !settings.SettingsGained)
        {
            //settings.rebinds = rebinds;
            settings.controlls = controlls;
            settings.SettingsGained = true;
        }
    }

    private void SetUpSettings()
    {
        //gamepad and mouse/keybord settaping
        if (rebinds.Actions.TryGetValue(AllSettingsKeys.CROUCH, out string value))
        {
            controlls.CSGOControl.MnKCrouch.LoadBindingOverridesFromJson(value);
            if (controlls.CSGOControl.MnKCrouch.bindings[0].overridePath != null)
                controlls.UIControl.MnKCrouch.ApplyBindingOverride(0, controlls.CSGOControl.MnKCrouch.bindings[0].overridePath);

        }

        if (rebinds.Actions.TryGetValue(AllSettingsKeys.JUMP, out string value1))
        {
            controlls.CSGOControl.MnKJump.LoadBindingOverridesFromJson(value1);
            if (controlls.CSGOControl.MnKJump.bindings[0].overridePath != null)
                controlls.UIControl.MnKJump.ApplyBindingOverride(0, controlls.CSGOControl.MnKJump.bindings[0].overridePath);

        }

        if (rebinds.Actions.TryGetValue(AllSettingsKeys.FIRE, out string value2))
        {
            controlls.CSGOControl.MnKFire.LoadBindingOverridesFromJson(value2);
            if (controlls.CSGOControl.MnKFire.bindings[0].overridePath != null)
                controlls.UIControl.MnKFire.ApplyBindingOverride(0, controlls.CSGOControl.MnKFire.bindings[0].overridePath);
        }

        if (rebinds.Actions.TryGetValue(AllSettingsKeys.ADS, out string value3))
        {
            controlls.CSGOControl.MnKAds.LoadBindingOverridesFromJson(value3);
            if (controlls.CSGOControl.MnKAds.bindings[0].overridePath != null)
                controlls.UIControl.MnKAds.ApplyBindingOverride(0, controlls.CSGOControl.MnKAds.bindings[0].overridePath);
        }

        if (rebinds.Actions.TryGetValue(AllSettingsKeys.USE, out string value4))
        {
            controlls.CSGOControl.MnKUse.LoadBindingOverridesFromJson(value4);
            if (controlls.CSGOControl.MnKUse.bindings[0].overridePath != null)
                controlls.UIControl.MnKUse.ApplyBindingOverride(0, controlls.CSGOControl.MnKUse.bindings[0].overridePath);
        }
    }

    private void Start()
    {
        GiveData.Invoke(rebinds);
    }

    public void Remap(int actionNumber)
    {
        switch (actionNumber)
        {
            case 1:
                RemapLogic(controlls.CSGOControl.MnKCrouch, controlls.UIControl.MnKCrouch, MnKTextFiedls[0], AllSettingsKeys.CROUCH);
                break;
            case 2:
                RemapLogic(controlls.CSGOControl.MnKJump, controlls.UIControl.MnKJump, MnKTextFiedls[1], AllSettingsKeys.JUMP);
                break;
            case 3:
                RemapLogic(controlls.CSGOControl.MnKFire, controlls.UIControl.MnKFire, MnKTextFiedls[2], AllSettingsKeys.FIRE);
                break;
            case 4:
                RemapLogic(controlls.CSGOControl.MnKAds, controlls.UIControl.MnKAds, MnKTextFiedls[3], AllSettingsKeys.ADS);
                break;
            case 5:
                RemapLogic(controlls.CSGOControl.MnKUse, controlls.UIControl.MnKUse, MnKTextFiedls[4], AllSettingsKeys.USE);
                break;
        }
    }

    private void RemapLogic(InputAction action, InputAction uiAction, Text visualName, string actionName)
    {
        action.Disable();
        var rebind = action.PerformInteractiveRebinding(0);
        visualName.text = "";


        rebind.OnComplete(
        operation =>
        {
            operation.Dispose();
            rebinds.Actions[actionName] = action.SaveBindingOverridesAsJson();
            rebinds.PetNames[actionName] = visualName.text = action.GetBindingDisplayString(0);

            uiAction.ApplyBindingOverride(0, action.bindings[0].overridePath);
        });
        rebind.Start();

        action.Enable();
    }

    private void SetUpMnKTextFields()
    {
        MnKTextFiedls[0].text = rebinds.PetNames[AllSettingsKeys.CROUCH];
        MnKTextFiedls[1].text = rebinds.PetNames[AllSettingsKeys.JUMP];
        MnKTextFiedls[2].text = rebinds.PetNames[AllSettingsKeys.FIRE];
        MnKTextFiedls[3].text = rebinds.PetNames[AllSettingsKeys.ADS];
        MnKTextFiedls[4].text = rebinds.PetNames[AllSettingsKeys.USE];
    }

    public void SaveRebinds()
    {
        settings.controlls = controlls;
       // settings.rebinds = rebinds;

       // loadData.Save(rebinds, AllSettingsKeys.CSGO_REBINDS);
    }

    private void ChangeSetting(string name, float value, bool expanded)
    {
        rebinds.Classic[name] = value;
    }
}
