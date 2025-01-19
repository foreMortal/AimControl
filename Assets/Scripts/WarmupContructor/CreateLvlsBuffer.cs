using UnityEngine;

public class CreateLvlsBuffer : MonoBehaviour
{
    [SerializeField] private GameObject Active;
    [SerializeField] private GameObject UnActive;
    [SerializeField] private WarmupManager manager;
    [SerializeField] private GameObject maxCountReached;
    [SerializeField] private Transform canvas;

    public void Set()
    {
        if(manager.lvlCount < 100)
        {
            if (Active != null)
                Active.SetActive(true);
            if (UnActive != null)
                UnActive.SetActive(false);
        }
        else
        {
            GameObject a = Instantiate(maxCountReached, canvas);
            a.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            a.GetComponent<RectTransform>().sizeDelta = new Vector2(513.96f, 67.278f);
            Destroy(a, 1.25f);
        }
    }
}
