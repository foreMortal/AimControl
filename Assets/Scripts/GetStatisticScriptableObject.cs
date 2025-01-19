using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StatisticScriptableObject", order = 1)]
public class GetStatisticScriptableObject : ScriptableObject
{
    public bool active;
    public float succeeded, failed, allShots, hits, headShots, bodyShots;
    public float misses, targetsLost, normalShots, exelentShots;
    public float playerDamageDelt, playerDamageTaken, timePlayed, trackingTime;
    public float missingTargetTime, recordTime;

    public float Hits { get { return hits; }  set { if (active) hits = value; } }
    public float AllShots { get { return allShots; } set { if (active) hits = value; } }
    public float Misses { get { return misses; } set { if (active) misses = value; } }

    public void Clear()
    {
        succeeded = failed = allShots = hits = headShots = bodyShots =
        misses = targetsLost = normalShots = exelentShots =
        playerDamageDelt = playerDamageTaken = timePlayed = trackingTime =
        missingTargetTime = recordTime = 0f;
    }
}