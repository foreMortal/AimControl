using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSP;
    [SerializeField] private Transform[] LedderUpSp;
    [SerializeField] private Transform[] LedderDownSp;
    private CharacterController controler;

    private void Awake()
    {
        controler = player.GetComponent<CharacterController>();
        HpScript.ResetScene.AddListener(ResetScene);
    }

    public void ResetScene()
    {
        controler.enabled = false;
        player.transform.position = playerSP.position;
        controler.enabled = true;
        target.GetComponent<HpScript>().Full();
        player.GetComponent<HpScript>().Full();
    }

    public void LedderUp()
    {
        int num1 = Random.Range(0, 3);
        switch (num1)
        {
            case 0:
                target.transform.position = LedderUpSp[0].position;
                target.transform.rotation = Quaternion.Euler(0f, 140f, 0f);
                break;
            case 1:
                target.transform.position = LedderUpSp[1].position;
                target.transform.rotation = Quaternion.Euler(0f, 17.1f, 0f);
                break;
            case 2:
                target.transform.position = LedderUpSp[2].position;
                target.transform.rotation = Quaternion.Euler(0f, -39.6f, 0f);
                break;
        }
    }

    public void LedderDown()
    {
        int num1 = Random.Range(0, 3);
        switch (num1)
        {
            case 0:
                target.transform.position = LedderDownSp[0].position;
                target.transform.rotation = Quaternion.Euler(0f, 41.057f, 0f);
                break;
            case 1:
                target.transform.position = LedderDownSp[1].position;
                target.transform.rotation = Quaternion.Euler(0f, 166.422f, 0f);
                break;
            case 2:
                target.transform.position = LedderDownSp[2].position;
                target.transform.rotation = Quaternion.Euler(0f, -156.958f, 0f);
                break;
        }
    }
}
