using UnityEngine.UI;
using UnityEngine;

public class CoDPickLevel : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private Text[] fields; 

    public void OpenLevel()
    {
        Menu.SetActive(true);
    }
    public void CloseLevel()
    {
        ClearAll();
        Menu.SetActive(false);
    }
    private void OnMouseDown()
    {
        CloseLevel();
    }

    private void ClearAll()
    {
        foreach(var item in fields)
        {
            item.text = "";
        }
    }
}
