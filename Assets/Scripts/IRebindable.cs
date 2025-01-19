public enum RebindType
{
    Universal,
    Apex,
    Valorant,
    CSGO,
}

public interface IRebindable
{
    public RebindType GetRebindType();

}
