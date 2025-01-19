using UnityEngine;

public class PickLevel : MonoBehaviour
{
    [SerializeField] private string level;
    [SerializeField] Stats stats;
    [SerializeField] private GameObject canvas;
    public void SetScene()
    {
        PlayerPrefs.SetString("LevelName", level);
        stats.LoadStats();
        canvas.SetActive(true);
    }

    public void SetFlickShot()
    {
        PlayerPrefs.SetString("LevelName", level);
        stats.LoadFlicksStats();
        canvas.SetActive(true);
    }
    public void SetArkLevel()
    {
        PlayerPrefs.SetString("LevelName", level);
        stats.LoadArk();
        canvas.SetActive(true);
    }
    public void Setshoot2level()
    {
        PlayerPrefs.SetString("LevelName", level);
        stats.LoadShoot2();
        canvas.SetActive(true);
    }
    public void LoadStrafe()
    {
        PlayerPrefs.SetString("LevelName", level);
        stats.LoadStrafe();
        canvas.SetActive(true);
    }

    public void Back()
    {
        canvas.SetActive(false);
    }
}
