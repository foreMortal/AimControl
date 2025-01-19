using UnityEngine;
using UnityEngine.UI;

public class PlayerSimpleGetHit : MonoBehaviour, IHitable
{
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private CreateInfoCanvas canvas;
    private float damageTaken;
    private Text damageText;

    private void Awake()
    {
        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Damage Taken:", new Vector3(-250f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        damageText = c.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void GetHited(HitInfo hitInfo, out bool headShot)
    {
        damageTaken += hitInfo.damage;
        stats.playerDamageTaken += hitInfo.damage;

        damageText.text = damageTaken.ToString();
        headShot = false;
    }
}
