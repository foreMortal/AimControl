using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlicksV2MovingTargets : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] GameObject Target;
    [SerializeField] Transform player;
    [SerializeField] private float lifeTime, scaleUpTime, scaleDownTime, timeBetweenSpawns = 1f, startScale, maxScale;

    private List<GameObject> targets = new();
    private List<GameObject> hidedTargets = new();
    private List<float> targetsLifeTime = new();
    private List<Transform> VacantPlaces = new(), occupiedPlaces = new();

    private Text killedText, lostText;
    private Transform[] places;
    private float globalTimer, deltaTime, spawnTimer = 1f;

    private void Awake()
    {
        places = GetComponentsInChildren<Transform>();

        foreach (var place in places)
        {
            VacantPlaces.Add(place);
        }

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();

        for(int i = 0; i < 20; i++)
        {
            GameObject newTarget = Instantiate(Target, Vector3.zero, Quaternion.identity);
            newTarget.GetComponent<GetHitedFlickV2>().GetFlicks(this);
            newTarget.SetActive(false);
            hidedTargets.Add(newTarget);
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
        globalTimer += deltaTime;
        SpawnerLogic();

        for(int i = 0; i < targets.Count; i++)
        {
            if(globalTimer < (targetsLifeTime[i] - scaleDownTime))
            {        //        here time it lived  |          get time it can live                 | and here we get "procent" of life it can live
                     //                 |          |                                               |
                     float scalingUpDelta = (scaleUpTime - (targetsLifeTime[i] - scaleDownTime - globalTimer)) / scaleUpTime;
                     //increasing scale by "procent" of alredy liven life so it will be at maxScale on middle of its life
                     targets[i].transform.localScale = new Vector3(maxScale * scalingUpDelta, maxScale * scalingUpDelta, maxScale * scalingUpDelta);
            }
            else
            {
                float scalingDownDelta = (targetsLifeTime[i] - globalTimer) / scaleDownTime;
                targets[i].transform.localScale = new Vector3(maxScale * scalingDownDelta, maxScale * scalingDownDelta, maxScale * scalingDownDelta);
            }
        }
        
        DieingLogic();
    }

    private void Spawn()
    {
        int index = Random.Range(0, VacantPlaces.Count);

        GameObject t = hidedTargets[0];
        t.transform.SetPositionAndRotation(VacantPlaces[index].position, Quaternion.identity);
        t.SetActive(true);
        targets.Add(t);
        hidedTargets.RemoveAt(0);

        occupiedPlaces.Add(VacantPlaces[index]);
        targetsLifeTime.Add(globalTimer + lifeTime);

        VacantPlaces.RemoveAt(index);
    }

    private void DieingLogic()
    {
        for (int i = 0; i < targetsLifeTime.Count; i++)
        {
            if (globalTimer >= targetsLifeTime[i])
            {
                VacantPlaces.Add(occupiedPlaces[i]);

                targetsLifeTime.RemoveAt(i);
                occupiedPlaces.RemoveAt(i);

                GameObject t = targets[i];
                t.SetActive(false);
                hidedTargets.Add(t);
                targets.RemoveAt(i);

                lostText.text = (++stats.targetsLost).ToString();
            }
        }
    }

    private void SpawnerLogic()
    {
        spawnTimer += deltaTime;
        if (spawnTimer >= timeBetweenSpawns)
        {
            spawnTimer = 0f;
            if (VacantPlaces.Count > 0)
            {
                Spawn();
            }
        }
    }

    public void TargetDied(GameObject target)
    {
        int index = targets.IndexOf(target);
        VacantPlaces.Add(occupiedPlaces[index]);

        targetsLifeTime.RemoveAt(index);
        occupiedPlaces.RemoveAt(index);

        target.SetActive(false);
        hidedTargets.Add(target);

        targets.Remove(target);

        killedText.text = (++stats.hits).ToString();
    }

    public void SetUpDificulty(float lifeTime, float scaleUp, float scaleDown, float betweenSpawn, float maxScale = 0.2f)
    {
        this.lifeTime = lifeTime;
        scaleUpTime = scaleUp;
        scaleDownTime = scaleDown;
        timeBetweenSpawns = betweenSpawn;
        this.maxScale = maxScale;
    }
}
