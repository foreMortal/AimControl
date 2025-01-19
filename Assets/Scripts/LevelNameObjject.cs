using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelNameObjject", order = 5)]

public class LevelNameObjject : ScriptableObject
{
    private GameState gameState;
    public string type = "";
    public string distance = "";
    public string dificulty = "";
    public string strafeDistance = "";
    public string weapon = "";
    public string petName = "";
    public bool targeting = false;
    public bool randomMovement = true;

    public bool accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_, exelentShots_, normalShots_, trackAccuracy_;
    public bool trackingTime_, missingTargetTime_, recordTime_;

    [NonSerialized] public string lastOpenedGame, lastOpenedCanvas;

    public void Clear()
    {
        type = "";
        distance = "";
        dificulty = "";
        strafeDistance = "";
        weapon = "";
        petName = "";
        targeting = false;
        randomMovement = true;
    }

    public void Fill(string type, string distance, string dificulty, 
        string strafeDistance, string weapon, string petName, bool targeting)
    {
        gameState = 0;
        this.type = type;
        this.distance = distance;
        this.dificulty = dificulty;
        this.strafeDistance = strafeDistance;
        this.weapon = weapon;
        this.petName = petName;
        this.targeting = targeting;
    }

    public string MakeLevelName()
    { 
        string LevelName = type + distance + dificulty + strafeDistance + weapon + petName;
        if (targeting)
            LevelName += "Targeting";
        
        return LevelName;
    }

    public void getChoosenStats(
    bool accuracy_, bool succeeded_, bool failed_, bool damageDelt_, 
    bool damageTaken_, bool headSH_, bool bodySH_, bool time_, bool hits_,
    bool misses_, bool targetsLost_, bool exelentShots_, bool normalShots_,
    bool trackAccuracy_, bool trackingTime_, bool missingTargetTime_,
    bool recordTime_)
    {
        this.accuracy_ = accuracy_; this.succeeded_ = succeeded_;
        this.failed_ = failed_; this.damageDelt_ = damageDelt_;
        this.damageTaken_ = damageTaken_; this.headSH_ = headSH_;
        this.bodySH_ = bodySH_; this.time_ = time_; this.hits_ = hits_;
        this.targetsLost_ = targetsLost_; this.exelentShots_ = exelentShots_;
        this.normalShots_ = normalShots_; this.trackAccuracy_ = trackAccuracy_;
        this.trackingTime_ = trackingTime_; this.missingTargetTime_ = missingTargetTime_;
        this.recordTime_ = recordTime_; this.misses_ = misses_;
    }

    public void SetByLevelSettings(LevelSettings sett)
    {
        accuracy_ = sett.accuracy_; succeeded_ = sett.succeeded_;
        failed_ = sett.failed_; damageDelt_ = sett.damageDelt_;
        damageTaken_ = sett.damageTaken_; headSH_ = sett.headSH_;
        bodySH_ = sett.bodySH_; time_ = sett.time_; hits_ = sett.hits_;
        targetsLost_ = sett.targetsLost_; exelentShots_ = sett.exelentShots_;
        normalShots_ = sett.normalShots_; trackAccuracy_ = sett.trackAccuracy_;
        trackingTime_ = sett.trackingTime_; missingTargetTime_ = sett.missingTargetTime_;
        recordTime_ = sett.recordTime_; misses_ = sett.misses_;

        type = sett.type;
        distance = sett.distance;
        dificulty = sett.dificulty;
        strafeDistance = sett.strafeDistance;
        weapon = sett.weapon;
        petName = sett.petName;
    }
}

public enum GameState
{
    classic,
    ranked,
}
