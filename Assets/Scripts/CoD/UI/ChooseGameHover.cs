using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameHover : MonoBehaviour
{
    private Image image;
    [SerializeField] private UnityEngine.Color alpha;
    private UnityEngine.Color beta;

    private void Awake()
    {
        image = GetComponent<Image>();
        beta = image.color;
    }

    private void OnMouseEnter()
    {
        image.color = alpha;
        StartCoroutine(nameof(Bigger));
    }
    private void OnMouseExit()
    {
        image.color = beta;
        StartCoroutine(nameof(Smaller));
    }

    private IEnumerator Bigger()
    {
        for (int i = 0; i < 20; i++)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + 0.01f, gameObject.transform.localScale.y + 0.01f, gameObject.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }
        yield return null;
    }
    private IEnumerator Smaller()
    {
        for (int i = 0; i < 20; i++)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - 0.01f, gameObject.transform.localScale.y - 0.01f, gameObject.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }
        yield return null;
    }
}
