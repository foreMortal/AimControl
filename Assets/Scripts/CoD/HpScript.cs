using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
    [SerializeField] Text delt;
    [SerializeField] private int num = 1;
    [SerializeField] private SuccededFailed succeeded;
    [SerializeField] private SuccededFailed failed;
    [SerializeField] private SniperLvLRespawn sniperResp;
    [SerializeField] private Respawn resp;
    public float maxHP = 250f;
    public float currHP = 250f;
    public float countDamage;
    public static UnityEvent ResetScene = new ();

    public void TakeHit(float damage)
    {
        if(num == 1)
        {
            currHP -= damage;
            /*if (currHP <= 0f && GetComponent<CoDPlayerMovement>())
            {
                ResetScene.Invoke();
                StartCoroutine(failed.Increase());
            }
            else if (currHP <= 0f && GetComponent<CrouchOnly>())
            {
                ResetScene.Invoke();
                StartCoroutine(succeeded.Increase());
            }*/
        }
        else if(num == 2)
        {
            countDamage += damage;
            if(countDamage >= 10000)
            {
                float d = countDamage / 1000;
                delt.text = d.ToString("F2") + "k";
            }
            else
            {
                delt.text = countDamage.ToString("F0");
            }
        }
        else if (num == 3)
        {
            currHP -= damage;
            countDamage += damage;
            if (countDamage >= 10000)
            {
                float d = countDamage / 1000;
                delt.text = d.ToString("F2") + "k";
            }
            else
            {
                delt.text = countDamage.ToString("F0");
            }
            if (currHP <= 0f)
            {
                resp.RespawnHim();
                Full();
            }
        }
        else if (num == 4)
        {
            currHP -= damage;
            
            if (currHP <= 0f)
            {
                sniperResp.ChangeSpawn();
                Full();
            }
        }
    }
    public void Full()
    {
        currHP = maxHP;
    }
}
