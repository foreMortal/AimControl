using UnityEngine;
using UnityEngine.InputSystem;

public class AimBotScript : MonoBehaviour
{
    private Transform target;
    private Quaternion targetRotation;
    private Camera fpsCam;
    private Collider[] colliders = new Collider[4];
    private float timer;

    private void Awake()
    {
        fpsCam = GetComponent<Camera>();
        Physics.OverlapSphereNonAlloc(transform.position, 50f, colliders, 1 << 1);
        target = colliders[0].transform;
    }

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timer = 0f;
        }
        if (target != null)
        {
            if(timer >= 0.3f)
            {
                Vector3 vectToTarget = target.position - transform.position;
                targetRotation = Quaternion.LookRotation(vectToTarget.normalized, Vector3.up);
                fpsCam.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 350f * Time.deltaTime);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            if(Physics.OverlapSphereNonAlloc(transform.position, 50f, colliders, 1 << 1) > 0)
                target = colliders[0].transform;
        }
    }
}
