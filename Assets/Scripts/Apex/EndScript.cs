using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    [SerializeField] private string hubName;

    public void Retry()
    {
        ApexStatsHendler.LevelQuit.Invoke();
        Time.timeScale = 1f;
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void BackToHub()
    {
        ApexStatsHendler.LevelQuit.Invoke();
        Time.timeScale = 1f;
        SceneManager.LoadScene(hubName, LoadSceneMode.Single);
    }
}
