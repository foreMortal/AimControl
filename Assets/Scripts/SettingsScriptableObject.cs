using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SettingsScriptableObject", order = 4)]
public class SettingsScriptableObject : ScriptableObject
{
    [NonSerialized] public bool SettingsGained = false;
    [NonSerialized] public int DeviceType = -1;
    public Sprite[] ps;
    public Sprite[] xb;
    public string[] ph;
    public int ButtonsType;
    public int RecoilIndex = 0;
    public string GameName;
    public UserRebinds rebinds;
    public Controlls controlls;
    public Dictionary<string, Sprite> psButtons = new();
    public Dictionary<string, Sprite> xbButtons = new();
 
    public Controlls GetControlls()
    {
        return controlls;
    }

    public Sprite SetImg(string path)
    {
        if(ButtonsType == 0f)
        {
            return psButtons[path];
        }
        else
        {
            return xbButtons[path];
        }
    }

    public void LevelStart(UserRebinds reb, Controlls cont)
    {
        rebinds = reb;
        controlls = cont;
    }

    public void SetUpButtons()
    {
        for(int i = 0; i < ph.Length; i++)
        {
            psButtons[ph[i]] = ps[i];
            xbButtons[ph[i]] = xb[i];
        }
    } 
}
