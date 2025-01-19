using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFlicksManager : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GameObject closePos, middlePos, longPos;
    [SerializeField] private FlicksPointPosition targetsPos;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Text text;
    [SerializeField] private Transform positionsHandler;
    [SerializeField] private int trainTargetsCount, statisticTargetsCount;

    private Text killedText, lostText;
    private float startTimer = 2.5f, procent, faideofTimer, betweenHitsTimer, exelentShotTime;
    private bool started, turnExtra;
    private int type = 0;
    private int trainTargetsHited, statTargetsHited;
    private Transform[] positions;
    private List<Transform> positionsLeft = new List<Transform>();
    private CameraMoveParent[] cameraMove;
    private GameObject[] targets;

    private void OnEnable()
    {
        stats.active = false;
    }
    private void OnDisable()
    {
        stats.active = true;
    }

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Close":
                    closePos.SetActive(true); break;
                case "Middle":
                    middlePos.SetActive(true); break;
                case "Long":
                    longPos.SetActive(true); break;
                default:
                    closePos.SetActive(true); break;
            }
            switch (levelName.dificulty)
            {
                case "Easy":
                    procent = 0.25f; turnExtra = false; exelentShotTime = 1.5f; break;
                case "Normal":
                    procent = 0.5f; turnExtra = false; exelentShotTime = 1f; break;
                case "Hard":
                    procent = 0.75f; turnExtra = true; exelentShotTime = 0.5f; break;
                case "UltraHard":
                    procent = 1f; turnExtra = true; exelentShotTime = 0.3f; break;
            }
            targetsPos.MovePoints();
        }

        positions = positionsHandler.GetComponentsInChildren<Transform>();
        positionsLeft.AddRange(positions);

        List<GameObject> tars = new List<GameObject>();
        for(int i = 0; i < trainTargetsCount; i++)
        {
            Transform t = positionsLeft[Random.Range(0, positionsLeft.Count)];
            positionsLeft.Remove(t);
            GameObject g = Instantiate(targetPrefab, t.position, Quaternion.identity);
            g.GetComponent<SpecialFlicksGetHited>().SelfAwake(this);
            tars.Add(g);
            g.name = "target" + tars.Count;
        }
        targets = tars.ToArray();

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas c2 = Instantiate(canvas, transform);

        c2.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = c2.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(type == 1)
        {
            betweenHitsTimer += Time.deltaTime;
        }
        if(startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            text.text = "Start in: " + startTimer.ToString("F2");
        }
        else if (!started)
        {
            cameraMove = fpsCam.GetComponentsInChildren<CameraMoveParent>();
            foreach(var cam in cameraMove)
            {
                if (cam != null)
                cam.RandomSense(procent, turnExtra);
            }
            text.text = "Sense Changed!";
            faideofTimer = 0.7f;
            started = true;
        }
        if(faideofTimer > 0f)
        {
            float a = text.color.a - 1.66f * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, a);
        }
    }

    private void ReRangeTargets()
    {
        positionsLeft.Clear();
        positionsLeft.AddRange(positions);

        if(type == 0)
        {
            for (int i = 0; i < trainTargetsCount; i++)
            {
                Transform t = positionsLeft[Random.Range(0, positionsLeft.Count)];
                positionsLeft.Remove(t);
                targets[i].transform.position = t.position;
                targets[i].SetActive(true);
            }
        }
        else if(type == 1)
        {
            for (int i = 0; i < statisticTargetsCount; i++)
            {
                Transform t = positionsLeft[Random.Range(0, positionsLeft.Count)];
                positionsLeft.Remove(t);
                targets[i].transform.position = t.position;
                targets[i].SetActive(true);
            }
        }
    }

    public void TargetHited()
    {
        if(type == 0)
        {
            if (++trainTargetsHited >= trainTargetsCount)
            {
                targetsPos.MovePoints();
                trainTargetsHited = 0;
                type = 1;
                stats.active = true;
                ReRangeTargets();
            }
        }
        else if (type == 1)
        {
            if (betweenHitsTimer <= exelentShotTime)
                killedText.text = (++stats.exelentShots).ToString();
            else
                lostText.text = (++stats.normalShots).ToString();

            betweenHitsTimer = 0f;
            if (++statTargetsHited >= statisticTargetsCount)
            {
                targetsPos.MovePoints();
                statTargetsHited = 0;
                type = 0;
                stats.active = false;
                ReRangeTargets();
                foreach (var cam in cameraMove)
                {
                    cam.RandomSense(procent, turnExtra);
                }
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                text.text = "Sense Changed!";
                faideofTimer = 0.7f;
            }
        }
    }

    public void WarmupSetup(LevelSettings obj)
    {
        if (obj.type == "Warmup")
        {
            switch (obj.distance)
            {
                case "Close":
                    closePos.SetActive(true); break;
                case "Middle":
                    middlePos.SetActive(true); break;
                case "Long":
                    longPos.SetActive(true); break;
                default:
                    closePos.SetActive(true); break;
            }
            switch (obj.dificulty)
            {
                case "Easy":
                    procent = 0.25f; turnExtra = false; exelentShotTime = 0.8f; break;
                case "Normal":
                    procent = 0.5f; turnExtra = false; exelentShotTime = 0.6f; break;
                case "Hard":
                    procent = 0.75f; turnExtra = true; exelentShotTime = 0.35f; break;
                case "UltraHard":
                    procent = 1f; turnExtra = true; exelentShotTime = 0.2f; break;
            }
            targetsPos.MovePoints();
        }
    }
}
