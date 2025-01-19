using UnityEngine;

public class OpenMiniCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public void Open()
    {
        canvas.SetActive(true);
    }
    public void Close()
    {
        canvas.SetActive(false);
    }
}
