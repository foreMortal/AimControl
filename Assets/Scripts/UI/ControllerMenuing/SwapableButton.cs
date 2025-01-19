using UnityEngine;
using UnityEngine.UI;

public class SwapableButton : MonoBehaviour
{
    private CanvasManager manager;
    private IMenuExecutable button;
    public bool crcl, crss, trngl, sqr, arUp, arLeft, arRight, arDown;
    private bool haveButton, actualButton;
    private event MenuingManager.MenuDelegate handl;
    private Button actButton;

    private void Awake()
    {
        manager = GetComponentInParent<CanvasManager>();
        button = GetComponent<IMenuExecutable>();
        if(TryGetComponent(out actButton))
        {
            actualButton = true;
        }
        haveButton = true;
    }
    private void OnEnable()
    {
        if (!haveButton)
        {
            manager = GetComponentInParent<CanvasManager>();
            button = GetComponent<IMenuExecutable>();
            if (TryGetComponent(out actButton))
            {
                actualButton = true;
            }
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
        if (crcl)
            manager.crcl = handl;
        if (crss)
            manager.crss = handl;
        if (trngl)
            manager.trngl = handl;
        if (sqr)
            manager.sqr = handl;
        if (arUp)
            manager.arUp = handl;
        if (arLeft)
            manager.arLft = handl;
        if (arRight)
            manager.arRgt = handl;
        if (arDown)
            manager.arDwn = handl;
    }

    private void Activate(int t)
    {
        if (actualButton)
            actButton.onClick.Invoke();
        else
            button.Execute();
    }
}
