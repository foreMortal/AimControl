using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    public string type;
    public string distance;
    public string dificulty;
    public string strafeDistance;
    public string weapon;
    public string petName;
    [SerializeField] private MovementType movementType;
    [SerializeField] private WarmUpConstructorLevelSettings levelSettings;

    private IButtonManager buttonManager;
    private Vector3 selectedPos;
    private bool selected, moving;
    private Text text;
    private RectTransform rect;
    private int index;
    private Sprite image;
    [NonSerialized] public LevelSettings sett = new();

    public string selectedName = "";
    public int fontSize = 5;
    public Transform parentCanvas;
    public Transform canvas;
    public float speed;
    public WarmupConstructor constructor;

    private void Awake()
    {
        image = GetComponent<Image>().sprite;
        buttonManager = GetComponent<IButtonManager>();
        text = GetComponentInChildren<Text>();
        rect = GetComponent<RectTransform>();

        sett = new(type, distance, dificulty, strafeDistance, weapon, petName, false);
    }

    public void OpenLevelSettings()
    {
        if (!selected)
        {
            levelSettings.LevelSettingsWindowOpen(this, image, text.text, movementType);
        }
        else
        {
            Select();
        }
    }

    public void CloseLevelSettings()
    {
        sett.type = type;
        sett.distance = distance;
        sett.dificulty = dificulty;
        sett.strafeDistance = strafeDistance;
        sett.weapon = weapon;
        sett.petName = petName;
        sett.targeting = false;
    }

    public void Select()
    {
        if (!moving)
        {
            constructor.ChangeLvlNames(sett, !selected, index, out selectedPos, out index);
            if (index == -1)
            {
                return;
            }
            if (selectedPos != Vector3.zero)
            {
                GameObject t = Instantiate(gameObject, transform.position, Quaternion.identity, canvas);
                SelectLevel l = t.GetComponent<SelectLevel>();
                IButtonManager b = t.GetComponent<IButtonManager>();

                l.SetUpInstance(selectedName, fontSize, parentCanvas, canvas, speed, constructor, type, distance, dificulty, strafeDistance, weapon, petName);
                l.MakeMove(!selected, true, selectedPos);
                b.ClearPaths();
                b.handl = new Color(1, 1, 1, 0);
                t.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);

                constructor.ChangeButtons(true, b);
            }
            else
            {
                constructor.ChangeButtons(false, buttonManager);
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        if(moving && selectedPos != Vector3.zero)
        {
            transform.SetLocalPositionAndRotation(Vector3.MoveTowards(transform.localPosition, selectedPos, speed * Time.deltaTime), Quaternion.identity) ;
            if((selectedPos - transform.localPosition).sqrMagnitude <= 1f)
            {
                transform.SetParent(parentCanvas);
                transform.SetLocalPositionAndRotation(selectedPos, Quaternion.identity);
                rect.localScale = new Vector3(0.75f, 0.75f, 1f);
                if (selectedName.Length > 0)
                    text.text += selectedName;
                text.fontSize = fontSize;
                moving = false;
            }
        }
    }

    public void MakeMove(bool selected, bool moving, Vector3 selectedPos)
    {
        this.selected = selected;
        this.moving = moving;
        this.selectedPos = selectedPos;
    }

    public void SetUpInstance(string selectedName, int fontSize,Transform parentCanvas, Transform canvas, float speed, WarmupConstructor constructor, string type, string distance, string dificulty, string strafeDistance, string weapon, string petName)
    {
        this.selectedName = selectedName;
        this.fontSize = fontSize;
        this.parentCanvas = parentCanvas;
        this.canvas = canvas;
        this.speed = speed;
        this.constructor = constructor;
        this.type = type;
        this.distance = distance;
        this.dificulty = dificulty;
        this.strafeDistance = strafeDistance;
        this.weapon = weapon;
        this.petName = petName;

        sett = new(type, distance, dificulty, strafeDistance, weapon, petName, false);
    }

}
