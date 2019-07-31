using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Focus;
    Vector3 Offset;

    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        Offset = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = Focus.transform.position;
        this.transform.position = pos + Offset;

        Quaternion rotNew = Quaternion.Euler(15,y,0);
        this.transform.localRotation = rotNew;

        y += 0.1f;
    }
}
