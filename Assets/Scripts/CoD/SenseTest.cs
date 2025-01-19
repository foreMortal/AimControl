using UnityEngine;
using UnityEngine.InputSystem;

public class SenseTest : MonoBehaviour
{
    [SerializeField] private Camera player;
    private float xRot, yRot;
    public float horizontal = 0.05f, vertical = 0.05f;
    private Controlls controls;
    private bool move;

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Awake()
    {
        controls = new Controlls();
    }

    private void Update()
    {
        if (move)
        {
            yRot += controls.Universal.MnKVertical.ReadValue<float>() * vertical;
            xRot += controls.Universal.MnKHorizantal.ReadValue<float>() * horizontal;
        }
        else
        {
            yRot = 0f;
            xRot = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            move = false;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            move = true;
        }

        yRot = Mathf.Clamp(yRot, -90f, 90f);
        player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }


}
