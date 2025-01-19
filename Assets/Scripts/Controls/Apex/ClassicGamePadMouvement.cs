using UnityEngine;

public class ClassicGamePadMouvement : CameraMoveParent, IRecoilProvider
{
    public Controlls controlls;
    public float xRot;
    public float yRot;
    public Camera player;
    private float horizontal, adsHorizontal, _deadzone, sensNumber, vertical, adsVertical, dopPovorotX, dopPovorotY;
    private float adsSensNumber, outerTreshold = 2, AdsDopX, AdsDopY, deltaTime;
    private int _expandedSettings;
    private float dopPovorotTime, AdsDopTime, dopTimeTimer;
    private float dopPovorotDelay, AdsDopDelay, dopDelayTimer;

    private float ver, hor;
    private bool ads, active;
    private DeviceType type = DeviceType.Gamepad;

    private UserRebinds rebinds;
    public override event AdsDelegate PerformAds;

    private float recoilTime, recX, recY, recSpeedX, recSpeedY;
    private float compinsieIn;
    private float recoilDeltaX, recoilDeltaY, compinsiedX, compinsiedY;
    private float compinsieSpeedX, compinsieSpeedY;
    private bool compinsied;

    private SetupWeapon weapon;
    private SettingsScriptableObject settings;
    private float assistForce, secondAssistForce;
    private LayerMask mask = 1 << 12;
    private bool lastFrameAssisted, assistEnabled;
    private Vector3 hit2, lastFramePos;
    [SerializeField] private Transform buddy;

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
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);

        controlls = new Controlls();
        settings = GetComponentInParent<PlayerMouvement>().GetSettings();
    }

    private void Start()
    {
        controlls = settings.controlls;
        rebinds = (UserRebinds)settings.rebinds;

        controlls.GamepadControl.Ads.performed += ctx => AdsOn();
        controlls.GamepadControl.Ads.canceled += ctx => AdsOff();

        SetStats();
    }

    private void Update()
    {
        if (active)
        {
            if (_expandedSettings == 1f)
            {
                ver = controlls.GamepadControl.Vertical.ReadValue<float>();
                hor = controlls.GamepadControl.Horizontal.ReadValue<float>();
                deltaTime = Time.deltaTime;

                RecoilLogic();

                if (!ads)
                {
                    if (!assistEnabled)
                    {
                        ClassGamePadMove();
                    }
                    else
                    {
                        if (hor != 0f || ver != 0f)
                        {
                            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
                            {
                                if (lastFrameAssisted)
                                {
                                    ClassGamePadMove();

                                    AimAssist(assistForce, 60f, hit);
                                    hit2 = hit.collider.transform.position;
                                }
                                else
                                {
                                    lastFrameAssisted = true;
                                    hit2 = hit.collider.transform.position;
                                    ClassGamePadMove();
                                }
                            }
                            else
                            {
                                lastFrameAssisted = false;

                                ClassGamePadMove();
                            }
                        }
                        else if (ver == 0f && hor == 0f)
                        {
                            if (controlls.GamepadControl.MoveVer.ReadValue<float>() != 0f || controlls.GamepadControl.MoveHor.ReadValue<float>() != 0f)
                            {
                                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
                                {
                                    if (lastFramePos != Vector3.zero)
                                    {
                                        ClassGamePadMove();
                                        SecondAssist(secondAssistForce, 60f, hit);
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
                                    ClassGamePadMove();
                                }
                            }
                        }
                        else
                        {
                            lastFrameAssisted = false;
                            lastFramePos = Vector3.zero;

                            ClassGamePadMove();
                        }
                    }
                }
                else if (ads)
                {
                    if (!assistEnabled)
                    {
                        AdsClassGamePadMove();
                    }
                    else
                    {
                        if (hor != 0f || ver != 0f)
                        {
                            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
                            {
                                if (lastFrameAssisted)
                                {
                                    AdsClassGamePadMove();

                                    AimAssist(assistForce, 60f, hit);
                                    hit2 = hit.collider.transform.position;
                                }
                                else
                                {
                                    lastFrameAssisted = true;
                                    hit2 = hit.collider.transform.position;
                                    AdsClassGamePadMove();
                                }
                            }
                            else
                            {
                                lastFrameAssisted = false;

                                AdsClassGamePadMove();
                            }
                        }
                        else if (hor == 0f && ver == 0f)
                        {
                            if (controlls.GamepadControl.MoveHor.ReadValue<float>() != 0f || controlls.GamepadControl.MoveVer.ReadValue<float>() != 0f)
                            {
                                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, mask))
                                {
                                    if (lastFramePos != Vector3.zero)
                                    {
                                        AdsClassGamePadMove();
                                        SecondAssist(secondAssistForce, 60f, hit);
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
                                    AdsClassGamePadMove();
                                }
                            }
                        }
                        else
                        {
                            lastFrameAssisted = false;
                            lastFramePos = Vector3.zero;

                            AdsClassGamePadMove();
                        }
                    }
                }
                ApplyCameraMove();
            }
        }
    }

    private void ApplyCameraMove()
    {
        if (xRot > 360f)
            xRot -= 360f;
        else if (xRot < -360f)
            xRot += 360f;

        yRot = Mathf.Clamp(yRot, -90f, 90f);
        if (buddy != null)
            buddy.rotation = Quaternion.Euler(0f, xRot, 0f);
        player.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }

    public override void RotatePlayer(float x, float y)
    {
        xRot = x;
        yRot = y;
        ApplyCameraMove();
    }
    public override float[] GetRotation()
    {
        return new float[2] { xRot, yRot };
    }

    private void ClassGamePadMove()
    {
        ///////X
        if (hor != 0f)
        {
            if (dopPovorotX <= 0f) ///////Without dopPovorot
            {
                xRot += CameraMove(hor, horizontal);
            }
            else ///////DopPovorot
            {
                xRot += CameraMoveDopPovort(hor, horizontal, dopPovorotX, dopPovorotTime, dopPovorotDelay);
            }
        }

        /////////Y
        if (ver != 0f)
        {
            if (dopPovorotY <= 0f)/////////Without DopPovorot
            {
                yRot += CameraMove(ver, vertical);
            }
            else/////////DopPovorot
            {
                yRot += CameraMoveDopPovort(ver, vertical, dopPovorotY, dopPovorotTime, dopPovorotDelay);
            }
        }
    }

    private float CameraMove(float move, float sense)
    {
        float absMove = Mathf.Abs(move);

        if ((absMove * 100f) > _deadzone)
            return move * sense * deltaTime;
        else
            return 0f;
    }

    private float CameraMoveDopPovort(float move, float sense, float dopPovorot, float dopPovorotTime, float dopPovorotDelay)
    {
        float absMove = Mathf.Abs(move);
        if ((absMove * 100f) > _deadzone)
        {
            if ((absMove * 100f) > (100f - outerTreshold))
            {
                if (dopDelayTimer >= dopPovorotDelay)
                {
                    float dop = 0f;

                    if (dopTimeTimer < dopPovorotTime)
                    {
                        dopTimeTimer += deltaTime;
                        dop = dopPovorot * (dopTimeTimer / dopPovorotTime);
                    }
                    else if (dopTimeTimer >= dopPovorotTime)
                    {
                        dop = dopPovorot;
                    }

                    return move * (sense + dop) * deltaTime;
                }
                else
                {
                    dopDelayTimer += deltaTime;

                    if (dopTimeTimer > 0f)
                        dopTimeTimer = 0f;

                    return move * sense * deltaTime;
                }
            }
            else
            {
                if (dopDelayTimer > 0f)
                    dopDelayTimer = 0f;

                return move * sense * deltaTime;
            }
        }
        else
            return 0f;
    }

    private void AdsOn()
    {
        ads = true;
        PerformAds?.Invoke(ads);
    }

    private void AdsOff() 
    {
        ads = false;
        PerformAds?.Invoke(ads);
    }

    private void AdsOnPressed()
    {
        if (!ads)
        {
            AdsOn();
        }
        else if (ads)
        {
            AdsOff();
        }
    }

    private void AdsClassGamePadMove()
    {
        ///////X
        if (hor != 0)
        {
            if(AdsDopX <= 0f)////////Without DopPovorot
            {
                xRot += CameraMove(hor, adsHorizontal);
            }
            else//////////DopPovorot
            {
                xRot += CameraMoveDopPovort(hor, adsHorizontal, AdsDopX, AdsDopTime, AdsDopDelay);
            }
        }
        
        /////////Y
        if (ver != 0)
        {
            if(AdsDopY <= 0f)/////////Without DopPovorot
            {
                yRot += CameraMove(ver, adsVertical);
            }
            else/////////////DopPovorot
            {
                yRot += CameraMoveDopPovort(ver, adsVertical, AdsDopY, AdsDopTime, AdsDopDelay);
            }
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

        ApplyCameraMove();
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
            range *= 0.25f;
            if (range < 1f)
                range = 1f;

            Quaternion targetX = Quaternion.LookRotation(moveVectorX, transform.up);
            Quaternion startX = Quaternion.Euler(0f, xRot, 0f);
            Quaternion assistX = Quaternion.Slerp(startX, targetX, assistForceX / range * Time.deltaTime);

            assistX.ToAngleAxis(out float b, out Vector3 a);

            xRot = b * a.y;

            yRot += moveVectorY * assistForceY / range * Time.deltaTime;

            ApplyCameraMove();
        }
        else
        {
            ApplyCameraMove();
        }
    }
    private float CorrectAssistForce(float assistForce)
    {
        float hor = Mathf.Abs(controlls.GamepadControl.MoveHor.ReadValue<float>());
        float ver = Mathf.Abs(controlls.GamepadControl.MoveVer.ReadValue<float>());
        if (hor > ver)
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

    private void RecoilLogic()
    {
        if (recoilTime > 0f)
        {
            float x = recSpeedX * Time.deltaTime;
            float y = recSpeedY * Time.deltaTime;
            xRot += x;
            yRot += y;
            recX += x;
            recY += y;
            recoilTime -= Time.deltaTime;
        }
        else if (recoilTime > -compinsieIn)
        {
            recoilTime -= Time.deltaTime;
        }
        else
        {
            if (!compinsied)
            {
                compinsied = true;
                if (recX > 0f && recoilDeltaX <= 0)
                {
                    if (recX + recoilDeltaX <= 0f)
                        recX = 0f;
                    else
                        recX = recX + recoilDeltaX;
                }
                else if (recX < 0f && recoilDeltaX >= 0)
                {
                    if (recX + recoilDeltaX >= 0f)
                        recX = 0f;
                    else
                        recX = recX + recoilDeltaX;
                }
                if (recY > 0f && recoilDeltaY <= 0)
                {
                    if (recY + recoilDeltaY <= 0f)
                        recY = 0f;
                    else
                        recY = recY + recoilDeltaY;
                }
                if (recX != 0f)
                {
                    compinsieSpeedX = recX / 0.15f;
                }
                if (recY != 0f)
                {
                    compinsieSpeedY = recY / 0.15f;
                }
            }
            ///////////Compinsie X
            if (recX > 0)
            {
                float deltaX = compinsieSpeedX * Time.deltaTime;
                if (compinsiedX + deltaX > recX)
                {
                    xRot -= recX - compinsiedX;
                    recX = 0f;
                }
                else
                {
                    compinsiedX += deltaX;
                    xRot -= deltaX;
                }
            }
            else if (recX < 0)
            {
                float deltaX = compinsieSpeedX * Time.deltaTime;
                if (compinsiedX + deltaX < recX)
                {
                    float t = -recX - -compinsiedX;
                    xRot += t;
                    recX = 0f;
                }
                else
                {
                    compinsiedX += deltaX;
                    xRot += -deltaX;
                }
            }
            if (recY > 0)//////////compinsie Y
            {
                float deltaY = compinsieSpeedY * Time.deltaTime;
                if (compinsiedY + deltaY > recY)
                {
                    yRot -= recY - compinsiedY;
                    recY = 0f;
                }
                else
                {
                    compinsiedY += deltaY;
                    yRot -= deltaY;
                }
            }
        }
    }

    public void BulletImpact(float compinsieIn, params float[] info)
    {
        if (compinsied)
        {
            compinsied = false;
            compinsiedX = 0f;
            compinsiedY = 0f;
            recoilDeltaX = 0f;
            recoilDeltaY = 0f;
            recX = 0f;
            recY = 0f;
            this.compinsieIn = compinsieIn;
        }
        recSpeedX = info[0];
        recSpeedY = info[1];
        recoilTime = info[2];
    }

    private void Reupload()
    {
        SetStats();
    }

    private void SetStats()
    {
        _expandedSettings = (int)rebinds.Data[AllSettingsKeys.EXPANDED_SETTINGS];

        if(_expandedSettings == 1)
        {
            recoilTime = recX = recY = recSpeedX = recSpeedY = 0f;
            compinsied = false;
            weapon.ChangeRecoilProvider(this);
        }

        switch (rebinds.Data[AllSettingsKeys.CL_DEADZONE])
        {
            case 1f:
                _deadzone = 0f;
                break;
            case 2f:
                _deadzone = 8f;
                break;
            case 3f:
                _deadzone = 16f;
                break;
        }

        assistEnabled = false;
        /*switch (rebinds.Data[CL_AIM_ASSIST])
        {
            case 1f:
                assistEnabled = false;
                break;
            case 2f:
                assistForce = 0.11f;
                secondAssistForce = 0.4f;
                assistEnabled = false;
                break;
        }*/

        sensNumber = (int)rebinds.Data[AllSettingsKeys.CL_SENSITIVITY];
        adsSensNumber = (int)rebinds.Data[AllSettingsKeys.CL_ADS_SENSITIVITY];
        //////////////Non Ads
        /*switch (rebinds.Classic[HORIZONTAL])
        {
            case 0f:
                dopPovorotDelay = 0.05f;
                dopPovorotTime = 0.30f;
                break;
            case 1f:
                horizontal = 50f; vertical = 50f;
                dopPovorotX = 65f; dopPovorotY = 0f;
                dopPovorotDelay = 0.15f;
                dopPovorotTime = 0.30f;
                break;
            case 2f:
                horizontal = 80f; vertical = 50f;
                dopPovorotX = 140f; dopPovorotY = 120f;
                goto case 0f;
            case 3f:
                horizontal = 180f; vertical = 120f;
                dopPovorotX = 180f; dopPovorotY = 0f;
                goto case 0f;
            case 4f:
                horizontal = 280f; vertical = 220f;
                dopPovorotX = 200f; dopPovorotY = 0f;
                goto case 0f;
            case 5f:
                horizontal = 360f; vertical = 225f;
                dopPovorotX = 0f; dopPovorotY = 0f;
                goto case 0f;
            case 6f:
                horizontal = 480f; vertical = 300f;
                dopPovorotX = 0f; dopPovorotY = 0f;
                goto case 0f;
            case 7f:
                horizontal = 500f; vertical = 450f;
                dopPovorotX = 0f; dopPovorotY = 0f;
                goto case 0f;
            case 8f:
                horizontal = 500f; vertical = 500f;
                dopPovorotX = 100f; dopPovorotY = 0f;
                goto case 0f;
        }
        ///////////Ads
        switch (rebinds.Classic[ADS_HORIZONTAL])
        {
            case 0f:
                AdsDopDelay = 0.10f;
                AdsDopTime = 0.50f;
                break;
            case 1f:
                adsHorizontal = 53f; adsVertical = 35f;
                AdsDopX = 0f; AdsDopY = 0f;
                AdsDopDelay = 0.15f;
                AdsDopTime = 0.50f;
                break;
            case 2f:
                adsHorizontal = 56f; adsVertical = 50f;
                AdsDopX = 40f; AdsDopY = 30f;
                goto case 0f;
            case 3f:
                adsHorizontal = 133f; adsVertical = 76f;
                AdsDopX = 0f; AdsDopY = 20f;
                goto case 0f;
            case 4f:
                adsHorizontal = 155f; adsVertical = 80f;
                AdsDopX = 0f; AdsDopY = 0f;
                goto case 0f;
            case 5f:
                adsHorizontal = 200f; adsVertical = 90f;
                AdsDopX = 0f; AdsDopY = 0f;
                goto case 0f;
            case 6f:
                adsHorizontal = 480f; adsVertical = 315f;
                AdsDopX = 0f; AdsDopY = 0f;
                goto case 0f;
            case 7f:
                adsHorizontal = 500f; adsVertical = 480f;
                AdsDopX = 0f; AdsDopY = 0f;
                goto case 0f;
            case 8f:
                adsHorizontal = 500f; adsVertical = 500f;
                AdsDopX = 33f; AdsDopY = 33f;
                goto case 0f;
        }*/
    }

    public void SetUpCameraSettings(Camera player, Transform buddy, SetupWeapon weapon)
    {
        this.player = player;
        this.buddy = buddy;
        this.weapon = weapon;
    }
    public override float[] ChangeDevice(DeviceType type)
    {
        if (this.type == type)
        {
            active = true;
            return new float[] { 0f, 0f };
        }
        else
        {
            if (_expandedSettings == 1)
                return new float[] { xRot, yRot };
            else
                return new float[] { 0f, 0f };
        }
    }
    public void Activate(bool state)
    {
        enabled = state;
    }
    public override void RandomSense(float procent, bool turnExtra) { }
}
