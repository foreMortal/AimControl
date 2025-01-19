using UnityEngine;

public class ReturnSideSpawn : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] private Transform closeSpawn, middleSpawn, longSpawn;

    private Transform[] closeSpawns, middleSpawns, longSpawns;
    private int range;

    private void Awake()
    {
        if (levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    closeSpawns = closeSpawn.GetComponentsInChildren<Transform>();
                    range = 0; break;
                case "Middle":
                    middleSpawns = middleSpawn.GetComponentsInChildren<Transform>();
                    range = 1; break;
                case "Long":
                    longSpawns = longSpawn.GetComponentsInChildren<Transform>();
                    range = 2; break;
            }
        }
    }

    public void SetupWarmup(LevelSettings obj)
    {
        switch (obj.distance)
        {
            case "Close":
                closeSpawns = closeSpawn.GetComponentsInChildren<Transform>();
                range = 0; break;
            case "Middle":
                middleSpawns = middleSpawn.GetComponentsInChildren<Transform>();
                range = 1; break;
            case "Long":
                longSpawns = longSpawn.GetComponentsInChildren<Transform>();
                range = 2; break;
        }
    }

    public Vector3 Return()
    {
        switch (range)
        {
            case 0:
                return closeSpawns[Random.Range(0, closeSpawns.Length)].position;
            case 1:
                return middleSpawns[Random.Range(0, middleSpawns.Length)].position;
            case 2:
                return longSpawns[Random.Range(0, longSpawns.Length)].position;
            default:
                goto case 0;
        }
    }
}
