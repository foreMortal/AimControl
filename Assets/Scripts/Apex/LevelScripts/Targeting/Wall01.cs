using UnityEngine;

public class Wall01 : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f, _timerSpeed, _timerLimit;
    private Rigidbody _rigidBody;

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
    }

    private void Update()
    {

        _timerSpeed += Time.deltaTime;
        ChangeSpeed();
    }

    private void FixedUpdate()
    {
        North();
    }

    private void North()
    {
        _rigidBody.velocity = new Vector3(_speed, 0f, 0f);
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

}
