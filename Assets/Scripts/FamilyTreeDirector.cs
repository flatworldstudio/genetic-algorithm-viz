using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FamilyTreeDirector : MonoBehaviour
{
    public GameObject BodyPrefab, LegPrefab, FamilyTreeCamera;
    public float panSpeed = 2.0f;

//    public GameObject
    private void Awake()
    {
    }

    private void Update()
    {
        if (FamilyTreeCamera.activeSelf)
        {
            var pos = FamilyTreeCamera.transform.localPosition;
            FamilyTreeCamera.transform.localPosition = new Vector3(pos.x, pos.y, pos.z + Time.deltaTime * panSpeed);
        }
    }

    public void ShowFamilyTree(ZeeSterEvolutie evolutie)
    {
        FamilyTreeCamera.SetActive(true);

        /**
         * For each generation...
         */
        for (int i = 0; i < evolutie.familyTree.Count; i++)
        {
            var generation = evolutie.familyTree[i];

            /**
             * Instantiate each zeester in that generation's group
             */
            foreach (Zeester ster in generation.population)
            {
                int NumberOfLegs = ster.NumberOfLegs;
                int LegLength = ster.LegLength;
                float Hue = 1f / 7f * ster.Hue;
                float Sat = 1f / 7f * ster.Sat;
                float Val = 1f / 7f * ster.Val;

                // Behaviour input
                float Freq = (1f / 7f * ster.Freq) * 4f;
                float Amp = 5f + 7f * ster.Amp;//0-7
                float Phase = 1f / 7f * ster.Phase;

                GameObject body = Instantiate(BodyPrefab);
                ster.GameObject = body;

                body.transform.SetParent(transform, false);
                body.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.1f, Random.Range(-5f, 5f) + i * 20);
                Agent agt = body.GetComponent<Agent>();
                agt.Legs = new GameObject[NumberOfLegs];

                Material newMaterial = new Material(Shader.Find("Standard"));
                newMaterial.color = Color.HSVToRGB(Hue, Sat, Val);

                //newMaterial.color = Color.red;
                agt.mat = newMaterial;

                body.GetComponentInChildren<Renderer>().material = newMaterial;

                for (int l = 0; l < NumberOfLegs; l++)
                {
                    GameObject Leg = Instantiate(LegPrefab);
                    Leg.transform.SetParent(body.transform, false);
                    Leg.transform.localScale = new Vector3(1, 1, LegLength);
                    Leg.transform.localRotation = Quaternion.Euler(10f, 360f / NumberOfLegs * l, 0);
                    Leg.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0.5f);
                    agt.Legs[l] = Leg;

                    Leg.GetComponentInChildren<Renderer>().material = newMaterial;

                }

                agt.Freq = 0;
                agt.Amp = 0;
                agt.Phase = 0;

             Debug.Log("dna: "+   ster.getGeneAsString());
//             Genes.text += ster.getGeneAsString() + "\n";
    }
}
}

public void IsDone()
{

    }

    public void HideFamilyTree()
    {

    }
}