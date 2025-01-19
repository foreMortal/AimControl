using UnityEngine;

public class TestingTestingTesting : MonoBehaviour
{
    public Camera player;
    public Transform buddy, Dummy;
    public GameObject point1;
    private GameObject point;
    public float _sens;
    public float xRot, yRot, timer, cashX, cashY;
    public LayerMask mask;
    private Vector3 hit2;
    private Vector3 lastFramePos;
    private bool lastFrameAssisted;

    private Controlls controlls;

    private void Awake()
    {
        controlls = new();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }
    void Update()
    {
        if(controlls.GamepadControl.Horizontal.ReadValue<float>() != 0f || controlls.GamepadControl.Vertical.ReadValue<float>() != 0f)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
            {
                if (lastFrameAssisted)
                {
                    cameraMove();

                    AimAssist(0.11f, 60f, hit);
                    hit2 = hit.collider.transform.position;
                }
                else
                {
                    lastFrameAssisted = true;
                    hit2 = hit.collider.transform.position;
                    cameraMove();
                    ApplyingCameraMove();
                }
            }
            else
            {
                lastFrameAssisted = false;
                if (point != null)
                {
                    Destroy(point);
                }
                cameraMove();
                ApplyingCameraMove();
            }
        }
        else if(controlls.GamepadControl.Horizontal.ReadValue<float>() == 0f && controlls.GamepadControl.Vertical.ReadValue<float>() == 0f && controlls.GamepadControl.MoveHor.ReadValue<float>() != 0f ||
                controlls.GamepadControl.Horizontal.ReadValue<float>() == 0f && controlls.GamepadControl.Vertical.ReadValue<float>() == 0f && controlls.GamepadControl.MoveVer.ReadValue<float>() != 0f)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
            {
                if(lastFramePos != Vector3.zero)
                {
                    cameraMove();
                    SecondAssist(0.5f, 240f, hit);
                    lastFramePos = transform.position;
                }
                else
                {
                    lastFramePos = transform.position;
                }
            }
            else
            {
                lastFramePos = Vector3.zero;
                cameraMove();
                ApplyingCameraMove();
            }
        }
        else
        {
            lastFrameAssisted = false;
            lastFramePos = Vector3.zero;
            if (point != null)
            {
                Destroy(point);
            }
            cameraMove();
            ApplyingCameraMove();
        }
    }

    private void cameraMove()
    {
        if (controlls.GamepadControl.Horizontal.ReadValue<float>() >= 0)
        {
            xRot += controlls.GamepadControl.Horizontal.ReadValue<float>() * _sens * Time.deltaTime;
        }
        if (controlls.GamepadControl.Horizontal.ReadValue<float>() < 0)
        {
            xRot += controlls.GamepadControl.Horizontal.ReadValue<float>() * _sens * Time.deltaTime;

        }
        /////////Y
        if (controlls.GamepadControl.Vertical.ReadValue<float>() >= 0)
        {
            yRot += controlls.GamepadControl.Vertical.ReadValue<float>() * _sens * Time.deltaTime;

        }
        if (controlls.GamepadControl.Vertical.ReadValue<float>() < 0)
        {
            yRot += controlls.GamepadControl.Vertical.ReadValue<float>() * _sens * Time.deltaTime;

        }
    }

    private void AimAssist(float assistForceX, float assistForceY, RaycastHit hit)
    {
        Vector3 moveVectorX = new Vector3(hit.collider.transform.position.x, 0f, hit.collider.transform.position.z) - new Vector3(hit2.x, 0f, hit2.z);
        float moveVectorY = hit.collider.transform.position.y - hit2.y;

        moveVectorX.Normalize();

        Vector3 rangeVector = hit.point - transform.position;
        rangeVector.y = 0f;

        float range = rangeVector.magnitude;
        range *= 0.15f;
        if (range < 1f)
            range = 1f;

        Quaternion targetX = Quaternion.LookRotation(moveVectorX, transform.up);
        Quaternion startX = Quaternion.Euler(0f, xRot, 0f);
        Quaternion assistX = Quaternion.Slerp(startX, targetX, assistForceX / range * Time.deltaTime);

        assistX.ToAngleAxis(out float xAngle, out Vector3 xVector);
        float deltaX = xAngle * xVector.y;

        xRot += deltaX - xRot;

        yRot += moveVectorY * assistForceY / range * Time.deltaTime;

        ApplyingCameraMove();
    }

    private void SecondAssist(float assistForceX, float assistForceY, RaycastHit hit)
    {
        assistForceX = CorrectAssistForce(assistForceX);

        Vector3 moveVectorX = new Vector3(lastFramePos.x, 0f, lastFramePos.z) - new Vector3(transform.position.x, 0f, transform.position.z);
        float moveVectorY = lastFramePos.y - transform.position.y;

        moveVectorX.Normalize();

        float dot = Vector3.Dot(moveVectorX, transform.forward);

        if (dot < 0.8f && dot > 0f || dot > -0.8f && dot < 0f || dot == 0f)
        {
            Vector3 rangeVector = hit.point - transform.position;
            rangeVector.y = 0f;

            float range = rangeVector.magnitude;
            range *= 0.5f;
            if (range < 1f)
                range = 1f;

            Quaternion targetX = Quaternion.LookRotation(moveVectorX, transform.up);
            Quaternion startX = Quaternion.Euler(0f, xRot, 0f);
            Quaternion assistX = Quaternion.Slerp(startX, targetX, assistForceX / range * Time.deltaTime);

            assistX.ToAngleAxis(out float b, out Vector3 a);

            xRot = b * a.y;

            yRot += moveVectorY * assistForceY / range * Time.deltaTime;

            ApplyingCameraMove();
        }
        else
        {
            ApplyingCameraMove();
        }
    }
    private void ApplyingCameraMove()
    {
        buddy.rotation = Quaternion.Euler(0f, xRot, 0f);
        player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }
        /*Vector3 moveVectorX = lastFramePos - transform.position;
        float moveVectorY = lastFramePos.y - transform.position.y;

        Quaternion targetX = Quaternion.FromToRotation(new Vector3(0f, 0f, 0f), moveVectorX);
        print(targetX);
        Quaternion startX = Quaternion.Euler(0f, xRot, 0f);
        Quaternion assistX = Quaternion.Slerp(startX, targetX, assistForce);

        assistX.ToAngleAxis(out float b, out Vector3 a);

        xRot = b * a.y;
        yRot += moveVectorY * assistForce * 600f;

        ApplyingCameraMove();*/

    private float CorrectAssistForce(float assistForce)
    {
        float hor = Mathf.Abs(controlls.GamepadControl.MoveHor.ReadValue<float>());
        float ver = Mathf.Abs(controlls.GamepadControl.MoveVer.ReadValue<float>());
        if(hor > ver)
        {
            assistForce = hor * assistForce;
            return assistForce;
        }
        else
        {
            assistForce = ver * assistForce;
            return assistForce;
        }
    }
}
