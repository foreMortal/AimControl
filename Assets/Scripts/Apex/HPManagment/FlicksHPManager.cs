using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlicksHPManager : MonoBehaviour
{
    [SerializeField] private Text targetsHited, targetsLost;
    private float hits, losses;
    public UnityEvent hited = new(), lost = new();

    private void Awake()
    {
        hited.AddListener(TargetHited);
        lost.AddListener(TargetMissed);
    }

    public void TargetHited()
    {
        hits++;
        targetsHited.text = "Target's hited: " + hits;
    }

    public void TargetMissed()
    {
        losses++;
        targetsLost.text = "Target's missed: " + losses;
    }
}
