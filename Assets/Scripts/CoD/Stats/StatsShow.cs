using UnityEngine;
using UnityEngine.UI;

public enum MovementType
{
    None = 0,
    AimControl = 1,
    ApexLegends = 2,
}


public class StatsShow : MonoBehaviour
{
    [SerializeField] bool targeting;
    [SerializeField] private string type, distance, dificulty, strafeDistance, weapon, petName;
    [SerializeField] private bool accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_, exelentShots_, normalShots_, trackAccuracy_;
    [SerializeField] private bool trackingTime_, missingTargetTime_, recordTime_;
    [SerializeField] MovementType moveType;

    private string ShowName;
    private LevelNameObjject levelName;
    private NewerPickLevel pick;
    private Sprite image;

    private void Awake()
    {
        pick = GameObject.FindWithTag("PickLevelHandler").GetComponent<NewerPickLevel>();
        image = GetComponent<Image>().sprite;
        ShowName = GetComponentInChildren<Text>().text;
    }

    public void NewerStatShow()
    {
        levelName.Clear();

        levelName.Fill(type, distance, dificulty, strafeDistance,
            weapon, petName, targeting);
        levelName.getChoosenStats(accuracy_, succeeded_, failed_, damageDelt_,
            damageTaken_, headSH_, bodySH_, time_, hits_, misses_,
            targetsLost_, exelentShots_, normalShots_,
            trackAccuracy_, trackingTime_, missingTargetTime_,
            recordTime_);

        pick.TakeData(ShowName, image, moveType);
    }

    public void TakeData(LevelNameObjject obj)
    {
        levelName = obj;
    }
}
