using UnityEngine;

public class II : MonoBehaviour
{
    private LineRenderer line;
    private float timer;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.1f)
        {
            timer = 0f;
            line.SetPosition(0, transform.position);
        }
    }
}
