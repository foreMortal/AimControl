using UnityEngine;

public class SniperLvLRespawn : MonoBehaviour
{
    [SerializeField] private Transform dummy;
    [SerializeField] private Transform[] positions;
    [SerializeField] private GameObject player;
    private void Awake()
    {
        ChangeSpawn();
    }
    public void ChangeSpawn()
    {
        int spawn = Random.Range(0, positions.Length);
        dummy.position = positions[spawn].position;
        Vector3 point = new(player.transform.position.x, dummy.position.y, player.transform.position.z);
        dummy.LookAt(point);
    }
}
