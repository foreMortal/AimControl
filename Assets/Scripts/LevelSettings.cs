public class LevelSettings 
{
    public string type = "";
    public string distance = "";
    public string dificulty = "";
    public string strafeDistance = "";
    public string weapon = "";
    public string petName = "";
    public string usersName = "";
    public bool targeting = false;
    public float timeForALevel = 0f;

    public bool accuracy_, succeeded_, failed_, damageDelt_, damageTaken_, headSH_, bodySH_, time_, hits_, misses_, targetsLost_, exelentShots_, normalShots_, trackAccuracy_;
    public bool trackingTime_, missingTargetTime_, recordTime_;

    public LevelSettings() { }

    public LevelSettings(string type, string distance, 
        string dificulty, string strafeDistance, string weapon, string petName, bool targeting) 
    {
        this.type = type;
        this.distance = distance;
        this.dificulty = dificulty;
        this.strafeDistance = strafeDistance;
        this.weapon = weapon;
        this.petName = petName;
        this.targeting = targeting;
    }
    
    public void SetParameters(
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

    public void ClearParameters()
    {
        accuracy_ = false; succeeded_ = false;
        failed_ = false; damageDelt_ = false;
        damageTaken_ = false; headSH_ = false;
        bodySH_ = false; time_ = false; hits_ = false;
        targetsLost_ = false; exelentShots_ = false;
        normalShots_ = false; trackAccuracy_ = false;
        trackingTime_ = false; missingTargetTime_ = false;
        recordTime_ = false; misses_ = false;
    }
}
