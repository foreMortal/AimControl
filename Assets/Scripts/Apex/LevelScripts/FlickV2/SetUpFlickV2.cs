using UnityEngine;

public class SetUpFlickV2 : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private Transform closeRange, middleRange, longRange;

    private Transform target;
    private FlicksV2MovingTargets flicks;

    private void Awake()
    {
        flicks = GetComponent<FlicksV2MovingTargets>();
        target = transform;

        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    target.position = closeRange.position;
                    break;
                case "Middle":
                    target.position = middleRange.position;
                    break;
                case "Long":
                    target.position = longRange.position;
                    break;
            }
            switch (levelName.dificulty)
            {
                case "Easy":
                    flicks.SetUpDificulty(3f, 1.5f, 1.5f, 1f);
                    break;
                case "Normal":
                    flicks.SetUpDificulty(3f, 1.5f, 1.5f, 0.7f);
                    break;
                case "Hard":
                    flicks.SetUpDificulty(2.5f, 1.25f, 1.25f, 0.5f);
                    break;
                case "UltraHard":
                    flicks.SetUpDificulty(2f, 1f, 1f, 0.3f);
                    break;
            }
        }
    }

    public void SetUpWarmup(LevelSettings obj)
    {
        flicks = GetComponent<FlicksV2MovingTargets>();
        target = transform;

        switch (obj.distance)
        {
            case "Close":
                target.position = closeRange.position;
                break;
            case "Middle":
                target.position = middleRange.position;
                break;
            case "Long":
                target.position = longRange.position;
                break;
        }
        switch (obj.dificulty)
        {
            case "Easy":
                flicks.SetUpDificulty(3f, 1.5f, 1.5f, 1f);
                break;
            case "Normal":
                flicks.SetUpDificulty(3f, 1.5f, 1.5f, 0.7f);
                break;
            case "Hard":
                flicks.SetUpDificulty(2.5f, 1.25f, 1.25f, 0.5f);
                break;
            case "UltraHard":
                flicks.SetUpDificulty(2f, 1f, 1f, 0.3f);
                break;
        }
    }
}
