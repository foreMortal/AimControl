using System.Collections.Generic;

public class CSRebinds : IRebindable
{
    private const RebindType type = RebindType.CSGO;
    private Dictionary<string, float> classic = new();
    private Dictionary<string, string> actions = new();
    private Dictionary<string, string> petNames = new();

    public CSRebinds()
    {
        petNames = new()
        {
            [AllSettingsKeys.CROUCH] = "Shift",
            [AllSettingsKeys.JUMP] = "Space",
            [AllSettingsKeys.FIRE] = "Mouse left",
            [AllSettingsKeys.ADS] = "Mouse right",
            [AllSettingsKeys.USE] = "F",
        };
        actions = new()
        {
            [AllSettingsKeys.CROUCH] = null,
            [AllSettingsKeys.JUMP] = null,
            [AllSettingsKeys.FIRE] = null,
            [AllSettingsKeys.ADS] = null,
            [AllSettingsKeys.USE] = null,
        };
        classic = new()
        {
            [AllSettingsKeys.SENSITIVITY] = 1.25f,
            [AllSettingsKeys.SCOP_SENSITIVITY] = 1f,
        };
    }

    public RebindType GetRebindType() { return type; }

    public void UploadRebinds(CSRebinds newRebinds)
    {
        Dictionary<string, float> newClassic = new();
        foreach (var item in classic)
        {
            if (newRebinds.classic.TryGetValue(item.Key, out float value))
                newClassic[item.Key] = value;
            else
                newClassic[item.Key] = classic[item.Key];
        }
        classic = newClassic;

        Dictionary<string, string> newActions = new();
        foreach (var item in actions)
        {
            if (newRebinds.actions.TryGetValue(item.Key, out string value))
                newActions[item.Key] = value;
            else
                newActions[item.Key] = actions[item.Key];
        }
        actions = newActions;

        Dictionary<string, string> newPetNames = new();
        foreach (var item in petNames)
        {
            if (newRebinds.petNames.TryGetValue(item.Key, out string value))
                newPetNames[item.Key] = value;
            else
                newPetNames[item.Key] = petNames[item.Key];
        }
        petNames = newPetNames;
    }

    public Dictionary<string, float> Classic
    {
        get { return classic; }
        set { classic = value; }
    }
    public Dictionary<string, string> Actions
    {
        get { return actions; }
        set { actions = value; }
    }
    public Dictionary<string, string> PetNames
    {
        get { return petNames; }
        set { petNames = value; }
    }
}
