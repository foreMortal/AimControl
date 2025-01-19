using UnityEngine;
public enum DummyState
{
    North,
    South,
    West,
    East,
    CorrectingStrafe,
    Circle
}

public enum DummyDificulty
{
    Easy,
    Normal,
    Hard,
    UltraHard,
}

public class CrouchOnly : MonoBehaviour
{
    private LayerMask layerMask = 1 << 6;
    private float jumpTimer, nextJumpTimer = 2.5f, groundDistance = 0.2f, gravity;
    private bool outOfBounds = false, canJump;
    private Rigidbody rigidBody;

    [SerializeField] LevelNameObjject levelName;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;
    [SerializeField] private float _speed = 0.5f, _swipespeed = 0.5f, swapTime, min, max, minCrouch, maxCrouch, minNextCr, maxNextCr;

    [SerializeField] private bool canSwapSides, moving, jumpingLogic = true;
    [SerializeField] private DummyState state = DummyState.North;

    private float _nexrtCrouch = 1f, _maxTimerCrouch, _timerNextCrouch, _timerCrouch, _timerSwap, _timerSpeed, _timerLimit;
    private Transform pointToMove;
    private bool _swipeSides, _crouch, grounded, pointed, collided;
    private Transform _northPoint, _westPoint, _eastPoint, _southPoint;

    private int ultraHardCrouchesCount, maxUltraHardCrouchesCount;
    private bool onGround;
    private Vector3 jumpDir;
    [SerializeField] private DummyDificulty dificulty;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        if(levelName.type != "Warmup")
        {
            switch (levelName.dificulty)
            {
                case "Easy":
                    dificulty = DummyDificulty.Easy;
                    break;
                case "Normal":
                    dificulty = DummyDificulty.Normal;
                    break;
                case "Hard":
                    dificulty = DummyDificulty.Hard;
                    jumpingLogic = false; break;
                case "UltraHard":
                    dificulty = DummyDificulty.UltraHard;
                    jumpingLogic = false; break;
            }
        }

