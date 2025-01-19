using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private string hubName;
    [SerializeField] private GameObject settings0;
    [SerializeField] private GameObject settings1;
    [SerializeField] private GameObject settings2;
    [SerializeField] private GameObject settings3;
    [SerializeField] private GameObject settings4;
    public static UnityEvent quitLvl = new();

    public void OpenCanvases()
    {
        settings0.SetActive(true);
        settings1.SetActive(false);
        settings2.SetActive(true);
        settings3.SetActive(false);
        settings4.SetActive(false);
    }

    public void Retry()
    {
        ApexStatsHendler.LevelQuit.Invoke();
        Time.timeScale = 1f;
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }

    public void SettingsButton()
    {
        settings0.SetActive(false);
        settings1.SetActive(true);
        settings3.SetActive(false);
        settings4.SetActive(false);
    }

    public void Return()
    {
        settings0.SetActive(true);
        settings1.SetActive(false);
        settings3.SetActive(false);
        settings4.SetActive(false);
    }

    public void NextButton()
    {
        settings1.SetActive(false);
        settings3.SetActive(false);
        settings4.SetActive(false);
    }
    public void NextButton2()
    {
        settings1.SetActive(false);
        settings3.SetActive(true);
        settings4.SetActive(false);
    }
    public void ExitButton()
    {
        settings1.SetActive(false);
        settings3.SetActive(false);
        settings4.SetActive(false);
    }

    public void Settings4Butt()
    {
        settings1.SetActive(false);
        settings3.SetActive(false);
        settings4.SetActive(true);
    }

    public void PlayMenu()
    {
        settings1.SetActive(false);
    }

    public void Resume()
    {
        InGameSettings.closeSettings.Invoke();
    }

    public void BackToHub()
    {
        ApexStatsHendler.LevelQuit.Invoke();
        Time.timeScale = 1f;
        quitLvl.Invoke();
        SceneManager.LoadScene(hubName, LoadSceneMode.Single);
    }
}
