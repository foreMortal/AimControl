using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class SuccededFailed : MonoBehaviour
{
    [SerializeField] private Color color;
    private Color natureCol;
    public int count = 0;

    private void Awake()
    {
        if (GetComponent<Text>() != null)
        {
            natureCol = GetComponent<Text>().color;
        }
    }

    public IEnumerator Increase()
    {
        IncreaseCount();
        GetComponent<Text>().color = color;
        for (int i = 0; i < 30; i++)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + 0.005f,
                                                          gameObject.transform.localScale.y + 0.005f,
                                                          gameObject.transform.localScale.z);
            yield return new WaitForSeconds(0.0001f);
        }
        StartCoroutine(nameof(ReturnToNature));
        yield return null;
    }

    private IEnumerator ReturnToNature()
    {
        for (int i = 0; i < 30; i++)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - 0.005f,
                                                          gameObject.transform.localScale.y - 0.005f,
                                                          gameObject.transform.localScale.z);
            yield return new WaitForSeconds(0.0001f);
        }
        GetComponent<Text>().color = natureCol;
        yield return null;
    }

    private void IncreaseCount()
    {
        count++;
        GetComponent<Text>().text = count.ToString();
    }
}
