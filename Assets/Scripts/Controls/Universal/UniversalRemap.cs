using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UniversalRemap : MonoBehaviour
{
    private UniversalRebinds rebinds = new();
    private SettingsScriptableObject settings;

    private const string BUTTONS_TYPE = "ButtonsType";

    private const string FIRE = "Fire";
    private const string CROUCH = "Crouch";
    private const string USE = "Use";
    private const string JUMP = "Jump";
    private const string ADS = "Ads";

    private UnityEvent<IRebindable> GiveData = new();

    [SerializeField] private List<Text> fields = new();
    [SerializeField] private Transform[] slidersHandlers;
    [SerializeField] private Dropdown drop;
    [SerializeField] private Image[] gamepadImages;
    [SerializeField] private Text[] MnKTextFiedls;

    public void SetUpSettings(SettingsScriptableObject sett)
    {
        settings = sett;
        //rebinds = (UniversalRebinds)settings.rebinds;

        List<SettingsSlider> sliders = new();

        foreach (var handler in slidersHandlers)
        {
            sliders.AddRange(handler.GetComponentsInChildren<SettingsSlider>());
        }

        foreach (var slider in sliders)
        {
            //GiveData.AddListener(slider.TakeData);
            slider.SettingsChange.AddListener(ChangeSetting);
        }

        SetUpMnKTextFields();

        drop.value = settings.ButtonsType;
        
        GiveData.Invoke(rebinds);
    }

    public List<Text> GetFields()
    {
        return fields;
    }

    public void Remap(int actionNumber)
    {
        switch (actionNumber)
        {
            case 1:
                RemapLogic(settings.controlls.Universal.GpdCrouch, settings.controlls.UIControl.GpdCrouch, gamepadImages[0], CROUCH);
                break;
            case 2:
                RemapLogic(settings.controlls.Universal.GpdJump, settings.controlls.UIControl.GpdJump, gamepadImages[1], JUMP);
                break;
            case 3:
                RemapLogic(settings.controlls.Universal.GpdFire, settings.controlls.UIControl.GpdFire, gamepadImages[2], FIRE);
                break;
            case 4:
                RemapLogic(settings.controlls.Universal.GpdAds, settings.controlls.UIControl.GpdAds, gamepadImages[3], ADS);
                break;
            case 5:
                RemapLogic(settings.controlls.Universal.GpdUse, settings.controlls.UIControl.GpdUse, gamepadImages[4], USE);
                break;
            case 6:
                RemapLogic(settings.controlls.Universal.GpdCrouch, settings.controlls.UIControl.MnKCrouch, MnKTextFiedls[0], CROUCH);
                break;
            case 7:
                RemapLogic(settings.controlls.Universal.GpdJump, settings.controlls.UIControl.MnKJump, MnKTextFiedls[1], JUMP);
                break;
            case 8:
                RemapLogic(settings.controlls.Universal.GpdFire, settings.controlls.UIControl.MnKFire, MnKTextFiedls[2], FIRE);
                break;
            case 9:
                RemapLogic(settings.controlls.Universal.GpdAds, settings.controlls.UIControl.MnKAds, MnKTextFiedls[3], ADS);
                break;
            case 10:
                RemapLogic(settings.controlls.Universal.GpdUse, settings.controlls.UIControl.MnKUse, MnKTextFiedls[4], USE);
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
        rebinds.GpdClassic[BUTTONS_TYPE] = type;

        gamepadImages[0].sprite = settings.SetImg(rebinds.GpdPetNames[CROUCH]);
        gamepadImages[1].sprite = settings.SetImg(rebinds.GpdPetNames[JUMP]);
        gamepadImages[2].sprite = settings.SetImg(rebinds.GpdPetNames[FIRE]);
        gamepadImages[3].sprite = settings.SetImg(rebinds.GpdPetNames[ADS]);
        gamepadImages[4].sprite = settings.SetImg(rebinds.GpdPetNames[USE]);
    }
    private void SetUpMnKTextFields()
    {
        MnKTextFiedls[0].text = rebinds.MnKPetNames[CROUCH];
        MnKTextFiedls[1].text = rebinds.MnKPetNames[JUMP];
        MnKTextFiedls[2].text = rebinds.MnKPetNames[FIRE];
        MnKTextFiedls[3].text = rebinds.MnKPetNames[ADS];
        MnKTextFiedls[4].text = rebinds.MnKPetNames[USE];
    }

    private void ChangeSetting(string name, float value, bool expanded)
    {
        if (expanded)
            rebinds.MnKClassic[name] = value;
        else
            rebinds.GpdClassic[name] = value;
    }
}
