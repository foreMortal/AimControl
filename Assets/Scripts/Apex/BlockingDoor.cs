using UnityEngine;

public class BlockingDoor : MonoBehaviour
{
    [SerializeField] private DoorScript door;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.Blocking();
        }
    }
}
