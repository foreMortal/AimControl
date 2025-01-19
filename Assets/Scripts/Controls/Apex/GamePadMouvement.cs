using Unity.VisualScripting;
using UnityEngine;

public class GamePadMouvement : CameraMoveParent, IRecoilProvider
{ 
    public float tyt1;
    public Controlls controlls;
    public float xRot;
    public float yRot;
    public bool active = true;
    public Camera player;
    private int _expandedSetings;
    private float _horizontal, _vertical, _dopPovorotX, _dopPovorotY, _deadzone, _outerTreshold, _recoilCurve, _dopRampTime, _dopDelay /*_corilation = 0.0098f*/;
    private float adsHorizontal, adsVertical, adsDopPovorotX, adsDopPovorotY, _adsRampTime, _adsDopDelay /*adsCorilation = 0.7f*/;
    private float deltaTime, blendDopX, blendDopY, dopTimer, dopSpeedX, dopSpeedY, adsDopSpeedX, adsDopSpeedY;
    private float horHand, verHand, adsHorHand, adsVerHand, extraHorHand, extraVerHand, extraAdsHorHand, extraAdsVerHand;
    private Vector2 cameraMove;
    private bool ads, AutoCompensation;
    private UserRebinds rebinds;
    private DeviceType type = DeviceType.Gamepad;

    private SetupWeapon weapon;
    private float recoilTime, recX, recY, recSpeedX, recSpeedY;
    private float compinsieIn;
    private float recoilDeltaX, recoilDeltaY, compinsiedX, compinsiedY;
    private float compinsieSpeedX, compinsieSpeedY;

    private bool compinsied = true, gamepadConnected;

    public override event AdsDelegate PerformAds;

    //private float assistForce, secondAssistForce;
    private LayerMask mask = 1 << 12;
    //private bool lastFrameAssisted, assistEnabled;
    private Vector3 hit2, lastFramePos;
    private SettingsScriptableObject settings;
    [SerializeField] private Transform buddy;

    private void Awake()
    {
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);

        controlls = new Controlls();

