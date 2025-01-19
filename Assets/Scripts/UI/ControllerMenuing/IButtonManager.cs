using System;
using UnityEngine;
using UnityEngine.UI;

public class IButtonManager: MonoBehaviour
{
    public bool canBePicked = true;
    public IButtonManager[] up, left, right, down;
    public int col;
    public int row;

    public Graphic graph;
    public Color highlitedColor;
    [NonSerialized] public Color handl;
    private IMenuExecutable executable;

    protected virtual void Awake()
    {
        handl = graph.color;
        executable = GetComponent<IMenuExecutable>();
    }

    public void ClearPaths()
    {
        up = new IButtonManager[0];
        left = new IButtonManager[0];
        right = new IButtonManager[0];
        down = new IButtonManager[0];
    }

    public void CopyPaths(IButtonManager[] up, IButtonManager[] left, IButtonManager[] right, IButtonManager[] down)
    {
        this.up = up;
        this.left = left;
        this.right = right;
        this.down = down;
    }

    public virtual void Activate()
    {
        executable.Execute();
    }

    public virtual IButtonManager Move(int side)
    {
        switch (side)
        {
            case 1:
                foreach(var t in up)
                {
                    if(t.canBePicked && t.isActiveAndEnabled)
                        return t;
                }
                return this;
            case 2:
                foreach (var t in left)
                {
                    if (t.canBePicked && t.isActiveAndEnabled)
                        return t;
                }
                return this;
            case 3:
                foreach (var t in right)
                {
                    if (t.canBePicked && t.isActiveAndEnabled)
                        return t;
                }
                return this;
            case 4:
                foreach (var t in down)
                {
                    if (t.canBePicked && t.isActiveAndEnabled)
                        return t;
                }
                return this;
        }
        return this;
    }

    public virtual void Highlight(bool state)
    {
        if (state)
        {
            graph.color = highlitedColor;
        }
        else
        {
            graph.color = handl;
        }
    }
}
