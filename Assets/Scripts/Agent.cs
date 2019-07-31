using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{


    public GameObject[] Legs;

    public float Freq = 0.5f;
    public float Amp = 2f;
    public float Phase = 0.5f;
    public float Hue;
    public float Sat;
        public float Val;

  public  Material mat;


    public float Value;
    // Start is called before the first frame update
    void Start()
    {
      
    

    }

    // Update is called once per frame
    void Update()
    {

        float Tbase = Time.time * Freq;

        for (int l = 0; l < Legs.Length; l++)
        {
            float t = Tbase + l * Phase;

            Quaternion rot = Legs[l].transform.localRotation;

            Vector3 eul = rot.eulerAngles;

            Value = Mathf.Sin(t * Mathf.PI * 2);

            Quaternion rotNew = Quaternion.Euler(10f + Value * Amp, eul.y, eul.z);

            Legs[l].transform.localRotation = rotNew;

        }




    }
}
