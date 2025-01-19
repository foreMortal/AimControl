using UnityEngine;

public class TrajectoryHit : MonoBehaviour
{
    [SerializeField] private GameObject hit;
    private float timer = 1f;
    private int i;
    private bool end;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BoxCollider>() || other.GetComponent<CapsuleCollider>() || other.GetComponent<MeshCollider>())
        {
            if(timer >= 0.2f)
            {
                timer = 0f;
                GameObject tt = Instantiate(hit, transform.position, Quaternion.identity, transform);
                Destroy(tt, 0.21f);
                end = true;
            }
        }
    }

    public void GetNumber(int tyt)
    {
        i = tyt;
    }
    public bool RetEnd()
    {
        return end;
    }
    public int RetI()
    {
        return i;
    }
    public void Distroy(int number)
    {
        if(i == number)
            Destroy(gameObject);
    }
}
