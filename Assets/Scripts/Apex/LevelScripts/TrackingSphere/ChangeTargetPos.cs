using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeTargetPos : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private Transform easyPoints, normalPoints, hardPoints;
    private List<Transform> points = new();
    public UnityEvent changePos = new();
    private Vector3 nextPosition;
    private float time, timeForTarget, min, max;

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    min = 1f; max = 1.5f;
                    points.AddRange(easyPoints.GetComponentsInChildren<Transform>());
                    break;
                case "Normal":
                    min = 0.7f; max = 1f;
                    points.AddRange(normalPoints.GetComponentsInChildren<Transform>());
                    break;
                case "Hard":
                    min = 0.5f; max = 0.7f;
                    points.AddRange(hardPoints.GetComponentsInChildren<Transform>());
                    break;
                case "UltraHard":
                    min = 0.3f; max = 0.5f;
                    points.AddRange(hardPoints.GetComponentsInChildren<Transform>());
                    break;

            }
            ChangePosition();
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= timeForTarget)
        {
            ChangePosition();
            time = 0f;
            timeForTarget = Random.Range(min, max);
        }
    }

    public void SetUpWarmup(LevelSettings obj)
    {
        switch (obj.dificulty)
        {
            case "Easy":
                min = 1f; max = 1.5f;
                points.AddRange(easyPoints.GetComponentsInChildren<Transform>());
                break;
            case "Normal":
                min = 0.7f; max = 1f;
                points.AddRange(normalPoints.GetComponentsInChildren<Transform>());
                break;
            case "Hard":
                min = 0.5f; max = 0.7f;
                points.AddRange(hardPoints.GetComponentsInChildren<Transform>());
                break;
            case "UltraHard"://print("fuck");
                min = 0.3f; max = 0.5f;
                points.AddRange(hardPoints.GetComponentsInChildren<Transform>());
                break;

        }
        ChangePosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanGetHitted"))
        {
            ChangePosition();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CanGetHitted"))
        {
            ChangePosition();
        }
    }

    private void ChangePosition()
    {
        changePos.Invoke();
        nextPosition = points[Random.Range(0, points.Count)].position;
        if (transform.position == nextPosition)
            ChangePosition();
        else
            transform.position = nextPosition;
    }
}
