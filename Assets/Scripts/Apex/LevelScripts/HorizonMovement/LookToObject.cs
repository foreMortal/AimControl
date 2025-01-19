using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToObject : MonoBehaviour
{
    public Transform LookTo;

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.LookAt(LookTo);
    }
}
