public interface IHitable
{
    void GetHited(HitInfo hitInfo, out bool headShot);
}

public class HitInfo
{
    public float damage;
    public float headDamage;
    public float hitsCount;

    public HitInfo() { }

    public HitInfo(float damage, float headDamage, float hitsCount)
    {
        this.damage = damage;
        this.headDamage = headDamage;
        this.hitsCount = hitsCount;
    }

    public void CreateNewInfo(float damage, float headDamage, float hitsCount)
    {
        this.damage = damage;
        this.headDamage = headDamage;
        this.hitsCount = hitsCount;
    }
}