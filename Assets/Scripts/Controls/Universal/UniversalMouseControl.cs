using UnityEngine;

public class UniversalMouseControl : CameraMoveParent, IRecoilProvider
{
    [SerializeField] private SettingsScriptableObject sett;
    private Controlls controls;
    public Camera fpsCam;
    public Transform buddy;
    private UniversalRebinds rebinds;
    private bool active;
    private DeviceType type = DeviceType.MnK;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string ADS_HORIZONTAL = "AdsHorizontal";
    private const string ADS_VERTICAL = "AdsVertical";
    private const float senseConst = 0.05f;
    private UniversalGamepadControl gamepadControl;

    private SetupWeapon weapon;
    private float recoilTime, recX, recY, recSpeedX, recSpeedY;
    private float xBeforeRecoil, yBeforeRecoil;
    private float xRot, yRot, horizontal = 0.05f , vertical = 0.05f, adsHorizontal = 0.05f, adsVertical = 0.05f;
    private bool ads, compinsied = true;

    public override event AdsDelegate PerformAds;


    private void Awake()
    {
        controls = new Controlls();
        gamepadControl = GetComponent<UniversalGamepadControl>();
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);

        controls = sett.controlls;
        //rebinds = (UniversalRebinds)sett.rebinds;

        controls.Universal.MnKAds.performed += ctx => AdsOn();
        controls.Universal.MnKAds.canceled += ctx => AdsOff();

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
            if (!ads)
            {
                yRot += controls.Universal.MnKVertical.ReadValue<float>() * vertical;
                xRot += controls.Universal.MnKHorizantal.ReadValue<float>() * horizontal;
            }
            else
            {
                yRot += controls.Universal.MnKVertical.ReadValue<float>() * adsVertical;
                xRot += controls.Universal.MnKHorizantal.ReadValue<float>() * adsHorizontal;
            }
            ApplyRotation();
        }
    }

    private void ApplyRotation()
    {
        if (xRot >= 360)
            xRot -= 360;
        else if (xRot < -360)
            xRot += 360;

        yRot = Mathf.Clamp(yRot, -90f, 90f);
        buddy.rotation = Quaternion.Euler(0f, xRot, 0f);
        fpsCam.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }

    public override void RotatePlayer(float x, float y)
    {
        xRot = x;
        yRot = y;
        ApplyRotation();
    }
    public override float[] GetRotation()
    {
        return new float[2] { xRot, yRot };
    }

    private void RecoilLogic()
    {
        if (recoilTime > 0f)
        {
            xRot += recSpeedX * Time.deltaTime;
            yRot += recSpeedY * Time.deltaTime;
            recoilTime -= Time.deltaTime;
        }
        else if (!compinsied && recoilTime > -0.1f)
        {
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
        ApplyRotation();
    }

    public void RotatePlayer(Quaternion rotation)
    {
        fpsCam.transform.rotation = rotation;
    }

    public void AdsOn()
    {
        ads = true;
        PerformAds?.Invoke(ads);
    }
    private void AdsOff()
    {
        ads = false;
        PerformAds?.Invoke(ads);
    }

    public void Reupload()
    {
        horizontal = senseConst * rebinds.MnKClassic[HORIZONTAL];
        vertical = senseConst * rebinds.MnKClassic[VERTICAL];
        adsHorizontal = senseConst * rebinds.MnKClassic[ADS_HORIZONTAL];
        adsVertical = senseConst * rebinds.MnKClassic[ADS_VERTICAL];
    }

    public void SetUpCameraSettings(Camera player, Transform buddy, SetupWeapon weapon)
    {
        fpsCam = player;
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
    public void Activate(bool state)
    {
        enabled = state;
    }
    public override void RandomSense(float procent, bool turnExtra) { }
}
