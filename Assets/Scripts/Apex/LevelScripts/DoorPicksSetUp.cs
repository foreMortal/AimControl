using UnityEngine;

public class DoorPicksSetUp : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private GameObject dummy;
    [SerializeField] private Transform closeRange, longRange;
    [SerializeField] private DummyInteractWithDoor interact;
    [SerializeField] private LevelNameObjject levelName;

    public void Start()
    {
        if (levelName.type != "Warmup")
        {
            if (levelName.distance == "Long")
                dummy.transform.position = longRange.position;
            else
                dummy.transform.position = closeRange.position;

            if (levelName.dificulty == "Hard")
                interact.enabled = true;

            dummy.SetActive(true);
            closeRange.gameObject.SetActive(true);
            longRange.gameObject.SetActive(true);
        }
    }

    public void SetUpDirectly(LevelSettings obj)
    {
        if (obj.distance == "Long")
            dummy.transform.position = longRange.position;
        else
            dummy.transform.position = closeRange.position;

        if (obj.dificulty == "Hard")
            interact.enabled = true;

        dummy.SetActive(true);
        closeRange.gameObject.SetActive(true);
        longRange.gameObject.SetActive(true);
    }
}
