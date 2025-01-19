using UnityEngine;

public class OpenLastCanvas : MonoBehaviour
{
    [SerializeField] private LevelNameObjject levelName;
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private GameObject apexSet, universalSet, valorantSet, CS2Set;
    [SerializeField] private GameObject grindHandler, chooseGameCanvas, apexGameMenu, warmup, levels, apexMaimMenu;
    [SerializeField] private Animator animator;

    public void SetLastCanvasName(string canvasName)
    {
        levelName.lastOpenedCanvas = canvasName;
    }

    private void Start()
    {
        switch (settings.GameName)
        {
            case "Apex Legends":

                apexSet.SetActive(true);
                goto case "Default";

            case "Universal Game":
                universalSet.SetActive(true);
                goto case "Default";

            case "VALORANT":
                valorantSet.SetActive(true);
                goto case "Default";

            case "Counter-Strike 2":
                CS2Set.SetActive(true);
                goto case "Default";

            case "Default":
                animator.Play("MainMenuGameChoosed");

                chooseGameCanvas.SetActive(false);
                apexGameMenu.SetActive(true);

                switch (levelName.lastOpenedCanvas)
                {
                    case "Warmup":
                        warmup.SetActive(true);
                        goto case "Default";
                    case "Levels":
                        levels.SetActive(true);
                        goto case "Default";
                    case "Default":
                        grindHandler.SetActive(true);
                        apexMaimMenu.SetActive(false);
                        break;
                }
                break;
            default:
                apexGameMenu.SetActive(false);
                break;
        }
    }
}
