using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class mouvingScript : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private float angularSpeed = 2f, angle = 0f, radius, sec1 = 0.5f, sec2 = 2f, _seconds;
    private float positionX, positionY, positionZ;
    [SerializeField] private SphereCollider sphere;
    private int num1 = 1, num2 = 2;
    private bool pickAside = true;
    public float _side;
    private float _itog;



    private void Awake()
    {
        radius = sphere.radius;
    }

    /*private void Update()
    {
        ChaoticMouvmentHorizontal();
        CricleMouvment();
    }*/

    public void CricleMouvment()
    {
        positionZ = center.position.z + Mathf.Sin(angle) * radius;
        positionX = center.position.x + Mathf.Cos(angle) * radius;
        //positionY = center.position.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(positionX, transform.position.y, positionZ);
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360f || angle <= -360)
        {
            angle = 0f;
        }
    }

    public IEnumerator sideSwaps()
    {
        yield return new WaitForSeconds(_seconds);
        pickAside = true;
    }

    public void ChaoticMouvmentHorizontal()
    {
        if (pickAside)
        {
            _itog = Random.Range(num1, num2);
            _seconds = Random.Range(sec1, sec2);
            if (_itog == 1)
            {
                _side = -1;
            }
            if (_itog == 2)
            {
                _side = 1;
            }

            angularSpeed *= _side;
            pickAside = false;
            StartCoroutine(sideSwaps());
        }
    }
}
