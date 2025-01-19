using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed, deltaSpeed, increasedSpeed, decreasedSpeed, minChangeTime, maxChangeTime;
    [SerializeField] private float regularSpeedLimit, minRegLimit, maxRegLimit, currSpeed;
    [SerializeField] private bool canChangeSpeed;

    private MeshRenderer mesh;
    private float time, changingTime, timeLimit;
    private bool changeSpeed, hidden;
    private int changeSpeedDir, num = 1;

    public void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void SetUp(TrackingSphereScriptableObject obj)
    {
        speed = obj.speed;
        deltaSpeed= obj.deltaSpeed;
        increasedSpeed= obj.increasedSpeed;
        decreasedSpeed = obj.deacreasedSpeed;
        minChangeTime = obj.minChangeTime;
        maxChangeTime = obj.maxChangeTime;
        regularSpeedLimit = obj.regularSpeedLimit;
        minRegLimit = obj.minRegLimit;
        maxRegLimit = obj.maxRegLimit;
        currSpeed = obj.currSpeed;
        canChangeSpeed= obj.canChangeSpeed;

        currSpeed = speed;
        regularSpeedLimit = Random.Range(minRegLimit, maxRegLimit);
    }

    private void Update()
    {
        if (!hidden)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, currSpeed * Time.deltaTime);

            if (canChangeSpeed)
            {
                ChangeSpeedTimeLogic();

                if (changeSpeed)
                    ChangingSpeedLogic();
            }
        }
    }

    public void HideSphere(bool state)
    {
        mesh.enabled = !state;
        hidden = state;
    }

    public void ChangeSpeed()
    {
        if (!changeSpeed)
        {
            changeSpeed = true;
            canChangeSpeed = false;
            changeSpeedDir = Random.Range(1, 3);
        }
    }

    private void IncreasingSpeedLogic()
    {
        if(num == 1)
        {
            changingTime += Time.deltaTime;
            currSpeed = Mathf.MoveTowards(currSpeed, increasedSpeed, deltaSpeed * Time.deltaTime);
            if(changingTime >= timeLimit)
            {
                num = 2;
                changingTime = 0f;
                NewTimeLimit();
            }
        }
        else if(num == 2)
        {
            currSpeed = Mathf.MoveTowards(currSpeed, speed, deltaSpeed * Time.deltaTime);
            if (currSpeed == speed)
            {
                changeSpeed = false;
                CanChangeSpeedLogic();
                num = 1;
            }
        }
    }

    private void DecreasingSpeedLogic()
    {
        if (num == 1)
        {
            changingTime += Time.deltaTime;
            currSpeed = Mathf.MoveTowards(currSpeed, decreasedSpeed, deltaSpeed * Time.deltaTime);
            if (changingTime >= timeLimit)
            {
                num = 2;
                changingTime = 0f;
                NewTimeLimit();
            }
        }
        else if (num == 2)
        {
            currSpeed = Mathf.MoveTowards(currSpeed, speed, deltaSpeed * Time.deltaTime);
            if (currSpeed == speed)
            {
                changeSpeed = false;
                num = 1;
                CanChangeSpeedLogic();
            }
        }
    }

    private void ChangingSpeedLogic()
    {
        if (changeSpeedDir == 1)
            IncreasingSpeedLogic();
        else if (changeSpeedDir == 2)
            DecreasingSpeedLogic();
    }

    private void ChangeSpeedTimeLogic()
    {
        time += Time.deltaTime;
        if (time >= regularSpeedLimit)
        {
            time = 0f;
            ChangeSpeed();
        }
    }

    private void NewTimeLimit()
    {
        timeLimit = Random.Range(minChangeTime, maxChangeTime); 
    }

    private void CanChangeSpeedLogic()
    {
        canChangeSpeed = true;
        regularSpeedLimit = Random.Range(minRegLimit, maxRegLimit);
    } 
}