        settings = GetComponentInParent<PlayerMouvement>().GetSettings();
    }

    private void Start()
    {
        controlls = settings.controlls;
        rebinds = settings.rebinds;
        controlls.GamepadControl.Ads.performed += ctx => AdsOn();
        controlls.GamepadControl.Ads.canceled += ctx => AdsOff();
        SetStats();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }

    private void Update()
    {
        if (active)
        {
            cameraMove = controlls.GamepadControl.Newaction1.ReadValue<Vector2>();
            deltaTime = Time.deltaTime;

            RecoilLogic();

            if (!ads)
            {
                InputPreProccesing(cameraMove, _horizontal, _vertical, _dopPovorotX, _dopPovorotY, dopSpeedX, dopSpeedY);
            }
            else if (ads)
            {
                InputPreProccesing(cameraMove, adsHorizontal, adsVertical, adsDopPovorotX, adsDopPovorotY, adsDopSpeedX, adsDopSpeedY);
            }
            ApplyCameraMove();
        }
    }

    private void ApplyCameraMove()
    {
        if (xRot > 360f)
            xRot -= 360f;
        else if (xRot < -360f)
            xRot += 360f;

        yRot = Mathf.Clamp(yRot, -90, 90);

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
    
    private void InputPreProccesing(Vector2 move, float hSense, float vSense, float dopPovorotX, float dopPovorotY,
        float dopSpeedX, float dopSpeedY)
    {
        float mag = move.magnitude;
        if (mag > _deadzone)
        {
            if (mag > 1)
                mag = 1;

            float t = 1 - _outerTreshold;
            t += _deadzone;

            mag = (mag - t) / (_outerTreshold - _deadzone);

            move = move.normalized * mag;

            float horMove = move.x;
            float verMove = move.y;

            if (mag >= 1)
            {
                blendDopX = DopPovorot(blendDopX, dopPovorotX, dopSpeedX);
                blendDopY = DopPovorot(blendDopY, dopPovorotY, dopSpeedY);

                hSense += blendDopX;
                vSense += blendDopY;
            }
            else if (dopTimer > 0 || blendDopX > 0 || blendDopY > 0)
            {
                blendDopX = 0f;
                blendDopY = 0f;
                dopTimer = 0f;
            }

            float x;
            float y;
            if (_recoilCurve == 0)
            {
                x = horMove * hSense * Time.deltaTime;
                y = verMove * vSense * Time.deltaTime;
            }
            else
            {
                float m = Mathf.Pow(mag, _recoilCurve);

                x = horMove * hSense * m * Time.deltaTime;
                y = verMove * vSense * m * Time.deltaTime;
            }

            xRot += x;
            yRot += y;
            if (recoilTime > 0f)
            {
                recoilDeltaX += x;
                recoilDeltaY += y;
            }
        }
    }
    private float DopPovorot(float currentState, float dopPovorot, float speed)
    {
        if (_dopDelay > 0)
        {
            if (dopTimer >= _dopDelay)
            {
                currentState = RampTime(currentState, dopPovorot, speed);
            }
            else
                dopTimer += Time.deltaTime;
        }
        else
        {
            currentState = RampTime(currentState, dopPovorot, speed);
        }
        return currentState;
    }

    private float RampTime(float currentState, float dopPovorot, float speed)
    {
        if (_dopRampTime > 0f)
        {
            if (currentState < dopPovorot)
            {
                currentState += speed * Time.deltaTime;
                if (currentState >= dopPovorot)
                    currentState = dopPovorot;
            }
            else
            {
                currentState = dopPovorot;
            }
        }
        else
        {
            currentState = dopPovorot;
        }
        return currentState;
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

    private void Reupload()
    {
        SetStats();
    }

    private void SetStats()
    {
        _expandedSetings = (int)rebinds.Data[AllSettingsKeys.EXPANDED_SETTINGS];
        if (_expandedSetings == 2)
        {
            horHand = (float)rebinds.Data[AllSettingsKeys.HORIZONTAL];
            verHand = (float)rebinds.Data[AllSettingsKeys.VERTICAL];
            extraHorHand = (float)rebinds.Data[AllSettingsKeys.EXTRA_HORIZONTAL];
            extraVerHand = (float)rebinds.Data[AllSettingsKeys.EXTRA_VERTICAL];
            _deadzone = (int)rebinds.Data[AllSettingsKeys.DEADZONE] / 100f;
            adsHorHand = (float)rebinds.Data[AllSettingsKeys.ADS_HORIZONTAL] * 0.75f;
            adsVerHand = (float)rebinds.Data[AllSettingsKeys.ADS_VERTICAL] * 0.75f;
            extraAdsHorHand = (float)rebinds.Data[AllSettingsKeys.EXTRA_ADS_HORIZONTAL] * 0.75f;
            extraAdsVerHand = (float)rebinds.Data[AllSettingsKeys.EXTRA_ADS_VERTICAL] * 0.75f;
            _outerTreshold = (100 - (float)rebinds.Data[AllSettingsKeys.OUTER_TRESHOLD]) / 100f;
            _recoilCurve = (float)rebinds.Data[AllSettingsKeys.RECOIL_CURVE] / 10;
            _dopRampTime = (float)rebinds.Data[AllSettingsKeys.RAMPTIME] / 100;
            _dopDelay = (float)rebinds.Data[AllSettingsKeys.DELAY] / 100;
            _adsRampTime = (float)rebinds.Data[AllSettingsKeys.ADS_RAMPTIME] / 100;
            _adsDopDelay = (float)rebinds.Data[AllSettingsKeys.ADS_DELAY] / 100;

            if (AcceptSettingsChanges)
            {
                if (_dopRampTime > 0)
                {
                    dopSpeedX = extraHorHand / _dopRampTime;
                    dopSpeedY = extraVerHand / _dopRampTime;
                }
                if (_adsRampTime > 0)
                {
                    adsDopSpeedX = extraAdsHorHand / _adsRampTime;
                    adsDopSpeedY = extraAdsVerHand / _adsRampTime;
                }
                _horizontal = horHand;
                _vertical = verHand;
                _dopPovorotX = extraHorHand;
                _dopPovorotY = extraVerHand;
                adsHorizontal = adsHorHand;
                adsVertical = adsVerHand;
                adsDopPovorotX = extraAdsHorHand;
                adsDopPovorotY = extraAdsVerHand;
            }
        }
        else
        {
            ClassicSettings();
        }

        if ((int)rebinds.Data[AllSettingsKeys.AUTO_COMPENSATION] == 1)
            AutoCompensation = true;
        else
            AutoCompensation = false;

        recoilTime = recX = recY = recSpeedX = recSpeedY = 0f;
        compinsied = false;
        weapon.ChangeRecoilProvider(this);
    }

    private void ClassicSettings()
    {
        switch ((int)rebinds.Data[AllSettingsKeys.CL_SENSITIVITY])
        {
            case 1:
                horHand = 50f;
                verHand = 50f;
                extraHorHand = 60f;
                extraVerHand = 0f;
                _dopRampTime = 0.5f;
                _dopDelay = 0.05f;
                break;
            case 2:
                horHand = 80f;
                verHand = 50f;
                extraHorHand = 150f;
                extraVerHand = 120f;
                _dopRampTime = 0.3f;
                _dopDelay = 0f;
                break;
            case 3:
                horHand = 160f;
                verHand = 120f;
                extraHorHand = 220f;
                extraVerHand = 0f;
                _dopRampTime = 0.33f;
                _dopDelay = 0f;
                break;
            case 4:
                horHand = 240f;
                verHand = 200f;
                extraHorHand = 220f;
                extraVerHand = 0f;
                _dopRampTime = 0.3f;
                _dopDelay = 0f;
                break;
            case 5:
                horHand = 380f;
                verHand = 240f;
                extraHorHand = 0f;
                extraVerHand = 0f;
                _dopRampTime = 0f;
                _dopDelay = 0f;
                break;
            case 6:
                horHand = 450f;
                verHand = 300f;
                extraHorHand = 0f;
                extraVerHand = 0f;
                _dopRampTime = 0f;
                _dopDelay = 0f;
                break;
            case 7:
                horHand = 500f;
                verHand = 500f;
                extraHorHand = 0f;
                extraVerHand = 0f;
                _dopRampTime = 0f;
                _dopDelay = 0f;
                break;
            case 8:
                horHand = 500f;
                verHand = 500f;
                extraHorHand = 0f;
                extraVerHand = 0f;
                _dopRampTime = 0f;
                _dopDelay = 0f;
                break;
        }
        switch ((int)rebinds.Data[AllSettingsKeys.CL_ADS_SENSITIVITY])
        {
            case 1:
                adsHorHand = 35f * 0.75f;
                adsVerHand = 35f * 0.75f;
                extraAdsHorHand = 20f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0.5f;
                _adsDopDelay = 0.05f;
                break;
            case 2:
                adsHorHand = 60f * 0.75f;
                adsVerHand = 50f * 0.75f;
                extraAdsHorHand = 35f * 0.75f;
                extraAdsVerHand = 35f * 0.75f;
                _adsRampTime = 0.5f;
                _adsDopDelay = 0f;
                break;
            case 3:
                adsHorHand = 110f * 0.75f;
                adsVerHand = 75f * 0.75f;
                extraAdsHorHand = 30f * 0.75f;
                extraAdsVerHand = 30f * 0.75f;
                _adsRampTime = 1f;
                _adsDopDelay = 0.25f;
                break;
            case 4:
                adsHorHand = 150f * 0.75f;
                adsVerHand = 80f * 0.75f;
                extraAdsHorHand = 0f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0f;
                _adsDopDelay = 0f;
                break;
            case 5:
                adsHorHand = 200f * 0.75f;
                adsVerHand = 90f * 0.75f;
                extraAdsHorHand = 0f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0f;
                _adsDopDelay = 0f;
                break;
            case 6:
                adsHorHand = 450f * 0.75f;
                adsVerHand = 300f * 0.75f;
                extraAdsHorHand = 0f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0f;
                _adsDopDelay = 0f;
                break;
            case 7:
                adsHorHand = 500f * 0.75f;
                adsVerHand = 500f * 0.75f;
                extraAdsHorHand = 0f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0f;
                _adsDopDelay = 0f;
                break;
            case 8:
                adsHorHand = 500f * 0.75f;
                adsVerHand = 500f * 0.75f;
                extraAdsHorHand = 0f * 0.75f;
                extraAdsVerHand = 0 * 0.75f;
                _adsRampTime = 0f;
                _adsDopDelay = 0f;
                break;
        }

        _outerTreshold = 0.99f;
        switch (rebinds.Data[AllSettingsKeys.CL_DEADZONE])
        {
            case 1f:
                _deadzone = 0f; break;
            case 2f:
                _deadzone = 0.08f; break;
            case 3f:
                _deadzone = 0.16f; break;
        }
        switch ((int)rebinds.Data[AllSettingsKeys.CL_RECOIL_CURVE])
        {
            case 1:
                _recoilCurve = 1f;break;
            case 2:
                _recoilCurve = 0f;break;
        }

        if (AcceptSettingsChanges)
        {
            if (_dopRampTime > 0)
            {
                dopSpeedX = extraHorHand / _dopRampTime;
                dopSpeedY = extraVerHand / _dopRampTime;
            }
            if (_adsRampTime > 0)
            {
                adsDopSpeedX = extraAdsHorHand / _adsRampTime;
                adsDopSpeedY = extraAdsVerHand / _adsRampTime;
            }
            _horizontal = horHand;
            _vertical = verHand;
            _dopPovorotX = extraHorHand;
            _dopPovorotY = extraVerHand;
            adsHorizontal = adsHorHand;
            adsVertical = adsVerHand;
            adsDopPovorotX = extraAdsHorHand;
            adsDopPovorotY = extraAdsVerHand;
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
            recoilTime -= Time.deltaTime;
            if (AutoCompensation)
            {
                recX += x;
                recY += y;
            }
        }
        else if (AutoCompensation && recoilTime > -compinsieIn)
        {
            recoilTime -= Time.deltaTime;
        }
        else if(AutoCompensation)
        {
            if (!compinsied)
            {
                compinsied = true;
                if(recX > 0f && recoilDeltaX <= 0)
                {
                    if (recX + recoilDeltaX <= 0f)
                        recX = 0f;
                    else
                        recX = recX + recoilDeltaX;
                }
                else if(recX < 0f && recoilDeltaX >= 0)
                {
                    if (recX + recoilDeltaX >= 0f)
                        recX = 0f;
                    else
                        recX = recX + recoilDeltaX;
                }
                if(recY > 0f && recoilDeltaY <= 0)
                {
                    if (recY + recoilDeltaY <= 0f)
                        recY = 0f;
                    else
                        recY = recY + recoilDeltaY;
                }
                if(recX != 0f)
                {
                    compinsieSpeedX = recX / 0.15f;
                }
                if(recY != 0f)
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
            else if(recX < 0)
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
            if(recY > 0)//////////compinsie Y
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

    public void SetUpCameraSettings(Camera player, Transform buddy, SetupWeapon recoil)
    {
        this.player = player;
        this.buddy = buddy;
        weapon = recoil;
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
            if (_expandedSetings == 2)
                return new float[] { xRot, yRot };
            else
                return new float[] { 0f, 0f };
        }
    }

    public void Activate(bool state)
    {
        enabled = state;
    }

    public override void RandomSense(float procent, bool turnExtra)
    {
        if (AcceptSettingsChanges)
            AcceptSettingsChanges = false;

        _horizontal = Random.Range(horHand - (horHand * procent), horHand + (horHand * procent));
        _vertical = Random.Range(verHand - (verHand * procent), verHand + (verHand * procent));
        adsHorizontal = Random.Range(adsHorHand - (adsHorHand * procent), adsHorHand + (adsHorHand * procent));
        adsVertical = Random.Range(adsVerHand - (adsVerHand * procent), adsVerHand + (adsVerHand * procent));

        if(extraHorHand > 0)
            _dopPovorotX = Random.Range(extraHorHand - (extraHorHand * procent), extraHorHand + (extraHorHand * procent));
        else if(turnExtra)
            _dopPovorotX = Random.Range(0, 250f * procent);
        if (extraVerHand > 0)
            _dopPovorotY = Random.Range(extraVerHand - (extraVerHand * procent), extraVerHand + (extraVerHand * procent));
        else if (turnExtra)
            _dopPovorotY = Random.Range(0, 250f * procent);
        if (extraAdsHorHand > 0)
            adsDopPovorotX = Random.Range(extraAdsHorHand - (extraAdsHorHand * procent), extraAdsHorHand + (extraAdsHorHand * procent));
        else if (turnExtra)
            adsDopPovorotX = Random.Range(0, 250f * procent);
        if (extraAdsVerHand > 0)
            adsDopPovorotY = Random.Range(extraAdsVerHand - (extraAdsVerHand * procent), extraAdsVerHand + (extraAdsVerHand * procent));
        else if (turnExtra)
            adsDopPovorotY = Random.Range(0, 250f * procent);

        if (_horizontal < 30)
            _horizontal = 30f;
        if (_vertical < 30f)
            _vertical = 30f;
        if (adsHorizontal < 30f)
            adsHorizontal = 30f;
        if (adsVertical < 30f)
            adsVertical = 30f;
        if (_dopRampTime > 0)
        {
            dopSpeedX = _dopPovorotX / _dopRampTime;
            dopSpeedY = _dopPovorotY / _dopRampTime;
        }
        if (_adsRampTime > 0)
        {
            adsDopSpeedX = adsDopPovorotX / _adsRampTime;
            adsDopSpeedY = adsDopPovorotY / _adsRampTime;
        }
    }
}