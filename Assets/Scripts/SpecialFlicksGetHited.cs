using UnityEngine;

public class SpecialFlicksGetHited : MonoBehaviour, IHitable
{
    private SpecialFlicksManager manager;

    public void SelfAwake(SpecialFlicksManager m)
    {
        manager = m;
    }
    public void GetHited(HitInfo info, out bool head)
    {
        head = false;
        gameObject.SetActive(false);
        manager.TargetHited();
    }
}
