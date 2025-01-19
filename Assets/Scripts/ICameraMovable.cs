using UnityEngine;

public interface ICameraMovable
{
    public delegate void AdsDelegate(bool ads);
    public void SetUpCameraSettings(Camera player, Transform buddy, out AdsDelegate ads);

    public void Activate(bool state) { }
}
