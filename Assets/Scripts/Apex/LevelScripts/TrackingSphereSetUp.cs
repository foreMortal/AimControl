using UnityEngine;

public class TrackingSphereSetUp : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] TrackingSphereScriptableObject easy, normal, hard, ultraHard;
    //[SerializeField] private StatsTracking player;
    [SerializeField] private TargetingSphere targetingSphere;
    [SerializeField] private GameObject closeRange, middleRange, longRange;
    private MoveSphere sphere;


    private void Awake()
    {
        sphere = GetComponent<MoveSphere>();

        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                   // player.SetUp(easy);
                    sphere.SetUp(easy);
                    break;
                case "Normal":
                   // player.SetUp(normal);
                    sphere.SetUp(normal);
                    break;
                case "Hard":
                    //player.SetUp(hard);
                    sphere.SetUp(hard);
                    break;
                case "UltraHard":
                    //player.SetUp(ultraHard);
                    sphere.SetUp(ultraHard);
                    break;
                default:
                   // player.SetUp(normal);
                    sphere.SetUp(normal);
                    break;

            }

            switch (levelName.distance)
            {
                case "Close":
                    closeRange.SetActive(true);
                    break;
                case "Middle":
                    middleRange.SetActive(true);
                    break;
                case "Long":
                    longRange.SetActive(true);
                    break;
            }

            if(levelName.targeting)
                targetingSphere.enabled = true;
        }
    }

    public void SetWarmup(LevelSettings obj)
    {
        sphere = GetComponent<MoveSphere>();

        switch (obj.dificulty)
        {
            case "Easy":
               // player.SetUp(easy);
                sphere.SetUp(easy);
                break;
            case "Normal":
               // player.SetUp(normal);
                sphere.SetUp(normal);
                break;
            case "Hard":
              //  player.SetUp(hard);
                sphere.SetUp(hard);
                break;
            case "UltraHard":
               // player.SetUp(ultraHard);
                sphere.SetUp(ultraHard);
                break;
        }

        switch (obj.distance)
        {
            case "Close":
                closeRange.SetActive(true);
                break;
            case "Middle":
                middleRange.SetActive(true);
                break;
            case "Long":
                longRange.SetActive(true);
                break;
        }

        if (obj.targeting)
            targetingSphere.enabled = true;
    } 
}
