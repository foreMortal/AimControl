using System;
using UnityEngine;
using UnityEngine.UI;

public class inputField : MonoBehaviour
{
    private Slider slider;
    private InputField text;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponentInParent<Slider>();
        }
        text = GetComponent<InputField>();
    }

    public void OnValueChanged(string value)
    {
        if (slider == null)
        {
            slider = GetComponentInParent<Slider>();
        }

        try
        {
            string redVal = value.Replace('.', ',');
            float val = float.Parse(redVal);
            slider.value = val;
        }
        catch (FormatException)
        {
            text.text = slider.value.ToString();
        }
    }
}
