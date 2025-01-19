using UnityEngine;

public class PassCheckChangeSide : MonoBehaviour
{
    [SerializeField] Collider[] colliders;
    [SerializeField] PassDummyMovement dummyMove;

    private LayerMask mask = 1 << 10;
    private Collider colider;

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 4f, mask))
        {
            if(hit.collider != colider)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] == hit.collider)
                    {
                        dummyMove.ChangeSide(i);
                        colider = hit.collider; 
                        break;
                    }
                }
            }
        }
    }
}
