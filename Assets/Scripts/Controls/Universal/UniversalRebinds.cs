using System.Collections.Generic;

public class UniversalRebinds : IRebindable
{
    private const RebindType rebindType = RebindType.Universal;
    private Dictionary<string, string> MnKactionsPetName = new();
    private Dictionary<string, string> MnKactions = new();
    private Dictionary<string, string> GpdactionsPetName = new();
    private Dictionary<string, string> Gpdactions = new();
    private Dictionary<string, float> MnKclassic = new();
    private Dictionary<string, float> Gpdclassic = new();

    public UniversalRebinds()
    {
        MnKactionsPetName = new()
        {
            [AllSettingsKeys.CROUCH] = "C",
            [AllSettingsKeys.JUMP] = "Space",
            [AllSettingsKeys.FIRE] = "Mouse left",
            [AllSettingsKeys.ADS] = "Mouse right",
            [AllSettingsKeys.USE] = "F",
        };
        MnKactions = new()
        {
            [AllSettingsKeys.CROUCH] = null,
            [AllSettingsKeys.JUMP] = null,
            [AllSettingsKeys.FIRE] = null,
            [AllSettingsKeys.ADS] = null,
            [AllSettingsKeys.USE] = null,
        };
        MnKclassic = new()
        {
            [AllSettingsKeys.HORIZONTAL] = 1f,
            [AllSettingsKeys.VERTICAL] = 1f,
            [AllSettingsKeys.ADS_HORIZONTAL] = 1f,
            [AllSettingsKeys.ADS_VERTICAL] = 1f,
        };

        GpdactionsPetName = new()
        {
            [AllSettingsKeys.CROUCH] = "<Gamepad>/buttonEast",
            [AllSettingsKeys.JUMP] = "<Gamepad>/buttonSouth",
            [AllSettingsKeys.FIRE] = "<Gamepad>/rightTrigger",
            [AllSettingsKeys.ADS] = "<Gamepad>/leftTrigger",
            [AllSettingsKeys.USE] = "<Gamepad>/buttonWest",
        };
        Gpdactions = new()
        {
            [AllSettingsKeys.CROUCH] = null,
            [AllSettingsKeys.JUMP] = null,
            [AllSettingsKeys.FIRE] = null,
            [AllSettingsKeys.ADS] = null,
            [AllSettingsKeys.USE] = null,
        };
        Gpdclassic = new()
        {
            [AllSettingsKeys.SPREAD_ACTIVE] = 1f,
            [AllSettingsKeys.DEADZONE] = 5f,
            [AllSettingsKeys.HORIZONTAL] = 1f,
            [AllSettingsKeys.VERTICAL] = 1f,
            [AllSettingsKeys.ADS_HORIZONTAL] = 1f,
            [AllSettingsKeys.ADS_VERTICAL] = 1f,
        };
    }

    public UniversalRebinds(Dictionary<string, string> MnKactions, Dictionary<string, string> MnKactionsPetName, Dictionary<string, float> MnKclassic, Dictionary<string, string> Gpdactions, Dictionary<string, string> GpdactionsPetName, Dictionary<string, float> Gpdclassic)
    {
        this.MnKactionsPetName = MnKactionsPetName;
        this.MnKactions = MnKactions;
        this.MnKclassic = MnKclassic;

        this.GpdactionsPetName = GpdactionsPetName;
        this.Gpdactions = Gpdactions;
        this.Gpdclassic = Gpdclassic;
    }

    public void UploadRebinds(UniversalRebinds rebinds)
    {
        //MnK default layout
        Dictionary<string, string> newMnKPetNames = new();
        foreach (var item in MnKPetNames)
        {
            if (rebinds.MnKPetNames.TryGetValue(item.Key, out string value))
                newMnKPetNames[item.Key] = value;
            else
                newMnKPetNames[item.Key] = MnKPetNames[item.Key];
        }
        MnKPetNames = newMnKPetNames;

        Dictionary<string, string> newMnKActions = new();
        foreach (var item in MnKActions)
        {
            if (rebinds.MnKActions.TryGetValue(item.Key, out string value))
                newMnKActions[item.Key] = value;
            else
                newMnKActions[item.Key] = MnKActions[item.Key];
        }
        MnKActions = newMnKActions;

        Dictionary<string, float> newMnKClassic = new();
        foreach (var item in MnKClassic)
        {
            if (rebinds.MnKClassic.TryGetValue(item.Key, out float value))
                newMnKClassic[item.Key] = value;
            else
                newMnKClassic[item.Key] = MnKClassic[item.Key];
        }
        MnKClassic = newMnKClassic;
        ///gamepad default layout
        Dictionary<string, string> newGpdPetNames = new();
        foreach (var item in GpdPetNames)
        {
            if (rebinds.GpdPetNames.TryGetValue(item.Key, out string value))
                newGpdPetNames[item.Key] = value;
            else
                newGpdPetNames[item.Key] = GpdPetNames[item.Key];
        }
        GpdPetNames = newGpdPetNames;

        Dictionary<string, string> newGpdActions = new();
        foreach (var item in GpdActions)
        {
            if (rebinds.GpdActions.TryGetValue(item.Key, out string value))
                newGpdActions[item.Key] = value;
            else
                newGpdActions[item.Key] = GpdActions[item.Key];
        }
        GpdActions = newGpdActions;

        Dictionary<string, float> newGpdClassic = new();
        foreach (var item in GpdClassic)
        {
            if (rebinds.GpdClassic.TryGetValue(item.Key, out float value))
                newGpdClassic[item.Key] = value;
            else
                newGpdClassic[item.Key] = GpdClassic[item.Key];
        }
        GpdClassic = newGpdClassic;
    }

    public RebindType GetRebindType()
    {
        return rebindType;
    }

    public Dictionary<string, string> MnKPetNames
    {
        get { return MnKactionsPetName; }
        set { MnKactionsPetName = value; }
    }

    public Dictionary<string, string> MnKActions
    {
        get { return MnKactions; }
        set { MnKactions = value; }
    }

    public Dictionary<string, float> MnKClassic
    {
        get { return MnKclassic; }
        set { MnKclassic = value; }
    }

    public Dictionary<string, string> GpdPetNames
    {
        get { return GpdactionsPetName; }
        set { GpdactionsPetName = value; }
    }

    public Dictionary<string, string> GpdActions
    {
        get { return Gpdactions; }
        set { Gpdactions = value; }
    }

    public Dictionary<string, float> GpdClassic
    {
        get { return Gpdclassic; }
        set { Gpdclassic = value; }
    }
}
