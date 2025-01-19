using UnityEngine;
using UnityEngine.UI;

public class CoDLookAtPlayer1 : MonoBehaviour
{
    [SerializeField] private Camera player;
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private float damage = 1000, timeBetwenShoots = 0.1f, range = 100;
    [SerializeField] CharacterController controler;
    private Collider playerCollider;
    public float playerDamageTaken, hittingProcent = 0.25f;
    private float shootTimer;

    private void Awake()
    {
        playerCollider = controler.GetComponent<Collider>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
        Shoot();
    }
    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
    }
    
    private void Shoot()
    {
        if (shootTimer >= timeBetwenShoots)
        {
            shootTimer = 0;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
            {
                if (hit.collider == playerCollider)
                {
                    targetPlayer.GetComponent<HpScript>().TakeHit(damage * hittingProcent);
                }
            }
        }
    }

}
