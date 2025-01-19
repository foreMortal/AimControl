using UnityEngine;
using UnityEngine.UI;

public class FlickV2HPManager : MonoBehaviour
{
    [SerializeField] private Text DamageDelt, DamageTaken;

    private GetStatisticScriptableObject stats;

    private void Awake()
    {
        ApexStatsHendler.objectPass.AddListener(GetStatisticObject);
    }

    public void TargetHited()
    {
        DamageDelt.text = "Target's hited: " + stats.hits;
    }

    public void TargetMissed()
    {
        stats.targetsLost++;
        DamageTaken.text = "Target's missed: " + stats.targetsLost;
    }

    private void GetStatisticObject(GetStatisticScriptableObject obj)
    {
        stats = obj;
    }
}
