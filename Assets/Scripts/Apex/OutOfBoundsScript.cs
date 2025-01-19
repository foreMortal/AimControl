using UnityEngine;
using UnityEngine.Events;

public class OutOfBoundsScript : MonoBehaviour
{
    public UnityEvent OutOfBounds = new();
    private bool alredyIn = false;
    private CrouchOnly dummy;

    private void Awake()
    {
        dummy = FindObjectOfType<CrouchOnly>(false);
        OutOfBounds.AddListener(dummy.OutOfBounds);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanGetHitted") && !alredyIn)
        {
            alredyIn = true;
            OutOfBounds.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanGetHitted"))
        {
            alredyIn = false;
        }
    }
}
