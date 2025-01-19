using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdjustFOV : MonoBehaviour
{
    [SerializeField] private Text text;

    private Camera cam;
    private GamePadMouvement cameraMove;

    private float desiredFov, startFov, deltaFov, adsFov, fovInterpolation = 1;
    private int scopeIndex = 0;
    bool state;

    private void OnDisable()
    {
        cameraMove.PerformAds -= ChangeFovOnAds;
    }

    public void CameraSetuped()
    {
        if (!transform.GetChild(transform.childCount-1).TryGetComponent<GamePadMouvement>(out cameraMove))
        {
            Destroy(this);
        }

        cameraMove.PerformAds += ChangeFovOnAds;
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ChangeScopeIndex(1);
    }

    private void Update()
    {
        InterPolateFOV();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!state)
            {
                text.gameObject.SetActive(true);
                ShowFOV();
                state = true;
            }
            else
            {
                ShowFOV();
                state = false;
                StartCoroutine(nameof(HideText));
            }
        }
        if (state)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeScopeIndex(1);
                ShowFOV();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeScopeIndex(-1);
                ShowFOV();
            }
        }
    }

    private void ChangeFovOnAds(bool state)
    {
        fovInterpolation = 1 - fovInterpolation;

        startFov = cam.fieldOfView;
        
        if (state)
            desiredFov = adsFov;
        else
            desiredFov = 110f;

        deltaFov = desiredFov - startFov;
    }

    private void InterPolateFOV()
    {
        if (fovInterpolation < 1)
        {
            fovInterpolation += Time.deltaTime * 5f;

            cam.fieldOfView = startFov + deltaFov * fovInterpolation;

            if (fovInterpolation >= 1)
            {
                fovInterpolation = 1;
                cam.fieldOfView = desiredFov;
            }
        }
    }

    private void ChangeScopeIndex(int delta)
    {
        scopeIndex += delta;

        if (scopeIndex > 3)
            scopeIndex = 0;
        else if (scopeIndex < 0)
            scopeIndex = 3;

        switch (scopeIndex)
        {
            case 0:
                adsFov = 110f; break;
            case 1:
                adsFov = 110f / 2f; break;
            case 2:
                adsFov = 110f / 3f; break;
            case 3:
                adsFov = 110f / 4f; break;
        }

        ShowFOV();
    }

    private void ShowFOV()
    {
        text.text = string.Format("FOV: {0} / {1} X", adsFov.ToString("F1"), scopeIndex + 1);
    }

    private IEnumerator HideText()
    {
        float t = 0f;
        while(t < 0.5)
        {
            t += Time.deltaTime;
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
}
