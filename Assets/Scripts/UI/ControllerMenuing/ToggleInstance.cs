using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInstance : IButtonManager
{
    private Toggle toggle;
    private bool haveTog, turnOn;

    protected override void Awake()
    {
        base.Awake();
        toggle = GetComponent<Toggle>();
        haveTog = true;
    }
    public override void Activate()
    {
        if (!haveTog)
        {
            toggle = GetComponent<Toggle>();
            haveTog = true;
        }
        toggle.isOn = !turnOn;
        turnOn = !turnOn;
    }
    public override void Highlight(bool state)
    {
        if (!haveTog)
        {
            toggle = GetComponent<Toggle>();
            haveTog = true;
        }
        base.Highlight(state);
    }
}
