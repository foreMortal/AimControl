using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private Camera player;
    private float _fov; 

    private void Awake()
    {
        _fov = PlayerPrefs.GetFloat("FOV");
        if (_fov > 110f)
        {
            _fov = 110f;
        }
        player.fieldOfView = _fov;
    }
}