        if (canSwapSides)
        {
            _northPoint = GameObject.FindWithTag("North").transform;
            _southPoint = GameObject.FindWithTag("South").transform;
            _westPoint = GameObject.FindWithTag("West").transform;
            _eastPoint = GameObject.FindWithTag("East").transform;
        }
    }

    public void SetupWarmup(LevelSettings obj)
    {
        switch (obj.dificulty)
        {
            case "Easy":
                dificulty = DummyDificulty.Easy;
                break;
            case "Normal":
                dificulty = DummyDificulty.Normal;
                break;
            case "Hard":
                dificulty = DummyDificulty.Hard;
                jumpingLogic = false; break;
            case "UltraHard":
                dificulty = DummyDificulty.UltraHard;
                jumpingLogic = false; break;
        }
    }

    public void SetParameters(DummyParameters parameters)
    {
        _swipespeed = parameters._swipespeed;
        swapTime = parameters.swapTime;
        min = parameters.min;
        max = parameters.max;
        minCrouch = parameters.minCrouch;
        maxCrouch = parameters.maxCrouch;
        minNextCr = parameters.minNextCr;
        maxNextCr = parameters.maxNextCr;
    }

    public void PointedLogicFalse()
    {
        collided = false;
    }
    public void PointedLogic()
    {
        collided = true;
        _speed *= -1;
        _timerSpeed = 0f;

        _timerLimit = Random.Range(min, max);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("point") && !collided)
        {
            collided = true;
            _speed *= -1;
            _timerSpeed = 0f;
            if (!pointed)
            {
                _timerLimit = 2f;
            }
            else if (pointed)
            {
                _timerLimit = Random.Range(min + 0.5f, max + 0.5f);
            }
        }
        if (collision.gameObject.CompareTag("North"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;

            state = DummyState.North;
        }
        else if (collision.gameObject.CompareTag("South"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;

            state = DummyState.South;
        }
        else if (collision.gameObject.CompareTag("West"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;

            state = DummyState.West;
        }
        else if (collision.gameObject.CompareTag("East"))
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            collision.gameObject.GetComponent<Collider>().enabled = false;

            state = DummyState.East;
        }
    }

    public void OutOfBounds()
    {
        if (state != DummyState.CorrectingStrafe)
        {
            outOfBounds = true;
        }
    }

    private void Update()
    {
        Gravitation();

        Movement();

        OutOfboundsLogic();

        if (jumpingLogic)
            JumpingLogic();

        ChangeSpeed();

        if (canSwapSides && onGround)
        {
            TimerSwap();
            SwipeSides();
            ChangeSides();
        }

        TimerCrouch();
    }

    public void SetTargetingPosition(DummyState state, bool onGround)
    {
        this.state = state;
        this.onGround = onGround;
    }

    private void Gravitation()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, layerMask);
        animator.SetBool("Grounded", grounded);
    }

    private void OutOfboundsLogic()
    {
        if (outOfBounds)
        {
            if (grounded)
            {
                _speed *= -1;
                _timerSpeed = 0f;
                _timerLimit = Random.Range(min + 0.5f, max + 0.5f);
                outOfBounds = false;
            }
        }
    }

    private void JumpingLogic()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= nextJumpTimer)
        {
            canJump = true;
        }
    }

    private void Movement()
    {
        if (moving)
        {
            switch (state)
            {
                case DummyState.North:
                case DummyState.South:
                    MovingXAxis();
                    break;
                case DummyState.West:
                case DummyState.East:
                    MovingZAxis();
                    break;
                case DummyState.Circle:
                    MovingCircle(); break;
            }
        }
        if (canJump)
        {
            if (state != DummyState.CorrectingStrafe && !_crouch && grounded)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 6f, rigidBody.velocity.z);

                jumpTimer = 0;
                nextJumpTimer = Random.Range(3f, 5f);
                canJump = false;
                animator.SetTrigger("jump");
            }
        }
    }

    private void MovingXAxis()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.right + transform.position, _speed * Time.deltaTime);
    }

    private void MovingZAxis()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.forward + transform.position, _speed * Time.deltaTime);
    }

    private void MovingCircle()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.right + transform.position, _speed * Time.deltaTime);
    }

    private void FindNearest()
    {
        float distToNorth = (_northPoint.position - transform.position).sqrMagnitude;
        float distToSouth = (_southPoint.position - transform.position).sqrMagnitude;
        float distToWest = (_westPoint.position - transform.position).sqrMagnitude;
        float distToEast = (_eastPoint.position - transform.position).sqrMagnitude;

        switch (state)
        {
            case DummyState.North:
            case DummyState.South:
                if (distToEast < distToWest)
                {
                    pointToMove = _eastPoint;
                    state = DummyState.CorrectingStrafe;
                }
                else
                {
                    pointToMove = _westPoint;
                    state = DummyState.CorrectingStrafe;
                }
                break;
            case DummyState.West:
            case DummyState.East:
                if (distToNorth < distToSouth)
                {
                    pointToMove = _northPoint;
                    state = DummyState.CorrectingStrafe;
                }
                else
                {
                    pointToMove = _southPoint;
                    state = DummyState.CorrectingStrafe;
                }
                break;
        }
    }

    private void ChangeSides()
    {
        if (state == DummyState.CorrectingStrafe)
        {
            pointToMove.GetComponent<Collider>().enabled = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointToMove.position.x, 164.27f, pointToMove.position.z), _swipespeed * Time.deltaTime);
        }
    }
    private void ChangeSpeed()
    {
        if (grounded)
        {
            _timerSpeed += Time.deltaTime;
            if (_timerSpeed >= _timerLimit)
            {
                switch (dificulty)
                {
                    case DummyDificulty.Easy:
                        ChangeSpeedEasy(0); break;
                    case DummyDificulty.Normal:
                        ChangeSpeedNormal(); break;
                    case DummyDificulty.Hard:
                    case DummyDificulty.UltraHard:
                        ChangeSpeedHard(); break;
                }
            }
        }
    }

    private void ChangeSpeedEasy(int type, float value = 0f)
    {
        if (!outOfBounds)
        {
            _speed *= -1;
            animator.SetFloat("Speed", _speed);
            _timerSpeed = 0;
            if (type == 0)
            {
                int num = Random.Range(0, 3);
                switch (num)
                {
                    case 0:
                        _timerLimit = min;
                        break;
                    case 1:
                        _timerLimit = max * 0.65f;
                        break;
                    case 2:
                        _timerLimit = max;
                        break;
                }
            }
            else
            {
                _timerLimit = value;
            }
        }
    }

    private void ChangeSpeedNormal()
    {
        if (!outOfBounds)
        {
            int num = Random.Range(0, 2);
            if (num == 1)
            {
                Crouch(0, Random.Range(0.4f, 0.5f), Random.Range(minNextCr, maxNextCr));
            }
            _speed *= -1;
            animator.SetFloat("Speed", _speed);
            _timerSpeed = 0;

            int num1 = Random.Range(0, 3);
            switch (num1)
            {
                case 0:
                    _timerLimit = min;
                    break;
                case 1:
                    _timerLimit = max * 0.65f;
                    break;
                case 2:
                    _timerLimit = max;
                    break;
            }
        }
    }

    private void ChangeSpeedHard()
    {
        if (!outOfBounds)
        {
            int num = Random.Range(0, 3);
            if (num == 1)
            {
                Crouch(0, Random.Range(0.4f, 0.5f), Random.Range(minNextCr, maxNextCr));
            }
            else if (num == 2)
            {
                _timerLimit = 0.3f;
            }
            else
            {
                int num1 = Random.Range(0, 3);
                switch (num1)
                {
                    case 0:
                        _timerLimit = min;
                        break;
                    case 1:
                        _timerLimit = max * 0.65f;
                        break;
                    case 2:
                        _timerLimit = max;
                        break;
                }
            }
            _speed *= -1;
            animator.SetFloat("Speed", _speed);
            _timerSpeed = 0;
        }
    }

    private void SwipeSides()
    {
        if (_swipeSides && grounded)
        {
            FindNearest();
            _swipeSides = false;
        }
    }

    private void TimerSwap()
    {
        _timerSwap += Time.deltaTime;
        if (_timerSwap > swapTime && canSwapSides)
        {
            _swipeSides = true;
            _timerSwap = 0f;
            swapTime = Random.Range(8.5f, 14.5f);
        }
    }



    private void TimerCrouch()
    {
        if (!_crouch)
        {
            _timerNextCrouch += Time.deltaTime;
            if (_timerNextCrouch >= _nexrtCrouch && grounded)
            {
                switch (dificulty)
                {
                    case DummyDificulty.Easy:
                        Crouch(0, Random.Range(minCrouch, maxCrouch), Random.Range(minNextCr, maxNextCr));
                        break;
                    case DummyDificulty.Normal:
                    case DummyDificulty.Hard:
                        CrouchNormal();
                        break;
                    case DummyDificulty.UltraHard:
                        CrouchUltraHard();
                        break;
                }
            }
        }
        if (_crouch)
        {
            _timerCrouch += Time.deltaTime;
            if (_timerCrouch >= _maxTimerCrouch)
            {
                _crouch = false;
                animator.SetBool("Crouch", false);
                
                if (dificulty == DummyDificulty.Hard || dificulty == DummyDificulty.UltraHard)
                {
                    if (!outOfBounds)
                    {
                        int num = Random.Range(0, 2);
                        if (num == 1)
                        {
                            _speed *= -1f;
                            _timerLimit = Random.Range(min, max);
                            _timerSpeed = 0f;
                        }
                    }
                }
            }
        }
    }

    private void Crouch(int type, float crouchTime, float nextCrouch = 0f)
    {
        if(type == 0)
            _maxTimerCrouch = crouchTime;
        _nexrtCrouch = nextCrouch;

        _timerNextCrouch = 0;
        _timerCrouch = 0f;

        _crouch = true;
        animator.SetBool("Crouch", true);
    }

    private void CrouchNormal()
    {
        int type = Random.Range(0, 2);
        if (type == 1 && !outOfBounds)
        {
            int num = Random.Range(0, 2);
            switch (num)
            {
                case 0:
                    float time = Random.Range(0.5f, 1f);
                    Crouch(0, Random.Range(0.4f, 0.5f), time);
                    ChangeSpeedEasy(1, time + 0.1f); break;
                case 1:
                    float time1 = Random.Range(0.5f, 1f);
                    Crouch(0, Random.Range(0.4f, 0.5f), time1);
                    _timerLimit = time1 + 0.1f;
                    _timerSpeed = 0f; break;
            }
        }
        else
        {
            Crouch(0, Random.Range(minCrouch, maxCrouch), Random.Range(minNextCr, maxNextCr));
        }
    }

    private void CrouchUltraHard()
    {
        int type = Random.Range(0, 3);
        if (type == 1 && !outOfBounds && ultraHardCrouchesCount == 0f)
        {
            int num = Random.Range(0, 2);
            switch (num)
            {
                case 0:
                    float time = Random.Range(0.5f, 1f);
                    Crouch(0, Random.Range(0.4f, 0.5f), time);
                    ChangeSpeedEasy(1, time + 0.1f); break;
                case 1:
                    float time1 = Random.Range(0.5f, 1f);
                    Crouch(0, Random.Range(0.4f, 0.5f), time1);
                    _timerLimit = time1 + 0.1f;
                    _timerSpeed = 0f; break;
            }
        }
        if (type == 2 && !outOfBounds || ultraHardCrouchesCount > 0 && !outOfBounds && ultraHardCrouchesCount < maxUltraHardCrouchesCount)
        {
            if (ultraHardCrouchesCount == 0)
                maxUltraHardCrouchesCount = Random.Range(1, 5);
            ultraHardCrouchesCount++;
            int num = Random.Range(0, 2);
            switch (num)
            {
                case 0:
                    float time = Random.Range(0.5f, 1f);
                    Crouch(0, Random.Range(0.4f, 0.5f), time);
                    ChangeSpeedEasy(1, time + 0.1f); break;
            }
        }
        else
        {
            Crouch(0, Random.Range(minCrouch, maxCrouch), Random.Range(minNextCr, maxNextCr));
            ultraHardCrouchesCount = 0;
        }
    }
}