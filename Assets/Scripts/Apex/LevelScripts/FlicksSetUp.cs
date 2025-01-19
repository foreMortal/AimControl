using UnityEngine;

public class FlicksSetUp : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] GameObject[] range;
    [SerializeField]private ApexFlickShotsBehavior flicks;

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    range[0].SetActive(true);
                    break;
                case "Middle":
                    range[1].SetActive(true);
                    break;
                case "Long":
                    range[2].SetActive(true);
                    break;
            }
            switch (levelName.dificulty)
            {
                case "Easy":
                    flicks.SetUp(0.5f, 3f);
                    break;
                case "Normal":
                    flicks.SetUp(0.5f, 1f);
                    break;
                case "Hard":
                    flicks.SetUp(0.3f, 0.7f);
                    break;
                case "UltraHard":
                    flicks.SetUp(0.3f, 0.6f);
                    break;
            }
        }
    }

    public void SetWarmup(LevelSettings sett)
    {
        switch (sett.distance)
        {
            case "Close":
                range[0].SetActive(true);
                break;
            case "Middle":
                range[1].SetActive(true);
                break;
            case "Long":
                range[2].SetActive(true);
                break;
        }
        switch (sett.dificulty)
        {
            case "Easy":
                flicks.SetUp(0.5f, 3f);
                break;
            case "Normal":
                flicks.SetUp(0.5f, 1f);
                break;
            case "Hard":
                flicks.SetUp(0.3f, 0.7f);
                break;
            case "UltraHard":
                flicks.SetUp(0.3f, 0.6f);
                break;
        }
    }
}
