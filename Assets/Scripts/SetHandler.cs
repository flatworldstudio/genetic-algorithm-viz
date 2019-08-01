
using UnityEngine;
using StoryEngine;
using StoryEngine.UI;
using UnityEngine.UI;

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


        public Canvas UserCanvas;
        Controller Controller;
        Mapping MainMapping;
        Layout MainLayout;
        InterFace MainInterface, UpperInterface, LowerInterface;
        Zeester[] pop;

        int Round = 1;

//        FamilyTree[] familyTree;

        ZeeSterEvolutie ZeeSterEvolutie;
        public SetController setController;
        readonly string ID = "SetHandler: ";


        // Copy these into every class for easy debugging. This way we don't have to pass an ID. Stack-based ID doesn't work across platforms.
        void Log(string message) => StoryEngine.Log.Message(message, ID);
        void Warning(string message) => StoryEngine.Log.Warning(message, ID);
        void Error(string message) => StoryEngine.Log.Error(message, ID);
        void Verbose(string message) => StoryEngine.Log.Message(message, ID, LOGLEVEL.VERBOSE);

        void Awake()
        {


        }


        void Start()
        {
            setController.addTaskHandler(TaskHandler);

        }


        float wait;

        public bool TaskHandler(StoryTask task)
        {

            bool done = false;

            switch (task.Instruction)
            {

                case "generationtitle":


                    if (task.GetFloatValue("wait", out wait))
                    {
                       
                        done |= Time.time > wait;
                        if (Time.time > wait)
                        {
                            Generation.gameObject.SetActive(false);
                            Title.gameObject.SetActive(false);
                            done = true;
                        }

                    }
                    else
                    {

                        Generation.gameObject.SetActive(true);
                        Title.gameObject.SetActive(true);

                        Generation.text = "GENERATION " + Round;
                        task.SetFloatValue("wait", Time.time + 1.5f);

                    }
                    break;

                case "genetitle":


                    if (task.GetFloatValue("wait", out wait))
                    {

                        done |= Time.time > wait;
                        if (Time.time > wait)
                        {
                            Genes.gameObject.SetActive(false);
                            done = true;
                        }

                    }
                    else
                    {
                        Genes.gameObject.SetActive(true);

                      
                        task.SetFloatValue("wait", Time.time + 1.5f);
                    }
                    break;

                case "startevolution":

                    ZeeSterEvolutie = new ZeeSterEvolutie();
                    pop = ZeeSterEvolutie.GetPop();
                    Genes.text = "";
                    for (int a = 0; a < pop.Length; a++)
                    {

                        Genes.text += pop[a].getGeneAsString() + "\n";

                    }

                      


                    done = true;
                    break;
                
                case "reset":

                    if (Round > 2)
                    {
                        // Show family tree
                        familyTreeDirector.ShowFamilyTree(ZeeSterEvolutie);


                        // Start again
                        ZeeSterEvolutie = new ZeeSterEvolutie();
                        pop = ZeeSterEvolutie.GetPop();
                        Genes.text = "";
                        for (int a = 0; a < pop.Length; a++)
                        {

                            Genes.text += pop[a].getGeneAsString() + "\n";

                        }

                        Round = 1;
                    }

                    done = true;
                    break;


                case "show family tree if reset":
                    Debug.Log("Family");
                    break;

                case "evolve":


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

                        Log("score " + Score[a]);
                        
                    }
                    
                    pop = ZeeSterEvolutie.Evolution(Score);
                    Round++;

                    done = true;
                    break;

                case "spawn":

                    //int AgentCount = 10;

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

                     Log("dna: "+   ster.getGeneAsString());
                        Genes.text += ster.getGeneAsString() + "\n";
                    }

                    



                    done = true;

                    break;


                case "kill":


                    foreach (Transform child in Root.transform)
                    {
                        Destroy(child.gameObject);
                    }


                    done = true;
                    break;


                case "showfamilytree":
                    break;

                case "makeinterface":

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

                    done = true;

                    break;

                case "interface":

                    // Update the interface(s) and get result.

                    UserCallBack result = Controller.updateUi(MainLayout);

                    if (result.trigger)
                    {
                        Log("User tapped " + result.sender + ", starting storyline " + result.label);
                        Director.Instance.NewStoryLine(result.label);
                    }

                    break;



                case "task1":
                case "task2":
                case "task3":
                case "repeatingtask":

                    if (task.GetFloatValue("wait", out wait))
                        done |= Time.time > wait;
                    else
                    {
                        task.SetFloatValue("wait", Time.time + 3f);
                        Log("Executing task " + task.Instruction);
                    }
                    break;

                case "wait1":

                    if (task.GetFloatValue("wait", out wait))
                        done |= Time.time > wait;
                    else
                        task.SetFloatValue("wait", Time.time + 1f);
                    break;

                case "wait5":

                    if (task.GetFloatValue("wait", out wait))
                        done |= Time.time > wait;
                    else
                        task.SetFloatValue("wait", Time.time + 5f);
                    break;


                case "wait10":

                    if (task.GetFloatValue("wait", out wait))
                        done |= Time.time > wait;
                    else
                        task.SetFloatValue("wait", Time.time + 10f);
                    break;

                default:
                    done = true;

                    break;
            }

            return done;

        }

        void Update()
        {
            // 
        }
    }
}

