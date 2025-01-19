using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remaper : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    private Controlls controlls;
    private UserRebinds rebinds;
    private MainRemap mainRemap;
    
    public void Initialize(Controlls Controlls, UserRebinds Rebinds)
    {
        controlls = Controlls;
        rebinds = Rebinds;
    }

    public void Remap(ActionType type)
    {
        string actionID;
        Dictionary<string, string[]> Actions = (Dictionary<string, string[]>)rebinds.Data[AllSettingsKeys.ACTIONS];

        switch (type)
        {
            case ActionType.Crouch:
                actionID = Actions[AllSettingsKeys.CROUCH][0];
                break;
            case ActionType.Jump:
                actionID = Actions[AllSettingsKeys.JUMP][0];
                break;
            case ActionType.Fire:
                actionID = Actions[AllSettingsKeys.FIRE][0];
                break;
            case ActionType.Ads:
                actionID = Actions[AllSettingsKeys.ADS][0];
                break;
            case ActionType.Use:
                actionID = Actions[AllSettingsKeys.USE][0];
                break;
            case ActionType.SwapGun:
                actionID = Actions[AllSettingsKeys.SWAP_GUN][0];
                break;
        }
    }

    private void RemapLogic(Dictionary<string, string[]> Actions, string actionID)
    {
        //action.Disable();
        //var rebind = action.PerformInteractiveRebinding(0);
        //img.enabled = false;

        /*rebind.OnComplete(
        operation =>
        {
            operation.Dispose();
            var overrideData = action.SaveBindingOverridesAsJson();
            ((Dictionary<string, string[]>)
            rebinds.Data[AllSettingsKeys.ACTIONS])[actionName][2] = overrideData;

            ((Dictionary<string, string[]>)
            rebinds.Data[AllSettingsKeys.ACTIONS])[actionName][1] = action.bindings[0].overridePath;

            img.sprite = settings.SetImg(action.bindings[0].overridePath);
            img.enabled = true;

        });
        rebind.Start();*/

        //action.Enable();
    }

    public void ChangeButtonsType(int type)
    {
        settings.ButtonsType = type;
        //UIButtons.ChangeButtonsHints();

        /*crouch1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.CROUCH]);
        jump1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.JUMP]);
        fire1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.FIRE]);
        ads1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.ADS]);
        use1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.USE]);
        swap1.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.SWAP_GUN]);*/
    }

    public void ChangeSetting(string name, float value, bool expanded)
    {
        rebinds.Data[name] = value;
    }
}

public enum ActionType
{
    None,
    Crouch,
    Jump,
    Fire,
    Ads,
    Use,
    SwapGun,
}
