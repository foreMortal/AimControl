using UnityEngine;

public class ValorantMouseControl : CameraMoveParent, IRecoilProvider
{
    [SerializeField] private SettingsScriptableObject sett;
    private Controlls controls;
    public Camera fpsCam;
    public Transform buddy;
    private ValorantRebinds rebinds;
    public bool sniper;
    public bool performSniper;

    private SetupWeapon weapon;
    private const string PERFORM_SNIPER = "PerformSniper";
    private const string SENSITIVITY = "Sensitivity";
    private const string ADS_SENSITIVITY = "AdsSensitivity";
    private const string SCOP_SENSITIVITY = "ScopSensitivity";

    private const float senseConst = 0.07f;

    private float xRot, yRot, sensitivity = 0.07f, adsSensitivity = 0.05f;
    private float sniperSens, sniperSens2;
    private int ads;

    public override event AdsDelegate PerformAds;

    private void Awake()
    {
        controls = new Controlls();
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);

        controls = sett.controlls;
        //rebinds = (ValorantRebinds)sett.rebinds;

        controls.ValorantControl.MnKAds.performed += ctx => AdsOn();

        Reupload();
    }

    private void OnEnable()
    {
        controls.ValorantControl.Enable();
    }
    private void OnDisable()
    {
        controls.ValorantControl.Disable();
    }

    private void Update()
    {
        if (sniper || performSniper)
        {
            switch (ads)
            {
                case 0:
                    yRot += controls.ValorantControl.MnKVertical.ReadValue<float>() * sensitivity;
                    xRot += controls.ValorantControl.MnKHorizantal.ReadValue<float>() * sensitivity;
                    break;
                case 1:
                    yRot += controls.ValorantControl.MnKVertical.ReadValue<float>() * sniperSens;   
                    xRot += controls.ValorantControl.MnKHorizantal.ReadValue<float>() * sniperSens;
                    break;
                case 2:
                    yRot += controls.ValorantControl.MnKVertical.ReadValue<float>() * sniperSens2;
                    xRot += controls.ValorantControl.MnKHorizantal.ReadValue<float>() * sniperSens2;
                    break;
            }
        }
        else
        {
            switch (ads)
            {
                case 0:
                    yRot += controls.ValorantControl.MnKVertical.ReadValue<float>() * sensitivity;
                    xRot += controls.ValorantControl.MnKHorizantal.ReadValue<float>() * sensitivity;
                    break;
                case 1:
                    yRot += controls.ValorantControl.MnKVertical.ReadValue<float>() * adsSensitivity;
                    xRot += controls.ValorantControl.MnKHorizantal.ReadValue<float>() * adsSensitivity;
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
        sensitivity = senseConst * rebinds.Classic[SENSITIVITY];
        adsSensitivity = sensitivity * rebinds.Classic[ADS_SENSITIVITY] * 0.8f;
        sniperSens = sensitivity * rebinds.Classic[SCOP_SENSITIVITY] * 0.4f;
        sniperSens2 = sensitivity * rebinds.Classic[SCOP_SENSITIVITY] * 0.4f * 0.5f;
        
        if(rebinds.Classic[PERFORM_SNIPER] == 1)
            performSniper = true;
        else 
            performSniper = false;
    }

    public void SetUpCameraSettings(Camera player, Transform buddy, SetupWeapon weapon)
    {
        fpsCam = player;
        this.buddy = buddy;
        this.weapon = weapon;
        this.weapon.ChangeRecoilProvider(this);
    }
    public void BulletImpact(float t, params float[] t_)
    {
        return;
    }
    public override float[] ChangeDevice(DeviceType type)
    {
        return new float[] { 0f, 0f };
    }


    public void Activate(bool state)
    {
        enabled = state;
    }
    public override void RandomSense(float procent, bool turnExtra) { }
}
