using UnityEngine;

public class TargetingSphere : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private Transform hiddenPlace;
    [SerializeField] private FlicksPointPosition pos;
    [SerializeField] private Transform[] Easy, Normal, Hard;
    private Transform[] points;
    private MoveSphere sphere;
    bool hidden;
    private float targetingTime = 5f, underGroundTime;

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    points = Easy;
                    break;
                case "Normal":
                    points = Normal;
                    break;
                case "Hard":
                case "UltraHard":
                    points = Hard;
                    break;
            }
        }
        sphere = GetComponent<MoveSphere>();
        targetingTime = Random.Range(2.5f, 5f);
    }

    private void Update()
    {
        if (targetingTime > 0f)
        {
            targetingTime -= Time.deltaTime;
        }
        else if (targetingTime <= 0f && !hidden)
        {
            hidden = true;
            sphere.HideSphere(hidden);
            transform.position = new Vector3(transform.position.x, hiddenPlace.position.y, transform.position.z);
            underGroundTime = Random.Range(0.5f, 1f);
        }
        if (underGroundTime > 0f)
        {
            underGroundTime -= Time.deltaTime;
        }
        else if (underGroundTime <= 0f && hidden)
        {
            hidden = false;
            sphere.HideSphere(hidden);
            pos.MovePoints();
            transform.position = points[Random.Range(0, points.Length)].position;
            targetingTime = Random.Range(2.5f, 5f);
        }
    }

    public void SetWarmUp(LevelSettings obj)
    {
        if (obj.type != "Warmup")
        {
            switch (obj.dificulty)
            {
                case "Easy":
                    points = Easy;
                    break;
                case "Normal":
                    points = Normal;
                    break;
                case "Hard":
                case "UltraHard":
                    points = Hard;
                    break;
            }
        }
    }
}
