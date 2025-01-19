using UnityEngine;

public class ParentScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _points;
    [SerializeField] private float _speed = 0.5f, _swipespeed = 0.5f, _timerSwap = 0f, _timerSpeed, _timerLimit;
    [SerializeField] private Transform _northPoint, _westPoint, _eastPoint, _southPoint;
    private Rigidbody _rigidBody;
    private bool _north = true, _south, _east, _west, _swipeSides;
    private int _positionNow = 1;

    private void Start()
    {
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
    }

    private void FixedUpdate()
    {
        if (_north)
        {
            North();
        }
        if (_south)
        {
            South();
        }
        if (_west)
        {
            West();
        }
        if (_east)
        {
            East();
        }
    }

    private void North()
    {
        _rigidBody.velocity = new Vector3(_speed, 0f, 0f);
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
            _timerLimit = Random.Range(1.5f, 5f);
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
        if (_timerSwap > 10)
        {
            _swipeSides = true;
            _timerSwap = 0f;
            _rigidBody.velocity = Vector3.zero;
        }
    }
    
}
