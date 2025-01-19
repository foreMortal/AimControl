using System.Collections.Generic;

public class PlayerStats
{
    private Dictionary<string, float> stats = new();

    public Dictionary<string, float> Stats
    {
        get { return stats; }
        set { stats = value; }
    }

    public PlayerStats() { stats = new(); }

    public PlayerStats(Dictionary<string, float> stats)
    {
        this.stats = stats;
    }
}

public class PlayerStatsDataHandler
{
    private Dictionary<string, PlayerStats> levels = new();

    public Dictionary<string, PlayerStats> Levels
    {
        get { return levels; }
        set { levels = value; }
    }
    public PlayerStatsDataHandler() { }
    public PlayerStatsDataHandler(Dictionary<string, PlayerStats> levels)
    {
        this.levels = levels;
    }
}
