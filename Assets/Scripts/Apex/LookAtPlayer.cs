using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;
    private float shootTimer;
    private IHitable target;
    private LayerMask mask = ~0;

    [SerializeField] private float damage = 1000, timeBetwenShoots = 0.1f, range = 100;
    public float hittingProcent = 0.25f;

    private void Awake()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        mask -= 1 << 10;
        mask -= 1 << 12;
        mask -= 1 << 13;
    }

    public void SetPlayerDirectly()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        transform.LookAt(player.transform.position);

        shootTimer += Time.deltaTime;

        Shoot();
    }
    
    private void Shoot()
    {
        if (shootTimer >= timeBetwenShoots)
        {
            shootTimer = 0;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("MainCamera"))
                {
                    if (target == null)
                    {
                        target = hit.collider.GetComponent<IHitable>();
                    }
                    var hitInfo = new HitInfo(damage * hittingProcent, 0f, 1f);
                    target.GetHited(hitInfo, out _);
                }
            }
        }
    }

}
