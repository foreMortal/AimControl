using UnityEngine;
using UnityEngine.UI;

public enum DoorState
{
    Open,
    Close,
    OpenReverse,
    OpenReverseBlocked,
    OpenBlocked
}

public class DoorsPosition
{
    public float rightDoor;
    public float leftDoor;

    public float rightDoorY;
    public float leftDoorY;
}

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private float speed, speed_;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private GameObject interactionUI;

    private Transform currRightState;
    private Transform currLeftState;

    [SerializeField] private bool whoIsInteracting, interactingDoor, blocked;

    private DoorsPosition Closed = new();
    private DoorsPosition Open = new();
    private DoorsPosition OpenReverse = new();

    [SerializeField] private DoorState state = DoorState.Close;

    [SerializeField] private Text stateText;
    [SerializeField] private Image gpdImage;
    [SerializeField] private Text MnKButton;
    [SerializeField] private Image MnKImage;

    private float target = 1f, current;

    private UniversalRebinds uRebinds;
    private UserRebinds rebinds;
    private ValorantRebinds valRebinds;
    private CSRebinds csRebinds;

    private void Awake()
    {
        SetupUI();
        ApexApplySettings.ReuploadSettings.AddListener(SetupUI);
    }

    private void Start()
    {
        Closed.rightDoor = 0f;
        Closed.leftDoor = 0f;

        Open.rightDoor = 120f;
        Open.leftDoor = -120f;

        OpenReverse.rightDoor = -90f;
        OpenReverse.leftDoor = 90f;

        Closed.rightDoorY = Quaternion.Euler(0f, 0f, 0f).y;
        Closed.leftDoorY = Quaternion.Euler(0f, 0f, 0f).y;

        Open.rightDoorY = Quaternion.Euler(0f, 120f, 0f).y;
        Open.leftDoorY = Quaternion.Euler(0f, -120f, 0f).y;

        OpenReverse.rightDoorY = Quaternion.Euler(0f, -90f, 0f).y;
        OpenReverse.leftDoorY = Quaternion.Euler(0f, 90f, 0f).y;
    }

    public void Blocking()
    {
        blocked = true;
    }

    public void Interact(bool whoIsInteracting)
    {
        if (!interactingDoor)
        {
            currRightState = rightDoor;
            currLeftState = leftDoor;
            this.whoIsInteracting = whoIsInteracting;// true == player/false == dummy
            interactingDoor = true;
        }
    }

    public void ShowInteractionUI(bool state, bool gamepad = false)
    {
        if (state)
        {
            if (gamepad)
            {
                gpdImage.enabled = true;
                MnKButton.enabled = false;
                MnKImage.enabled = false;
            }
            else
            {
                gpdImage.enabled = false;
                MnKButton.enabled = true;
                MnKImage.enabled = true;
            }
        }
        interactionUI.SetActive(state);
    }

    private void Update()
    {
        if (interactingDoor)//переменная становиться false когда срабатывает одна из проверок
        {
            current = Mathf.MoveTowards(current, target, speed_ * Time.deltaTime);
            if (state == DoorState.Close)
            {
                ClosedState();
            }
            else if (state == DoorState.Open)
            {
                OpenState();
            }
            else if (state == DoorState.OpenReverse)
            {
                OpenReverseState();
            }
            else if (state == DoorState.OpenBlocked)
            {
                OpenStateBlocked();
            }
            else if (state == DoorState.OpenReverseBlocked)
            {
                OpenReverseBlocked();
            }
        }
    }

    private void ClosedState()
    {
        if (whoIsInteracting)
        {
            if (rightDoor.rotation.y >= Open.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y <= Open.leftDoorY /*- 0.07f*/ || blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.Open;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f ,Open.rightDoor + 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Open.leftDoor - 5f, 0f), curve.Evaluate(current));
        }
        else if(!whoIsInteracting)
        {
            if (rightDoor.rotation.y <= OpenReverse.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y >= OpenReverse.leftDoorY /*- 0.07f*/ || blocked)
            {
                current = 0f;
                state = DoorState.OpenReverse;
                interactingDoor = false;
                blocked = false;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, OpenReverse.rightDoor - 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, OpenReverse.leftDoor + 5f, 0f), curve.Evaluate(current));
        }
    }

    private void OpenState()
    {
        if (whoIsInteracting)
        {
            if (blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.OpenBlocked;
                SetText();
            }
            else if (rightDoor.rotation.y <= Closed.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y >= Closed.leftDoorY /*- 0.07f*/)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.Close;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Closed.rightDoor - 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Closed.leftDoor + 5f, 0f), curve.Evaluate(current));
        }
        else if (!whoIsInteracting)
        {
            if (blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.OpenBlocked;
                SetText();
            }
            else if (rightDoor.rotation.y <= Closed.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y >= Closed.leftDoorY /*- 0.07f*/ || blocked)
            {
                interactingDoor = false;
                current = 0f;
                state = DoorState.Close;
                interactingDoor = false;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Closed.rightDoor - 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Closed.leftDoor + 5f, 0f), curve.Evaluate(current));
        }
    }

    private void OpenReverseState()
    {
        if (whoIsInteracting)
        {
            if (blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.OpenReverseBlocked;
                SetText();
            }
            else if (rightDoor.rotation.y >= Closed.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y <= Closed.leftDoorY /*- 0.07f*/)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.Close;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Closed.rightDoor + 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Closed.leftDoor - 5f, 0f), curve.Evaluate(current));
        }
        else if (!whoIsInteracting)
        {
            if (blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.OpenReverseBlocked;
                SetText();
            }
            else if (rightDoor.rotation.y >= Closed.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y <= Closed.leftDoorY /*- 0.07f*/ || blocked)
            {
                current = 0f;
                state = DoorState.Close;
                interactingDoor = false;
                blocked = false;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Closed.rightDoor + 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Closed.leftDoor - 5f, 0f), curve.Evaluate(current));
        }
    }

    private void OpenReverseBlocked()
    {
        if (whoIsInteracting)
        {
            if (rightDoor.rotation.y <= OpenReverse.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y >= OpenReverse.leftDoorY /*- 0.07f*/ || blocked)
            {
                current = 0f;
                state = DoorState.OpenReverse;
                interactingDoor = false;
                blocked = false;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, OpenReverse.rightDoor - 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, OpenReverse.leftDoor + 5f, 0f), curve.Evaluate(current));
        }
        else if (!whoIsInteracting)
        {
            if (rightDoor.rotation.y <= OpenReverse.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y >= OpenReverse.leftDoorY /*- 0.07f*/ || blocked)
            {
                current = 0f;
                state = DoorState.OpenReverse;
                interactingDoor = false;
                blocked = false;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, OpenReverse.rightDoor - 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, OpenReverse.leftDoor + 5f, 0f), curve.Evaluate(current));
        }
    }

    private void OpenStateBlocked()
    {
        if (whoIsInteracting)
        {
            if (rightDoor.rotation.y >= Open.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y <= Open.leftDoorY /*- 0.07f*/ || blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.Open;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Open.rightDoor + 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Open.leftDoor - 5f, 0f), curve.Evaluate(current));
        }
        else if(!whoIsInteracting)
        {
            if (rightDoor.rotation.y >= Open.rightDoorY /*- 0.07f*/ &&
                leftDoor.rotation.y <= Open.leftDoorY /*- 0.07f*/ || blocked)
            {
                interactingDoor = false;
                blocked = false;
                current = 0f;
                state = DoorState.Open;
                SetText();
            }

            rightDoor.rotation = Quaternion.Lerp(currRightState.rotation, Quaternion.Euler(0f, Open.rightDoor + 5f, 0f), curve.Evaluate(current));
            leftDoor.rotation = Quaternion.Lerp(currLeftState.rotation, Quaternion.Euler(0f, Open.leftDoor - 5f, 0f), curve.Evaluate(current));
        }
    }

    public void SetText( )
    {
        if (state == DoorState.Close)
            stateText.text = "Open";
        else if (state == DoorState.Open)
            stateText.text = "Close";
        else if (state == DoorState.OpenReverse)
            stateText.text = "Close";
        else if (state == DoorState.OpenBlocked)
            stateText.text = "Open";
        else if (state == DoorState.OpenReverseBlocked)
            stateText.text = "Open";
    }

    private void SetupUI()
    {
        rebinds = settings.rebinds;
        gpdImage.sprite = settings.SetImg((string)rebinds.Data[AllSettingsKeys.USE]);
    }
}
