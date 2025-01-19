using UnityEngine;
using UnityEngine.UI;

public class CreateInfoCanvas : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    [SerializeField] private Text text;
    private bool isOpen;

    private void Awake()
    {
        InGameSettings.openSettings.AddListener(Open);
        InGameSettings.closeSettings.AddListener(Open);
    }

    public void Setup(string text = "", params Vector3[] vecs)
    {
        if(text == "")
            transforms[1].gameObject.SetActive(false);
        this.text.text = text;

        int j = 0;
        for (int i = 0; i < vecs.Length; i++)
        {
            if (j >= 3)
                j = 0;
            if(i < 3)
                transforms[j].localPosition = vecs[i];
            else
                transforms[j].localScale = vecs[i];
            j++;
        }
    }

    private void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            gameObject.SetActive(false);
        }
        else
        {
            isOpen = false;
            gameObject.SetActive(true);
        }
    }
}
