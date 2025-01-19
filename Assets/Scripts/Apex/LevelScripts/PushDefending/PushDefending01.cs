using Unity.VisualScripting;
using UnityEngine;

public class PushDefending01 : MonoBehaviour
{
    public MeshCollider body;
    public MeshCollider hands;
    public CapsuleCollider head;
    public MeshCollider crouchBody;
    public CapsuleCollider crouchHead;

    [SerializeField] private float speed = 0.5f, _timerSpeed, _timerLimit;
    private Rigidbody _rigidBody;
    [SerializeField] private Transform shelter2, average, averege2;
    [SerializeField] private int num;
    public float averageTimer, averageTimer2;
    [SerializeField] private Transform average3;
    public bool respawned;
    [SerializeField] private Animator animator;
    private void Start()
    {
        _rigidBody= GetComponent<Rigidbody>();
        animator.SetFloat("Speed", -1f);
    }

    private void Update()
    {
        if(averageTimer > 0f)
        {
            averageTimer -= Time.deltaTime;
            respawned = false;
        }
        else if(averageTimer2 > 0f)
        {
            averageTimer2 -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (averageTimer > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, average.position, speed * Time.deltaTime);
        }
        else if (averageTimer <= 0f)
        {
            if(averageTimer2 > 0f)
            {
                average3.position = new Vector3(averege2.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, average3.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(shelter2.position.x, 164.136f, shelter2.position.z), speed * Time.deltaTime);
            }
        }
    }
}
