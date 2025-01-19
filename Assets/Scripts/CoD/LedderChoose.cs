using UnityEngine;
using UnityEngine.Events;

public class LedderChoose : MonoBehaviour
{
    public UnityEvent StairsChoosen;

    private void OnTriggerEnter(Collider other)
    {
        StairsChoosen.Invoke();
    }
}
