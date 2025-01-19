using UnityEngine;

public class ApexChangeWeapons : MonoBehaviour
{
    private SetupWeapon gun;
    [SerializeField] private SettingsScriptableObject settings;
    private UserRebinds rebinds;

    public void Setup(SetupWeapon gun)
    {
        this.gun = gun;
        rebinds = (UserRebinds)settings.rebinds;
    }
    public void ChangeWeapon(int recoilIndex)
    {
        gun.ChangeWeapon(recoilIndex);
        settings.RecoilIndex = recoilIndex;
        rebinds.Data[AllSettingsKeys.CHOOSEN_WEAPON] = recoilIndex;
        InGameSettings.closeSettings.Invoke();
    }
}
