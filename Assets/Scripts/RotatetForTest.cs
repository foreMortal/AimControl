using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatetForTest : MonoBehaviour
{
    private void Start()
    {
        GamePadMouvement cam = GetComponentInChildren<GamePadMouvement>();
        cam.RotatePlayer(180f, 0f);
    }
}
