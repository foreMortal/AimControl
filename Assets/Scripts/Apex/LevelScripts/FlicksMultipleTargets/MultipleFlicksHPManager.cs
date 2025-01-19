using UnityEngine;
using UnityEngine.UI;

public class MultipleFlicksHPManager : MonoBehaviour
{
    [SerializeField] string deltString, takenString;
    [SerializeField] private Text damageDelt, damageTaken;

    private void Awake()
    {
        damageDelt.text = deltString + 0;
        damageTaken.text = takenString + 0;
    }

    public void SetHitedLostTargets(int type, float times)
    {
        switch (type)
        {
            case 0:
                damageDelt.text = deltString + times;
                break;
            case 1:
                damageTaken.text = takenString + times;
                break;
        }
    }
}
