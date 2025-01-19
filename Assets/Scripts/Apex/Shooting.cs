using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour, IShootable, IGunRecoilable
{
    [SerializeField] private Text bulletsText; 
    [SerializeField] Transform firePoint;
    [SerializeField] private float timeBetwenShoots, damage;
    [SerializeField] private GameObject R99cross, valorantcross, point;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private bool canShoot = true, single;
    [SerializeField] private HitMarker marker;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private GetStatisticScriptableObject stats;
    [SerializeField] private float bulletSpeed;
    private List<GameObject> unactiveParticles = new();
    private List<GameObject> removeList = new();
    private Dictionary<GameObject, float> activeParticles = new();
    private GameObject fpsCam, actualCross;
    private LayerMask mask = ~0;
    private Animator animator;
    private float range = 100f, timer, hits, swapDelay, reloadDelay, allShots;
    private Controlls controlls;
    private string gunName;

    private delegate void ShootingEffect(float shots);
    private event ShootingEffect shootEvent;

    private BulletInfo bullet = new();
    private HitInfo hitInfo = new();
    private BulletsManager bulletsManager;
    private SetupWeapon weaponManager;
    private IRecoilProvider recoilHandler;
    private float[][] recoil;
    private int bullets = 1, maxBullets = 1, bulletsIndex = 0, bulletsInBurst, burstBulletsCount;
    private bool haveDelay, burst, instatiated;
    private float delayTime, delayTimer, timeBetweenBursts, compinsieIn;
    private bool useSpread = false, changesTimeBetweenShots;
    private RebindType type = RebindType.Universal;
    private RaycastHit hit;
    private bool ads, shooted, recoilEnabled = false, autoReload = true;
    Vector3 direction, spread;
    private InputAction fireAction;
    private Canvas crossCanvas;
    private string maxBulletsString;

    private void OnEnable()
    {
        fireAction.Enable();
        swapDelay = 0.15f;
    }

    private void OnDisable()
    {
        fireAction.Disable();
        canShoot = false;
    }

    private void Awake()
    {
        bullets = maxBullets;
        mask -= 1 << 10;
        mask -= 1 << 11;
        mask -= 1 << 12;
        mask -= 1 << 13;

        bullet.firePoint = firePoint;
        ApexApplySettings.ReuploadSettings.AddListener(Reupload);
        controlls = new Controlls();
        controlls = settings.controlls;
        animator = GetComponent<Animator>();
        fpsCam = GetComponentInParent<Camera>().gameObject;
        crossCanvas = GetComponentInChildren<Canvas>();

        for(int i = 0; i < 4; i++)
        {
            GameObject particle = Instantiate(impactEffect);
            particle.SetActive(false);
            unactiveParticles.Add(particle);
        }

        type = settings.rebinds.Type;
        Reupload();
        instatiated = true;
    }

    private void Update()
    {
        if(timer < timeBetwenShoots)
            timer += Time.deltaTime;

        ManageParticles(false);
        CanShoot();

        if (!autoReload)
        {
            if (reloadDelay < 0.5f)
                reloadDelay += Time.deltaTime;
            else if (bullets < maxBullets)
            {
                Reload();
            }
        }
    }

    private void GetObject(GetStatisticScriptableObject obj)
    {
        stats = obj;
    }

    private void CanShoot()
    {
        if (canShoot && swapDelay < 0.15f)
        {
            if (single)
            {
                if (burst)
                {
                    Burst(!shooted);
                    if (timer > timeBetweenBursts + compinsieIn && bulletsIndex != 0)
                        bulletsIndex = 0;
                }
                else if (fireAction.ReadValue<float>() > 0f)
                {
                    if (!shooted)
                    {
                        Shoot();
                        shooted = true;
                    }
                }
                else
                {
                    if (timer > timeBetwenShoots + 0.1f && bulletsIndex != 0)
                        bulletsIndex = 0;
                    if (shooted)
                        shooted = false;
                }
            }
            else if(haveDelay)
            {
                if (fireAction.ReadValue<float>() > 0f)
                {
                    if (delayTimer < delayTime)
                        delayTimer += Time.deltaTime;
                    else
                        Shoot();
                }
                else if (delayTimer > 0f)
                {
                    if(bulletsIndex != 0)
                        bulletsIndex = 0;

                    delayTimer -= Time.deltaTime;
                    if (delayTimer < 0f)
                        delayTimer = 0f;
                }
                else if (bulletsIndex != 0)
                    bulletsIndex = 0;
            }
            else if(burst)
            {
                Burst(true);
                if (timer > timeBetweenBursts + 0.1f && bulletsIndex != 0)
                    bulletsIndex = 0;
            }
            else
            {
                if (fireAction.ReadValue<float>() > 0f)
                    Shoot();
                else if (bulletsIndex != 0)
                {
                    bulletsIndex = 0;
                    if (changesTimeBetweenShots)
                        timeBetwenShoots = recoil[bulletsIndex][3];
                }
            }
        }
        else if(swapDelay > 0f)
        {
            swapDelay -= Time.deltaTime;
        }
        else
        {
            canShoot = true;
        }
        if (autoReload)
        {
            if(bullets <= 0)
            {
                Reload();
            }
        }
    }

    private void Shoot()
    {
        if(timer >= timeBetwenShoots && bullets > 0)
        {
            hits = 0f;
            reloadDelay = 0f;
            timer -= timeBetwenShoots;
            stats.allShots++;
            allShots++;
            if (!ads && useSpread)
            {
                spread = GetDirection();

                bullet.CreateNewBullet(gunName, firePoint.transform.position, fpsCam.transform.position, fpsCam.transform.rotation, spread, 5f, bulletSpeed, allShots);
                hitInfo.CreateNewInfo(damage, 17f, 1f);
                bulletsManager.CreateABullet(bullet, hitInfo);
            }
            else
            {
                bullet.CreateNewBullet(gunName, firePoint.transform.position, fpsCam.transform.position, fpsCam.transform.rotation, fpsCam.transform.forward, 5f, bulletSpeed, allShots);
                hitInfo.CreateNewInfo(damage, 17f, 1f);
                bulletsManager.CreateABullet(bullet, hitInfo);
            }
            if (recoilEnabled)
            {
                recoilHandler.BulletImpact(compinsieIn, recoil[bulletsIndex]);
                bulletsIndex++;
                if (changesTimeBetweenShots && bulletsIndex < 48)
                    timeBetwenShoots = recoil[bulletsIndex][3];
                bullets -= 1;
                bulletsText.text = bullets + maxBulletsString;
            }
            shootEvent?.Invoke(hits);
        }
    }

    private Vector3 GetDirection()
    {
        direction = fpsCam.transform.forward;
        direction += new Vector3
        (
        Random.Range(-0.05f, 0.05f),
        Random.Range(-0.05f, 0.05f),
        Random.Range(-0.05f, 0.05f)
        );
        direction.Normalize();
        return direction;
    }

    public void PerformAds(bool ads)
    {
        this.ads = ads;
        animator.SetBool("Ads", ads);
    }

    private void ManageParticles(bool _state)
    {
        if (_state)
        {
            GameObject _patricle = unactiveParticles[0];
            unactiveParticles.Remove(_patricle);
            _patricle.SetActive(true);
            _patricle.transform.SetPositionAndRotation(hit.point, Quaternion.Euler(hit.normal));
            activeParticles.Add(_patricle, Time.time + timeBetwenShoots);
        }
        else
        {
            foreach(var pair in activeParticles)
            {
                if(pair.Value < Time.time)
                {
                    pair.Key.SetActive(false);
                    removeList.Add(pair.Key);
                }
            }
            if(removeList.Count > 0)
            {
                foreach(var part in removeList)
                {
                    unactiveParticles.Add(part);
                    activeParticles.Remove(part);
                }
                removeList.Clear();
            }
        }
    }

    private void Burst(bool tap)
    {
        if (fireAction.ReadValue<float>() > 0f && tap && burstBulletsCount <= 0)
        {
            if (delayTimer >= timeBetweenBursts)
            {
                delayTimer = 0f;
                burstBulletsCount = bulletsInBurst;
                Shoot();
                shooted = true;
                burstBulletsCount--;
            }
            else if (delayTimer < timeBetweenBursts)
            {
                delayTimer += Time.deltaTime;
            }
        }
        else if (burstBulletsCount > 0 && timer >= timeBetwenShoots)
        {
            Shoot();
            burstBulletsCount--;
        }
        else if (delayTimer < timeBetweenBursts)
        {
            delayTimer += Time.deltaTime;
        }
        else if(fireAction.ReadValue<float>() <= 0f && shooted)
        {
            shooted = false;
        }
        /*else if (bulletsIndex != 0)
        {
            bulletsIndex = 0;
            bullets = maxBullets;
        }*/
    }

    public void ShowHitMarker()
    {
        marker.ShowHitMarker(1);
    }

    private void PrintDot()
    {
        point.SetActive(true);
    }
    private void HideDot()
    {
        point.SetActive(false);
    }
    private void HideCross()
    {
        actualCross.SetActive(false);
    }
    private void PrintCross()
    {
        actualCross.SetActive(true);
    }
    private void DisableShoot()
    {
        canShoot = false;
    }
    private void Hide()
    {
        weaponManager.RevealGun();
        gameObject.SetActive(false);
    }

    private void Reveale()
    {
        canShoot = true;
        fireAction.Enable();
        weaponManager.GunRevealed();
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void ChangeRecoilProvider(IRecoilProvider provider)
    {
        recoilHandler = provider;
    }

    public void ChangeWeapon(ApexRecoilScripatbleObject recoil)
    {
        gunName = recoil.WeaponName;
        bulletSpeed = recoil.BulletsSpeed;
        bullets = maxBullets = recoil.BulletsCount;
        timeBetwenShoots = recoil.TimeBetweenShots;
        haveDelay = recoil.HaveDelay;
        delayTime = recoil.DelayTime;
        burst = recoil.Burst;
        timeBetweenBursts = recoil.TimeBetweenBursts;
        bulletsInBurst = recoil.BulletsInBurst;
        compinsieIn = recoil.CompinsieIn;
        single = recoil.Single;
        changesTimeBetweenShots = recoil.ChangesTimeBetweenShots;
        this.recoil = recoil.GetRecoil();

        maxBulletsString = "/" + maxBullets;
        bulletsText.text = bullets + maxBulletsString;
    }

    private void Reload()
    {
        bullets = maxBullets;
        bulletsIndex = 0;
        reloadDelay = 0f;
        if (changesTimeBetweenShots)
            timeBetwenShoots = recoil[bulletsIndex][3];
        bulletsText.text = bullets + maxBulletsString;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void Setup(SetupWeapon setup, BulletsManager bulletsManager)
    {
        weaponManager = setup;
        this.bulletsManager = bulletsManager;
    }

    public IGunRecoilable GetGunRecoilable()
    {
        return this;
    }

    private void Reupload()
    {
        UserRebinds rebinds = settings.rebinds;

        switch (type)
        {
            case RebindType.Apex:
                fireAction = controlls.GamepadControl.Fire;

                if ((int)rebinds.Data[AllSettingsKeys.RECOIL_ACTIVE] == 1)
                    recoilEnabled = true;
                else
                    recoilEnabled = false;

                if ((int)rebinds.Data[AllSettingsKeys.AUTO_RELOAD] == 1)
                    autoReload = true;
                else
                    autoReload = false;

                if ((int)rebinds.Data[AllSettingsKeys.SPREAD_ACTIVE] == 1)
                    useSpread = true;
                else
                    useSpread = false;
                if(!instatiated)
                    actualCross = Instantiate(R99cross, crossCanvas.transform);
                break;
            case RebindType.Universal:
                fireAction = controlls.Universal.GpdFire;
                if ((int)rebinds.Data[AllSettingsKeys.SPREAD_ACTIVE] == 1)
                    useSpread = true;
                else
                    useSpread = false;
                if(!instatiated)
                    actualCross = Instantiate(R99cross, crossCanvas.transform);
                break;
            case RebindType.Valorant:
                useSpread = false;
                fireAction = controlls.ValorantControl.MnKFire;
                if(!instatiated)
                    actualCross = Instantiate(valorantcross, crossCanvas.transform);
                break;
            case RebindType.CSGO:
                useSpread = false;
                fireAction = controlls.CSGOControl.MnKFire;
                if(!instatiated)
                    actualCross = Instantiate(valorantcross, crossCanvas.transform);
                break;
        }
    }
}
