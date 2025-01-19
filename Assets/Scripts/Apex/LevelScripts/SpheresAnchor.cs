using UnityEngine;

public class SpheresAnchor : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z);
    }
}
