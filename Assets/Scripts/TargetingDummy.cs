using UnityEngine;

public class TargetingDummy : MonoBehaviour
{
    [SerializeField] private Transform underGround;
    [SerializeField] private Transform player;

    private CrouchOnly dummyStarfe;
    private float targetingTime = 3f, underGroundTime;
    private bool ground = true;
    private LayerMask mask = 1 << 10;

    private void Awake()
    {
        dummyStarfe= GetComponent<CrouchOnly>();
    }

    private void Update()
    {
        if (targetingTime > 0f)
        {
            targetingTime -= Time.deltaTime;
        }
        else if (targetingTime <= 0f && ground)
        {
            ground = false;
            dummyStarfe.SetTargetingPosition(DummyState.North, ground);
            transform.position = new Vector3(transform.position.x, underGround.position.y, transform.position.z);
            underGroundTime = Random.Range(0.5f, 1f);
        }
        if (underGroundTime > 0f)
        {
            underGroundTime -= Time.deltaTime;
        }
        else if (underGroundTime <= 0f && !ground)
        {
            if(Physics.Raycast(player.position, player.forward, out RaycastHit hit, 10f, mask))
            {
                TargetingSideAccesion side = hit.transform.GetComponent<TargetingSideAccesion>();
                transform.position = side.GetPosition();
                ground = true;
                dummyStarfe.SetTargetingPosition(side.state, ground);
                targetingTime = Random.Range(2.5f, 5f);
            }
        }
    }
}
