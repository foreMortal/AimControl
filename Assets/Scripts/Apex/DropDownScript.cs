using UnityEngine;
using UnityEngine.UI;

public class DropDownScript : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    private float value;

    private void Awake()
    {
        if(PlayerPrefs.GetFloat("Time") == 0f)
            PlayerPrefs.SetFloat("Time", 90f);
        switch (PlayerPrefs.GetFloat("Time"))
        {
            case 1f:
                obj.GetComponent<Dropdown>().value = 2;
                break;
            case 90f:
                obj.GetComponent<Dropdown>().value = 0;
                break;
            case 180f:
                obj.GetComponent<Dropdown>().value = 1;
                break;
        }
    }

    public void TimeSet()
    {
        value = obj.GetComponent<Dropdown>().value;
        switch (value)
        {
            case 0:
                PlayerPrefs.SetFloat("Time", 90f);
                break;
            case 1:
                PlayerPrefs.SetFloat("Time", 180f);
                break;
            case 2:
                PlayerPrefs.SetFloat("Time", 1f);
                break;
        }
    }
}
