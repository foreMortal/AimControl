using UnityEngine;

public class HitMarker : MonoBehaviour
{
    [SerializeField] private RectTransform[] markers;
    private Vector3[] nativePositions;
    private float lifeTime;
    private bool active;


    private void Awake()
    {
        nativePositions = new Vector3[markers.Length];
        for(int i = 0; i < markers.Length; i++)
        {
            nativePositions[i] = markers[i].localPosition;
        }
    }

    private void Update()
    {
        if (active)
        {
            if (lifeTime <= 0f)
            {
                foreach (var marker in markers)
                {
                    marker.localPosition = Vector3.MoveTowards(marker.localPosition, Vector3.zero, 400 * Time.deltaTime);
                }
                if (markers[0].localPosition.sqrMagnitude <= 1f)
                {
                    active = false;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                lifeTime -= Time.deltaTime;
            }
        }
    }

    public void ShowHitMarker(float shots)
    {
        if(shots > 0f)
        {
            active = true;
            gameObject.SetActive(true);
            lifeTime = 0.2f;
            for (int i = 0; i < markers.Length; i++)
            {
                markers[i].localPosition = nativePositions[i];
            }
        }
    }
}
