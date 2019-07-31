using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotcrawl : MonoBehaviour
{
    public GameObject[] Legs;
    float[] Legrotation;

    // Start is called before the first frame update
    void Start()
    {
        Legrotation = new float[Legs.Length];
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int l = 0; l < Legs.Length; l++)
        {

            Legs[l].transform.localRotation = Quaternion.Euler(0, 0, Legrotation[l]);
            Legrotation[l] += 0.5f;


        }


    }
}
