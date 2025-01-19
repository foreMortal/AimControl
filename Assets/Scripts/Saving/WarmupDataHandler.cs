using System.Collections.Generic;

public class WarmupDataHandler
{
    private List<UserDataWarmUpLevel> UserLevelsList = new();
    private int lastIndex = 0;

    public int LastIndex
    {
        get { return lastIndex; }
        set { lastIndex = value; }
    }
    public List<UserDataWarmUpLevel> UserPrefs
    {
        get { return UserLevelsList; }
        set { UserLevelsList = value; }
    }

    public WarmupDataHandler() 
    {
        UserLevelsList = new();
        lastIndex = 0;
    }

    public WarmupDataHandler(List<UserDataWarmUpLevel> levels)
    {
        UserPrefs = levels;
    }
}

public class UserDataWarmUpLevel
{
    private string name;
    private float timeForLevel;
    private int index;
    private List<LevelSettings> levels = new();
    private LevelSettings levelSettings;
    private int imageIndex;

    public UserDataWarmUpLevel() { }

    public UserDataWarmUpLevel(string name, float time, int index, List<LevelSettings> levels, LevelSettings levelSettings, int imageIndex)
    {
        Name = name;
        Time = time;
        Index = index;
        Levels = levels;
        LevelSettings = levelSettings;
        ImageIndex = imageIndex;
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ImageIndex
    {
        get { return imageIndex; }
        set { imageIndex = value; }
    }

    public LevelSettings LevelSettings
    {
        get { return levelSettings; }
        set { levelSettings = value; }
    }

    public float Time
    {
        get { return timeForLevel; }
        set { timeForLevel = value; }
    }
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public List<LevelSettings> Levels
    {
        get { return levels; }
        set { levels = value; }
    }
}
