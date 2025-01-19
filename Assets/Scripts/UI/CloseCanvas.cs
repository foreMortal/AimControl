using UnityEngine;

public class CloseCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private void OnMouseDown()
    {
        canvas.SetActive(false);
    }

    public void Close()
    {
        canvas.SetActive(false);
    }
}
