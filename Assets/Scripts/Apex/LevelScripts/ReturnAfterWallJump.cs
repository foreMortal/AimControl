using UnityEngine;

public class ReturnAfterWallJump : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DummyCanDie dummy;
    private BoxCollider box;
    private PlayerMouvement movement;
    private CameraMoveParent[] cameras;
    private CharacterController controler;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        movement = player.GetComponent<PlayerMouvement>();
        controler = player.GetComponent<CharacterController>();
        cameras = player.GetComponentInChildren<Camera>().GetComponentsInChildren<CameraMoveParent>();
        movement.WallJump += Activate;
        Deactivate();
    }

    private void Activate()
    {
        box.enabled = true;
    }
    private void Deactivate()
    {
        box.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMouvement>())
        {
            controler.enabled = false;
            player.position = spawnPoint.position;
            foreach (var cam in cameras)
            {
                cam.RotatePlayer(0f, 0f);
            }
            movement.slideCdTimer = 0f;
            controler.enabled = true;
            dummy.SwapDummy();
            Deactivate();
        }
    }
}
