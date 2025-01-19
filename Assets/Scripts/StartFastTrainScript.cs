using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartFastTrainScript : MonoBehaviour
{
    [SerializeField] private bool original;
    [SerializeField] private string[] types;
    [SerializeField] private string[] distance;
    [SerializeField] private string[] dificulty;
    [SerializeField] private string[] strafeDistance;

    public string petName;
    public LevelNameObjject levelName;
    public NewerPickLevel pick;

    public FastTrainScript warmupStart;
    public LevelSettings lS;
    public List<LevelSettings> levels = new();

    public bool accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_, exelentShots_, normalShots_, trackAccuracy_;
    public bool trackingTime_, missingTargetTime_, recordTime_;

    private string type = "Warmup";
    private bool delete, chosen;
    private GameObject markOnDelete;

    [NonSerialized] public Text lvlName;
    [NonSerialized] public Sprite image;
    [NonSerialized] public int index = -1;
    [NonSerialized] public float time = 30f;

    [NonSerialized] public GameObject deleteMark;
    
    [NonSerialized] public UnityEvent<List<LevelSettings>, LevelSettings> start = new();
    [NonSerialized] public UnityEvent<int, GameObject> giveToDelete = new();
    [NonSerialized] public UnityEvent<int, GameObject> revertToDelete = new();

    private void Awake()
    {
        lvlName = GetComponentInChildren<Text>();

        if(types.Length > 0)
        {
            for (int i = 0; i < types.Length; i++)
            {
                LevelSettings newLevel = new(types[i], distance[i], dificulty[i], strafeDistance[i], "", "", false);
                levels.Add(newLevel);
            }
        }
    }

    private void Start()
    {
        image = GetComponent<Image>().sprite;
    }

    public void StartTrain()
    {
        if (!delete)
        {
            if (original)
            {
                lS = new(type, "", "", "", "", petName, false);
                lS.timeForALevel = time;
                lS.SetParameters(accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_, exelentShots_, normalShots_, trackAccuracy_,
                trackingTime_, missingTargetTime_, recordTime_);
            }

            pick.TakeData(lS, this);

            warmupStart.GetLevels(levels, lS);
        }
        else if (!chosen)
        {
            if(index != -1)
            {
                giveToDelete.Invoke(index, gameObject);
                markOnDelete = Instantiate(deleteMark, transform);
                markOnDelete.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                markOnDelete.GetComponent<RectTransform>().sizeDelta = new Vector2(170.494f, 45.568f);
                chosen = true;
            }
        }
        else if (chosen)
        {
            if(index != -1)
            {
                revertToDelete.Invoke(index, gameObject);
                Destroy(markOnDelete);
                markOnDelete = null;
                chosen = false;
            }
        }
    }

    public void ChangeName(string mess)
    {
        lvlName.text = mess;
    }

    public void CancelInvoked()
    {
        chosen = false;
        Destroy(markOnDelete);
        markOnDelete = null;
    }

    public void SetDeleteTrue(bool state)
    {
        delete = state;
    }
}
