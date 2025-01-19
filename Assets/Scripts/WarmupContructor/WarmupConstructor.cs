using System;
using System.Collections.Generic;
using UnityEngine;

public class WarmupConstructor : MonoBehaviour
{
    private List<LevelSettings> lvlNames = new();
    private WarmupDataHandler data = new();
    private IDataService service = new JsondataService();
    private string usersName = "";
    private float usersTime;
    private bool buttonsModified;
    private int canvasType = 0;
    private List<int> vacantIndexes = new();
    private LevelSettings levelSettings = new("Warmup", "", "", "", "", "", false);

    [SerializeField] private WarmupManager manager;
    [SerializeField] private PopUpCanvas selectedButtonsCanvas;
    [SerializeField] private List<IButtonManager> buttons;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private FastTrainScript warmup;
    [SerializeField] private List<Vector3> points = new();
    [SerializeField] private string relativePath;
    [SerializeField] private Transform canvas;
    [SerializeField] private Vector3 firstPos;
    [SerializeField] private GameObject maxReached;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Transform image;
    [SerializeField] private Vector3[] positions;

    private void Awake()
    {
        int rows = 0;
        int numberInRow = 0;
        var newPoint = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            if(numberInRow > 9)
            {
                numberInRow = 0;
                rows++;
            }
            newPoint = new Vector3(firstPos.x + 70f * numberInRow, firstPos.y - 40f * rows, 0f);
            numberInRow++;
            points.Add(newPoint);
        }
    }

    public void ChangeLvlNames(LevelSettings lvlName, bool add, int deleteIndex, out Vector3 place, out int index)
    {
        if (add)
        {
            try
            {
                if (vacantIndexes.Count > 0)
                {
                    lvlNames[vacantIndexes[0]] = lvlName;
                    index = vacantIndexes[0];
                    place = points[vacantIndexes[0]];
                    vacantIndexes.RemoveAt(0);
                }
                else
                {
                    int num = lvlNames.Count;
                    lvlNames.Insert(num, lvlName);
                    index = num;
                    place = points[num];
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                index = -1;
                place = Vector3.zero;
                GameObject a = Instantiate(maxReached, canvas);
                a.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.identity);
                a.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 1f);
                Destroy(a, 1.25f);
            }
        }
        else
        {
            vacantIndexes.Add(deleteIndex);
            vacantIndexes.Sort();
            place = Vector3.zero;
            index = 0;
            lvlNames[deleteIndex] = null;
        }
    }
    public void Save()
    {
        lvlNames.RemoveAll((LevelSettings name) => name == null);
        service.LoadData(relativePath, false, out data);

        if (data != null)
        {
            CheckLvl();
            levelSettings.petName = "Warmup" + data.LastIndex + 1;
            int imgIndex = UnityEngine.Random.Range(0, images.Length);
            var UsersLvl = new UserDataWarmUpLevel(usersName, usersTime, ++data.LastIndex, lvlNames, levelSettings, imgIndex);
            data.UserPrefs.Add(UsersLvl);
        }
        else
        {
            data = new WarmupDataHandler();
            CheckLvl();
            levelSettings.petName = "Warmup0";
            int imgIndex = UnityEngine.Random.Range(0, images.Length);
            var UsersLvl = new UserDataWarmUpLevel(usersName, usersTime, 0, lvlNames, levelSettings, imgIndex);
            data.UserPrefs.Add(UsersLvl);
        }
        service.SaveData(relativePath, data, false);

        levelName.SetByLevelSettings(levelSettings);
        warmup.GetLevels(lvlNames, levelSettings);
        warmup.StartLevel();
    }

    public void GetName(string name)
    {
        levelSettings.usersName = name;
    }
    public void GetTime(string time)
    {
        try
        {
            levelSettings.timeForALevel = float.Parse(time);
        }
        catch (FormatException)
        {
            levelSettings.timeForALevel = 0f;
        }
    }
    private void CheckLvl()
    {
        if (levelSettings.timeForALevel <= 0f)
        {
            levelSettings.timeForALevel = 30f;
        }
        if (levelSettings.usersName.Length <= 0)
        {
            levelSettings.usersName = "New warmup" + data.LastIndex;
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

    public void ChangeButtons(bool state, IButtonManager butt)
    {
        if (state)
        {
            buttons.Add(butt);
        }
        else
        {
            if (butt.left.Length > 0)
                selectedButtonsCanvas.StartButton = butt.left[0];
            else if (butt.right.Length > 0)
                selectedButtonsCanvas.StartButton = butt.right[0];
            else if (butt.up.Length > 0)
                selectedButtonsCanvas.StartButton = butt.up[0];
            else if (butt.down.Length > 0)
                selectedButtonsCanvas.StartButton = butt.down[0];
            else
                selectedButtonsCanvas.ResetButton();

            buttons.Remove(butt);
        }
        manager.MakeControllerPickable(buttons);
    }

    public void SwitchCanvases()
    {
        if(canvasType != 0)
        {
            image.localPosition = positions[0];
            selectedButtonsCanvas.gameObject.SetActive(false);
            canvasType = 0;
        }
        else if(canvasType != 1)
        {
            image.localPosition = positions[1];
            if(buttons.Count > 3)
                selectedButtonsCanvas.StartButton = buttons[3];
            else
                selectedButtonsCanvas.StartButton = buttons[0];
            manager.MakeControllerPickable(buttons);
            selectedButtonsCanvas.gameObject.SetActive(true);
            canvasType = 1;
        }
    }
}
