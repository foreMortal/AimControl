using UnityEngine;

public class MultipleFlicksGetHited : MonoBehaviour, IHitable
{
    private FlicksMultipleTargets flicks;

    public void GetFlicks(FlicksMultipleTargets flicks)
    {
        this.flicks = flicks;
    }

    public void GetHited(HitInfo info, out bool headshot)
    {
        headshot = false;
        flicks.KillTarget(gameObject);
    }
}
