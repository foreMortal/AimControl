using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonHints : MonoBehaviour
{
    [SerializeField] private Image img;
    public string path;

    public void SetHints(Sprite sprite)
    {
        img.sprite = sprite;
    }
}
