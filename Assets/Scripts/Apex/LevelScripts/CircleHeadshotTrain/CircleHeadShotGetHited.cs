using UnityEngine;

public class CircleHeadShotGetHited : MonoBehaviour, IHitable
{
    private MoveTargetToCenter move;
    private GameObject self;

    public void GetHited(HitInfo info, out bool headshot)
    {
        headshot = true;
        move.KillTarget(self);
    }

    public void Setup(MoveTargetToCenter move, GameObject obj)
    {
        this.move = move;
        self = obj;
    }
}
