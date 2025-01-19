using UnityEngine;
using UnityEngine.UI;

public class ExpandSettingsActive : MonoBehaviour
{
    [SerializeField] private GameObject HideClassicSettings;
    [SerializeField] private IButtonManager[] buttons_;

    public void ExpandSettingsSetActive(float value)
    {
        switch (value)
        {
            case 1f:
                gameObject.SetActive(false);
                HideClassicSettings.SetActive(false);
                foreach(var b in buttons_)
                {
                    b.canBePicked = true;
                }
                break;
            case 2f:
                gameObject.SetActive(true);
                HideClassicSettings.SetActive(true);
                foreach (var b in buttons_)
                {
                    b.canBePicked = false;
                }
                break;
        }
    }
}
