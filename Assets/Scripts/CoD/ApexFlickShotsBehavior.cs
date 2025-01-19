using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ApexFlickShotsBehavior : MonoBehaviour, IHitable
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private Transform hndl;
    [SerializeField] private float timeToExelentShot, liveTime;
    public UnityEvent movePoints = new();

    Transform[] points;
    private Text killedText, lostText;
    private float timeAlive;
    private GetStatisticScriptableObject stats;

    private void Awake()
    {
        ApexStatsHendler.objectPass.AddListener(GetObject);

        CreateInfoCanvas c = Instantiate(canvas, transform);
        points = hndl.GetComponentsInChildren<Transform>();
        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void SetUp(float exShot, float liveTime)
    {
        timeToExelentShot = exShot;
        this.liveTime = liveTime;
    }

    private void GetObject(GetStatisticScriptableObject obj)
    {
        stats = obj;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive > liveTime)
        {
            lostText.text = (++stats.targetsLost).ToString();
            Respawn();
            timeAlive = 0;
        }
    }
    public void GetHited(HitInfo hitInfo, out bool head)
    {
        if(timeAlive <= timeToExelentShot)
            stats.exelentShots++;        
        else
            stats.normalShots++;

        killedText.text = stats.hits.ToString();
        Respawn();
        timeAlive = 0;
        head = false;
    }

    private void Respawn()
    {
        movePoints.Invoke();
        transform.position = points[Random.Range(0, points.Length)].position;
    }
}
