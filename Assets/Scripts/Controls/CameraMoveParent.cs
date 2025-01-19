using UnityEngine;

public enum DeviceType
{
    MnK,
    Gamepad,
}

public abstract class CameraMoveParent : MonoBehaviour
{
    public bool AcceptSettingsChanges = true;
    public abstract event AdsDelegate PerformAds;
    public delegate void AdsDelegate(bool ads);

    public abstract void RotatePlayer(float x, float y);
    public abstract float[] GetRotation();
    public abstract float[] ChangeDevice(DeviceType type);
    public abstract void RandomSense(float procent, bool turnExtra);
}

