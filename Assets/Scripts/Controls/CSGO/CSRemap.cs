using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CSRemap : MonoBehaviour
{
    private CSRebinds rebinds;
    private SettingsScriptableObject settings;

    private UnityEvent<IRebindable> GiveData = new();

    [SerializeField] private List<Text> fields = new();
    [SerializeField] private Transform[] slidersHandlers;
    [SerializeField] private Text[] MnKTextFiedls;

    public void SetUpSettings(SettingsScriptableObject sett)
    {
        settings = sett;
        //rebinds = (CSRebinds)settings.rebinds;

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
                RemapLogic(settings.controlls.CSGOControl.MnKCrouch, settings.controlls.UIControl.MnKCrouch, MnKTextFiedls[0], AllSettingsKeys.CROUCH);
                break;
            case 2:
                RemapLogic(settings.controlls.CSGOControl.MnKJump, settings.controlls.UIControl.MnKJump, MnKTextFiedls[1], AllSettingsKeys.JUMP);
                break;
            case 3:
                RemapLogic(settings.controlls.CSGOControl.MnKFire, settings.controlls.UIControl.MnKFire, MnKTextFiedls[2], AllSettingsKeys.FIRE);
                break;
            case 4:
                RemapLogic(settings.controlls.CSGOControl.MnKAds, settings.controlls.UIControl.MnKAds, MnKTextFiedls[3], AllSettingsKeys.ADS);
                break;
            case 5:
                RemapLogic(settings.controlls.CSGOControl.MnKUse, settings.controlls.UIControl.MnKUse, MnKTextFiedls[4], AllSettingsKeys.USE);
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

    private void ChangeSetting(string name, float value, bool expanded)
    {
        rebinds.Classic[name] = value;
    }
}
