using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shooting2 : MonoBehaviour, IShootable, IGunRecoilable
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject shootMarker;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private  GameObject impactEffect, cross, point;
    [SerializeField] private float timeBetwenShoots, damage;
    [SerializeField] private HitMarker marker;
    [SerializeField] private GetStatisticScriptableObject stats;

    public UnityEvent<float> ShootingEffect = new();

    private List<GameObject> removeList = new();
    private List<GameObject> unactiveParticles = new();
    private Dictionary<GameObject, float> activeParticles = new();
    private Animator animator;
    private LayerMask layerMask = ~0;
    private Camera fpsCam;
    private Controlls controlls;

    private BulletInfo bullet = new();
    private HitInfo hitInfo = new();
    private SetupWeapon weaponManager;
    private int bulletsInBurst, bullets, maxBullets;
    private IRecoilProvider recoilHandler;
    private float[][] recoil;

    private string gunName;
    private BulletsManager bulletsManager;
    private Vector3 pkDir, pkSpread;
    private RaycastHit hit;
    private float range = 100f, timer, swapDelay, bulletSpeed, allShots;
    private bool ads, shooted, single, canShoot = true;
    private InputAction fireAction;

    private Vector3 up { get { return fpsCam.transform.up; } }
    private Vector3 right { get { return fpsCam.transform.right; } }

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
        fpsCam = GetComponentInParent<Camera>();
        animator = GetComponent<Animator>();

        layerMask -= 1 << 10;
        layerMask -= 1 << 11;
        layerMask -= 1 << 12;
        layerMask -= 1 << 13;

        controlls = new Controlls();
        controlls = settings.controlls;

        switch (settings.rebinds.Type)
        {
            case RebindType.Apex:
                fireAction = controlls.GamepadControl.Fire;
                break;
            case RebindType.Universal:
                fireAction = controlls.Universal.GpdFire;
                break;
            case RebindType.Valorant:
                fireAction = controlls.ValorantControl.MnKFire;
                break;
            case RebindType.CSGO:
                fireAction = controlls.CSGOControl.MnKFire;
                break;
        }

        for(int i = 0; i < 11; i++)
        {
            GameObject particle = Instantiate(impactEffect);
            particle.SetActive(false);
            unactiveParticles.Add(particle);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        ManageParticles(false);

        if (canShoot && swapDelay <= 0f)
        {
            if (single)
            {
                if (fireAction.ReadValue<float>() > 0f && !shooted)
                {
                    PeaceKeeper();
                    shooted = true;
                }
                else if (fireAction.ReadValue<float>() <= 0f && shooted)
                    shooted = false;
            }
            else
            {
                if (fireAction.ReadValue<float>() > 0f)
                {
                    PeaceKeeper();
                }
            }
            if (timer >= 0.5f && bullets < maxBullets)
            {
                bullets = maxBullets;
            }
        }
        else if (swapDelay > 0f)
        {
            swapDelay -= Time.deltaTime;
        }
        else
        {
            canShoot = true;
        }
    }

    private void PeaceKeeper()
    {
        if (timer >= timeBetwenShoots)
        {
            float hits = 0f;
            timer = 0f;
            stats.allShots += bulletsInBurst;
            for (int i = 0; i < bulletsInBurst; i++)
            {
                if (!ads)
                {
                    pkSpread = PeaceKeeperSpread(i);
                }
                else if (ads)
                {
                    pkSpread = PeaceKeeperSpread(i);
                }
                bullet.CreateNewBullet(gunName, firePoint.transform.position, fpsCam.transform.position, fpsCam.transform.rotation, fpsCam.transform.forward, 5f, bulletSpeed, ++allShots);
                hitInfo.CreateNewInfo(damage, 17f, 1f);
                bulletsManager.CreateABullet(bullet, hitInfo);
                /*if (Physics.Raycast(fpsCam.transform.position, pkSpread, out hit, range, layerMask))
                {
                    if (hit.collider.CompareTag("CanGetHitted"))
                    {
                        hits++;
                        stats.hits++;
                        IHitable target = hit.collider.GetComponent<IHitable>();
                        HitInfo hitInfo = new(damage, 11f, 1f);
                        target.GetHited(hitInfo, out bool headShot);
                        if (headShot)
                        {
                            stats.headShots++;
                        }
                        else
                        {
                            stats.bodyShots++;
                        }
                    }
                    if (unactiveParticles.Count > 0)
                    {
                        ManageParticles(true);
                    }
                    //Instantiate(shootMarker, hit.point, Quaternion.identity);
                }*/
            }
            ShootingEffect.Invoke(hits);
            bullets -= 1;
        }
    }

    private Vector3 PeaceKeeperAdsSpread(int i)
    {
        switch (i)
        {
            case 0:
                pkDir = fpsCam.transform.forward;
                break;
            case 1:
                pkDir = fpsCam.transform.forward + fpsCam.transform.up * 0.008f;
                break;
            case 2:
                pkDir = fpsCam.transform.forward + fpsCam.transform.up * 0.004f;
                break;
            case 3:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * -0.007f + fpsCam.transform.up * 0.004f;
                break;
            case 4:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * 0.007f + fpsCam.transform.up * 0.004f;
                break;
            case 5:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * -0.003f + fpsCam.transform.up * 0.002f;
                break;
            case 6:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * 0.003f + fpsCam.transform.up * 0.002f;
                break;
            case 7:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * -0.005f + fpsCam.transform.up * -0.006f;
                break;
            case 8:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * 0.005f + fpsCam.transform.up * -0.006f;
                break;
            case 9:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * -0.002f + fpsCam.transform.up * -0.003f;
                break;
            case 10:
                pkDir = fpsCam.transform.forward + fpsCam.transform.right * 0.002f + fpsCam.transform.up * -0.003f;
                break;
        }
        pkDir.Normalize();
        return pkDir;
    }

    private Vector3 PeaceKeeperSpread(int i)
    {
        pkDir = fpsCam.transform.forward + up * recoil[i][0] + right * recoil[i][1];
        pkDir.Normalize();
        return pkDir;
    }

    public void ChangeRecoilProvider(IRecoilProvider provider)
    {
        recoilHandler = provider;
    }

    private void ManageParticles(bool _state)
    {
        if (_state)
        {
            GameObject _patricle = unactiveParticles[0];
            unactiveParticles.Remove(_patricle);
            _patricle.SetActive(true);
            _patricle.transform.SetPositionAndRotation(hit.point, Quaternion.Euler(hit.normal));
            activeParticles.Add(_patricle, Time.time + 0.1f);
        }
        else
        {
            foreach (var pair in activeParticles)
            {
                if (pair.Value < Time.time)
                {
                    pair.Key.SetActive(false);
                    removeList.Add(pair.Key);
                }
            }
            if (removeList.Count > 0)
            {
                foreach (var part in removeList)
                {
                    unactiveParticles.Add(part);
                    activeParticles.Remove(part);
                }
                removeList.Clear();
            }
        }
    }

    public void ChangeWeapon(ApexRecoilScripatbleObject recoil)
    {
        gunName = recoil.WeaponName;
        bulletSpeed = recoil.BulletsSpeed;
        bullets = maxBullets = recoil.BulletsCount;
        timeBetwenShoots = recoil.TimeBetweenShots;
        bulletsInBurst = recoil.BulletsInBurst;
        single = recoil.Single;
        this.recoil = recoil.GetRecoil(bulletsInBurst);
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
        cross.SetActive(false);
    }
    private void PrintCross()
    {
        cross.SetActive(true);
    }

    public IGunRecoilable GetGunRecoilable()
    {
        return this;
    }

    public void PerformAds(bool ads)
    {
        this.ads = ads;
        animator.SetBool("Ads", ads);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void Setup(SetupWeapon setup, BulletsManager bulletsManager)
    {
        this.bulletsManager = bulletsManager;
        weaponManager = setup;
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
}
