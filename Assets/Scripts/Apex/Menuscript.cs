using UnityEngine.SceneManagement;
using UnityEngine;

public class Menuscript : MonoBehaviour
{
    [SerializeField] private GameObject settings1;
    [SerializeField] private GameObject settings2;
    [SerializeField] private GameObject settings3;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _Playmenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void SettingsButton()
    {
        _menu.SetActive(false);
        settings1.SetActive(true);
        settings2.SetActive(false);
        settings3.SetActive(false);
    }
    public void NextButton()
    {
        _menu.SetActive(false);
        settings1.SetActive(false);
        settings2.SetActive(true);
        settings3.SetActive(false);
    }
    public void NextButton2()
    {
        _menu.SetActive(false);
        settings1.SetActive(false);
        settings2.SetActive(false);
        settings3.SetActive(true);
    }
    public void ExitButton()
    {
        _menu.SetActive(true);
        settings1.SetActive(false);
        settings2.SetActive(false);
        _Playmenu.SetActive(false);
    }

    public void PlayMenu()
    {
        _menu.SetActive(false);
        settings1.SetActive(false);
        settings2.SetActive(false);
        _Playmenu.SetActive(true);
    }

    public void StartButton()
    {
        _menu.SetActive(false);
        settings1.SetActive(false);
        settings2.SetActive(false);
        _Playmenu.SetActive(false);
        SceneManager.LoadScene(1);
    }
    public void StartButton2()
    {
        _menu.SetActive(false);
        settings1.SetActive(false);
        settings2.SetActive(false);
        _Playmenu.SetActive(false);
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
