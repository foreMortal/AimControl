using UnityEngine;

public class MultipleFlicksSetup : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GameObject closeRange, middleRange, longRange;
    [SerializeField] private FlicksMultipleTargets flicks;

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    closeRange.SetActive(true); break;
                case "Middle":
                    middleRange.SetActive(true); break;
                case "Long":
                    longRange.SetActive(true); break;
            }
            switch (levelName.dificulty)
            {
                case "Easy":
                    flicks.SetupDificulty(2f, 2f, 1f); break;
                case "Normal":
                    flicks.SetupDificulty(3f, 1f, 0.5f); break;
                case "Hard":
                    flicks.SetupDificulty(4f, 0.7f, 0.3f); break;
                case "UltraHard":
                    flicks.SetupDificulty(5f, 0.5f, 0.3f); break;
            }
        }
    }

    public void WarmupSetup(LevelSettings obj)
    {
        switch (obj.distance)
        {
            case "Close":
                closeRange.SetActive(true); break;
            case "Middle":
                middleRange.SetActive(true); break;
            case "Long":
                longRange.SetActive(true); break;
        }
        switch (obj.dificulty)
        {
            case "Easy":
                flicks.SetupDificulty(2f, 2f, 1f); break;
            case "Normal":
                flicks.SetupDificulty(3f, 1f, 0.5f); break;
            case "Hard":
                flicks.SetupDificulty(4f, 0.7f, 0.3f); break;
            case "UltraHard":
                flicks.SetupDificulty(5f, 0.5f, 0.3f); break;
        }
    }
}
