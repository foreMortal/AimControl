using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    //[SerializeField] private TrailRenderer trail;
    private Vector3[] points;
    private Vector3 garvity = new Vector3(0f, -10f, 0f);
    [SerializeField] private Transform arkSpawn;
    [SerializeField] private GameObject ark;
    private float timer = 1f;
    public bool end;
    private GameObject bb;

    public void ShowLine(Vector3 origin, Vector3 speed)
    {
        timer += Time.deltaTime;
        if(timer >= 0.1f)
        {
            //TrailRenderer ww = Instantiate(trail, arkSpawn.transform, true);
            timer = 0f;
            points = new Vector3[100];
            for (int i = 0; i < points.Length; i++)
            {
                float time = i * 0.01f;
                points[i] = origin + speed * time + garvity * time * time / 2f;
                //trail.AddPosition(points[i]);
                bb = Instantiate(ark, points[i], Quaternion.identity, arkSpawn.transform);
                Destroy(bb, 0.11f);
            }
        }
    }
}
