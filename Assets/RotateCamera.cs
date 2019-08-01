using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public double speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, (float) (Time.deltaTime * speed), 0.0f);
    }
}
