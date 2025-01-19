using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class FastTrainScript : MonoBehaviour
{
    private float _timer;
    private List<LevelSettings> levels = new();
    private LevelSettings levelSettings;

    private int levelCount;
    [NonSerialized] public float timeLimit = 30f;
    private bool lvlEnded, started = false;

    private void Awake()
    {
        SettingsWindow.quitLvl.AddListener(Quitlvl);
        SetUpManager.levelStarted.AddListener(SetupWarmup);
    }
    
    private void SetupWarmup(SetUpManager manager)
    {
        manager.setDirectly.Invoke(levels[levelCount - 1]);
        _timer = 0f;
        lvlEnded = false;
    }

    private void Update()
    {
        if (started)
        {
            _timer += Time.deltaTime;
            if (_timer >= timeLimit && !lvlEnded)
            {
                NextLevel();
                lvlEnded = true;
            }
        }
    }

    public void GetLevels(List<LevelSettings> list, LevelSettings levelSettings)
    {
        this.levelSettings = levelSettings;
        levels = list;
    }

    public void StartLevel()
    {
        timeLimit = levelSettings.timeForALevel;
        started = true;
        DontDestroyOnLoad(gameObject);
        NextLevel();
    }

    public void NextLevel()
    {
        if (levelCount >= levels.Count)
        {
            levelCount = 0;
            LoadNextLvl();
        }
        else
        {
            LoadNextLvl();
        }
    }
    
    private void LoadNextLvl()
    {

        SceneManager.LoadScene(levels[levelCount].type, LoadSceneMode.Single);
        levelCount++;
    }

    private void Quitlvl()
    {
        Destroy(gameObject);
    }
}
