using UnityEngine;
using UnityEngine.UI;

public class FullRotateFlickshots : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private Transform player, dummy;
    [SerializeField] private Text sideText;

    public float dummyLife;
    private DummyDificulty dificulty;
    private LayerMask mask = 1 << 10;
    private Animator animator;

    private Text killedText, lostText;
    private float textLife = 1f, dummyLifeTimer;

    private void Awake()
    {
        animator = dummy.GetComponent<Animator>();

        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    dificulty = DummyDificulty.Easy;
                    dummyLife = 3f; break;
                case "Normal":
                    dificulty = DummyDificulty.Normal;
                    dummyLife = 2.5f; break;
                case "Hard":
                    dificulty = DummyDificulty.Hard;
                    dummyLife = 2f; break;
                case "UltraHard":
                    dificulty = DummyDificulty.UltraHard;
                    dummyLife = 1.5f; break;
            }
        }

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void SetupWarmup(LevelSettings obj)
    {
        switch (obj.dificulty)
        {
            case "Easy":
                dificulty = DummyDificulty.Easy;
                dummyLife = 3f; break;
            case "Normal":
                dificulty = DummyDificulty.Normal;
                dummyLife = 2.5f; break;
            case "Hard":
                dificulty = DummyDificulty.Hard;
                dummyLife = 2f; break;
            case "UltraHard":
                dificulty = DummyDificulty.UltraHard;
                dummyLife = 1.5f; break;
        }
    }

    private void Update()
    {
        if (textLife >= 0f)
        {
            textLife -= Time.deltaTime;
            sideText.color = new Color(sideText.color.r, sideText.color.g, sideText.color.b, sideText.color.a - 1f * Time.deltaTime);
        }
        if(dummyLife <= dummyLifeTimer)
        {
            dummyLifeTimer = 0f;
            lostText.text = (++stats.targetsLost).ToString();
            Spawn();
        }
        else
        {
            dummyLifeTimer += Time.deltaTime;
        }
    }

    public void TargetKilled(float num)
    {
        killedText.text = num.ToString();
        dummyLifeTimer = 0f;
        Spawn();
    }

    private void Spawn()
    {
        if((int)dificulty < 2)
        {
            GetSpawnVector(Random.Range(0, 2));
        }
        else
        {
            GetSpawnVector(Random.Range(0, 3));
        }

        if (Random.Range(0, 2) == 1)
            animator.SetBool("Crouch", true);
        else
            animator.SetBool("Crouch", false);
    }

    private void GetSpawnVector(int num)
    {
        Vector3 vector;
        switch (num)
        {
            case 0:
                vector = -player.right;break;
            case 1:
                vector = player.right; break;
            case 2:
                vector = -player.forward; break;
            default:
                vector = -player.right; break;
        }
        if (Physics.Raycast(player.position, vector, out RaycastHit hit, 4f, mask))
        {
            dummy.transform.position = 
            hit.collider.GetComponent<ReturnSideSpawn>().Return();

            ShowSide(num);
        }
    }

    private void ShowSide(int num)
    {
        switch (num)
        {
            case 0:
                sideText.text = "Left!";break;
            case 1:
                sideText.text = "Right!"; break;
            case 2:
                sideText.text = "Behind!"; break;
        }
        sideText.color = new Color(sideText.color.r, sideText.color.g, sideText.color.b, 1f);
        textLife = 1f;
    }
}
