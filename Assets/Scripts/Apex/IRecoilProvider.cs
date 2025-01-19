public interface IRecoilProvider 
{
    public void BulletImpact(float compinsieIn, params float [] info);
}

public interface IGunRecoilable
{
    public void ChangeRecoilProvider(IRecoilProvider provider);
    public void ChangeWeapon(ApexRecoilScripatbleObject recoil);
}
