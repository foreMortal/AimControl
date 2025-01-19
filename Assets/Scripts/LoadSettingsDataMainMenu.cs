using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSettingsDataMainMenu : MonoBehaviour
{
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] SettingsScriptableObject settings;
    [SerializeField] private OpenLastCanvas canvas;
    [SerializeField] private Text gameName;

    private IDataService service = new JsondataService();
    private UserSettingsRebindsData data = new();

    private const string RELATIVE_PATH = "/SettingsRebinds";

    private void Awake()
    {
        stats.active = true;

        service.LoadData(RELATIVE_PATH, false, out data);

        if (data == null && !settings.SettingsGained)
        {
            data = new();
            settings.GameName = "None";
        }
        else if(!settings.SettingsGained)
        {
            settings.GameName = data.LastOpenedGame;
            settings.ButtonsType = data.HintsType;
            gameName.text = data.LastOpenedGame;
        }
        else
            gameName.text = settings.GameName;
    }

    public void SetLastOpenedGame(string game)
    {
        gameName.text = game;
        data.LastOpenedGame = game;
        settings.GameName = game;
    }

    public void Save(UserRebinds rebinds, string name)
    {
        data.HintsType = settings.ButtonsType;
        data.Rebinds[name] = rebinds;
        data.LastOpenedGame = settings.GameName;
        service.SaveData(RELATIVE_PATH, data, false);
    }

    /*public void Save(UniversalRebinds rebinds, string name)
    {
        data.HintsType = settings.ButtonsType;
        data.UniversalRebinds[name] = rebinds;
        data.LastOpenedGame = settings.GameName;
        service.SaveData(RELATIVE_PATH, data, false);
    }

    public void Save(ValorantRebinds rebinds, string name)
    {
        data.HintsType = settings.ButtonsType;
        data.ValorantRebinds[name] = rebinds;
        data.LastOpenedGame = settings.GameName;
        service.SaveData(RELATIVE_PATH, data, false);
    }
    public void Save(CSRebinds rebinds, string name)
    {
        data.HintsType = settings.ButtonsType;
        data.CSRebinds[name] = rebinds;
        data.LastOpenedGame = settings.GameName;
        service.SaveData(RELATIVE_PATH, data, false);
    }*/
    public void Save()
    {
        data.HintsType = settings.ButtonsType;
        data.LastOpenedGame = settings.GameName;
        service.SaveData(RELATIVE_PATH, data, false);
    }


    public void SetUpNewData(UserSettingsRebindsData data)
    {
        this.data = data;
        service.SaveData(RELATIVE_PATH, data, false);
    }

    public UserSettingsRebindsData GetData()
    {
        return data;
    }
}
