using UnityEngine;
using UnityEngine.Events;

public class NewerAimAssist : MonoBehaviour
{
    public LayerMask mask;
    private Transform dummy;
    private Transform cam;
    public UnityEvent<Vector3> aimAssist;
    private RaycastHit hit;

    private void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").transform;
        dummy = GameObject.FindWithTag("CanGetHitted").transform;
    }

    private void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100f, mask))
        {
            Vector3 delta = transform.forward - cam.forward;
            aimAssist.Invoke(delta);
            transform.LookAt(hit.point);
            print(delta);
        }
    }

    private void AimAssist()
    {
        transform.LookAt(dummy);

    }
}
