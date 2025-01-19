using UnityEngine;
using UnityEngine.Events;

public class SetUpManager : MonoBehaviour
{
    public static UnityEvent<SetUpManager> levelStarted = new(); 
    public UnityEvent<LevelSettings> setDirectly = new();

    private void Awake()
    {
        levelStarted.Invoke(this);
    }
}