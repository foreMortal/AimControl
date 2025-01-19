using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject Targeting; 
    [SerializeField] private GameObject Flickshots;
    [SerializeField] private int Value;
    
    private void Update()
    {
        Value = GetComponent<Dropdown>().value;
        if(Value == 1)
        {
            Targeting.SetActive(true);
            Flickshots.SetActive(false);
        }

        if (Value == 2)
        {
            Flickshots.SetActive(true);
            Targeting.SetActive(false);
        }
    }
}
