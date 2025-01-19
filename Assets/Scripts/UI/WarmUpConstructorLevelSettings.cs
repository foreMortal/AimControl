using UnityEngine;
using UnityEngine.UI;

public class WarmUpConstructorLevelSettings : MonoBehaviour
{
    [SerializeField] private GameObject dummyStrafes, distance, dificulty, doorDistance, dummyDistance, targeting, doorDificulty, weapon;
    [SerializeField] private Dropdown[] dropDowns;
    [SerializeField] private GameObject[] moveTypes;
    [SerializeField] private IButtonManager[] buttons;

    private Toggle t_targeting;

    private bool awaked;
    private IButtonManager handl;
    private StandartCanvas canvas;
    private Image selfImg;
    private Text text;
    private SelectLevel sett;

    private void Awake()
    {
        if (!awaked)
        {
            canvas = GetComponent<StandartCanvas>();
            selfImg = GetComponent<Image>();
            t_targeting = targeting.GetComponent<Toggle>();

            text = GetComponentInChildren<Text>();
            awaked = true;
        }
    }

    private void OnMouseDown()
    {
        Close();
    }

    public void Close()
    {
        sett.CloseLevelSettings();
        gameObject.SetActive(false);
        sett = null;

        t_targeting.isOn = false;
        foreach(var drop in dropDowns)
        {
            drop.value = 0;
        }
    }

    public void ChooseLevel()
    {
        sett.Select();
        gameObject.SetActive(false);
        sett = null;

        t_targeting.isOn = false;
        foreach (var drop in dropDowns)
        {
            drop.value = 0;
        }
    }

    public void LevelSettingsWindowOpen(SelectLevel sett, Sprite image, string name, MovementType type)
    {
        if (!awaked)
            Awake();

        foreach(var mtype in moveTypes)
        {
            mtype.SetActive(false);
        }
        moveTypes[(int)type].SetActive(true);


        this.sett = sett;

        dummyStrafes.SetActive(false); distance.SetActive(false);
        dificulty.SetActive(false); doorDistance.SetActive(false);
        dummyDistance.SetActive(false); doorDificulty.SetActive(false);
        weapon.SetActive(false); targeting.SetActive(false);
        
        handl = buttons[0];
        
        switch (sett.type)
        {
            case "DummyClose":
            case "DummyShort":
            case "DummyMiddle":
            case "DummyLong":
                handl = buttons[0];
                dummyDistance.SetActive(true);
                dummyStrafes.SetActive(true);
                dificulty.SetActive(true);
                targeting.SetActive(true);
                dropDowns[0].value = 0;
                dropDowns[1].value = 0;
                dropDowns[3].value = 0;
                t_targeting.isOn = false;
                break;
            case "DoorPicks":
                handl = buttons[1];
                doorDistance.SetActive(true);
                dificulty.SetActive(true);
                dropDowns[3].value = 0;
                dropDowns[4].value = 0;
                break;
            case "FlickShots":
            case "FlickShotsV2":
            case "FlickShotsMultipleTargets":
            case "PassChecking":
            case "FullRotateFlicks":
                handl = buttons[2];
                distance.SetActive(true);
                dificulty.SetActive(true);
                dropDowns[2].value = 0;
                dropDowns[3].value = 0;
                break;
            case "TrakingSphere":
                handl = buttons[2];
                distance.SetActive(true);
                dificulty.SetActive(true);
                targeting.SetActive(true);
                dropDowns[2].value = 0;
                dropDowns[3].value = 0;
                t_targeting.isOn = false;
                break;
            case "BehindNockDownShield":
            case "CircleHeadshotTrain":
            case "FromHgToLg":
            case "FromLgToHg":
            case "LowWallJumpFlicks":
            case "ogibanieStenki":
            case "SlideFlickshots":
            case "StraightWallJumpFlicks":
                handl = buttons[3];
                dificulty.SetActive(true);
                dummyStrafes.SetActive(true);
                dropDowns[1].value = 0;
                dropDowns[3].value = 0;
                break;
        }
        canvas.StartButton = handl;
        gameObject.SetActive(true);
        canvas.StartButton.handl = new Color(1, 1, 1);
        selfImg.sprite = image;
        text.text = name;
    }

    public void SetDummyDistance(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.type = "DummyClose";
                    break;
                case 1:
                    sett.sett.type = "DummyShort";
                    break;
                case 2:
                    sett.sett.type = "DummyMiddle";
                    break;
                case 3:
                    sett.sett.type = "DummyLong";
                    break;
            }
        }
    }
    public void SetDummyStrafes(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.strafeDistance = "Short";
                    break;
                case 1:
                    sett.sett.strafeDistance = "Long";
                    break;
            }
        }
    }

    public void SetFlicksDistance(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.distance = "Close";
                    break;
                case 1:
                    sett.sett.distance = "Middle";
                    break;
                case 2:
                    sett.sett.distance = "Long";
                    break;
            }
        }
    }

    public void SetDoorDistance(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.distance = "Close";
                    break;
                case 1:
                    sett.sett.distance = "Long";
                    break;
            }
        }
    }

    public void SetDoorDificulty(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.dificulty = "Easy";
                    break;
                case 1:
                    sett.sett.dificulty = "Hard";
                    break;
            }
        }
    }

    public void SetDificulty(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.dificulty = "Easy";
                    break;
                case 1:
                    sett.sett.dificulty = "Normal";
                    break;
                case 2:
                    sett.sett.dificulty = "Hard";
                    break;
                case 3:
                    sett.sett.dificulty = "UltraHard";
                    break;
            }
        }
    }

    public void SetWeapon(int num)
    {
        if(sett != null)
        {
            switch (num)
            {
                case 0:
                    sett.sett.weapon = "R99";
                    break;
                case 1:
                    sett.sett.weapon = "PK";
                    break;
            }
        }
    }

    public void SetTargeting(bool state)
    {
        if(sett != null)
        {
            sett.sett.targeting = state;
        }
    }
}
