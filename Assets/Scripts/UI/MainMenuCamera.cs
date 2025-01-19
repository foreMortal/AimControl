using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    private Animator animator;
    private int game = 0;
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private GameObject GameChoose, ApexMenu, apexSet, universalSet, valorantSet, cs2Set;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetGame(int num)
    {
        GameChoose.SetActive(false);ApexMenu.SetActive(false);
        animator.Play("MainMenuCamera");
        game = num;
    }

    private void OpenMenu()
    {
        switch (game)
        {
            case -1:
                GameChoose.SetActive(true); settings.SettingsGained = false; break;
            case 0:
                ApexMenu.SetActive(true);
                apexSet.SetActive(true);
                levelName.lastOpenedGame = "ApexMenu";
                break;
            case 1:
                ApexMenu.SetActive(true);
                universalSet.SetActive(true);
                levelName.lastOpenedGame = "Universal";
                break;
            case 2:
                ApexMenu.SetActive(true);
                valorantSet.SetActive(true);
                levelName.lastOpenedGame = "Valorant";
                break;
            case 3:
                ApexMenu.SetActive(true);
                cs2Set.SetActive(true);
                levelName.lastOpenedGame = "CounterStrike";
                break;
        }
    }

    public void Return(int num)
    {
        apexSet.SetActive(false);
        universalSet.SetActive(false);
        valorantSet.SetActive(false);
        cs2Set.SetActive(false);
        GameChoose.SetActive(false); ApexMenu.SetActive(false);
        animator.Play("MainMenuCameraReturn");
        game = num;
    }
}
