using UnityEngine;

public class PassCheckGetHited : MonoBehaviour, IHitable
{
    [SerializeField] bool head;
    private PassCheckDummyCanDie health;

    public void GetHited(HitInfo info, out bool head)
    {
        head = this.head;
        if (this.head)
            health.GetHited(info.headDamage);
        else
            health.GetHited(info.damage);
    }

    public void GetHealthScript(PassCheckDummyCanDie health)
    {
        this.health = health;
    }
}
