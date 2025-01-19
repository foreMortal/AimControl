using UnityEngine;
using UnityEngine.UI;

public class FpsTracker : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private float timerLimit;
    private int fpsCount;
    private float fpsTimer;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        fpsCount++;
        fpsTimer += Time.unscaledDeltaTime;
        if(fpsTimer >= timerLimit)
        {
            fpsText.text = (fpsCount / fpsTimer).ToString("F0");
            fpsCount = 0;
            fpsTimer = 0f;
        }
    }
}
