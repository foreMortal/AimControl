using UnityEngine;
using UnityEngine.UI;

public class GenburtonHPManager : MonoBehaviour
{
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private Transform handl;
    [SerializeField] private CreateInfoCanvas canvas;

    private Text killedText;

    private void Awake()
    {
        GenburtonsGetHited[] t = handl.GetComponentsInChildren<GenburtonsGetHited>();

        foreach(var g in t)
            g.GetManager(this);
        
        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Hits:", new Vector3(-295f, 215F, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void ShowHit()
    {
        killedText.text = stats.hits.ToString();
    }
}
