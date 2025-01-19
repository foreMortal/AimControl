using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmupManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private Transform canvas;
    [SerializeField] private string relativePath;
    [SerializeField] private LevelNameObjject obj;
    [SerializeField] private NewerPickLevel pick;
    [SerializeField] GameObject deleteMark;
    [SerializeField] private FastTrainScript warmUp;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Slider slider;
    public List<IButtonManager> buttons = new();

    [NonSerialized] public int lvlCount;

    private DeleteCustomLevels deleter;
    private IDataService service = new JsondataService();
    private WarmupDataHandler data = new();

    private void Awake()
    {
        deleter = GetComponent<DeleteCustomLevels>();
        //join to relative path Apex/CoD
        if (service.LoadData(relativePath, false, out data))
        {
            int numberInRow = 0;
            int rows = 0;
            for(int i = 0; i < data.UserPrefs.Count; i++)
            {
                lvlCount++;
                if(numberInRow > 3) 
                {
                    numberInRow = 0;
                    rows++;
                }

                GameObject butt = Instantiate(button, canvas);
                buttons.Add(butt.GetComponent<IButtonManager>());
                butt.transform.SetLocalPositionAndRotation(new Vector3(-262.5f + numberInRow++ * 175f, 10f - 105f * rows, 0f), Quaternion.identity);
                butt.GetComponent<Image>().sprite = images[data.UserPrefs[i].ImageIndex];

                var script = butt.GetComponent<StartFastTrainScript>();

                deleter.DeleteModOn.AddListener(script.SetDeleteTrue);
                deleter.CancelDeleting.AddListener(script.CancelInvoked);

                script.giveToDelete.AddListener(deleter.AddLevelToDelete);
                script.revertToDelete.AddListener(deleter.RevertLevelToDelete);
                script.deleteMark = deleteMark;
                script.index = data.UserPrefs[i].Index;
                script.levels = data.UserPrefs[i].Levels;
                script.time = data.UserPrefs[i].LevelSettings.timeForALevel;
                script.levelName = obj;
                script.pick = pick;
                script.lS = data.UserPrefs[i].LevelSettings;
                script.warmupStart = warmUp;

                butt.GetComponentInChildren<Text>().text = data.UserPrefs[i].LevelSettings.usersName;
            }

            MakeControllerPickable(buttons);
            
            if(rows > 1)
            {
                slider.gameObject.SetActive(true);

                if(rows > 2)
                {
                    slider.maxValue = 40 + 105 * (rows - 2);
                }
            }
            else
            {
                slider.gameObject.SetActive(false);
            }
        }
    }

    public void MakeControllerPickable(List<IButtonManager> buttons)
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if (i - 1 >= 0)
                buttons[i].left = new IButtonManager[] { buttons[i - 1] };
            else
                buttons[i].left = new IButtonManager[] {};

            if (i + 1 <= buttons.Count - 1)
                buttons[i].right = new IButtonManager[] { buttons[i + 1] };
            else
                buttons[i].right = new IButtonManager[] { };

            if (i - 4 >= 0)
                buttons[i].up = new IButtonManager[] { buttons[i - 4] };
            else
                buttons[i].up = new IButtonManager[] { };

            if (i + 4 <= buttons.Count - 1)
                buttons[i].down = new IButtonManager[] { buttons[i + 4] };
            else
                buttons[i].down = new IButtonManager[] { };
        }
    }

    public void SaveWarmups()
    {
        service.SaveData(relativePath, data, false);
    }
}
