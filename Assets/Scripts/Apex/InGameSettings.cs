using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameSettings : MonoBehaviour
{
    public static UnityEvent closeSettings = new();
    public static UnityEvent openSettings = new();

    [SerializeField] private SettingsScriptableObject sett;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject ApexSettings, UniversalSettings, ValorantSettings, CSSettings;
    [SerializeField] private SetupWeapon weapon;
    private GameObject settings;
    private GameObject userUI;
    private SettingsWindow setWindow;
    private Controlls controlls;
    private Controlls settingsControlls;
    private ApexStatsHendler handler;
    private bool open = false, levelEnded;

    private void OnEnable()
    {
        controlls.Enable();
        settingsControlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
        settingsControlls.Disable();
    }

    private void Awake()
    {
        controlls = new Controlls();
        settingsControlls = new Controlls();
        controlls = sett.controlls;

        settingsControlls.MenuControl.OpenSett.performed += ctx => OpenSettings();
        Timer.endOfLevel.AddListener(LvlEnded);

        closeSettings.AddListener(CloseSettings);
        handler = GetComponent<ApexStatsHendler>();

        switch (sett.rebinds.Type)
        {
            case RebindType.Apex:
                settings = Instantiate(ApexSettings);
                RemapScript script = settings.GetComponent<RemapScript>();
                script.SetUpSettings(sett);
                settings.GetComponent<ApexChangeWeapons>().Setup(weapon);
                handler.GetStopGameTextFields(script.GetFields());
                break;
            case RebindType.Universal:
                settings = Instantiate(UniversalSettings);
                UniversalRemap Uscript = settings.GetComponent<UniversalRemap>();
                Uscript.SetUpSettings(sett);
                handler.GetStopGameTextFields(Uscript.GetFields());
                break;
            case RebindType.Valorant:
                settings = Instantiate(ValorantSettings);
                ValorantRemap Vscript = settings.GetComponent<ValorantRemap>();
                Vscript.SetUpSettings(sett);
                handler.GetStopGameTextFields(Vscript.GetFields());
                break;
            case RebindType.CSGO:
                settings = Instantiate(CSSettings);
                CSRemap CSscript = settings.GetComponent<CSRemap>();
                CSscript.SetUpSettings(sett);
                handler.GetStopGameTextFields(CSscript.GetFields());
                break;
        }
        setWindow = settings.GetComponentInChildren<SettingsWindow>();
        userUI = fpsCam.transform.GetChild(0).gameObject;
    }

    public void OpenSettings()
    {
        if (!open)
        {
            controlls.Disable();
            handler.OpenStopMenu();
            Cursor.lockState = CursorLockMode.None;
            ManageSettings(true);
            setWindow.OpenCanvases();
            Time.timeScale = 0f;
            openSettings.Invoke();
        }
        else if (open)
        {
            closeSettings.Invoke();
        }
    }

    private void CloseSettings()
    {
        controlls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        ManageSettings(false);
        Time.timeScale = 1f;
        ApexApplySettings.ReuploadSettings.Invoke();
    }

    private void ManageSettings(bool state)
    {
        if (!levelEnded)
        {
            settings.SetActive(state);

            userUI.SetActive(!state);
            weapon.SetActive(!state);

            open = state;
        }
    }

    private void HideGunAndUI()
    {
        settings.SetActive(false);
        userUI.SetActive(false);
        weapon.SetActive(false);
    }

    private void LvlEnded()
    {
        HideGunAndUI();
        levelEnded = true;
    }
}
