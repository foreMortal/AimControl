using UnityEngine;

public class Nerovnosty : MonoBehaviour
{
    [SerializeField] private GameObject[] _points;
    [SerializeField] private float _speed = 0.5f, _swipespeed = 0.5f, swapTime, min, max, minCrouch, maxCrouch, minNextCr, maxNextCr, crouchHeight = 166.87f, standHeight = 167.69f;
    private float _nexrtCrouch = 1f, _maxTimerCrouch, _timerNextCrouch, _timerCrouch, _timerSwap, _timerSpeed, _timerLimit;
    [SerializeField] private Transform _northPoint, _westPoint, _eastPoint, _southPoint;
    private Rigidbody _rigidBody;
    private bool _north = true, _south, _east, _west, _swipeSides, _crouch;
    private int _positionNow = 1;
    private float x, z;
    [SerializeField] private bool canSwapSides;

    private Transform _head, _capsule;
    [SerializeField] private GameObject _body;

    private void Start()
    {
        _capsule = _body.GetComponent<Transform>();
        _head = GetComponent<Transform>();
        _rigidBody= GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("point"))
        {
            _speed *= -1;
        }
        if (collision.gameObject.CompareTag("North"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            _swipeSides = false;
            _north = true;
            _positionNow = 1;
            foreach (GameObject i in _points)
            {
                i.SetActive(true);
            }
        }
        else if (collision.gameObject.CompareTag("South"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            _swipeSides = false;
            _south = true;
            _positionNow = 2;
            foreach (GameObject i in _points)
            {
                i.SetActive(true);
            }
        }
        else if (collision.gameObject.CompareTag("West"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            _swipeSides = false;
            _west = true;
            _positionNow = 3;
            foreach (GameObject i in _points)
            {
                i.SetActive(true);
            }
        }
        else if (collision.gameObject.CompareTag("East"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            _swipeSides = false;
            _east = true;
            _positionNow = 4;
            foreach(GameObject i in _points)
            {
                i.SetActive(true);
            }
        }
    }

    private void Update()
    {
        _timerSwap += Time.deltaTime;
        _timerSpeed += Time.deltaTime;
        ChangeSpeed();
        TimerSwap();
        SwipeSides();
        TimerCrouch();
    }

    private void FixedUpdate()
    {
        if (_north)
        {
            North();
            if (_crouch)
            {
                Crouch1();
            }
            
        }
        if (_south)
        {
            South();
            if (_crouch)
            {
                Crouch1();
            }
        }
        if (_west)
        {
            West();
            if (_crouch)
            {
                Crouch2();
            }
        }
        if (_east)
        {
            East();
            if (_crouch)
            {
                Crouch2();
            }
        }
    }

    private void North()
    {
        _rigidBody.velocity = new Vector3(_speed, -1f, 0f);
    }

    private void South()
    {
        _rigidBody.velocity = new Vector3(_speed, 0f, 0f);
    }

    private void East()
    {
        _rigidBody.velocity = new Vector3(0f, 0f, _speed);
    }

    private void West()
    {
        _rigidBody.velocity = new Vector3(0f, 0f, _speed);
    }
    private void FindNearest()
    {
        if(_positionNow == 1)
        {
            if (Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _eastPoint.position) &&
            Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_westPoint);
            }
            else if (Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_eastPoint);
            }
            else if (Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _eastPoint.position))
            {
                ChangeSides(_southPoint);
            }
        }
        if(_positionNow == 2)
        {
            if (Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _eastPoint.position) &&
            Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _northPoint.position))
            {
                ChangeSides(_westPoint);
            }
            else if (Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _northPoint.position))
            {
                ChangeSides(_eastPoint);
            }
            else if (Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _eastPoint.position))
            {
                ChangeSides(_northPoint);
            }
        }
        if(_positionNow == 3)
        {
            if (Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _northPoint.position) &&
                Vector3.Distance(transform.position, _eastPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_eastPoint);
            }
            else if (Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _eastPoint.position) &&
                Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_northPoint);
            }
            else if (Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _northPoint.position) &&
                Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _eastPoint.position))
            {
                ChangeSides(_southPoint);
            }
        }
        if(_positionNow == 4)
        {
            if (Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _northPoint.position) &&
            Vector3.Distance(transform.position, _westPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_westPoint);
            }
            else if (Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _northPoint.position) < Vector3.Distance(transform.position, _southPoint.position))
            {
                ChangeSides(_northPoint);
            }
            else if (Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _westPoint.position) &&
                Vector3.Distance(transform.position, _southPoint.position) < Vector3.Distance(transform.position, _northPoint.position))
            {
                ChangeSides(_southPoint);
            }
        }
    }
    private void ChangeSides(Transform point)
    {
        foreach(GameObject i in _points)
        {
            i.SetActive(false);
        }
        point.GetComponent<Collider>().enabled = true;
        transform.position = Vector3.MoveTowards(transform.position, point.position, _swipespeed * Time.deltaTime);
    }
    private void ChangeSpeed()
    {
        if(_timerSpeed >= _timerLimit)
        {
            _speed *= -1;
            _timerSpeed = 0;
            _timerLimit = Random.Range(min, max);
        }
    }

    private void SwipeSides()
    {
        if (_swipeSides)
        {
            _north = false; _south = false; _east = false; _west = false;
            FindNearest();
        }
    }

    private void TimerSwap()
    {
        if (_timerSwap > swapTime && canSwapSides)
        {
            _swipeSides = true;
            _timerSwap = 0f;
            _rigidBody.velocity = Vector3.zero;
        }
    }

    private void Crouch1()
    {
        _rigidBody.velocity = Vector3.zero;
        x = _head.position.x;
        x += _speed * Time.fixedDeltaTime;
        _capsule.localScale = new Vector3(_capsule.localScale.x, 0.82f, _capsule.localScale.z);
        _capsule.position = new Vector3(_head.position.x, _head.position.y - 1.2f, _head.position.z);
        _head.position = new Vector3(x, crouchHeight, _head.position.z);
    }

    private void Crouch2()
    {
        _rigidBody.velocity = Vector3.zero;
        z = _head.position.z;
        z += _speed * Time.fixedDeltaTime;
        _capsule.localScale = new Vector3(_capsule.localScale.x, 0.82f, _capsule.localScale.z);
        _capsule.position = new Vector3(_head.position.x, _head.position.y - 1.2f, _head.position.z);
        _head.position = new Vector3(_head.position.x, crouchHeight, z);
    }

    private void TimerCrouch()
    {
        if (!_crouch)
        {
            _timerNextCrouch += Time.deltaTime;
            if (_timerNextCrouch >= _nexrtCrouch)
            {
                _crouch = true;
                _timerNextCrouch = 0;
                _maxTimerCrouch = Random.Range(minCrouch, maxCrouch);
            }
        }
    
        if(_crouch == true)
        {
            _timerCrouch += Time.deltaTime;
            if(_timerCrouch >= _maxTimerCrouch)
            {
                _crouch = false;
                _timerCrouch = 0;
                _nexrtCrouch = Random.Range(minNextCr, maxNextCr);
                _capsule.localScale = new Vector3(_capsule.localScale.x, 1.29075f, _capsule.localScale.z);
                _capsule.position = new Vector3(_head.position.x, _head.position.y - 1.6f, _head.position.z);
                _head.position = new Vector3(_head.position.x, standHeight, _head.position.z);
            }
        }
    }
}
