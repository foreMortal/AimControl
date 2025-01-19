using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text text;
    private float time;
    private int frames, type;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            type++;
            text.gameObject.SetActive(true);
            if (type > 3)
            {
                type = 0;
                text.gameObject.SetActive(false);
            }
        }
        if(type == 1)
        {
            text.text = (1 / Time.deltaTime).ToString();
        }
        else if(type == 2)
        {
            time += Time.deltaTime;
            frames++;

            if (time >= 0.1f)
            {
                text.text = (frames / time).ToString();
                time = 0;
                frames = 0;
            }
        }
        else if (type == 3)
        {
            time += Time.deltaTime;
            frames++;

            if (time >= 0.2f)
            {
                text.text = (frames / time).ToString();
                time = 0;
                frames = 0;
            }
        }
    }
}
