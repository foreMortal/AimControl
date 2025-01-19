using System.Collections.Generic;

public class RebindsParent 
{
    private readonly RebindType type;

    protected Dictionary<string, object> data;

    public RebindType Type { get { return type; } }

    public Dictionary<string, object> Data
    {
        get { return data; }
        set { data = value; }
    }

    public RebindsParent(RebindType Type)
    {
        type = Type;
        data = new();
    }
}
