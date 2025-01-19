using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeleteCustomLevels : MonoBehaviour
{
    private WarmupManager manager;
    private List<int> LevelsToDelete = new();
    private List<GameObject> deleteButtons = new();
    private WarmupDataHandler data = new();
    private IDataService service = new JsondataService();

    [SerializeField] private StandartCanvas canvas;
    [SerializeField] NewerPickLevel pick;
    [SerializeField] GameObject deleteButton, cancelButton;
    public UnityEvent<bool> DeleteModOn;
    public UnityEvent CancelDeleting = new();
    [SerializeField] string relativePath;

    private void Awake()
    {
        manager = GetComponent<WarmupManager>();
    }

    public void AddLevelToDelete(int index, GameObject button)
    {
        if(index == -1)
        {
            return;
        }
        else
        {
            LevelsToDelete.Add(index);
            deleteButtons.Add(button);
        }
    }
    public void RevertLevelToDelete(int index, GameObject button)
    {
        if (index == -1)
        {
            return;
        }
        else
        {
            LevelsToDelete.Remove(index);
            deleteButtons.Remove(button);
        }
    }
    public void Delete()
    {
        if(LevelsToDelete.Count > 0)
        {
            service.LoadData(relativePath, false, out data);
            for (int i = 0; i < LevelsToDelete.Count; i++)
            {
                for (int j = 0; j < data.UserPrefs.Count; j++)
                {
                    if (data.UserPrefs[j].Index == LevelsToDelete[i])
                    {
                        pick.DeleteWarmupLevel(data.UserPrefs[j].LevelSettings.petName);
                        data.UserPrefs.RemoveAt(j);
                    }
                }
            }
            foreach (var obj in deleteButtons)
            {
                manager.buttons.Remove(obj.GetComponent<IButtonManager>());
                Destroy(obj);
            }
            manager.lvlCount -= LevelsToDelete.Count;
            service.SaveData(relativePath, data, false);
            LevelsToDelete.Clear();
            DeleteMod(false);

            manager.MakeControllerPickable(manager.buttons);
            canvas.ResetButton();
        }
        else
        {
            DeleteMod(false);
        }
    }

    private void DeleteMod(bool state)
    {
        deleteButton.SetActive(state);
        cancelButton.SetActive(state);
        DeleteModOn.Invoke(state);
    }

    public void SetDeleteModeOn()
    {
        DeleteMod(true);
    }
    public void Cancel()
    {
        DeleteMod(false);
        LevelsToDelete.Clear();
        CancelDeleting.Invoke();
    }
}
