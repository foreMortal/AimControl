using UnityEngine;

public class CloseRadialDummyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f, range, maxRange;
    [SerializeField] private Transform player;

    private int type;
    private float timer;
    private Vector3 targetPosition;

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            if ((player.position - transform.position).magnitude > maxRange)
                GeneratePath();
            else
                RadialMovement();

            timer = Random.Range(0.3f, 0.8f);
        }

        if (type == 1)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x + targetPosition.x, transform.position.y, player.position.z + targetPosition.z), speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, transform.position + targetPosition * range, speed * Time.deltaTime);
    }

    private void RadialMovement()
    {
        int side = 1;
        if (Random.Range(-1, 1) < 0)
            side = -1;

        targetPosition = transform.right * side;
        type = 0;
    }

    private void GeneratePath()
    {
        int side = 1;
        if (Random.Range(-1, 1) < 0)
            side = -1;

        float value = Random.Range(0f, 1f);

        targetPosition = side * value * player.right + (1 - value) * player.forward;

        targetPosition *= range;
        targetPosition.y = transform.position.y;

        type = 1;
    }
}
