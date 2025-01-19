using UnityEngine;

public class GetHitedFlickV2 : MonoBehaviour, IHitable
{
    private FlicksV2MovingTargets flicks;

    public void GetHited(HitInfo hitInfo, out bool head)
    {
        head = false;

        flicks.TargetDied(gameObject);
    }

    public void GetFlicks(FlicksV2MovingTargets tar)
    {
        flicks = tar;
    }
}
