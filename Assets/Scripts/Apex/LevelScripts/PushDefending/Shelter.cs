using UnityEngine;

public class Shelter : MonoBehaviour
{
    [SerializeField] private Transform end;
    [SerializeField] private Transform end1;
    [SerializeField] private Transform end2;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform start;
    [SerializeField] private PushDefending01 push;
    private int num;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CapsuleCollider>())
        {
            num = Random.Range(1, 4);
            if (num == 1)
            {
                animator.SetFloat("Speed", -1);
                transform.position = end.position;
                push.averageTimer2 = 0.2f;
            }
            else if (num == 2)
            {
                animator.SetFloat("Speed", -1);
                transform.position = end1.position;
                push.averageTimer2 = 0.2f;
            }
            else if (num == 3)
            {
                animator.SetFloat("Speed", 1);
                transform.position = end2.position;
                push.averageTimer2 = 0.2f;
            }
        }
    }

    public void Respawn()
    {
        transform.position = start.position;
    }
}
