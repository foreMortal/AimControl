using UnityEngine;

public class DummyInteractWithDoor : MonoBehaviour
{
    [SerializeField] private DoorScript door;
    [SerializeField] private bool side;
    [SerializeField] private float timeGone, timeNeed, minTime, maxTime;
    private bool canOpen;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanGetHitted"))
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanGetHitted"))
        {
            canOpen = false;
        }
    }

    private void Update()
    {
        timeGone += Time.deltaTime;
        if (canOpen && timeGone >= timeNeed)    
        {
            Interact();
            timeGone = 0;
            timeNeed = Random.Range(minTime, maxTime);
        }
    }

    private void Interact()
    {
        door.Interact(side);
    }
}
