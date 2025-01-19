using UnityEngine;

public class WeaponIndexer : MonoBehaviour
{
    [SerializeField] private int weaponIndex, recoilIndex;
    [SerializeField] private ApexChangeWeapons change;
    
    public void Change()
    {
        //change.ChangeWeapon(weaponIndex, recoilIndex);
    }
}
