using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class SniperAds : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Controlls controls;
    [SerializeField] private GameObject interface_;
    [SerializeField] private GameObject weaponCam;
    [SerializeField] private GameObject sniperScope;
    [SerializeField] Camera cam;
    [SerializeField] float sniperFOV;
    [SerializeField] float natureFOV;
    private bool scope = false;

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Awake()
    {
        controls = new Controlls();

        controls.CoDScheme.Ads.performed += ctx => AdsOn();
        controls.CoDScheme.Ads.canceled += ctx => AdsOff();

        if (PlayerPrefs.GetString("CoDRemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("CoDRemapFire");
            controls.CoDScheme.Fire.LoadBindingOverridesFromJson(rebind);
        }
        if (PlayerPrefs.GetString("CoDRemapAds") != null)
        {
            var rebind = PlayerPrefs.GetString("CoDRemapAds");
            controls.CoDScheme.Ads.LoadBindingOverridesFromJson(rebind);
        }
    }

    private void AdsOn()
    {
        scope = !scope;
        interface_.SetActive(false);
        animator.SetBool("Ads", scope);
    }

    private void AdsOff()
    {
        scope = !scope;
        interface_.SetActive(true);
        animator.SetBool("Ads", scope);
    }

    public void RemapFire()
    {
        if (PlayerPrefs.GetString("CoDRemapFire") != null)
        {
            var rebind = PlayerPrefs.GetString("CoDRemapFire");
            controls.CoDScheme.Fire.LoadBindingOverridesFromJson(rebind);
        }
    }
    public void RemapAds()
    {
        if (PlayerPrefs.GetString("CoDRemapAds") != null)
        {
            var rebind = PlayerPrefs.GetString("CoDRemapAds");
            controls.CoDScheme.Ads.LoadBindingOverridesFromJson(rebind);
        }
    }
}
