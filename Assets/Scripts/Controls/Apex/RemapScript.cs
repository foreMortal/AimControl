using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RemapScript : MonoBehaviour
{
    [SerializeField] private GameObject[] sliderHandlers;
    [SerializeField] private List<Text> fields;

    private SettingsScriptableObject settings;
    private UserRebinds rebinds;

    private UnityEvent<UserRebinds> GiveData = new();

    [SerializeField] private Image crouch1, jump1, fire1, ads1, use1, swap1;
    [SerializeField] private Dropdown drop;

    public void SetUpSettings(SettingsScriptableObject sett)
    {
        settings = sett;
        rebinds = settings.rebinds;

        drop.value = settings.ButtonsType;

        SetSliders();

        GiveData.Invoke(settings.rebinds);
    }

    public List<Text> GetFields()
    {
        return fields;
    }

    private void SetSliders()
    {
        List<SettingsSlider> sliders = new();
        foreach (var handler in sliderHandlers)
        {
            sliders.AddRange(handler.GetComponentsInChildren<SettingsSlider>());
        }

        foreach (var slider in sliders)
        {
            GiveData.AddListener(slider.TakeData);
            slider.SettingsChange.AddListener(ChangeSetting);
        }
    }

    public void Remap(int actionNumber)
    {
        switch (actionNumber)
        {
            case 1:
                RemapLogic(settings.controlls.GamepadControl.CrouchOnPressed, crouch1, AllSettingsKeys.CROUCH);
                break;
            case 2:
                RemapLogic(settings.controlls.GamepadControl.Jump, jump1, AllSettingsKeys.JUMP);
                break;
            case 3:
                RemapLogic(settings.controlls.GamepadControl.Fire, fire1, AllSettingsKeys.FIRE);
                break;
            case 4:
                RemapLogic(settings.controlls.GamepadControl.Ads, ads1, AllSettingsKeys.ADS);
                break;
            case 5:
                RemapLogic(settings.controlls.GamepadControl.Use, use1, AllSettingsKeys.USE);
                break;
            case 6:
                RemapLogic(settings.controlls.GamepadControl.SwapGun, swap1, AllSettingsKeys.SWAP_GUN);
                break;
        }
    }

    private void RemapLogic(InputAction action, Image img, string actionName)
    {
        action.Disable();
        var rebind = action.PerformInteractiveRebinding(0);
        img.enabled = false;

        rebind.OnComplete(
        operation =>
        {
            operation.Dispose();
            var overrideData = action.SaveBindingOverridesAsJson();
            rebinds.Data[actionName] = overrideData;
            rebinds.Data[actionName + AllSettingsKeys.PET] = action.bindings[0].overridePath;
            img.sprite = settings.SetImg(action.bindings[0].overridePath);
            img.enabled = true;
        });
        rebind.Start();

        action.Enable();
    }

    public void ChangeButtonsType(int type)
    {
        settings.ButtonsType = type;

        crouch1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.CROUCH + AllSettingsKeys.PET]);
        jump1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.JUMP + AllSettingsKeys.PET]);
        fire1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.FIRE + AllSettingsKeys.PET]);
        ads1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.ADS + AllSettingsKeys.PET]);
        use1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.USE + AllSettingsKeys.PET]);
        swap1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.SWAP_GUN + AllSettingsKeys.PET]);
    }

    private void ChangeSetting(string name, float value, bool expanded)
    {
        rebinds.Data[name] = value;
    }
}
