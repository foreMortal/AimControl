using UnityEngine;

public class GenburtonsGetHited : MonoBehaviour, IHitable
{
    [SerializeField] private MeshRenderer hited, ready;
    [SerializeField] private bool head;
    private GenburtonHPManager manager;
    private MeshRenderer self;
    private float time;

    private void Awake()
    {
        self = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(time > 0f)
        {
            time -= Time.deltaTime;
            if(time <= 0f)
            {
                hited.enabled = false;
                ready.enabled = true;
            }
        }
    }

    public void GetHited(HitInfo info, out bool head)
    {
        if(time <= 0f)
        {
            head = this.head;
            hited.enabled = true;
            ready.enabled = false;
            time = 1f;
        }
        else
        {
            head = this.head;
            time = 1f;
        }
        manager.ShowHit();
    }

    public void GetManager(GenburtonHPManager manager)
    {
        this.manager = manager;
    }
}
