using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting1 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target, spawn1, spawn2;
    [SerializeField] private Shelter shelter;
    [SerializeField] private PlayerMouvement move;
    [SerializeField] private float timeBetwenShoots, damage; 
    public GameObject fpsCam;
    private float range = 100f, timer, damageCount;
    private int headID, bodyID, handsID, crouchBodyID, crouchHeadID;
    public Controlls controlls;
    [SerializeField] private  GameObject impactEffect;
    private RaycastHit hit;
    private GameObject particle;
    [SerializeField] private PushDefending01 push;
    public float allShots, hiting, headShots, bodyShots;
    private bool ads;
    Vector3 direction, spread;


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
        controlls = new Controlls();
        if (isActiveAndEnabled)
        {
            headID = push.head.GetInstanceID();
            bodyID = push.body.GetInstanceID();
            handsID = push.hands.GetInstanceID();
            crouchBodyID = push.crouchBody.GetInstanceID();
            crouchHeadID = push.crouchHead.GetInstanceID();
        }

        controlls.GamepadControl.Ads.performed += ctx => AdsOn();
        controlls.GamepadControl.Ads.canceled += ctx => AdsOff();
        if (PlayerPrefs.GetString("RemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapFire");
            controlls.GamepadControl.Fire.LoadBindingOverridesFromJson(rebind);
        }
        if (PlayerPrefs.GetString("RemapAds") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapAds");
            controlls.GamepadControl.Ads.LoadBindingOverridesFromJson(rebind);
        }

    }
    private void Update()
    {
        if(playerDamageDelt > 100000)
        {
            damageDeltText.text = playerDamageDelt.ToString() + "k";
        }
        else
        {
            damageDeltText.text = playerDamageDelt.ToString();
        }
        timer += Time.deltaTime;
        if(controlls.GamepadControl.Fire.ReadValue<float>() > 0f)
        {
            Shoot();
        }
    }

    [SerializeField] private Text damageDeltText;
    public float playerDamageDelt = 0f;
    private void Shoot()
    {
        if(timer >= timeBetwenShoots)
        {
            timer = 0f;
            allShots++;
            if (!ads)
            {
                spread = GetDirection();
                if (Physics.Raycast(fpsCam.transform.position, spread, out hit, range))
                {
                    if (hit.colliderInstanceID == headID)
                        HeadShot();
                    if (hit.colliderInstanceID == bodyID)
                        BodyShot();
                    if (hit.colliderInstanceID == handsID)
                        BodyShot();
                    if (hit.colliderInstanceID == crouchHeadID)
                        HeadShot();
                    if (hit.colliderInstanceID == crouchBodyID)
                        BodyShot();
                    particle = Instantiate(impactEffect, hit.point, Quaternion.identity);
                    Destroy(particle, 0.1f);
                }
            }
            else
            {
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    if (hit.colliderInstanceID == headID)
                        HeadShot();
                    if (hit.colliderInstanceID == bodyID)
                        BodyShot();
                    if (hit.colliderInstanceID == handsID)
                        BodyShot();
                    if (hit.colliderInstanceID == crouchHeadID)
                        HeadShot();
                    if (hit.colliderInstanceID == crouchBodyID)
                        BodyShot();
                    particle = Instantiate(impactEffect, hit.point, Quaternion.identity);
                    Destroy(particle, 0.1f);
                }
            }
        }
    }


    private void HeadShot()
    {
        hiting++;
        headShots++;
        playerDamageDelt += 17f;
        damageCount += 17f;
        Killed1();
    }

    private void BodyShot()
    {
        hiting++;
        bodyShots++;
        playerDamageDelt += damage;
        damageCount += damage;
        Killed1();
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

    public void Killed()
    {
        shelter.Respawn();
        target.position = spawn1.position;
        push.averageTimer = 0.1f;
        push.respawned = true;
        damageCount = 0f;
    }

    public void Killed1()
    {
        if(damageCount >= 200f)
        {
            shelter.Respawn();
            target.position = spawn1.position;
            push.averageTimer = 0.1f;
            push.respawned = true;
            damageCount = 0f;
        }
    }

    private void AdsOn()
    {
        ads = true;
        animator.SetBool("Ads", true);
    }
    private void AdsOff()
    {
        ads = false;
        animator.SetBool("Ads", false);
    }

    public void RemapShoot1()
    {
        if (PlayerPrefs.GetString("RemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapFire");
            controlls.GamepadControl.Fire.LoadBindingOverridesFromJson(rebind);
        }
        if (PlayerPrefs.GetString("RemapAds") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapAds");
            controlls.GamepadControl.Ads.LoadBindingOverridesFromJson(rebind);
        }
    }
}
