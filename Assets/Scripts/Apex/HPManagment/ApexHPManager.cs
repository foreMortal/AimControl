using UnityEngine;
using UnityEngine.UI;

public class ApexHPManager : MonoBehaviour
{
    //[SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] private GetStatisticScriptableObject stats;
    //private Text damageText;
    private float damageDelt;

    private void Awake()
    {
        SimpleGetHit[] t = GetComponentsInChildren<SimpleGetHit>();

        foreach (var hitBox in t)
            hitBox.Setup(this);

        /*CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Damage Delt:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        damageText = c.transform.GetChild(0).GetComponentInChildren<Text>();*/
    }

    public void ApplyDelt(float damage)
    {
        //damageDelt += damage;
        stats.playerDamageDelt += damage;
        

        //damageText.text = damageDelt.ToString();
    }
}
