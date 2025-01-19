using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnHover : MonoBehaviour
{
    [SerializeField] private GameObject smthToShow;
    [SerializeField]private Color imgColor, textColor;

    private Image img;
    private Text text;
    private float show;

    private void Awake()
    {
        img= smthToShow.GetComponent<Image>();
        text = smthToShow.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (show > 0f)
        {
            show -= Time.unscaledDeltaTime;
            img.color = new(img.color.r, img.color.g, img.color.b, img.color.a - 0.5f * Time.unscaledDeltaTime);
            text.color = new(text.color.r, text.color.g, text.color.b, text.color.a - 0.5f * Time.unscaledDeltaTime);
        }
    }

    public void Show()
    {
        img.color = imgColor;
        text.color = textColor;
        show = 2f;
    }
}
