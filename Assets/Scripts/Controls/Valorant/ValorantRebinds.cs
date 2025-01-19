using System.Collections.Generic;

public class ValorantRebinds : IRebindable
{
    private const RebindType type = RebindType.Valorant;
    private Dictionary<string, float> classic = new();
    private Dictionary<string, string> actions = new();
    private Dictionary<string, string> petNames = new();

    private const string FIRE = "Fire";
    private const string CROUCH = "Crouch";
    private const string USE = "Use";
    private const string JUMP = "Jump";
    private const string ADS = "Ads";

    private const string PERFORM_SNIPER = "PerformSniper";
    private const string SENSITIVITY = "Sensitivity";
    private const string ADS_SENSITIVITY = "AdsSensitivity";
    private const string SCOP_SENSITIVITY = "ScopSensitivity";

    public ValorantRebinds() 
    {
        petNames = new()
        {
            [CROUCH] = "Ctrl",
            [JUMP] = "Space",
            [FIRE] = "Mouse left",
            [ADS] = "Mouse right",
            [USE] = "F",
        };
        actions = new()
        {
            [CROUCH] = null,
            [JUMP] = null,
            [FIRE] = null,
            [ADS] = null,
            [USE] = null,
        };
        classic = new()
        {
            [SENSITIVITY] = 1f,
            [ADS_SENSITIVITY] = 1f,
            [SCOP_SENSITIVITY] = 1f,
            [PERFORM_SNIPER] = 0f,
        };
    }

    public ValorantRebinds(Dictionary<string, float> classic, Dictionary<string, string> actions, Dictionary<string, string> petNames)
    {
        this.classic = classic;
        this.actions = actions;
        this.petNames = petNames;
    }

    public RebindType GetRebindType(){ return type; }

    public void UploadRebinds(ValorantRebinds newRebinds)
    {
        Dictionary<string, float> newClassic = new();
        foreach (var item in classic)
        {
            if(newRebinds.classic.TryGetValue(item.Key, out float value))
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
