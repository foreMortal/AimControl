using UnityEngine;

public class CoDLookToPlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        SetPlayerDirectly();
    }

    public void SetPlayerDirectly()
    {
        player = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 point = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(point);
    }
}
