using UnityEngine;

public class DummyCanDie : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint1, spawnPoint2;
    [SerializeField] private GameObject RightArrow, LeftArrow;
    private int sp = 1;
    public void SwapDummy()
    {
        if (sp == 1)
        {
            transform.position = spawnPoint2.position;
            LeftArrow.SetActive(false);
            RightArrow.SetActive(true);
            sp = 2;
        }
        else
        {
            transform.position = spawnPoint1.position;
            LeftArrow.SetActive(true);
            RightArrow.SetActive(false);
            sp = 1;
        }
    }

}
