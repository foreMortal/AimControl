using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private DummyCanDie dummy;
    private CharacterController controler;

    private void Start()
    {
        controler = player.GetComponent<CharacterController>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controler.enabled = false;
            player.transform.position = playerSpawnPoint.position;
            controler.enabled = true;
            dummy.SwapDummy();
        }
    }
}
