using UnityEngine;

public class DummySetUpLevel : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] DummyParameters longStrafe, shortStrafe;
    [SerializeField] private TargetingDummy targeting;

    private CrouchOnly dummy;

    private void Awake()
    {
        dummy = GetComponent<CrouchOnly>();

        if(levelName.type != "Warmup")
        {
            switch (levelName.distance)
            {
                case "Long":
                    dummy.SetParameters(longStrafe);
                    break;
                case "Short":
                    dummy.SetParameters(shortStrafe);
                    break;
            }

            if (levelName.targeting)
                targeting.enabled = true;
        }
    }

    public void SetWarmup(LevelSettings name)
    {
        dummy = GetComponent<CrouchOnly>();

        switch (name.distance)
        {
            case "Long":
                dummy.SetParameters(longStrafe);
                break;
            case "Short":
                dummy.SetParameters(shortStrafe);
                break;
        }

        if(name.targeting)
            targeting.enabled = true;
    }
}
