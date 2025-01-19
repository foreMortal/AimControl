using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TrackingSphereScriptableObject", order = 5)]

public class TrackingSphereScriptableObject : ScriptableObject
{
    public float speed, deltaSpeed, increasedSpeed, deacreasedSpeed, minChangeTime;
    public float maxChangeTime, regularSpeedLimit, minRegLimit, maxRegLimit, currSpeed;
    public bool canChangeSpeed, hard;
}
