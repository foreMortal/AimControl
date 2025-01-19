using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsTracking : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private bool hard;
    [SerializeField] private UnityEvent charge;

    //private Text killedText, lostText;
    private LayerMask mask = 1 << 2;
    private RaycastHit hit;
    private float range = 25f, timeOnSphere, shootTimer, chargeTimer, record;
    private GetStatisticScriptableObject stats;

    private void Awake()
    {
        ApexStatsHendler.objectPass.AddListener(GetObject);

        /*CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Tracking time:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Record:", new Vector3(-280f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();*/
    }

    public void SetUp(TrackingSphereScriptableObject obj)
    {
        hard = obj.hard;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
        {
            if (hit.collider.CompareTag("CanGetHitted"))
            {
                timeOnSphere += Time.deltaTime;
                stats.trackingTime += Time.deltaTime;
                if (hard)
                    HardLogic();
                if (timeOnSphere > stats.recordTime)
                {
                    stats.recordTime = timeOnSphere;
                    record = timeOnSphere;
                    //lostText.text = record.ToString("F2") + "sec";
                }
                //killedText.text = timeOnSphere.ToString("F2") + "sec";
            }
            else
            {
                stats.missingTargetTime += Time.deltaTime;
                if (timeOnSphere > 0f)
                {
                    timeOnSphere = 0f;
                }
            }
        }
        else
        {
            stats.missingTargetTime += Time.deltaTime;
            if (timeOnSphere > 0f)
            {
                timeOnSphere = 0f;
            }
        }
    }

    private void GetObject(GetStatisticScriptableObject obj)
    {
        stats = obj;
    }

    private void HardLogic()
    {
        chargeTimer -= Time.deltaTime;
        if (timeOnSphere >= 0.2f && chargeTimer <= 0f)
        {
            charge.Invoke();
            chargeTimer = Random.Range(1.5f, 2.5f);
        }
    }
}
