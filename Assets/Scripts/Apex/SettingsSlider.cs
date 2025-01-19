using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private string SETTINGS_NAME = "", dopSYmbols = "";
    [SerializeField] bool expanded = false;

    public UnityEvent<float> someAction = new();
    public UnityEvent<string, float, bool> SettingsChange;

    private Slider slider;

    public void OnChangeValue()
    {
        inputField.SetTextWithoutNotify(slider.value.ToString() + dopSYmbols);
        SettingsChange.Invoke(SETTINGS_NAME, slider.value, expanded);
        someAction.Invoke(slider.value);
    }

    public void TakeData(UserRebinds Rebinds)
    {
        slider = GetComponent<Slider>();

        slider.SetValueWithoutNotify((float)Rebinds.Data[SETTINGS_NAME]);

        inputField.SetTextWithoutNotify(slider.value.ToString());
    }
}
