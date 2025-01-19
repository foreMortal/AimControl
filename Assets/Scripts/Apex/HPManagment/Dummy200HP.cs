using UnityEngine;

public class Dummy200HP : MonoBehaviour
{
    [SerializeField] private Transform spawnpoint;
    private float hp = 200f;
    private SimpleGetHit[] hitScripts; 

    private void Awake()
    {
        hitScripts = FindObjectsOfType<SimpleGetHit>();
        foreach(var hit in hitScripts)
        {
            //hit.takeDamage.AddListener(DummyGetDamaged);
        }
    }

    public void DummyGetDamaged(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            transform.position = spawnpoint.position;
            hp = 200f;
        }
    }
}
