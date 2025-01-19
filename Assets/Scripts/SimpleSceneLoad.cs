using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoad : MonoBehaviour
{
    [SerializeField] private string lvlName;

    public void Load()
    {
        SceneManager.LoadScene(lvlName);
    }
}
