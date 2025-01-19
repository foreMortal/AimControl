using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainRemap : MonoBehaviour
{
    [SerializeField] private RebindType RebindsType;
    [SerializeField] private LoadSettingsDataMainMenu loadData;
    [SerializeField] private UIRebindButtons UIButtons;

    private Remaper remaper;
    private LoadDefaultRebinds loadRebinds;
    private Controlls controlls;
    private UserSettingsRebindsData data;
    private UserRebinds rebinds;

    public UnityEvent<UserRebinds> GiveData = new();

    [SerializeField] private GameObject[] sliderHandlers, buttonHandlers;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Dropdown drop;
    [SerializeField] private Image crouch1, jump1, fire1, ads1, use1, swap1;

    private const string ApexRebinds = "ApexRebinds";

    private void Awake()
    {
        remaper = GetComponent<Remaper>();

        List<SettingsSlider> sliders = new();
        foreach (var handler in sliderHandlers)
        {
            sliders.AddRange(handler.GetComponentsInChildren<SettingsSlider>());
        }

        data = loadData.GetData();

        /*foreach (var slider in sliders)
        {
            GiveData.AddListener(slider.TakeData);
            slider.SettingsChange.AddListener(ChangeSetting);
        }*/

        if (!settings.SettingsGained || RebindsType != settings.rebinds.Type)
        {
            controlls = new Controlls();
            rebinds = loadRebinds.LoadRebindOfType(RebindsType);

            settings.RecoilIndex = 0;

            if (data.Rebinds.ContainsKey(ApexRebinds))
            {
                rebinds.UploadRebinds(data.Rebinds[ApexRebinds]);
                settings.RecoilIndex = (int)rebinds.Data[AllSettingsKeys.CHOOSEN_WEAPON];
            }
        }
        else
        {
            rebinds = settings.rebinds;
            controlls = settings.controlls;
        }

        settings.SetUpButtons();
        SetUpSettings();

        remaper.Initialize(controlls, rebinds);
    }

    private void OnEnable()
    {
        if (!settings.SettingsGained || RebindsType != settings.rebinds.Type)
        {
            settings.rebinds = rebinds;
            settings.controlls = controlls;

            settings.SettingsGained = true;
        }
    }

    private void SetUpSettings()
    {
        Dictionary<string, string[]> actions = (Dictionary<string, string[]>)rebinds.Data[AllSettingsKeys.ACTIONS];
        foreach(var action in actions)
        {
            if (action.Value[2].Length > 0)
                controlls.FindAction(action.Value[0]).LoadBindingOverridesFromJson(action.Value[2]);
        }
    }

    private void Start()
    {
        drop.value = settings.ButtonsType;
        GiveData.Invoke(rebinds);
    }

    public void SaveRebinds()
    {
        settings.controlls = controlls;
        settings.rebinds = rebinds;

        loadData.Save(rebinds, AllSettingsKeys.APEX_REBINDS);
    }
}