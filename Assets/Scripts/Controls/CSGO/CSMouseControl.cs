using UnityEngine;

public class CSMouseControl : CameraMoveParent, IRecoilProvider
{
    private float senseConst = 0.022f;
    [SerializeField] private SettingsScriptableObject sett;
    private Controlls controls;
    public Camera fpsCam;
    public Transform buddy;
    private CSRebinds rebinds;
    public bool sniper;
    public bool performSniper = true;

    private SetupWeapon weapon;
    private float xRot, yRot, sensitivity = 0.022f, adsSensitivity = 0.05f;
    private float sniperSens, sniperSens2;
    private int ads;

    public override event AdsDelegate PerformAds;


    private void Awake()
    {
        controls = new Controlls();
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);

        controls = sett.controlls;
        //rebinds = (CSRebinds)sett.rebinds;

        controls.CSGOControl.MnKAds.performed += ctx => AdsOn();

        Reupload();
    }

    private void OnEnable()
    {
        controls.CSGOControl.Enable();
    }
    private void OnDisable()
    {
        controls.CSGOControl.Disable();
    }

    private void Update()
    {
        if (sniper || performSniper)
        {
            switch (ads)
            {
                case 0:
                    yRot += controls.CSGOControl.MnKVertical.ReadValue<float>() * sensitivity;
                    xRot += controls.CSGOControl.MnKHorizantal.ReadValue<float>() * sensitivity;
                    break;
                case 1:
                    yRot += controls.CSGOControl.MnKVertical.ReadValue<float>() * sniperSens;
                    xRot += controls.CSGOControl.MnKHorizantal.ReadValue<float>() * sniperSens;
                    break;
                case 2:
                    yRot += controls.CSGOControl.MnKVertical.ReadValue<float>() * sniperSens2;
                    xRot += controls.CSGOControl.MnKHorizantal.ReadValue<float>() * sniperSens2;
                    break;
            }
        }
        else
        {
            switch (ads)
            {
                case 0:
                    yRot += controls.CSGOControl.MnKVertical.ReadValue<float>() * sensitivity;
                    xRot += controls.CSGOControl.MnKHorizantal.ReadValue<float>() * sensitivity;
                    break;
                case 1:
                    yRot += controls.CSGOControl.MnKVertical.ReadValue<float>() * adsSensitivity;
                    xRot += controls.CSGOControl.MnKHorizantal.ReadValue<float>() * adsSensitivity;
                    break;
            }
        }

        ApplyRotation();
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

    private void AdsOn()
    {
        if (performSniper)
        {
            if (ads < 2)
            {
                ads++;
                fpsCam.fieldOfView -= 2.5f;
                PerformAds?.Invoke(true);
            }
            else
            {
                ads = 0;
                fpsCam.fieldOfView = 90f;
                PerformAds?.Invoke(false);
            }
        }
        else
        {
            if (ads < 1)
            {
                ads++;
                fpsCam.fieldOfView = 87f;
                PerformAds?.Invoke(true);
            }
            else
            {
                ads = 0;
                fpsCam.fieldOfView = 90f;
                PerformAds?.Invoke(false);
            }
        }
    }

    public void Reupload()
    {
        sensitivity = senseConst * rebinds.Classic[AllSettingsKeys.SENSITIVITY];
        sniperSens = sensitivity * rebinds.Classic[AllSettingsKeys.SCOP_SENSITIVITY] * 0.45f;
        sniperSens2 = sensitivity * rebinds.Classic[AllSettingsKeys.SCOP_SENSITIVITY] * 0.45f * 0.25f;
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
        return new float[]{ 0f, 0f};
    }
    public void BulletImpact(float t, params float[] t_)
    {
        return;
    }
    public override void RandomSense(float procent, bool turnExtra) { }
}
