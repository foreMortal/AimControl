using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    private Controlls controlls;
    private Transform cam;

    private RaycastHit hit;
    private LayerMask layerMask = 1 << 13;
    private IInteractable thing;
    private RebindType type;

    private bool gamepad;
    private bool lookingAt;
    private float range = 5f;

    private void OnDisable()
    {
        controlls.Disable();
    }
    private void OnEnable()
    {
        controlls.Enable();
    }

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>().transform;
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);
        type = settings.rebinds.Type;   
        
        controlls = new Controlls();
        controlls = settings.controlls;
    }

    private void Start()
    {
        switch (type)
        {
            case RebindType.Apex:
                controlls.GamepadControl.Use.performed += ctx => Interact();
                gamepad = true;
                break;
            case RebindType.Universal:
                controlls.Universal.GpdUse.performed += ctx => Interact();
                SetUpUniversalUIChange();
                break;
            case RebindType.Valorant:
                controlls.ValorantControl.MnKUse.performed += ctx => Interact();
                gamepad = false;
                break;
            case RebindType.CSGO:
                controlls.CSGOControl.MnKUse.performed += ctx => Interact();
                gamepad = false;
                break;
        }
    }

    private void Update()
    {
        if(Physics.Raycast(cam.position, cam.forward, out hit, range, layerMask))
        {
            if (!lookingAt)
            {
                thing = hit.collider.GetComponent<IInteractable>();
                lookingAt = true;
                thing.ShowInteractionUI(true, gamepad);
            }
        }
        else
        {
            if (lookingAt)
            {
                thing.ShowInteractionUI(false);
                lookingAt = false;
                thing = null;
            }
        }
    }

    private void Interact()
    {
        if (lookingAt)
        {
            float dot = Vector3.Dot(transform.forward, hit.transform.forward);
            if(dot > 0f)
            {
                thing.Interact(true);
            }
            else if(dot < 0f)
            {
                thing.Interact(false);
            }
        }
    }

    private void Reupload()
    {
        thing?.ShowInteractionUI(true, gamepad);
    }

    private void SetMnKUI()
    {
        if (gamepad)
        {
            gamepad = false;
            thing?.ShowInteractionUI(true, false);
        }
    }

    private void SetGpdUI()
    {
        if (!gamepad)
        {
            gamepad = true;
            thing?.ShowInteractionUI(true, true);
        }
    }

    private void SetUpUniversalUIChange()
    {
        controlls.UIControl.GpdCrouch.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdJump.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdFire.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdAds.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdUse.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdMoveHor.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdMoveVer.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdHorizantal.performed += ctx => SetGpdUI();
        controlls.UIControl.GpdVertical.performed += ctx => SetGpdUI();

        controlls.UIControl.MnKCrouch.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKJump.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKFire.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKAds.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKUse.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKMoveHor.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKMoveVer.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKHorizantal.performed += ctx => SetMnKUI();
        controlls.UIControl.MnKVertical.performed += ctx => SetMnKUI();
    }
}
