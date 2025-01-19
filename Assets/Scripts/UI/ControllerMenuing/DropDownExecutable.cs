using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownExecutable : MonoBehaviour, IMenuExecutable
{
    public GameObject images;
    public int maxValue;
    private int val;
    private Dropdown dropdown;
    private CanvasManager canvasManager;
    private MenuingManager.MenuDelegate crss, crcl, trngl, sqr;
    private bool haveDrop;

    private void Awake()
    {
        canvasManager = GetComponentInParent<CanvasManager>();
        dropdown = GetComponent<Dropdown>();
        haveDrop = true;
    }

    public void Execute()
    {
        if (!haveDrop)
        {
            canvasManager = GetComponentInParent<CanvasManager>();
            dropdown = GetComponent<Dropdown>();
            haveDrop = true;
        }
        dropdown.Show();
        images.SetActive(true);

        crss = canvasManager.crss;
        crcl = canvasManager.crcl;
        trngl = canvasManager.trngl;
        sqr = canvasManager.sqr;

        canvasManager.crss = Cross;
        canvasManager.trngl = pickTrianle;
        canvasManager.sqr = Square;
        canvasManager.crcl = Circle;
    }

    private void pickTrianle(int t)
    {
        val = 0;
        Activate();
    }
    private void Circle(int t)
    {
        val = 1;
        Activate();
    }
    private void Square(int t)
    {
        val = 2;
        Activate();
    }
    private void Cross(int t)
    {
        val = 3;
        Activate();
    }

    private void Activate()
    {
        canvasManager.trngl = trngl;
        canvasManager.sqr = sqr;
        canvasManager.crcl = crcl;
        canvasManager.crss = crss;

        dropdown.value = val;
        dropdown.Hide();
        images.SetActive(false);
    }
}
