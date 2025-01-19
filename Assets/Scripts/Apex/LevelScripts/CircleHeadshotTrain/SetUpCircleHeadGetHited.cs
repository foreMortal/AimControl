using UnityEngine;

public class SetUpCircleHeadGetHited : MonoBehaviour
{
    [SerializeField] private CircleHeadShotGetHited hit;

    public void Setup(MoveTargetToCenter move)
    {
        hit.Setup(move, gameObject);
    }
}
