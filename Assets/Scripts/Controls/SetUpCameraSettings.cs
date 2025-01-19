using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetUpCameraSettings : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Transform buddy;
    [SerializeField] private Camera player;
    [SerializeField] private GameObject ApexCameraSettings, UniversalCameraSettings, ValorantCameraSettings, CSCameraSettings;

    private SetupWeapon weapon;
    private List<CameraMoveParent> move = new();

    public UnityEvent cameraSettSetaped = new();

    private void Awake()
    {
        weapon = GetComponent<SetupWeapon>();

        switch (settings.rebinds.Type)
        {
            case RebindType.Apex:
                GameObject cameraSett = Instantiate(ApexCameraSettings, transform);
                GamePadMouvement gMove = cameraSett.GetComponent<GamePadMouvement>();
                move.Add(gMove);
                gMove.SetUpCameraSettings(player, buddy, weapon);

                gMove.PerformAds += buddy.GetComponent<PlayerMouvement>().AdsOn;

                break;
            case RebindType.Universal:
                GameObject UcameraSett = Instantiate(UniversalCameraSettings, transform);
                UniversalMouseControl uMouseControl = UcameraSett.GetComponent<UniversalMouseControl>();
                UniversalGamepadControl uGamepadControl = UcameraSett.GetComponent<UniversalGamepadControl>();
                move.Add(uMouseControl);
                move.Add(uGamepadControl);

                uMouseControl.SetUpCameraSettings(player, buddy, weapon);
                uGamepadControl.SetUpCameraSettings(player, buddy, weapon);

                uMouseControl.PerformAds += buddy.GetComponent<PlayerMouvement>().AdsOn;
                uGamepadControl.PerformAds += buddy.GetComponent<PlayerMouvement>().AdsOn;

                break;
            case RebindType.Valorant:
                GameObject vCameraSett = Instantiate(ValorantCameraSettings, transform);
                ValorantMouseControl vMouseControl = vCameraSett.GetComponent<ValorantMouseControl>();
                move.Add(vMouseControl);

                vMouseControl.SetUpCameraSettings(player, buddy, weapon);
                vMouseControl.PerformAds += buddy.GetComponent<PlayerMouvement>().AdsOn;

                break;
            case RebindType.CSGO:
                GameObject CSCameraSett = Instantiate(CSCameraSettings, transform);
                CSMouseControl CSMouseControl = CSCameraSett.GetComponent<CSMouseControl>();
                move.Add(CSMouseControl);

                CSMouseControl.SetUpCameraSettings(player, buddy, weapon);
                CSMouseControl.PerformAds += buddy.GetComponent<PlayerMouvement>().AdsOn;

                break;
        }

        cameraSettSetaped.Invoke();
        foreach(var m in move)
        {
            m.enabled = false;
        }
    }

    private void Start()
    {
        foreach (var m in move)
        {
            m.enabled = true;
        }
    }
}
