using UnityEngine;

public class PassCheckSetupDistance : MonoBehaviour
{
    [SerializeField] private Transform[] walls, closePos, middlePos, longPos;
    [SerializeField] private LevelNameObjject levelName;

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    for(int i = 0; i < walls.Length; i++)
                    {
                        walls[i].position = closePos[i].position;
                    }
                    break;
                case "Middle":
                    for (int i = 0; i < walls.Length; i++)
                    {
                        walls[i].position = middlePos[i].position;
                    }
                    break;
                case "Long":
                    for (int i = 0; i < walls.Length; i++)
                    {
                        walls[i].position = longPos[i].position;
                    }
                    break;
            }
        }
    }

    public void SetupWarmup(LevelSettings obj)
    {
        switch (obj.distance)
        {
            case "Close":
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].position = closePos[i].position;
                }
                break;
            case "Middle":
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].position = middlePos[i].position;
                }
                break;
            case "Long":
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].position = longPos[i].position;
                }
                break;
        }
    }
}
