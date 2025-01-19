using UnityEngine;
using UnityEngine.UI;

public class SliderInstance : IButtonManager
{
    public float slowDiv, fastDiv;
    private float timer;
    private Slider slider;
    private CanvasManager manager;
    private MenuingManager.MenuDelegate left, right;
    private bool setuped;

    protected override void Awake()
    {
        base.Awake();
        manager = GetComponentInParent<CanvasManager>();
        slider = GetComponent<Slider>();
        setuped = true;
    }

    public override void Highlight(bool state)
    {
        if (!setuped)
        {
            manager = GetComponentInParent<CanvasManager>();
            slider = GetComponent<Slider>();
            setuped = true;
        }
        base.Highlight(state);
        if (state)
        {
            left = manager.arLft;
            right = manager.arRgt;

            manager.arLft = MoveLeft;
            manager.arRgt = MoveRight;
        }
        else
        {
            manager.arLft = left;
            manager.arRgt = right;
        }
    }

    private void MoveLeft(int t)
    {
        if(t == 0)
        {
            slider.value -= slowDiv;
        }
        else
        {
            timer += Time.deltaTime;
            if(timer >= 0.1f)
            {
                timer = 0;
                slider.value -= fastDiv;
            }
        }
    }

    private void MoveRight(int t) 
    {
        if (t == 0)
        {
            slider.value += slowDiv;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {
                timer = 0;
                slider.value += fastDiv;
            }
        }
    }
    public override void Activate()
    {

    }
}
