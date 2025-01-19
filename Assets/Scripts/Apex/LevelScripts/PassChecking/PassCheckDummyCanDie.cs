using UnityEngine;
using UnityEngine.Events;

public class PassCheckDummyCanDie : MonoBehaviour
{
    private float health = 4f;
    private float timesDied;
    public UnityEvent<float> flicks = new();

    private void Awake()
    {
        PassCheckGetHited[] hits = GetComponentsInChildren<PassCheckGetHited>();

        foreach(var hit in hits)
        {
            hit.GetHealthScript(this);
        }
    }

    public void GetHited(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            flicks.Invoke(++timesDied);
            health = 4f;
        }
    }
}
