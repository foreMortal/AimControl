using UnityEngine;

public class RandomPlayerMovement : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private float minTime= 0.3f, maxTime = 0.7f;

    private Vector3 mainDirection, primaryPosition;
    private CharacterController controller;
    private float time;

    public void Awake()
    {
        if (levelName.randomMovement)
        {
            controller = GetComponent<CharacterController>();
            time = Random.Range(minTime, maxTime);
            primaryPosition = transform.position;
            mainDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        CheckForDistance();

        time -= Time.deltaTime;
        if(time <= 0f)
        {
            ChangeDiretion();
            time = Random.Range(minTime, maxTime);
        }

        controller.Move(7f * Time.deltaTime * mainDirection);
    }

    private void ChangeDiretion()
    {
        Vector3 n = primaryPosition - transform.position;
        mainDirection = (n.normalized + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))).normalized;
    }

    private void CheckForDistance()
    {
        if(Time.frameCount % 3 == 1)
        {
            if ((transform.position - primaryPosition).magnitude > 3f)
                ChangeDiretion();
        }
    }
}
