using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UniversalMainRemap : MonoBehaviour
{

    private Controlls controlls;
    private UserSettingsRebindsData data = new();
    private UniversalRebinds rebinds = new();

    public UnityEvent<IRebindable> GiveData = new();

    [SerializeField] private LoadSettingsDataMainMenu loadData;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Transform[] slidersHandlers;
    [SerializeField] private Dropdown drop;
    [SerializeField] private Image[] gamepadImages;
    [SerializeField] private Text[] MnKTextFiedls;

    private void Awake()
    {
        List<SettingsSlider> sliders = new();

        foreach(var handler in slidersHandlers)
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
            //if (data.UniversalRebinds.TryGetValue(AllSettingsKeys.UNIVERSAL_REBINDS, out UniversalRebinds rebinds))
               // this.rebinds.UploadRebinds(rebinds);

           // settings.rebinds = this.rebinds;
            settings.controlls = controlls;

            settings.SettingsGained = true;
        }
        else
        {
            //this.rebinds = (UniversalRebinds)settings.rebinds;
            controlls = settings.controlls;
        }

        SetUpSettings();

        settings.SetUpButtons();
        SetUpMnKTextFields();
    }

    private void OnEnable()
    {
        if(rebinds != null && !settings.SettingsGained)
        {
            //settings.rebinds = rebinds;
            settings.controlls = controlls;
            settings.SettingsGained = true;
        }
    }

    private void SetUpSettings()
    {
        //gamepad and mouse/keybord settaping
        if (rebinds.GpdActions.TryGetValue(AllSettingsKeys.CROUCH, out string value))
        {
            controlls.Universal.GpdCrouch.LoadBindingOverridesFromJson(value);
            if(controlls.Universal.GpdCrouch.bindings[0].overridePath != null)
                controlls.UIControl.GpdCrouch.ApplyBindingOverride(0, controlls.Universal.GpdCrouch.bindings[0].overridePath);
            if(controlls.Universal.GpdCrouch.bindings[1].overridePath != null)
                controlls.UIControl.MnKCrouch.ApplyBindingOverride(0, controlls.Universal.GpdCrouch.bindings[1].overridePath);
        }

        if (rebinds.GpdActions.TryGetValue(AllSettingsKeys.JUMP, out string value1))
        {
            controlls.Universal.GpdJump.LoadBindingOverridesFromJson(value1);
            if (controlls.Universal.GpdJump.bindings[0].overridePath != null)
                controlls.UIControl.GpdJump.ApplyBindingOverride(0, controlls.Universal.GpdJump.bindings[0].overridePath);
            if (controlls.Universal.GpdJump.bindings[1].overridePath != null)
                controlls.UIControl.MnKJump.ApplyBindingOverride(0, controlls.Universal.GpdJump.bindings[1].overridePath);
        }

        if (rebinds.GpdActions.TryGetValue(AllSettingsKeys.FIRE, out string value2))
        {
            controlls.Universal.GpdFire.LoadBindingOverridesFromJson(value2);
            if (controlls.Universal.GpdFire.bindings[0].overridePath != null)
                controlls.UIControl.GpdFire.ApplyBindingOverride(0, controlls.Universal.GpdFire.bindings[0].overridePath);
            if (controlls.Universal.GpdFire.bindings[1].overridePath != null)
                controlls.UIControl.MnKFire.ApplyBindingOverride(0, controlls.Universal.GpdFire.bindings[1].overridePath);
        }

        if (rebinds.GpdActions.TryGetValue(AllSettingsKeys.ADS, out string value3))
        {
            controlls.Universal.GpdAds.LoadBindingOverridesFromJson(value3);
            if (controlls.Universal.GpdAds.bindings[0].overridePath != null)
                controlls.UIControl.GpdAds.ApplyBindingOverride(0, controlls.Universal.GpdAds.bindings[0].overridePath);
            if (controlls.Universal.GpdAds.bindings[1].overridePath != null)
                controlls.UIControl.MnKAds.ApplyBindingOverride(0, controlls.Universal.GpdAds.bindings[1].overridePath);
        }

        if (rebinds.GpdActions.TryGetValue(AllSettingsKeys.USE, out string value4))
        {
            controlls.Universal.GpdUse.LoadBindingOverridesFromJson(value4);
            if (controlls.Universal.GpdUse.bindings[0].overridePath != null)
                controlls.UIControl.GpdUse.ApplyBindingOverride(0, controlls.Universal.GpdUse.bindings[0].overridePath);
            if (controlls.Universal.GpdUse.bindings[1].overridePath != null)
                controlls.UIControl.MnKUse.ApplyBindingOverride(0, controlls.Universal.GpdUse.bindings[1].overridePath);
        }
    }

    private void Start()
    {
        drop.value = settings.ButtonsType;
        GiveData.Invoke(rebinds);
    }

    public void Remap(int actionNumber)
    {
        switch (actionNumber)
        {
            case 1:
                RemapLogic(controlls.Universal.GpdCrouch, controlls.UIControl.GpdCrouch, gamepadImages[0], AllSettingsKeys.CROUCH);
                break;
            case 2:
                RemapLogic(controlls.Universal.GpdJump, controlls.UIControl.GpdJump, gamepadImages[1], AllSettingsKeys.JUMP);
                break;
            case 3:
                RemapLogic(controlls.Universal.GpdFire, controlls.UIControl.GpdFire, gamepadImages[2], AllSettingsKeys.FIRE);
                break;
            case 4:
                RemapLogic(controlls.Universal.GpdAds, controlls.UIControl.GpdAds, gamepadImages[3], AllSettingsKeys.ADS);
                break;
            case 5:
                RemapLogic(controlls.Universal.GpdUse, controlls.UIControl.GpdUse, gamepadImages[4], AllSettingsKeys.USE);
                break;
            case 6:
                RemapLogic(controlls.Universal.GpdCrouch, controlls.UIControl.MnKCrouch, MnKTextFiedls[0], AllSettingsKeys.CROUCH);
                break;
            case 7:
                RemapLogic(controlls.Universal.GpdJump, controlls.UIControl.MnKJump, MnKTextFiedls[1], AllSettingsKeys.JUMP);
                break;
            case 8:
                RemapLogic(controlls.Universal.GpdFire, controlls.UIControl.MnKFire, MnKTextFiedls[2], AllSettingsKeys.FIRE);
                break;
            case 9:
                RemapLogic(controlls.Universal.GpdAds, controlls.UIControl.MnKAds, MnKTextFiedls[3], AllSettingsKeys.ADS);
                break;
            case 10:
                RemapLogic(controlls.Universal.GpdUse, controlls.UIControl.MnKUse, MnKTextFiedls[4], AllSettingsKeys.USE);
                break;
        }
    }

    private void RemapLogic(InputAction action, InputAction uiAction, Image img, string actionName)
    {
        action.Disable();
        var rebind = action.PerformInteractiveRebinding(0);
        img.enabled = false;


        rebind.OnComplete(
        operation =>
        {
            operation.Dispose();
            var overrideData = action.SaveBindingOverridesAsJson();
            rebinds.GpdActions[actionName] = overrideData;
            rebinds.GpdPetNames[actionName] = action.bindings[0].overridePath;

            uiAction.ApplyBindingOverride(0, action.bindings[0].overridePath);

            img.sprite = settings.SetImg(action.bindings[0].overridePath);
            img.enabled = true;
        });
        rebind.Start();

        action.Enable();
    }

    private void RemapLogic(InputAction action, InputAction uiAction, Text visualName, string actionName)
    {
        action.Disable();
        var rebind = action.PerformInteractiveRebinding(1);
        visualName.text = "";


        rebind.OnComplete(
        operation =>
        {
            operation.Dispose();
            rebinds.GpdActions[actionName] = action.SaveBindingOverridesAsJson();
            rebinds.MnKPetNames[actionName] = visualName.text = action.GetBindingDisplayString(1);

            uiAction.ApplyBindingOverride(0, action.bindings[1].overridePath);
        });
        rebind.Start();

        action.Enable();
    }

    public void ChangeButtonsType(int type)
    {
        settings.ButtonsType = type;

        gamepadImages[0].sprite = settings.SetImg(rebinds.GpdPetNames[AllSettingsKeys.CROUCH]);
        gamepadImages[1].sprite = settings.SetImg(rebinds.GpdPetNames[AllSettingsKeys.JUMP]);
        gamepadImages[2].sprite = settings.SetImg(rebinds.GpdPetNames[AllSettingsKeys.FIRE]);
        gamepadImages[3].sprite = settings.SetImg(rebinds.GpdPetNames[AllSettingsKeys.ADS]);
        gamepadImages[4].sprite = settings.SetImg(rebinds.GpdPetNames[AllSettingsKeys.USE]);
    }
    private void SetUpMnKTextFields()
    {
        MnKTextFiedls[0].text = rebinds.MnKPetNames[AllSettingsKeys.CROUCH];
        MnKTextFiedls[1].text = rebinds.MnKPetNames[AllSettingsKeys.JUMP];
        MnKTextFiedls[2].text = rebinds.MnKPetNames[AllSettingsKeys.FIRE];
        MnKTextFiedls[3].text = rebinds.MnKPetNames[AllSettingsKeys.ADS];
        MnKTextFiedls[4].text = rebinds.MnKPetNames[AllSettingsKeys.USE];
    }

    public void SaveRebinds()
    {
        settings.controlls = controlls;
        //settings.rebinds = rebinds;

       // loadData.Save(rebinds, AllSettingsKeys.UNIVERSAL_REBINDS);
    }

    private void ChangeSetting(string name, float value, bool expanded)
    {
        if (expanded)
            rebinds.MnKClassic[name] = value;
        else
            rebinds.GpdClassic[name] = value;
    }
}
