using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public MenuingManager.MenuDelegate arUp;
    public MenuingManager.MenuDelegate arDwn;
    public MenuingManager.MenuDelegate arLft;
    public MenuingManager.MenuDelegate arRgt;
    public MenuingManager.MenuDelegate trngl;
    public MenuingManager.MenuDelegate crcl;
    public MenuingManager.MenuDelegate sqr;
    public MenuingManager.MenuDelegate crss;

    protected int deviceType;
    protected MenuingManager manager;
    protected bool setuped;

    public void Setup(MenuingManager m)
    {
        manager = m;
    }

    protected virtual void Awake()
    {
        
    }

    protected void Update()
    {
        if (!setuped)
        {
            Enable();
            setuped = true;
        }
    }

    public void ReEnable()
    {
        Enable();
    }

    protected virtual void OnEnable()
    {
        setuped = false;
    }

    protected virtual void Enable()
    {
        manager.canvas = this;
        deviceType = manager.ClearEvents();

        manager.arUp += InvArUp;
        manager.arDwn += InvArDwn;
        manager.arLft += InvArLft;
        manager.arRgt += InvArRgt;
        manager.trngl += InvTrngl;
        manager.crcl += InvCrcl;
        manager.crss += InvCrss;
        manager.sqr += InvSqr;
    }

    protected void InvArUp(int t)
    {
        arUp?.Invoke(t);
    }
    protected void InvArLft(int t)
    {
        arLft?.Invoke(t);
    }
    protected void InvArDwn(int t)
    {
        arDwn?.Invoke(t);
    }
    protected void InvArRgt(int t)
    {
        arRgt?.Invoke(t);
    }
    protected void InvTrngl(int t)
    {
        trngl?.Invoke();
    }
    protected void InvCrcl(int t)
    {
        crcl?.Invoke();
    }
    protected void InvCrss(int t)
    {
        crss?.Invoke();
    }
    protected void InvSqr(int t)
    {
        sqr?.Invoke();
    }
}
