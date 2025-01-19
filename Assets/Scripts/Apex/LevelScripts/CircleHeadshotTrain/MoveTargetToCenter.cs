using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveTargetToCenter : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private Transform center, pointsHandler;
    [SerializeField] private GameObject dummyPrfab;
    [SerializeField] private float speed, maxTargets, timeBetweenSpawns;

    public UnityEvent movePoints = new();

    private Text lostText, killedText;
    private bool warmup;
    LevelSettings lS = new();
    private float spawnTimer;
    private List<GameObject> targets = new();
    private List<GameObject> hidedTargets = new();
    [SerializeField] private List<Transform> positions = new();

    private void Awake()
    {
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    maxTargets = 1; break;
                case "Normal":
                    maxTargets = 2; break;
                case "Hard":
                    maxTargets = 3; break;
                case "UltraHard":
                    maxTargets = 4; break;
            }
        }

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();
        
        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();

        positions.AddRange(pointsHandler.GetComponentsInChildren<Transform>());
        positions.Remove(pointsHandler);
        

        for(int i = 0; i < maxTargets + 1; i++)
        {
            GameObject newTarget = Instantiate(dummyPrfab, positions[Random.Range(0, positions.Count)].position, Quaternion.identity);
            newTarget.GetComponent<SetUpCircleHeadGetHited>().Setup(this);
            if (warmup)
                newTarget.GetComponent<CrouchOnly>().SetupWarmup(lS);
            hidedTargets.Add(newTarget);
            newTarget.SetActive(false);
        }
    }

    public void WsrmupSetup(LevelSettings obj)
    {
        warmup = true;
        lS = obj;
        switch (obj.dificulty)
        {
            case "Easy":
                maxTargets = 1; break;
            case "Normal":
                maxTargets = 2; break;
            case "Hard":
                maxTargets = 3; break;
            case "UltraHard":
                maxTargets = 4; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            RemoveT(other.gameObject);

            lostText.text = (++stats.targetsLost).ToString();
        }
    }

    private void Update()
    {
        foreach(var target in targets)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, new Vector3(center.position.x, target.transform.position.y, center.position.z), speed * Time.deltaTime);
        }

        spawnTimer += Time.deltaTime;
        if(spawnTimer >= timeBetweenSpawns)
        {
            if (targets.Count < maxTargets)
                Spawn();
        }
    }

    public void KillTarget(GameObject target)
    {
        RemoveT(target);

        killedText.text = stats.hits.ToString();
    }

    private void RemoveT(GameObject target)
    {
        targets.Remove(target);
        target.SetActive(false);
        hidedTargets.Add(target);
    }

    private void Spawn()
    {
        movePoints.Invoke();
        spawnTimer = 0f;

        if(hidedTargets.Count > 0)
        {
            GameObject t = hidedTargets[0];
            t.transform.position = positions[Random.Range(0, positions.Count)].position;
            t.SetActive(true);
            hidedTargets.RemoveAt(0);
            targets.Add(t);
        }
    }
}
