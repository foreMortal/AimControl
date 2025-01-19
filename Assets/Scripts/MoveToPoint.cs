using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed, changePointTarget;
    [SerializeField] private Animator animator;
    private Transform currTarget;
    private bool rightPointed;
    private float maxTime = 0.25f;

    private void Start()
    {
        currTarget = points[1];
        rightPointed = true;
    }

    private void Update()
    {
        Timerlogic();
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currTarget.position.x, transform.position.y, currTarget.position.z), speed * Time.deltaTime);
        animator.SetFloat("Speed", speed);
    }

    private void Timerlogic()
    {
        changePointTarget += Time.deltaTime;
        if(changePointTarget >= maxTime)
        {
            if (rightPointed)
                SetLeftPoint();
            else if (!rightPointed)
                SetRightPoint();
            changePointTarget = 0f;
            int num = Random.Range(0, 3);
            switch (num)
            {
                case 0:
                    maxTime = .35f; break;
                case 1:
                    maxTime = .45f; break;
                case 2:
                    maxTime = .65f; break;

            }
        }
    }

    public void SetRightPoint()
    {
        currTarget = points[1];
        rightPointed = true;
        changePointTarget = 0f;
    }
    public void SetLeftPoint()
    {
        currTarget = points[0];
        rightPointed = false;
        changePointTarget = 0f;
    }
}
