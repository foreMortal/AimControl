using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform resp;

    public void RespawnHim()
    {
        transform.position = resp.position;
    }
}
