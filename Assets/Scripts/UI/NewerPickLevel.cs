using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class NewerPickLevel : MonoBehaviour
{
    [SerializeField] private Vector3[] settPositions;
    [SerializeField] private Toggle[] drops;
    [SerializeField] WarmupManager warmupManager;
    [SerializeField] private GameObject warmupThing, statisticToggles, nameInputField, timeDropDown;
    [SerializeField] private GameObject Menu;
    [SerializeField] private Text ShowName;
    [SerializeField] private Text[] fields;
    [SerializeField] private string relativePath;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GameObject dummyDistance, dummyStrafes, distance, dificulty, targeting, doorDistance, doorDificulty, weapon;
    [SerializeField] private GameObject[] movementTypes;
    [SerializeField] private StandartCanvas canvas;
    [SerializeField] private IButtonManager[] startButtons;

    private Dropdown dDistance, dStrafes, dist, dif, drDistance, drDif, wp;
    private Toggle t_targeting;

    public UnityEvent<LevelNameObjject> giveData;


    private IDataService service = new JsondataService();
    private PlayerStatsDataHandler data = new();

    private Image image;

    private float accuracy = 0f, succeded = 0f, failed = 0f, damageDelt = 0f, damageTaken = 0f, headSH = 0f, bodySH = 0f, time = 0f, hit, miss, exelent, normal, lost;
    private float trackAccur, trackingTime, missingTargetTime, recordTime;
    
    private List<float> parameters = new();
    private List<string> parameterNames = new();
    private List<string> DopSymbols = new();

    private LevelSettings levelSettings;
    private StartFastTrainScript start;
    private PlayerStats stats;
    private bool statsGiven;

    private void Awake()
    {
        service.LoadData(relativePath, true, out data);
        StatsShow[] levels = FindObjectsOfType<StatsShow>(true);

        dDistance = dummyDistance.GetComponent<Dropdown>();
        dStrafes = dummyStrafes.GetComponent<Dropdown>();
        dist = distance.GetComponent<Dropdown>();
        dif = dificulty.GetComponent<Dropdown>();
        drDistance = doorDistance.GetComponent<Dropdown>();
        drDif = doorDificulty.GetComponent<Dropdown>();
        wp = weapon.GetComponent<Dropdown>();
        t_targeting = targeting.GetComponent<Toggle>();

        image = Menu.GetComponentInChildren<Image>();//SetTar

        foreach(var level in levels)
        {
            giveData.AddListener(level.TakeData);
        }
        giveData.Invoke(levelName);
    }

    public void OpenLevel()
    {
        ClearAll();
        Menu.SetActive(true);
    }
    public void CloseLevel()
    {
        ClearAll();
        levelSettings = null;
        start = null;
        Menu.SetActive(false);
    }

    private void OnMouseDown()
    {
        CloseLevel();
    }

    public void SetStats()
    {
        OpenLevel();
        NewerCalculate();

        for (int i = 0; i < parameters.Count; i++)
        {
            if(i < fields.Length)
                fields[i].text = parameterNames[i] + parameters[i].ToString("F2") + DopSymbols[i];
        }
    }

    private void ClearAll()
    {
        foreach (var item in fields)
        {
            item.text = "";
        }
        parameters.Clear();
        parameterNames.Clear();
        DopSymbols.Clear();
    }

    public void DeleteWarmupLevel(string warmupName)
    {
        data.Levels.Remove(warmupName);
        service.SaveData(relativePath, data, true);
    }

    public void TakeData(string name, Sprite img, MovementType type)
    {
        canvas.StartButton = startButtons[0];
        foreach (var mType in movementTypes)
        {
            mType.SetActive(false);
        }
        movementTypes[(int)type].SetActive(true);
        ShowName.text = name;
        image.sprite = img; 

        timeDropDown.SetActive(true); dummyDistance.SetActive(false); dummyStrafes.SetActive(false);
        distance.SetActive(false); dificulty.SetActive(false);
        doorDistance.SetActive(false); doorDificulty.SetActive(false);
        weapon.SetActive(false); targeting.SetActive(false);
        warmupThing.SetActive(false);

        switch (levelName.type)
        {
            case "DummyClose":
            case "DummyShort":
            case "DummyMiddle":
            case "DummyLong":
                OpenLevelSettings(dummyDistance, dificulty, dummyStrafes, targeting);
                t_targeting.isOn = false;
                dDistance.value = 0;
                dif.value = 0;
                dStrafes.value = 0;
                break;
            case "DoorPicks":
                OpenLevelSettings(doorDistance, dificulty);
                drDistance.value = 0;
                dif.value = 0;
                wp.value = 0;
                break;
            case "FlickShots":
            case "FlickShotsV2":
            case "FlickShotsMultipleTargets":
            case "PassChecking":
            case "FullRotateFlicks":
            case "SpecialFlicks":
                OpenLevelSettings(distance, dificulty);
                dist.value = 0;
                dif.value = 0;
                break;
            case "TrakingSphere":
                OpenLevelSettings(distance, dificulty, targeting);
                dist.value = 0;
                t_targeting.isOn = false;
                dif.value = 0;
                break;
            case "BehindNockDownShield":
            case "CircleHeadshotTrain":
            case "FromHgToLg":
            case "FromLgToHg":
            case "LowWallJumpFlicks":
            case "ogibanieStenki":
            case "SlideFlickshots":
            case "StraightWallJumpFlicks":
                OpenLevelSettings(dificulty, dummyStrafes);
                dif.value = 0;
                dStrafes.value = 0;
                break;
        }

        ChangeStats();
        startButtons[0].handl = new Color(1, 1, 1);
    }

    public void TakeData(LevelSettings sett, StartFastTrainScript start)
    {
        canvas.StartButton = startButtons[1];
        image.sprite = start.image;
        ShowName.text = start.lvlName.text;

        timeDropDown.SetActive(false); dummyDistance.SetActive(false); dummyStrafes.SetActive(false);
        distance.SetActive(false); dificulty.SetActive(false);
        doorDistance.SetActive(false); doorDificulty.SetActive(false);
        weapon.SetActive(false); targeting.SetActive(false);

        switch (sett.petName)
        {
            case "-1":
            case "-2":
            case "-3":
            case "-4":
                nameInputField.SetActive(false);
                break;
            default: 
                nameInputField.SetActive(true);
                break;
        }

        levelSettings = sett;
        this.start = start;

        levelName.SetByLevelSettings(levelSettings);
        warmupThing.SetActive(true);

        ChangeStats();
        startButtons[1].handl = new Color(1, 1, 1);
    }

    private void OpenLevelSettings(params GameObject[] setts)
    {
        for(int i = 0; i < setts.Length; i++)
        {
            setts[i].SetActive(true);
            setts[i].transform.localPosition = settPositions[i];
        }
    }

    public void ChangeStats()
    {
        if (data != null)
        {
            try
            {
                stats = data.Levels[levelName.MakeLevelName()];
                statsGiven = true;
            }
            catch (KeyNotFoundException)
            {
                statsGiven = false;
            }
        }
        else
        {
            statsGiven = false;
        }
        SetStats();
    }

    private void NewerCalculate()
    {
        if (statsGiven)
        {
            if (levelName.accuracy_)
            {
                accuracy = CountProcent(stats.Stats["Hit"], stats.Stats["AllShots"]);
                parameters.Add(accuracy);
                parameterNames.Add("Accuracy ");
                DopSymbols.Add("%");
            }
            if (levelName.succeeded_)
            {
                succeded = stats.Stats["Succeeded"];
                parameters.Add(succeded);
                parameterNames.Add("Succeeded: ");
                DopSymbols.Add(" ");
            }
            if (levelName.failed_)
            {
                failed = stats.Stats["Failed"];
                parameters.Add(failed);
                parameterNames.Add("Failed: ");
                DopSymbols.Add(" ");
            }
            if (levelName.headSH_)
            {
                headSH = CountProcent(stats.Stats["HeadShots"], stats.Stats["Hit"]);
                parameters.Add(headSH);
                parameterNames.Add("Headshots: ");
                DopSymbols.Add("%");
            }
            if (levelName.bodySH_)
            {
                bodySH = CountProcent(stats.Stats["BodyShots"], stats.Stats["Hit"]);
                parameters.Add(bodySH);
                parameterNames.Add("Bodyshots: ");
                DopSymbols.Add("%");
            }
            if (levelName.damageDelt_)
            {
                damageDelt = CountDamage(stats.Stats["DamageDelt"]);
                parameters.Add(damageDelt);
                parameterNames.Add("Damage delt: ");
                DopSymbols.Add("k");
            }
            if (levelName.damageTaken_)
            {
                damageTaken = CountDamage(stats.Stats["DamageTaken"]);
                parameters.Add(damageTaken);
                parameterNames.Add("Damage taken: ");
                DopSymbols.Add("k");
            }
            if (levelName.exelentShots_)
            {
                exelent = stats.Stats["ExelentShots"];
                parameters.Add(exelent);
                parameterNames.Add("Excelent shots: ");
                DopSymbols.Add(" ");

            }
            if (levelName.normalShots_)
            {
                normal = stats.Stats["NormalShots"];
                parameters.Add(normal);
                parameterNames.Add("Normal shots: ");
                DopSymbols.Add(" ");
            }
            if (levelName.hits_)
            {
                hit = stats.Stats["Hit"];
                parameters.Add(hit);
                parameterNames.Add("Hits: ");
                DopSymbols.Add(" ");
            }
            if (levelName.misses_)
            {
                miss = stats.Stats["Miss"];
                parameters.Add(miss);
                parameterNames.Add("Misses: ");
                DopSymbols.Add(" ");
            }
            if (levelName.targetsLost_)
            {
                lost = stats.Stats["TargetsLost"];
                parameters.Add(lost);
                parameterNames.Add("Targets lost: ");
                DopSymbols.Add(" ");
            }
            if (levelName.trackAccuracy_)
            {
                trackAccur = CountProcent(stats.Stats["TrackingTime"], stats.Stats["TimePlayed"]);
                parameters.Add(trackAccur);
                parameterNames.Add("Tracking accuracy: ");
                DopSymbols.Add("%");
            }
            if (levelName.trackingTime_)
            {
                trackingTime = stats.Stats["TrackingTime"];
                parameters.Add(trackingTime);
                parameterNames.Add("Tracking time: ");
                DopSymbols.Add("sec");
            }
            if (levelName.missingTargetTime_)
            {
                missingTargetTime = stats.Stats["MissingTime"];
                parameters.Add(missingTargetTime);
                parameterNames.Add("Missing target time: ");
                DopSymbols.Add("sec");
            }
            if (levelName.recordTime_)
            {
                recordTime = stats.Stats["RecordTime"];
                parameters.Add(recordTime);
                parameterNames.Add("Record: ");
                DopSymbols.Add("sec");
            }
            if (levelName.time_)
            {
                time = CountTime(stats.Stats["TimePlayed"]);
                parameters.Add(time);
                parameterNames.Add("Time played ");
                DopSymbols.Add("h");
            }
        }
        else
        {
            if (levelName.accuracy_)
            {
                parameters.Add(0f);
                parameterNames.Add("Accuracy ");
                DopSymbols.Add("%");
            }
            if (levelName.succeeded_)
            {
                parameters.Add(0f);
                parameterNames.Add("Succeeded: ");
                DopSymbols.Add(" ");
            }
            if (levelName.failed_)
            {
                parameters.Add(0f);
                parameterNames.Add("Failed: ");
                DopSymbols.Add(" ");
            }
            if (levelName.headSH_)
            {
                parameters.Add(0f);
                parameterNames.Add("Headshots: ");
                DopSymbols.Add("%");
            }
            if (levelName.bodySH_)
            {
                parameters.Add(0f);
                parameterNames.Add("Bodyshots: ");
                DopSymbols.Add("%");
            }
            if (levelName.damageDelt_)
            {
                parameters.Add(0f);
                parameterNames.Add("Damage delt: ");
                DopSymbols.Add("k");
            }
            if (levelName.damageTaken_)
            {
                parameters.Add(0f);
                parameterNames.Add("Damage taken: ");
                DopSymbols.Add("k");
            }
            if (levelName.exelentShots_)
            {
                parameters.Add(0f);
                parameterNames.Add("Excelent shots: ");
                DopSymbols.Add(" ");

            }
            if (levelName.normalShots_)
            {
                parameters.Add(0f);
                parameterNames.Add("Normal shots: ");
                DopSymbols.Add(" ");
            }
            if (levelName.hits_)
            {
                parameters.Add(0f);
                parameterNames.Add("Hits: ");
                DopSymbols.Add(" ");
            }
            if (levelName.misses_)
            {
                parameters.Add(0f);
                parameterNames.Add("Misses: ");
                DopSymbols.Add(" ");
            }
            if (levelName.targetsLost_)
            {
                parameters.Add(0f);
                parameterNames.Add("Targets lost: ");
                DopSymbols.Add(" ");
            }
            if (levelName.trackAccuracy_)
            {
                parameters.Add(0f);
                parameterNames.Add("Tracking accuracy: ");
                DopSymbols.Add("%");
            }
            if (levelName.trackingTime_)
            {
                parameters.Add(0f);
                parameterNames.Add("Tracking time: ");
                DopSymbols.Add("sec");
            }
            if (levelName.missingTargetTime_)
            {
                parameters.Add(0f);
                parameterNames.Add("Missing target time: ");
                DopSymbols.Add("sec");
            }
            if (levelName.recordTime_)
            {
                parameters.Add(0f);
                parameterNames.Add("Record: ");
                DopSymbols.Add("sec");
            }
            if (levelName.time_)
            {
                parameters.Add(0f);
                parameterNames.Add("Time played ");
                DopSymbols.Add("h");
            }
        }
    }

    private float CountMaxDamage(float time)
    {
        float maxDm = time * 10 * 12f;
        return maxDm / 1000f;
    }

    private float CountProcent(float shots, float allShots)
    {
        float procent = 0f;
        if (shots != 0f && allShots != 0f)
            procent = shots / allShots * 100;
        return procent;
    }

    private float CountDamage(float inputDamage)
    {
        float damageTaken = inputDamage / 1000;
        return damageTaken;
    }

    private float CountTime(float inputTime)
    {
        float time = inputTime / 3600;
        return time;
    }
    private float CountCurrentDamage(float damage)
    {
        return damage / 1000f;
    }
    private float ProcentTakenDamage(float time, float damage)
    {
        float res = 0f;
        if (time != 0f && damage != 0f)
        {
            float maxDm = time * 10 * 12f;
            float procent = damage / maxDm * 100f;
            res = 100f - procent;
        }
        return res;
    }

    public void ChangeWarmupStatistic()
    {
        if (!statisticToggles.activeSelf)
        {
            warmupThing.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Save";
            statisticToggles.SetActive(true);
            foreach(var field in fields)
            {
                field.gameObject.SetActive(false);
            }
            foreach(var drop in drops)
            {
                drop.isOn = false;
            }
            levelSettings.ClearParameters();
        }
        else
        {
            warmupThing.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Change statistic";
            statisticToggles.SetActive(false);
            foreach (var field in fields)
            {
                field.gameObject.SetActive(true);
            }
            warmupManager.SaveWarmups();
            levelName.SetByLevelSettings(levelSettings);
            ChangeStats();
        }
    }

    public void Accuracy_(bool b)
    {
        levelSettings.accuracy_ = b;
    }
    public void Succeeded_(bool b)
    {
        levelSettings.succeeded_ = b;
    }
    public void Failed_(bool b)
    {
        levelSettings.failed_ = b;
    }
    public void DamageDelt_(bool b)
    {
        levelSettings.damageDelt_ = b;
    }
    public void DamageTaken_(bool b)
    {
        levelSettings.damageTaken_ = b;
    }
    public void HeadSH_(bool b)
    {
        levelSettings.headSH_ = b;
    }
    public void BodySH_(bool b)
    {
        levelSettings.bodySH_ = b;
    }
    public void Time_(bool b)
    {
        levelSettings.time_ = b;
    }
    public void Hits_(bool b)
    {
        levelSettings.hits_ = b;
    }
    public void Misses_(bool b)
    {
        levelSettings.misses_ = b;
    }
    public void TargetsLost_(bool b)
    {
        levelSettings.targetsLost_ = b;
    }
    public void ExelentShots_(bool b)
    {
        levelSettings.exelentShots_ = b;
    }
    public void NormalShots_(bool b)
    {
        levelSettings.normalShots_ = b;
    }
    public void TrackAccuracy_(bool b)
    {
        levelSettings.trackAccuracy_ = b;
    }
    public void TrackingTime_(bool b)
    {
        levelSettings.trackingTime_ = b;
    }
    public void MissingTargetTime_(bool b)
    {
        levelSettings.missingTargetTime_ = b;
    }
    public void RecordTime_(bool b)
    {
        levelSettings.recordTime_ = b;
    }

    public void ChangeName(string mess)
    {
        levelSettings.usersName = mess;
        ShowName.text = mess;
        start.ChangeName(mess);
        warmupManager.SaveWarmups();
    }

    public void ChangeTime(string mess)
    {
        float newTime = levelSettings.timeForALevel;
        try
        {
            levelSettings.timeForALevel = float.Parse(mess);
        }
        catch (Exception)
        {
            levelSettings.timeForALevel = newTime;
        }
        warmupManager.SaveWarmups();
    }
}
