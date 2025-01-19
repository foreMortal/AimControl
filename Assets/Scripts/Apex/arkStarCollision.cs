using UnityEngine;

public class arkStarCollision : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity, dir;
    private float gravity = -37f;
    private ArkStar arkStar;
    private bool worked;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.GetComponent<MeshCollider>() || other.GetComponent<CapsuleCollider>()) 
        {
            if (!worked)
            {
                //arkStar.BodyShot();
                worked = true;
            }
        }
        else
        {
            arkStar.Missed();
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void GetDir(Vector3 tyt)
    {
        dir = tyt;
    }

    private void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        controller.Move(60f * Time.deltaTime * dir);
    }
    public void GetArk(GameObject ark)
    {
        arkStar = ark.GetComponent<ArkStar>();
    }
}
