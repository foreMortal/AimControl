using UnityEngine;
using UnityEngine.UI;

public class ChangeSettingsOnButtonMainMenu : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private AllApexRecoilsScriptableObject recoils;
    [SerializeField] private Text gunName;
    private UserRebinds rebinds;
    private void Awake()
    {
        rebinds = (UserRebinds)settings.rebinds;
        gunName.text = "Choosen: " + recoils.Recoils[settings.RecoilIndex].WeaponName;
    }

    public void TakeWeapon(int value)
    {
        rebinds.Data[AllSettingsKeys.CHOOSEN_WEAPON] = value;
        settings.RecoilIndex = value;
        gunName.text = "Choosen: " + recoils.Recoils[value].WeaponName;
    }
}
