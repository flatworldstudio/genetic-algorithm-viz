using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public double speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(15f + (float) (Mathf.Sin(Time.time) * 1.25f), (float) (Time.deltaTime * speed) + transform.localEulerAngles.y, 0.0f);
    }
}
