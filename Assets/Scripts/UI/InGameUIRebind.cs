using UnityEngine;
using UnityEngine.UI;

public class InGameUIRebind : MonoBehaviour
{
    private Image image;
    private SettingsScriptableObject settings;
    private const string BUTTONS_TYPE = "ButtonsType";
    private const string USE = "Use";

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetinteractiveUI(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
