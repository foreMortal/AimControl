using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArkStar : MonoBehaviour
{
    [SerializeField] private CharacterController controler;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] Spawnpoints1;
    [SerializeField] private Transform[] Spawnpoints2;
    [SerializeField] private Transform[] Spawnpoints3;
    [SerializeField] private Transform[] Spawnpoints4;
    [SerializeField] private Transform[] Spawnpoints5;
    [SerializeField] private Transform[] Playerpoints;
    [SerializeField] private TrajectoryRenderer line;
    [SerializeField] private Transform arkSpawn;
    [SerializeField] private GameObject arkStar;
    [SerializeField] private CrouchOnly targetMove;
    [SerializeField] private Transform target, spawnpoint;
    [SerializeField] PlayerMouvement move;
    [SerializeField] private float timeBetwenShoots, damage, gravity;
    [SerializeField] private Camera cam;
    public GameObject fpsCam;
    private float timer, damageCount, timeKilled;
    public Controlls controlls;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private bool canDie, zamah;
    public float allShots, hiting, headShots, bodyShots;
    private GameObject projectile;
    private Vector3 dir, dir1;
    private int posNow;

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
        if (PlayerPrefs.GetString("RemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapFire");
            controlls.GamepadControl.Fire.LoadBindingOverridesFromJson(rebind);
        }
    }

    private void Update()
    {
        if (playerDamageDelt > 100000)
        {
            damageDeltText.text = playerDamageDelt.ToString() + "k";
        }
        else
        {
            damageDeltText.text = playerDamageDelt.ToString();
        }
        timer += Time.deltaTime;
        if (controlls.GamepadControl.Fire.ReadValue<float>() > 0f)
        {
            zamah = true;
            dir1 = fpsCam.transform.forward + new Vector3(0f, 0.1f, 0f);
            dir1.Normalize();
            line.ShowLine(arkSpawn.position, dir1 * 30f);
        }
        if (controlls.GamepadControl.Fire.ReadValue<float>() <= 0f && zamah)
        {
            zamah = false;
            Shoot();
        }
    }

    [SerializeField] private Text damageDeltText;
    public float playerDamageDelt = 0f;
    private void Shoot()
    {
        if (timer >= timeBetwenShoots)
        {
            timer = 0f;
            allShots++;
            GameObject aa = projectile = Instantiate(arkStar, arkSpawn.position, fpsCam.transform.rotation);
            Destroy(aa, 1f);
            dir = fpsCam.transform.forward + new Vector3(0f, 0.1f, 0f);
            dir.Normalize();
            projectile.GetComponent<arkStarCollision>().GetDir(dir);
            projectile.GetComponent<arkStarCollision>().GetArk(gameObject);
        }
    }

    public void BodyShot()
    {
        hiting++;
        bodyShots++;
        playerDamageDelt += damage;
        damageCount += damage;
        Killed();
    }

    public void Missed()
    {
        headShots++;
        switch (posNow)
        {
            case 0:
                controler.enabled = false;
                player.position = Playerpoints[4].position;
                controler.enabled = true;
                break;
            case 1:
                controler.enabled = false;
                player.position = Playerpoints[0].position;
                controler.enabled = true;
                break;
            case 2:
                controler.enabled = false;
                player.position = Playerpoints[1].position;
                controler.enabled = true;
                break;
            case 3:
                controler.enabled = false;
                player.position = Playerpoints[2].position;
                controler.enabled = true;
                break;
            case 4:
                controler.enabled = false;
                player.position = Playerpoints[3].position;
                controler.enabled = true;
                break;
        }
    }

    private void Killed()
    {
        if (damageCount >= 1f && canDie)
        {
            timeKilled+=1;
            switch (timeKilled)
            {
                case 1:
                    posNow = 1;
                    target.SetPositionAndRotation(Spawnpoints2[Spawn()].position, Quaternion.Euler(0f, 180f, 0f));
                    controler.enabled = false;
                    player.position = Playerpoints[0].position;
                    controler.enabled = true;
                    damageCount = 0f;
                    break;
                case 2:
                    posNow = 2;
                    target.SetPositionAndRotation(Spawnpoints3[Spawn()].position, Quaternion.Euler(0f, -90f, 0f));
                    controler.enabled = false;
                    player.position = Playerpoints[1].position;
                    controler.enabled = true;
                    damageCount = 0f;
                    break;
                case 3:
                    posNow = 3;
                    target.SetPositionAndRotation(Spawnpoints4[Spawn()].position, Quaternion.Euler(0f, 90f, 0f));
                    controler.enabled = false;
                    player.position = Playerpoints[2].position;
                    controler.enabled = true;
                    damageCount = 0f;
                    break;
                case 4:
                    posNow = 4;
                    target.SetPositionAndRotation(Spawnpoints5[Spawn()].position, Quaternion.Euler(0f, 180f, 0f));
                    controler.enabled = false;
                    player.position = Playerpoints[3].position;
                    controler.enabled = true;
                    damageCount = 0f;
                    break;
                case 5:
                    posNow = 0;
                    timeKilled = 0f;
                    target.SetPositionAndRotation(Spawnpoints1[Spawn()].position, Quaternion.Euler(0f, 180f, 0f));
                    controler.enabled = false;
                    player.position = Playerpoints[4].position;
                    controler.enabled = true;
                    damageCount = 0f;
                    break;
            }
        }
    }

    private int Spawn()
    {
        int num = Random.Range(0, 3);
        return num;
    }
    public void RemapArk()
    {
        if (PlayerPrefs.GetString("RemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("RemapFire");
            controlls.GamepadControl.Fire.LoadBindingOverridesFromJson(rebind);
        }
    }

}

