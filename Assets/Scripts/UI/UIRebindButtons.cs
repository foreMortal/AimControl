using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRebindButtons : MonoBehaviour
{
    [SerializeField] private MenuingManager manager;
    [SerializeField] private SettingsScriptableObject settings;
    
    private ChangeButtonHints[] buttonHints;
    private Dictionary<string, Sprite> psButtons = new();
    private Dictionary<string, Sprite> xbButtons = new();
    public ChangeButtonHints[] ButtonHints { get { return buttonHints; } set { buttonHints = value; if (settings.DeviceType == 0) { SetHintsVisible(true); } else { SetHintsVisible(false); } settings.SetUpButtons(); ChangeButtonsHints(); } }
   
    public void RebindButton(Image img, string path, float buttonsType)
    {
        if (buttonsType == 0)
        {
            img.sprite = psButtons[path];
        }
        else if (buttonsType == 1)
        {
            img.sprite = xbButtons[path];
        }
    }

    public void ChangeButtonsType(Dictionary<string, Image> buttons, float buttonsType)
    {
        if(buttonsType == 0)
        {
            foreach(var button in buttons)
            {
                button.Value.sprite = psButtons[button.Key];
            }
        }
        else if(buttonsType == 1)
        {
            foreach (var button in buttons)
            {
                button.Value.sprite = xbButtons[button.Key];
            }
        }
    }

    public void SetHintsVisible(bool state)
    {
        foreach (var b in buttonHints)
            b.gameObject.SetActive(state);
    }

    public void ChangeButtonsHints()
    {
        foreach (var button in buttonHints)
        {
            button.SetHints(settings.SetImg(button.path));
        }
    }
}
