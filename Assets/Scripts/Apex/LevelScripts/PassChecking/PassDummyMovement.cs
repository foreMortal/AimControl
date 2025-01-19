using UnityEngine;
using UnityEngine.UI;

public class PassDummyMovement : MonoBehaviour
{
    [SerializeField] private CreateInfoCanvas canvas;
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] GetStatisticScriptableObject stats;
    [SerializeField] Transform[] spawns;
    [SerializeField] float actualSpeed = 5f;
    [SerializeField] private DummyDificulty dificulty;

    private Text killedText, lostText;

    private float speed, timeBeforeMove, beforeMoveTimer, movingTimer, movingTime;
    private float changeSpeedTimer, changeSpeedTime, crouchTime, crouchTimer, deltaTime, lifeTime;
    private Rigidbody rigidBody;
    private int side = 1, spawn;
    private Animator animator;
    private bool mustJump, jump, crouch, changeSpeed, move, grounded;
    LayerMask layerMask = 1 << 6;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        speed = actualSpeed;
        Spawn();
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    dificulty = DummyDificulty.Easy;
                    lifeTime = 1f;break;
                case "Normal":
                    dificulty = DummyDificulty.Normal;
                    lifeTime = 0.5f; break;
                case "Hard":
                    dificulty = DummyDificulty.Hard;
                    lifeTime = 0.25f; break;
                case "UltraHard":
                    dificulty = DummyDificulty.UltraHard;
                    lifeTime = 0f; break;
            }
        }

        CreateInfoCanvas c = Instantiate(canvas, transform);

        c.Setup("Target's hited:", new Vector3(-255f, 215f, 0f), new Vector3(-317f, 215f, 0f), new Vector3(-333f, 215f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        killedText = c.transform.GetChild(0).GetComponentInChildren<Text>();

        CreateInfoCanvas t = Instantiate(canvas, transform);

        t.Setup("Target's lost:", new Vector3(-260f, 194f, 0f), new Vector3(-317f, 194f, 0f), new Vector3(-333f, 194f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0.6f, 0.4f, 1f));
        lostText = t.transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public void SetupWarmup(LevelSettings obj)
    {
        switch (obj.dificulty)
        {
            case "Easy":
                dificulty = DummyDificulty.Easy;
                lifeTime = 1f; break;
            case "Normal":
                dificulty = DummyDificulty.Normal;
                lifeTime = 0.75f; break;
            case "Hard":
                dificulty = DummyDificulty.Hard;
                lifeTime = 0.5f; break;
            case "UltraHard":
                dificulty = DummyDificulty.UltraHard;
                lifeTime = 0.25f; break;
        }
    }

    private void MovingXAxis()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.right + transform.position, speed * deltaTime);
    }

    private void MovingZAxis(float vector)
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.forward + transform.position, speed * vector * deltaTime);
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        Gravitation();

        if (beforeMoveTimer >= timeBeforeMove && move)
        {
            if (mustJump)
            {
                jump = true;
                mustJump= false;
            }
            Move();
            if(crouch)
                Crouch();
            if(changeSpeed)
                ChangeSpeed();
        }
        else
        {
            beforeMoveTimer += deltaTime;
        }
    }

    private void Gravitation()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 0.521f, transform.position.z), 0.2f, layerMask);
        animator.SetBool("Grounded", grounded);
    }

    public void KillTarget(float timesDied)
    {
        killedText.text = timesDied.ToString();
        Refresh();
        Spawn();
    }

    private void TargetLost()
    {
        lostText.text = (++stats.targetsLost).ToString();
        Refresh();
        Spawn();
    }

    private void Spawn()
    {
        switch (side)
        {
            case 0:
                spawn = 0;
                transform.position = spawns[0].position; break;
            case 1:
                spawn = 1;
                transform.position = spawns[1].position; break;
            case 2:
                int num = Random.Range(2, 4);
                spawn = num;
                transform.position = spawns[num].position; break;
        }

        if((int)dificulty < 1)
        {
            GeneratePattern(false);
        }
        else
        {
            GeneratePattern(true);
        }
        move = true;
    }

    public void Refresh()
    {
        mustJump = jump = crouch = changeSpeed = move = false;
        speed = actualSpeed;

        timeBeforeMove = beforeMoveTimer = movingTimer = movingTime =
        changeSpeedTimer = changeSpeedTime = crouchTime =
        crouchTimer = 0f;
        if(animator != null)
        {
            animator.SetBool("Crouch", false);
        }
    }

    private void Move()
    {
        movingTimer += deltaTime;
        if (movingTimer < movingTime)
        {
            switch (spawn)
            {
                case 0:
                    MovingZAxis(-1f);
                    break;
                case 1:
                    MovingXAxis();
                    break;
                case 2:
                    MovingZAxis(-1f);
                    break;
                case 3:
                    MovingZAxis(1f);
                    break;
            }
            animator.SetFloat("Speed", speed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            if (movingTimer >= movingTime + lifeTime)
                TargetLost();
        }
    }

    private void Crouch()
    {
        crouchTimer += deltaTime;
        if (crouchTimer >= crouchTime && grounded)
        {
            animator.SetBool("Crouch", true);
            crouchTime = 10f;
        }
    }
    
    private void ChangeSpeed()
    {
        if(changeSpeedTimer < changeSpeedTime && grounded)
        {
            changeSpeedTimer += deltaTime;
            if (changeSpeedTimer >= changeSpeedTime)
                speed *= -1f;
        }
    }

    public void ChangeSide(int side)
    {
        this.side = side;
        Refresh();
        Spawn();
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rigidBody.AddForce(transform.up * 6f);
            animator.SetTrigger("jump");
            jump = false;
        }
    }

    private void GeneratePattern(bool changeSpeed)
    {
        timeBeforeMove = Random.Range(0.1f, 0.45f);

        int jumpNum = Random.Range(0, 2);
        if(jumpNum == 1)
        {
            mustJump = true;
        }

        movingTime = Random.Range(0.4f, 1.2f);
        
        int crouchNum = Random.Range(0, 2);
        if(crouchNum == 1)
        {
            crouch = true;
            crouchTime = movingTime - 0.2f;
        }
        if (changeSpeed)
        {
            int change = Random.Range(0, 4);
            if(change == 3)
            {
                changeSpeedTime = movingTime * 0.6f;
            }
            if (change == 4)
            {
                changeSpeedTime = movingTime * 0.85f;
            }
        }
    }
}
