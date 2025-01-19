using UnityEngine;
using UnityEngine.Events;

public class SimpleGetHit : MonoBehaviour, IHitable
{
    [SerializeField] private bool head;

    private ApexHPManager manager;

    public void Setup(ApexHPManager m)
    {
        manager = m;
    }

    public void GetHited(HitInfo hitInfo, out bool headShot)
    {
        if (head)
        {
            manager.ApplyDelt(hitInfo.headDamage);
            headShot = true;
        }
        else
        {
            manager.ApplyDelt(hitInfo.damage);
            headShot = false;
        }
    }
}
