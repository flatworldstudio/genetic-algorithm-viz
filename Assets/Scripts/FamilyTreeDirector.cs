using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FamilyTreeDirector : MonoBehaviour
{
    public GameObject BodyPrefab, LegPrefab, FamilyTreeCamera;
    public float panSpeed = 2.0f;
    public float generationSpacing = 20;
    public bool finished;
    public bool started = false;
    private ZeeSterEvolutie evolutie;
    public GameObject GenTitles;
    public PlayMakerFSM fsm;
    public Transform spawnPoint;

    private void Update()
    {
        if (FamilyTreeCamera.activeSelf)
        {
            var pos = FamilyTreeCamera.transform.localPosition;
            FamilyTreeCamera.transform.localPosition = new Vector3(pos.x, pos.y, pos.z + Time.deltaTime * panSpeed);
            if (FamilyTreeCamera.transform.position.z > evolutie.generations.Count * 20)
            {
                fsm.SendEvent("at the end of the tree");
            }
        }
    }

    public void ShowFamilyTree(ZeeSterEvolutie evolutie)
    {
        this.evolutie = evolutie;
        finished = false;
        started = true;
        FamilyTreeCamera.SetActive(true);
    //    GenTitles.SetActive(true);

        /**
         * For each generation...
         */
        for (int i = 0; i < evolutie.generations.Count; i++)
        {
            var generation = evolutie.generations[i];

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

                GameObject body = Instantiate(BodyPrefab);
                ster.GameObject = body;

                body.transform.SetParent(spawnPoint, false);

                /**
                 * Spawn in groups of generations with some random offset
                 */
                body.transform.localPosition = new Vector3(Random.Range(-7f, 7f), 0.1f, Random.Range(-1f, 1f) + i * generationSpacing);

                /**
                 * Title
                 */


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

                Debug.Log("dna: " + ster.getGeneAsString());
            }
        }
    }

    public void ResetHideAndResetFamilyTree()
    {
        finished = false;
        started = false;
        FamilyTreeCamera.SetActive(false);
        FamilyTreeCamera.transform.localPosition = new Vector3(0, 12.04f, -10);
        foreach (Transform child in spawnPoint.transform)
        {
            Destroy(child.gameObject);
        }
    }
}