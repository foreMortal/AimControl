using UnityEngine;
using UnityEngine.Events;

public class ApexApplySettings : MonoBehaviour
{
    public static UnityEvent<SettingsScriptableObject> GiveData = new();
    public static UnityEvent ReuploadSettings = new();

    [SerializeField] private SettingsScriptableObject settings;

    private IDataService service = new JsondataService();
    private UserSettingsRebindsData data = new();

    private const string RELATIVE_PATH = "/SettingsRebinds";

    private void Awake()
    {
        ApexStatsHendler.LevelQuit.AddListener(SaveData);
    }

    private void SaveData()
    {
        service.LoadData(RELATIVE_PATH, false, out data);

        data ??= new();

        switch (settings.rebinds.Type)
        {
            case RebindType.Apex:
                data.Rebinds[AllSettingsKeys.APEX_REBINDS] = settings.rebinds;
                break;
            case RebindType.Universal:
                //data.UniversalRebinds[AllSettingsKeys.UNIVERSAL_REBINDS] = (UniversalRebinds)settings.rebinds;
                break;
            case RebindType.Valorant:
                //data.ValorantRebinds[AllSettingsKeys.VALORANT_REBINDS] = (ValorantRebinds)settings.rebinds;
                break;
            case RebindType.CSGO:
                //data.CSRebinds[AllSettingsKeys.CSGO_REBINDS] = (CSRebinds)settings.rebinds;
                break;
        }

        service.SaveData(RELATIVE_PATH, data, false);
    }
}
