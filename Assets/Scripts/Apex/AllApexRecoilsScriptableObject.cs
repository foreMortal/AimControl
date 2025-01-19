using UnityEngine;

[CreateAssetMenu(fileName = "NewApexRecoils", menuName = "ScriptableObjects/ApexRecoil/AllApexRecoilsScriptableObject", order = 0)]
public class AllApexRecoilsScriptableObject : ScriptableObject
{
    [SerializeField] private ApexRecoilScripatbleObject[] recoils;

    public ApexRecoilScripatbleObject[] Recoils { get { return recoils; } }
}
