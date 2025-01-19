using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void StartLevel()
    {
        string name = PlayerPrefs.GetString("CoDLevelName");
        SceneManager.LoadScene(name);
    }
}
