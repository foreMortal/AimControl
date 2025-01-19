using UnityEngine;
using UnityEngine.UI;

public class ButtonInstance : IButtonManager
{
    private Button butt;
    private bool haveButt;

    protected override void Awake()
    {
        base.Awake();
        butt = GetComponent<Button>();
        haveButt = true;
    }

    public override void Activate()
    {
        if (!haveButt)
        {
            butt = GetComponent<Button>();
            haveButt = true;
        }
        butt.onClick.Invoke();
    }
    public override void Highlight(bool state)
    {
        if (!haveButt)
        {
            butt = GetComponent<Button>();
            haveButt = true;
        }
        base.Highlight(state);
    }
}
