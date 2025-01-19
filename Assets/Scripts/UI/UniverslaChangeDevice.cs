using UnityEngine;
using UnityEngine.UI;

public class UniverslaChangeDevice : MonoBehaviour
{
    [SerializeField] GameObject firstCanvas, secondCanvas;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        slider.value = 0;
    }

    public void SwapCanvases(float num)
    {
        switch (num)
        {
            case 0:
                firstCanvas.SetActive(true);
                secondCanvas.SetActive(false);
                break;
            case 1:
                firstCanvas.SetActive(false);
                secondCanvas.SetActive(true);
                break;
        }
    }
}
