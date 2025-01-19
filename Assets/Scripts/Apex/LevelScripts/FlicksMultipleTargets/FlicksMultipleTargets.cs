using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlicksMultipleTargets : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] GetStatisticScriptableObject stats;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform placesHandler;
    [SerializeField] private float targetsCount, targetsLife, timeForExelentShot;

    public UnityEvent movePoints = new();

    private List<Transform> vacantPlaces = new();
    private List<Transform> occupiedPlaces = new();
    private List<GameObject> targets = new();
    private List<GameObject> hidedTargets = new();

    private float targetsLifeTimer;
    private Text killedText, lostText;

    private void Awake()
    {
        vacantPlaces.AddRange(placesHandler.GetComponentsInChildren<Transform>());
        vacantPlaces.Remove(placesHandler);

        for(int i = 0; i < 6; i++)
        {
            GameObject newTarget = Instantiate(target, Vector3.zero, Quaternion.identity);
            newTarget.GetComponent<MultipleFlicksGetHited>().GetFlicks(this);
            hidedTargets.Add(newTarget);
            newTarget.SetActive(false);
        }

        SpawnTragets(1);

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void SetupDificulty(float targetsCount, float targetsLife, float timeForExelentShot)
    {
        this.targetsCount = targetsCount;
        this.targetsLife = targetsLife;
        this.timeForExelentShot = timeForExelentShot;
    }

    private void Update()
    {
        targetsLifeTimer -= Time.deltaTime;

        if(targetsLifeTimer <= 0f)
        {
            SpawnTragets(targetsCount);
        }
    }

    public void KillTarget(GameObject target)
    {
        if(targetsLifeTimer >= targetsLife - timeForExelentShot)
            stats.exelentShots++;
        else
            stats.normalShots++;
        
        killedText.text = stats.hits.ToString();
        
        targets.Remove(target);
        target.SetActive(false);
        hidedTargets.Add(target);

        targetsLifeTimer += targetsLife / 0.5f;

        if(targets.Count <= 0)
        {
            SpawnTragets(targetsCount);
        }
    }

    private void TragetsLifeEnded()
    {
        vacantPlaces.AddRange(occupiedPlaces);
        foreach(var target in targets)
        {
            target.SetActive(false);
            hidedTargets.Add(target);

            lostText.text = (++stats.targetsLost).ToString();
        }
        targets.Clear();
        occupiedPlaces.Clear();
    }

    private void SpawnTragets(float targetsCount)
    {
        movePoints.Invoke();
        targetsLifeTimer = targetsLife;

        if(targets.Count > 0)
        {
            TragetsLifeEnded();
        }
        else
        {
            vacantPlaces.AddRange(occupiedPlaces);
            HideTargets();
            targets.Clear();
            occupiedPlaces.Clear();
        }

        for(int i = 0; i < targetsCount; i++)
        {
            int index = Random.Range(0, vacantPlaces.Count);

            GameObject t = hidedTargets[0];
            targets.Add(t);
            t.transform.SetPositionAndRotation(vacantPlaces[index].position, Quaternion.identity);
            t.SetActive(true);
            hidedTargets.RemoveAt(0);

            occupiedPlaces.Add(vacantPlaces[index]);
            vacantPlaces.RemoveAt(index);
        }
    }

    private void HideTargets()
    {
        foreach(var t in targets)
        {
            t.SetActive(false);
            hidedTargets.Add(t);
        }
    }
}
