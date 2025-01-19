using UnityEngine;
using UnityEngine.UI;

public class InteractWithDoor : MonoBehaviour
{
    [SerializeField] private GameObject messege;
    [SerializeField] private DoorScript door;
    [SerializeField] private bool side;
    private Text text;
    private Controlls controlls;
    private bool closeEnough;

    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }

    private void Awake()
    {
        controlls = new Controlls();
        controlls.GamepadControl.Use.performed += ctx => Interact();

        text = messege.GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messege.SetActive(true);
            closeEnough = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messege.SetActive(false);
            closeEnough = false;
        }
    }

    public void SetText(DoorState state)
    {
        if (state == DoorState.Close)
            text.text = "Open";
        else if (state == DoorState.Open)
            text.text = "Close";
        else if (state == DoorState.OpenReverse)
            text.text = "Close";
        else if (state == DoorState.OpenBlocked)
            text.text = "Open";
        else if (state == DoorState.OpenReverseBlocked)
            text.text = "Open";
    }

    private void Interact()
    {
        if (closeEnough)
        {
            door.Interact(side);
        }
    }
}
