using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DummyParametersScriptableObject", order = 2)]
public class DummyParameters : ScriptableObject
{
    public float _speed, _swipespeed, swapTime, min, max, minCrouch, maxCrouch, minNextCr, maxNextCr;
}
