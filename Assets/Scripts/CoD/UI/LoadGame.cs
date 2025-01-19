using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private string gameName;

    private void Awake()
    {
       Cursor.lockState = CursorLockMode.None;
    }

    public void GoGetTrain()
    {
        SceneManager.LoadScene(gameName);
    }
}
