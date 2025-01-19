using System.Collections.Generic;

public class UserSettingsRebindsData
{
    private string lastOpenedGame;
    private int hintsType;

    private Dictionary<string, UserRebinds> rebinds;

    public int HintsType { get { return hintsType; } set { hintsType = value; } }
    public string LastOpenedGame { get { return lastOpenedGame; } set { lastOpenedGame = value; } }
    
    public Dictionary<string, UserRebinds> Rebinds
    {
        get { return rebinds; }
        set { rebinds = value; }
    }

    public UserSettingsRebindsData() { }
}

public class UserRebinds : RebindsParent
{
    public UserRebinds(RebindType Type) : base(Type)
    {

    }

    public void UploadRebinds(UserRebinds rebinds)
    {
        foreach(var newData in rebinds.Data)
        {
            data[newData.Key] = newData.Value;
        }
    }
}
