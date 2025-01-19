using UnityEngine;

public class UniversalGamepadControl : CameraMoveParent, IRecoilProvider
{
    [SerializeField] private SettingsScriptableObject sett;
    private Controlls controls;
    private Camera player;
    private Transform buddy;
    private bool active;
    private DeviceType type = DeviceType.Gamepad;
    private UniversalMouseControl mouseControl;
    private UniversalRebinds rebinds;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string ADS_HORIZONTAL = "AdsHorizontal";
    private const string ADS_VERTICAL = "AdsVertical";
    private const string DEADZONE = "DeadZone";

    private SetupWeapon weapon;
    private float recoilTime, recX, recY, recSpeedX, recSpeedY;
    private float xBeforeRecoil, yBeforeRecoil;
    private float hor, ver, deltaTime, _deadzone;
    private float xRot, yRot, horizontal = 300f, vertical = 300f, adsHorizontal = 300f, adsVertical = 300f;
    private bool ads, compinsied;

    public override event AdsDelegate PerformAds;


    private void Awake()
    {
        controls = new Controlls();
        mouseControl = GetComponent<UniversalMouseControl>();
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);
        controls = sett.controlls;
        //rebinds = (UniversalRebinds)sett.rebinds;

        controls.Universal.GpdAds.performed += ctx => AdsOn();
        controls.Universal.GpdAds.canceled += ctx => AdsOff();

        Reupload();
    }

    private void OnEnable()
    {
        controls.Universal.Enable();
    }
    private void OnDisable()
    {
        controls.Universal.Disable();
    }

    private void Update()
    {
        if (active)
        {
            hor = controls.Universal.GpdHorizantal.ReadValue<float>();
            ver = controls.Universal.GpdVertical.ReadValue<float>();
            deltaTime = Time.deltaTime;

            ClassGamePadMove();

            ApplyCameraMove();
        }
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
        if (!ads)
        {
            ///////X
            if (hor != 0f)
            {
                xRot += CameraMove(hor, horizontal);
            }

            /////////Y
            if (ver != 0f)
            {
                yRot += CameraMove(ver, vertical);
            }
        }
        else
        {
            ///////X
            if (hor != 0f)
            {
                xRot += CameraMove(hor, adsHorizontal);
            }

            /////////Y
            if (ver != 0f)
            {
                yRot += CameraMove(ver, adsVertical);
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

    private float CameraMove(float move, float sense)
    {
        float absMove = Mathf.Abs(move);

        if ((absMove * 100f) > _deadzone)
            return move * sense * deltaTime;
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

    private void RecoilLogic()
    {
        if (recoilTime > 0f)
        {
            xRot += recSpeedX * Time.deltaTime;
            yRot += recSpeedY * Time.deltaTime;
            recoilTime -= Time.deltaTime;
        }
        else if (!compinsied)
        {
            float tX = xRot - (xBeforeRecoil + recX);
            float tY = yRot - (yBeforeRecoil + recY);
            if (recX < 0f && tX + recX >= 0f)
            {
                recSpeedX = 0f;
            }
            else if (recX > 0f && tX + recX <= 0f)
            {
                recSpeedX = 0f;
            }
            else
            {
                if (recX > 0 && tX < 0)
                    recX += tX;
                else if (recX < 0 && tX > 0)
                    recX += tX;
                recSpeedX = -recX / 0.1f;
            }
            if (tY + recY <= 0f)
            {
                recSpeedY = 0f;
            }
            else
            {
                if (tY < 0f)
                    recY += tY;
                recSpeedY = -recY / 0.1f;
            }
            recoilTime = 0.1f;
            compinsied = true;
            recX = 0f;
            recY = 0f;
        }
    }

    public void BulletImpact(params float[] info)
    {
        if (compinsied)
        {
            compinsied = false;
            xBeforeRecoil = xRot;
            yBeforeRecoil = yRot;
        }
        recSpeedX = info[0];
        recSpeedY = info[1];
        recoilTime = info[2];
        recX += info[3];
        recY += info[4];
        ApplyCameraMove();
    }

    private void Reupload()
    {
        _deadzone = rebinds.GpdClassic[DEADZONE];
        horizontal = 100f * rebinds.GpdClassic[HORIZONTAL];
        vertical = 100f * rebinds.GpdClassic[VERTICAL];
        adsHorizontal = 100f * rebinds.GpdClassic[ADS_HORIZONTAL];
        adsVertical = 100f * rebinds.GpdClassic[ADS_VERTICAL];
    }

    public void SetUpCameraSettings(Camera player, Transform buddy, SetupWeapon weapon)
    {
        this.player = player;
        this.buddy = buddy;
        this.weapon = weapon;
        weapon.ChangeRecoilProvider(this);
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
            active = false;
            return new float[] { xRot, yRot };
        }
    }
    public void BulletImpact(float t, params float[] t_)
    {
        return;
    }
    public override void RandomSense(float procent, bool turnExtra) { }
}
