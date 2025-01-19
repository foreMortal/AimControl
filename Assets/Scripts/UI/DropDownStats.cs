using UnityEngine;
using UnityEngine.UI;

public class DropDownStats : MonoBehaviour
{
    [SerializeField] LevelNameObjject levelName;
    [SerializeField] private NewerPickLevel pick;

    public void SetDummyDistance(int num)
    {
        switch (num)
        {
            case 0:
                levelName.type = "DummyClose";
                break;
            case 1:
                levelName.type = "DummyShort";
                break;
            case 2:
                levelName.type = "DummyMiddle";
                break;
            case 3:
                levelName.type = "DummyLong";
                break;
        }
        pick.ChangeStats();
    }
    public void SetDummyStrafes(int num)
    {
        switch (num)
        {
            case 0:
                levelName.strafeDistance = "Short";
                break;
            case 1:
                levelName.strafeDistance = "Long";
                break;
        }
        pick.ChangeStats();
    }

    public void SetFlicksDistance(int num)
    {
        switch (num)
        {
            case 0:
                levelName.distance = "Close";
                break;
            case 1:
                levelName.distance = "Middle";
                break;
            case 2:
                levelName.distance = "Long";
                break;
        }
        pick.ChangeStats();
    }

    public void SetDoorDistance(int num)
    {
        switch (num)
        {
            case 0:
                levelName.distance = "Close";
                break;
            case 1:
                levelName.distance = "Long";
                break;
        }
        pick.ChangeStats();
    }

    public void SetDoorDificulty(int num)
    {
        switch (num)
        {
            case 0:
                levelName.dificulty = "Easy";
                break;
            case 1:
                levelName.dificulty = "Hard";
                break;
        }
        pick.ChangeStats();
    }

    public void SetDificulty(int num)
    {
        switch (num)
        {
            case 0:
                levelName.dificulty = "Easy";
                break;
            case 1:
                levelName.dificulty = "Normal";
                break;
            case 2:
                levelName.dificulty = "Hard";
                break;
            case 3:
                levelName.dificulty = "UltraHard";
                break;
        }
        pick.ChangeStats();
    }

    public void SetWeapon(int num)
    {
        switch (num)
        {
            case 0:
                levelName.weapon = "R99";
                break;
            case 1:
                levelName.weapon = "PK";
                break;
        }
        pick.ChangeStats();
    }

    public void SetTargeting(bool state)
    {
        levelName.targeting = state;
        pick.ChangeStats();
    }
    public void SetRandomMovement(bool state)
    {
        levelName.randomMovement = state;
        pick.ChangeStats();
    }
}
