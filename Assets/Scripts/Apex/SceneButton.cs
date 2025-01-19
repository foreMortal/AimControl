using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour, IMenuExecutable
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private FastTrainScript warmup;

    public void Execute()
    {
        if (levelName.type != "Warmup")
            SceneManager.LoadScene(levelName.type, LoadSceneMode.Single);
        else
            warmup.StartLevel();
    }
}
