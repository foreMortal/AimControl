public interface IShootable
{
    public void PerformAds(bool ads) { }
    public void SelfDestroy() { }
    public void Setup(SetupWeapon setup, BulletsManager bManager) { }
    public void SetActive(bool state) { }
    public void ShowHitMarker() { }
    public IGunRecoilable GetGunRecoilable();
}
