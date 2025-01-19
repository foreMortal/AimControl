using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    [SerializeField] private Shooting1 shooting1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buddy"))
        {
            shooting1.Killed();
        }
    }
}
