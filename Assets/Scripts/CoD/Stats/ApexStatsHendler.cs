using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ApexStatsHendler : MonoBehaviour
{
    public LevelNameObjject levelName;
    [SerializeField] private GetStatisticScriptableObject statsObject;
    [SerializeField] private Text[] EndGameMenu;
    
    //[SerializeField] private bool accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_;
    //[SerializeField] private bool flikNormalShots, flikExelentShots, trackingTime_, missingTargetTime_, recordTime_, trackAccur_;
    
    public static UnityEvent<GetStatisticScriptableObject> objectPass = new();
    public static UnityEvent LevelQuit = new();

    private List<Text> StopGameMenu;
    private List<float> parameters = new();
    private List<string> parametersName = new();
    private List<string> DopSymbols = new();
    private float accuracy=0f, succeded=0f, failed=0f, damageDelt=0f, damageTaken=0f, headSH=0f, bodySH=0f, time=0f, hit, miss, lost, normal, exelent;
    private float trackTime, missTime, record, trackAccur;

    private IDataService service = new JsondataService();
    private PlayerStats level = new();
    private PlayerStatsDataHandler data = new();

    public void GetStopGameTextFields(List<Text> fields)
    {
        StopGameMenu = fields;
    }

    private void Start()
    {
        objectPass.Invoke(statsObject);
    }

    private void Awake()
    {
        LevelQuit.AddListener(Save);

        if(levelName.type != "Warmup")
        {
            statsObject.Clear();
        }
    }

    public void OpenStopMenu()
    {
        Calculate();
        for(int i = 0; i < parameters.Count; i++) 
        {
            if(i < StopGameMenu.Count)
                StopGameMenu[i].text = parametersName[i] + parameters[i].ToString("F2") + DopSymbols[i];
        }
        parameters.Clear();
    }

    public void OpenEndMenu()
    {
        Calculate();
        for (int i = 0; i < parameters.Count; i++)
        {
            if (i < EndGameMenu.Length)
                EndGameMenu[i].text = parametersName[i] + parameters[i].ToString("F2") + DopSymbols[i];
        }
        parameters.Clear();
    }

    public void Save()
    {
        service.LoadData("/Apex-stats", true, out data);

        if(data != null)
        {
            try
            {
                level = data.Levels[levelName.MakeLevelName()];
            }
            catch (KeyNotFoundException)
            {
                level = new PlayerStats();
                SetUpNewLevel();
            }
            
        }
        else
        {
            level = new PlayerStats();
            SetUpNewLevel();
        }

        level.Stats["AllShots"] += statsObject.allShots;
        level.Stats["Hit"] += statsObject.hits;
        level.Stats["Succeeded"] += statsObject.succeeded;
        level.Stats["Failed"] += statsObject.failed;
        level.Stats["HeadShots"] += statsObject.headShots;
        level.Stats["BodyShots"] += statsObject.bodyShots;
        level.Stats["TimePlayed"] += statsObject.timePlayed;
        level.Stats["DamageDelt"] += statsObject.playerDamageDelt;
        level.Stats["DamageTaken"] += statsObject.playerDamageTaken;
        level.Stats["Miss"] += statsObject.misses;
        level.Stats["TargetsLost"] += statsObject.targetsLost;
        level.Stats["ExelentShots"] += statsObject.exelentShots;
        level.Stats["NormalShots"] += statsObject.normalShots;
        level.Stats["TrackingTime"] += statsObject.trackingTime;
        level.Stats["MissingTime"] += statsObject.missingTargetTime;

        if (statsObject.recordTime > level.Stats["RecordTime"])
        {
            level.Stats["RecordTime"] = statsObject.recordTime;
        }

        if(data != null)
        {
            data.Levels[levelName.MakeLevelName()] = level;
        }
        else
        {
            data = new();
            data.Levels[levelName.MakeLevelName()] = level;
        }

        service.SaveData("/Apex-stats", data, true);
        
        statsObject.Clear();
    }

    private void Calculate()
    {
        if (levelName.accuracy_)
        {
            accuracy = CountProcent(statsObject.hits, statsObject.allShots);
            parameters.Add(accuracy);
            parametersName.Add("Accuracy: ");
            DopSymbols.Add("%");
        }
        if (levelName.succeeded_)
        {
            succeded = statsObject.succeeded;
            parameters.Add(succeded);
            parametersName.Add("Succeeded: ");
            DopSymbols.Add(" ");
        }
        if (levelName.failed_)
        {
            failed = statsObject.failed;
            parameters.Add(failed);
            parametersName.Add("Failed: ");
            DopSymbols.Add(" ");
        }
        if (levelName.headSH_)
        {
            headSH = CountProcent(statsObject.headShots, statsObject.hits);
            parameters.Add(headSH);
            parametersName.Add("Headshots: ");
            DopSymbols.Add("%");
        }
        if (levelName.bodySH_)
        {
            bodySH = CountProcent(statsObject.bodyShots, statsObject.hits);
            parameters.Add(bodySH);
            parametersName.Add("Bodyshots: ");
            DopSymbols.Add("%");
        }
        if (levelName.damageDelt_)
        {
            damageDelt = statsObject.playerDamageDelt;
            parameters.Add(damageDelt);
            parametersName.Add("Damage delt: ");
            DopSymbols.Add(" ");
        }
        if (levelName.damageTaken_)
        {
            damageTaken = statsObject.playerDamageTaken;
            parameters.Add(damageTaken);
            parametersName.Add("Damage taken: ");
            DopSymbols.Add(" ");
        }
        if (levelName.exelentShots_)
        {
            exelent = statsObject.exelentShots;
            parameters.Add(exelent);
            parametersName.Add("Excelent shots: ");
            DopSymbols.Add(" ");
        }
        if (levelName.normalShots_)
        {
            normal = statsObject.normalShots;
            parameters.Add(normal);
            parametersName.Add("Normal shots: ");
            DopSymbols.Add(" ");
        }
        if (levelName.hits_)
        {
            hit = statsObject.hits;
            parameters.Add(hit);
            parametersName.Add("Hits: ");
            DopSymbols.Add(" ");
        }
        if (levelName.misses_)
        {
            miss = statsObject.misses;
            parameters.Add(miss);
            parametersName.Add("Misses: ");
            DopSymbols.Add(" ");
        }
        if (levelName.targetsLost_)
        {
            lost = statsObject.targetsLost;
            parameters.Add(lost);
            parametersName.Add("Targets lost: ");
            DopSymbols.Add(" ");
        }
        if (levelName.trackAccuracy_)
        {
            trackAccur = CountProcent(statsObject.trackingTime, statsObject.timePlayed);
            parameters.Add(trackAccur);
            parametersName.Add("Tracking accuracy: ");
            DopSymbols.Add("%");
        }
        if (levelName.trackingTime_)
        {
            trackTime = statsObject.trackingTime;
            parameters.Add(trackTime);
            parametersName.Add("Tracking time: ");
            DopSymbols.Add("sec");
        }
        if (levelName.missingTargetTime_)
        {
            missTime = statsObject.missingTargetTime;
            parameters.Add(missTime);
            parametersName.Add("Missing target time: ");
            DopSymbols.Add("sec");
        }
        if (levelName.recordTime_)
        {
            record = statsObject.recordTime;
            parameters.Add(record);
            parametersName.Add("Record: ");
            DopSymbols.Add("sec");
        }
        if (levelName.time_)
        {
            time = CountTime(statsObject.timePlayed);
            parameters.Add(time);
            parametersName.Add("Time played: ");
            DopSymbols.Add("sec");
        }
    }
    private float CountProcent(float shots, float allshots)
    {
        float procent = 0f;
        if(shots != 0f && allshots != 0f)
            procent = shots / allshots * 100;
        return procent;
    }
    private float CountTime(float timeplayed)
    {
        if(timeplayed >= 600)
        {
            float time = timeplayed / 60;
            return time;
        }
        else
        {
            return timeplayed;
        }
    }

    private void SetUpNewLevel()
    {
        level.Stats["AllShots"] = 0f;
        level.Stats["Hit"] = 0f;
        level.Stats["Succeeded"] = 0f;
        level.Stats["Failed"] = 0f;
        level.Stats["HeadShots"] = 0f;
        level.Stats["BodyShots"] = 0f;
        level.Stats["TimePlayed"] = 0f;
        level.Stats["DamageDelt"] = 0f;
        level.Stats["DamageTaken"] = 0f;
        level.Stats["Miss"] = 0f;
        level.Stats["TargetsLost"] = 0f;
        level.Stats["ExelentShots"] = 0f;
        level.Stats["NormalShots"] = 0f;
        level.Stats["TrackingTime"] = 0f;
        level.Stats["MissingTime"] = 0f;
        level.Stats["RecordTime"] = 0f;
    }
}