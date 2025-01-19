using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject EndMenu;
    [SerializeField] private CreateInfoCanvas timerCanvas;

    private Text _timerText;
    private GetStatisticScriptableObject stats;
    private float _timer = 120f, updateHZ;
    private bool infinite;
    private ApexStatsHendler statsHandler;

    public static UnityEvent endOfLevel = new();

    private void Awake()
    {
        ApexStatsHendler.objectPass.AddListener(GetObject);
        statsHandler = GetComponent<ApexStatsHendler>();

        Cursor.lockState = CursorLockMode.Locked;
        _timer = PlayerPrefs.GetFloat("Time");
        Time.timeScale = 1f;

        if(_timer == 1f)
        {
            infinite = true;
            _timer = 0.1f;
        }
        if (statsHandler.levelName.type == "Warmup")
            infinite = true;

        CreateInfoCanvas c = Instantiate(timerCanvas, transform);

        c.Setup("Time: ", new Vector3(85f, 215f, 0f), new Vector3(54f, 215f, 0f), new Vector3(0f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.25f, 0.4f, 1f));
        _timerText = c.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    private void GetObject(GetStatisticScriptableObject obj)
    {
        stats = obj;
    }

    private void Update()
    {
        stats.timePlayed += Time.deltaTime;
        updateHZ += Time.deltaTime;

        if (!infinite)
        {
            _timer -= Time.deltaTime;
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
        
        if(updateHZ >= 0.1f)
        {
            if (!infinite)
            {
                _timerText.text = _timer.ToString("F1");
            }
            else
            {
                _timerText.text = _timer.ToString("F1");
                /*if (_timer >= 960)
                {
                    float tt = _timer / 60f;
                    _timerText.text = tt.ToString("F2") + " min";
                }
                else
                {
                    _timerText.text = _timer.ToString("F1") + " sec";
                }*/
            }
            updateHZ -= 0.1f;
        }

        if(_timer <= 0)
        {
            endOfLevel.Invoke();
            
            Cursor.lockState = CursorLockMode.None;
            
            

            Time.timeScale = 0f;
            
            statsHandler.OpenEndMenu();
            
            EndMenu.SetActive(true);
        }
    }


    private float CountMaxDamage1(float time)
    {
        float maxDm = time * 10 * 12f;
        return maxDm;
    }

    private string CountMaxDamage(float time)
    {
        float maxDm = time * 10 * 12f;
        if(maxDm > 100000)
        {
            maxDm /= 1000;
            return maxDm.ToString("F2") + "k";
        }
        else
        {
            return maxDm.ToString("F0");
        }
    }

    private string ProcentTakenDamage(float time, float damage)
    {
        float maxDm = time * 10 * 12f;
        float procent = damage / maxDm * 100;
        return procent.ToString("F2") + "%";
    }

    private string CountCurrentDamage(float damage)
    {
        if (damage > 100000)
        {
            damage /= 1000;
            return damage.ToString("F2") + "k";
        }
        else
        {
            return damage.ToString();
        }
    }

    private float CountProcent(float shots, float allshots)
    {
        float procent = 0f;
        if(shots != 0f && allshots != 0f)
            procent = shots / allshots * 100;
        return procent;
    }

    private string CountDamage(float inputDamage)
    {
        float damageTaken = inputDamage;
        return damageTaken.ToString();
    }

    private string CountTime(float inputTime)
    {
        float time = inputTime;
        return "Time played: " + time.ToString("F2") + "sec";
    }
    private string CountAvgDamage(float inputDamage, float timesShooted)
    {
        float avgDamage = inputDamage / timesShooted;
        return avgDamage.ToString("F2");
    }

    private void SetInfinite()
    {
        infinite = true;
    }
}