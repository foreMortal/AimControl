using UnityEngine;
using UnityEngine.Events;

public class PointedLogic : MonoBehaviour
{
    [SerializeField] private UnityEvent pointed;
    [SerializeField] private UnityEvent disPointed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("point"))
        {
            pointed.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("point"))
        {
            disPointed.Invoke();
        }
    }
}
