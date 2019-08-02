
using System;
using HutongGames.PlayMaker;
using UnityEngine;
using StoryEngine;
using StoryEngine.UI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//using StoryEngine.UI;   

namespace Starfish
{

    public class SetHandler : MonoBehaviour
    {

        public GameObject Root;
        int AgentCount = 10;


        public GameObject BodyPrefab, LegPrefab;
        public Text Title, Subtitle, Generation,Genes;

        public FamilyTreeDirector familyTreeDirector;
        public GameObject GenTitle,GenTitles;

        public Canvas UserCanvas;
        Controller Controller;
        Mapping MainMapping;
        Layout MainLayout;
        InterFace MainInterface, UpperInterface, LowerInterface;
        Zeester[] pop;


        public Fsm fsm;


        ZeeSterEvolutie ZeeSterEvolutie;

        void ShowTitle() // 1.5s
        {
            Generation.gameObject.SetActive(true);
            Title.gameObject.SetActive(true);
            Generation.text = "GENERATION " + FsmVariables.GlobalVariables.GetFsmInt("round").Value;
            Genes.gameObject.SetActive(true);
        }

        void HideTitle()
        {
            Generation.gameObject.SetActive(false);
            Title.gameObject.SetActive(false);
            Genes.gameObject.SetActive(false);
        }


        void StartEvolution()
        {
            ZeeSterEvolutie = new ZeeSterEvolutie();
            pop = ZeeSterEvolutie.GetPop();
            Genes.text = "";
            for (int a = 0; a < pop.Length; a++)
            {
                Genes.text += pop[a].getGeneAsString() + "\n";
            }
        }

        void ShowFamilyTree()
        {
            familyTreeDirector.ShowFamilyTree(ZeeSterEvolutie);

        }

        void Reset()
        {
            // Start again
            ZeeSterEvolutie = new ZeeSterEvolutie();
            pop = ZeeSterEvolutie.GetPop();
            Genes.text = "";
            for (int a = 0; a < pop.Length; a++)
            {
                Genes.text += pop[a].getGeneAsString() + "\n";
            }
            FsmVariables.GlobalVariables.GetFsmInt("round").Value = 1;
        }

        void Evolve()
        {
            int[] Score = new int[AgentCount];

            for (int a = 0; a < AgentCount; a++)
            {

                GameObject go = pop[a].GameObject;
                float distance=0;

                if (go != null)
                {
                    distance = go.transform.position.magnitude;
                }
                Score[a] = (int)(distance);
            }

            pop = ZeeSterEvolutie.Evolution(Score);
            FsmVariables.GlobalVariables.GetFsmInt("round").Value += 1;
        }


        void Spawn()
        {

                    Genes.text = "";

                    for (int a = 0; a < AgentCount; a++)
                    {

                        // Shape input

                        Zeester ster = pop[a];



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

                        body.transform.SetParent(Root.transform, false);
                        body.transform.localPosition = new Vector3(0, 0, Random.Range(0f, 0.4f));
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

                        agt.Freq = Freq;
                        agt.Amp = Amp;
                        agt.Phase = Phase;

                     Debug.Log("dna: "+   ster.getGeneAsString());
                        Genes.text += ster.getGeneAsString() + "\n";
                    }
        }

        void MakeInterface()
        {

                // Create a controller
                Controller = new Controller();

                // Create a layout (can hold multiple planes and interfaces)
                MainLayout = new Layout();

                // Create a plane
                //Plane UpperPlane3d = new Plane(GameObject.Find("UpperPlane"));
                //Plane LowerPlane3d = new Plane(GameObject.Find("LowerPlane"));

                // Create an interface
                UpperInterface = new InterFace(UserCanvas.gameObject, "upper");
                //LowerInterface = new InterFace(UserCanvas.gameObject, "lower");

                // Create a mapping and add it to the interface
                MainMapping = new Mapping();
                MainMapping.ux_single_none += Methods.OrbitCamera;
                MainMapping.ux_double_none += Methods.LateralCamera;
                MainMapping.ux_double_none += Methods.LongitudinalCamera;
                MainMapping.ux_single_2d += Methods.Drag2D;
                MainMapping.ux_tap_2d += Methods.tapButton2D;

                // Create an orbit cam with a pitch constraint.
                Constraint orbitConstraint = new Constraint()
                {
                    pitchClamp = true,
                    pitchClampMin = 15f,
                    pitchClampMax = 85f

                };

                UiCam3D uppercam = new UiCam3D(GameObject.Find("MainCamera"));
                uppercam.AddContraint(orbitConstraint);

                //UiCam3D lowercam = new UiCam3D(GameObject.Find("CameraLower"));
                //lowercam.AddContraint(orbitConstraint);

                // Create an exit button and add it to the interface
                //button = new Button("Exit");
                //button.AddConstraint(Constraint.LockInPlace(button));
                //button.AddCallback("startmenu");
                //UpperInterface.addButton(button);

                // Add together.

                UpperInterface.AddUiCam3D(uppercam);
                //LowerInterface.AddUiCam3D(lowercam);

                UpperInterface.AddMapping(MainMapping);
                //LowerInterface.AddMapping(MainMapping);

                //UpperPlane3d.AddInterface(UpperInterface);
                //LowerPlane3d.AddInterface(LowerInterface);

                // Add to layout
                MainLayout.AddInterface(UpperInterface);
                //MainLayout.AddPlane(UpperPlane3d);
                //MainLayout.AddPlane(LowerPlane3d);
        }

        void Kill()
        {
            foreach (Transform child in Root.transform)
            {
                Destroy(child.gameObject);
            }
        }

        void Interface()
        {
            // Update the interface(s) and get result.
            UserCallBack result = Controller.updateUi(MainLayout);
            if (result.trigger)
            {
                Director.Instance.NewStoryLine(result.label);
            }
        }
    }
}

