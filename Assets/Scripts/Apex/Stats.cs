using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [SerializeField] private Text acurracyText;
    [SerializeField] private Text headShotsText;
    [SerializeField] private Text bodyShotsText;
    [SerializeField] private Text timePlayedText;
    [SerializeField] private Text damageDeltText;
    [SerializeField] private Text damageTakenText;
    //[SerializeField] private Text avgDamageText;
    private string level;
    private float allShots, hit, headShots, bodyShots, timePlayed, damageDelt, damageTaken;

    public void LoadStats()
    {
        acurracyText.text = "";
        headShotsText.text = "";
        bodyShotsText.text = "";
        damageDeltText.text = "";
        damageTakenText.text = "";
        timePlayedText.text = "";
        //avgDamageText.text = "";

        level = PlayerPrefs.GetString("LevelName");
        allShots = PlayerPrefs.GetFloat(level + "AllShots");
        hit = PlayerPrefs.GetFloat(level + "Hit");
        headShots = PlayerPrefs.GetFloat(level + "HeadShots");
        bodyShots = PlayerPrefs.GetFloat(level + "BodyShots");
        timePlayed = PlayerPrefs.GetFloat(level + "TimePlayed");
        damageDelt = PlayerPrefs.GetFloat(level + "DamageDelt");
        damageTaken = PlayerPrefs.GetFloat(level + "DamageTaken");

        acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Headshots: " + CountProcent(headShots).ToString("F2") + "%";
        bodyShotsText.text = "Bodyshots: " + CountProcent(bodyShots).ToString("F2") + "%";
        damageDeltText.text = "Damage delt: " + CountDamage(damageDelt);
        damageTakenText.text = "Damage taken: " + CountDamage(damageTaken);
        timePlayedText.text = CountTime(timePlayed);
    }

    public void LoadStrafe()
    {
        acurracyText.text = "";
        headShotsText.text = "";
        bodyShotsText.text = "";
        damageDeltText.text = "";
        damageTakenText.text = "";
        timePlayedText.text = "";
        //avgDamageText.text = "";

        level = PlayerPrefs.GetString("LevelName");
        allShots = PlayerPrefs.GetFloat(level + "AllShots");
        hit = PlayerPrefs.GetFloat(level + "Hit");
        headShots = PlayerPrefs.GetFloat(level + "HeadShots");
        bodyShots = PlayerPrefs.GetFloat(level + "BodyShots");
        timePlayed = PlayerPrefs.GetFloat(level + "TimePlayed");
        damageDelt = PlayerPrefs.GetFloat(level + "DamageDelt");
        damageTaken = PlayerPrefs.GetFloat(level + "DamageTaken");

        /*acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Headshots: " + CountProcent(headShots).ToString("F2") + "%";
        bodyShotsText.text = "Bodyshots: " + CountProcent(bodyShots).ToString("F2") + "%";
        damageDeltText.text = "Damage delt: " + CountDamage(damageDelt);
        damageTakenText.text = "Damage taken: " + CountDamage(damageTaken);
        timePlayedText.text = CountTime(timePlayed);*/

        acurracyText.text = "Max damage: " + CountMaxDamage(timePlayed);
        headShotsText.text = "Damage taken: " + CountCurrentDamage(damageTaken);
        bodyShotsText.text = "Taked damage procent: " + ProcentTakenDamage(timePlayed, damageTaken);
        damageDeltText.text = CountTime(timePlayed);
    }

    public void LoadArk()
    {
        acurracyText.text = "";
        headShotsText.text = "";
        bodyShotsText.text = "";
        damageDeltText.text = "";
        damageTakenText.text = "";
        timePlayedText.text = "";
        //avgDamageText.text = "";

        level = PlayerPrefs.GetString("LevelName");
        allShots = PlayerPrefs.GetFloat(level + "AllShots");
        hit = PlayerPrefs.GetFloat(level + "Hit");
        headShots = PlayerPrefs.GetFloat(level + "HeadShots");
        bodyShots = PlayerPrefs.GetFloat(level + "BodyShots");
        timePlayed = PlayerPrefs.GetFloat(level + "TimePlayed");
        damageDelt = PlayerPrefs.GetFloat(level + "DamageDelt");
        damageTaken = PlayerPrefs.GetFloat(level + "DamageTaken");

        /*acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Headshots: " + CountProcent(headShots).ToString("F2") + "%";
        bodyShotsText.text = "Bodyshots: " + CountProcent(bodyShots).ToString("F2") + "%";
        damageDeltText.text = "Damage delt: " + CountDamage(damageDelt);
        damageTakenText.text = "Damage taken: " + CountDamage(damageTaken);
        timePlayedText.text = CountTime(timePlayed);*/

        acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Hits: " + bodyShots.ToString();
        bodyShotsText.text = "Misses: " + headShots.ToString();
        damageDeltText.text = "Damage taken: " + CountDamage(damageTaken);
        damageTakenText.text = CountTime(timePlayed);
    }

    public void LoadShoot2()
    {
        acurracyText.text = "";
        headShotsText.text = "";
        bodyShotsText.text = "";
        damageDeltText.text = "";
        damageTakenText.text = "";
        timePlayedText.text = "";
        //avgDamageText.text = "";

        level = PlayerPrefs.GetString("LevelName");
        allShots = PlayerPrefs.GetFloat(level + "AllShots");
        hit = PlayerPrefs.GetFloat(level + "Hit");
        headShots = PlayerPrefs.GetFloat(level + "HeadShots");
        bodyShots = PlayerPrefs.GetFloat(level + "BodyShots");
        timePlayed = PlayerPrefs.GetFloat(level + "TimePlayed");
        damageDelt = PlayerPrefs.GetFloat(level + "DamageDelt");
        damageTaken = PlayerPrefs.GetFloat(level + "DamageTaken");
        //float timesShooted = PlayerPrefs.GetFloat(level + "TimesShoot");

        acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Headshots: " + CountProcent(headShots).ToString("F2") + "%";
        bodyShotsText.text = "Bodyshots: " + CountProcent(bodyShots).ToString("F2") + "%";
        damageDeltText.text = "Damage delt: " + CountDamage(damageDelt);
        damageTakenText.text = "Damage taken: " + CountDamage(damageTaken);
        timePlayedText.text = CountTime(timePlayed);
        //avgDamageText.text = CountAvgDamage(damageDelt, timesShooted);
    }

    public void LoadFlicksStats()
    {
        acurracyText.text = "";
        headShotsText.text = "";
        bodyShotsText.text = "";
        damageDeltText.text = "";
        damageTakenText.text = "";
        timePlayedText.text = "";
        //avgDamageText.text = "";

        level = PlayerPrefs.GetString("LevelName");
        allShots = PlayerPrefs.GetFloat(level + "AllShots");
        hit = PlayerPrefs.GetFloat(level + "Hit");
        headShots = PlayerPrefs.GetFloat(level + "HeadShots");
        bodyShots = PlayerPrefs.GetFloat(level + "BodyShots");
        timePlayed = PlayerPrefs.GetFloat(level + "TimePlayed");
        damageDelt = PlayerPrefs.GetFloat(level + "DamageDelt");
        damageTaken = PlayerPrefs.GetFloat(level + "DamageTaken");

        acurracyText.text = "Accuracy: " + CountProcent(hit).ToString("F2") + "%";
        headShotsText.text = "Exelent shots: " + CountProcent(headShots).ToString("F2") + "%";
        bodyShotsText.text = "Normal shots: " + CountProcent(bodyShots).ToString("F2") + "%";
        damageDeltText.text = "Damage delt: " + CountDamage(damageDelt);
        damageTakenText.text = "Targets lost: " + CountDamage(damageTaken);
        timePlayedText.text = CountTime(timePlayed);
    }

    private string CountAvgDamage(float inputDamage, float timesShooted)
    {
        float avgDamage = inputDamage / timesShooted;
        return avgDamage.ToString("F2");
    }

    private float CountProcent(float shots)
    {
        float procent = shots / allShots * 100;
        return procent;
    }

    private string CountDamage(float inputDamage)
    {
        float damageTaken = inputDamage / 1000;
        return damageTaken.ToString("F2") + "k";
    }

    private string CountTime(float inputTime)
    {
        float time = inputTime / 3600;
        return "Time played: "+ time.ToString("F2") + "h";
    }

    private string CountCurrentDamage(float damage)
    {
        if (damage > 100000)
        {
            damage /= 1000;
            return damage.ToString("F2") + "k";
        }
        else
        {
            return damage.ToString();
        }
    }
    private string ProcentTakenDamage(float time, float damage)
    {
        float maxDm = time * 10 * 12f;
        float procent = damage / maxDm * 100;
        return procent.ToString("F2") + "%";
    }

    private string CountMaxDamage(float time)
    {
        float maxDm = time * 10 * 12f;
        if (maxDm > 100000)
        {
            maxDm /= 1000;
            return maxDm.ToString("F2") + "k";
        }
        else
        {
            return maxDm.ToString("F0");
        }
    }


}
