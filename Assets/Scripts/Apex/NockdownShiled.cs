using UnityEngine;

public class NockdownShiled : MonoBehaviour
{
    [SerializeField] private GameObject LookAt;
    Vector3 point;

    void Update()
    {
        point = new Vector3(LookAt.transform.position.x, transform.position.y, LookAt.transform.position.z);
        transform.LookAt(point);
        //transform.forward = new Vector3(transform.position.x, LookAt.transform.position.y, transform.position.z);
    }
}
