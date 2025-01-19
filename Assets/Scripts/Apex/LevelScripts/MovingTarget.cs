using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private Transform[] targets;
    [SerializeField] private float speed;

    private int targetNum = 1;
    private bool reverse;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targets[targetNum].position, speed * Time.deltaTime);
        
        Vector3 delta = targets[targetNum].position - transform.position;
        
        if(delta.sqrMagnitude <= 0.25)
        {
            if (!reverse)
            {
                if (targetNum < targets.Length - 1)
                    targetNum++;
                else
                {
                    targetNum--;
                    reverse = true;
                }
            }
            else
            {
                if(targetNum > 0)
                    targetNum--;
                else
                {
                    targetNum++;
                    reverse = false;
                }
            }
        }
    }
}
