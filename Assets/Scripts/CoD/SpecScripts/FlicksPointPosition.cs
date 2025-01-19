using UnityEngine;

public class FlicksPointPosition : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] private Transform points, EasyTargets, NormalTargets, HardTargets;
    private float range = 10f;
    RaycastHit hit;
    private LayerMask mask = 1 << 10;

    private void Awake()
    {
        points = EasyTargets;
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    points = EasyTargets;
                    break;
                case "Normal":
                    points = NormalTargets;
                    break;
                case "Hard":
                case "UltraHard":
                    points = HardTargets;
                    break;
            }
            points.gameObject.SetActive(true);
            MovePoints();
        }
    }

    public void MovePoints()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
        {
            points.position = new Vector3(hit.point.x, points.position.y, hit.point.z);
        }
        points.LookAt(new Vector3(transform.position.x, points.position.y, transform.position.z));
    }


    public void SetUpWarmup(LevelSettings obj)
    {
        switch (obj.dificulty)
        {
            case "Easy":
                points = EasyTargets;
                break;
            case "Normal":
                points = NormalTargets;
                break;
            case "Hard":
            case "UltraHard":
                points = HardTargets;
                break;
        }
        points.gameObject.SetActive(true);
    }
}
