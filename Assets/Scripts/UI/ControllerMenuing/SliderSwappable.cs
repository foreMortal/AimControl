using UnityEngine.UI;
using UnityEngine;

public class SliderSwappable : MonoBehaviour
{
    private CanvasManager manager;
    public bool crcl, crss, trngl, sqr, arUp, arLeft, arRight, arDown;
    private bool haveButton;
    private event MenuingManager.MenuDelegate handl;
    private Slider slider;

    private void Awake()
    {
        manager = GetComponentInParent<CanvasManager>();
        slider = GetComponent<Slider>();
        haveButton = true;
    }
    private void OnEnable()
    {
        if (!haveButton)
        {
            manager = GetComponentInParent<CanvasManager>();
            slider = GetComponent<Slider>();
            haveButton = true;
        }
        if (crcl)
        {
            handl = manager.crcl;
            manager.crcl = Activate;
        }
        if (crss)
        {
            handl = manager.crss;
            manager.crss = Activate;
        }
        if (trngl)
        {
            handl = manager.trngl;
            manager.trngl = Activate;
        }
        if (sqr)
        {
            handl = manager.sqr;
            manager.sqr = Activate;
        }
        if (arUp)
        {
            handl = manager.arUp;
            manager.arUp = Activate;
        }
        if (arLeft)
        {
            handl = manager.arLft;
            manager.arLft = Activate;
        }
        if (arRight)
        {
            handl = manager.arRgt;
            manager.arRgt = Activate;
        }
        if (arDown)
        {
            handl = manager.arDwn;
            manager.arDwn = Activate;
        }
    }
    private void OnDisable()
    {
        manager.crcl = handl;
    }

    private void Activate(int t)
    {
        if (slider.value == 2f)
            slider.value = 1f;
        else
            slider.value = 2f;
    }
}
