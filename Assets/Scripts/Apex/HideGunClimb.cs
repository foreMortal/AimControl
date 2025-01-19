using UnityEngine;

public class HideGunClimb : MonoBehaviour
{
    [SerializeField] private GameObject climbHands;
    private SetupWeapon weapon;
    //private GameObject cross;

    private void Awake()
    {
        weapon = GetComponentInChildren<SetupWeapon>();
        //cross = weapon.GetComponentInChildren<Canvas>().gameObject;
    }

    public void StartClimbing()
    {
        weapon.Climb(true);
        //cross.SetActive(false);
        climbHands.SetActive(true);
    }

    public void StopClimbing()
    {
        weapon.Climb(false);
        //cross.SetActive(true);
        climbHands.SetActive(false);
    }
}
