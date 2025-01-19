using UnityEngine;

public class SetActive : MonoBehaviour, IMenuExecutable
{
    [SerializeField] private GameObject[] Active;
    [SerializeField] private GameObject[] UnActive;

    public void Execute()
    {
        if(Active != null)
        {
            foreach(var obj in Active)
            {
                obj.SetActive(true);
            }
        }
        if(UnActive != null)
        {
            foreach (var obj in UnActive)
            {
                obj.SetActive(false);
            }
        }
    }
}
