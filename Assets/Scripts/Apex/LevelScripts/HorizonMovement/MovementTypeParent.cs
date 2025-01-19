using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeParent : MonoBehaviour
{
    protected DummyHorizonMovement movement;

    public virtual bool Starfe(float deltaTime, out Vector3 strafe)
    {
        strafe = Vector3.zero;
        return false;
    }

    public void Setup(DummyHorizonMovement movement)
    {
        this.movement = movement;
    }
}
