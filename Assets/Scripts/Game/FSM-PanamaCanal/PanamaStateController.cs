using UnityEngine;

public class PanamaStateController : MonoBehaviour
{

    #region State Machine
    PanamaStateMachine stateMachine;
    public MoveBoat MoveBoat { get; private set; }
    public PillarUp PillarUp { get; private set; }
    public PillarDown PillarDown { get; private set; }
    public WaterUp WaterUp { get; private set; }
    public WaterDown WaterDown { get; private set; }
    public BoatUp BoatUp { get; private set; }
    public BoatDown BoatDown { get; private set; }
    #endregion

    #region GameObjects
    public GameObject boat;
    public GameObject currentPillar;
    public GameObject nextPillar;
    public GameObject water;
    #endregion

    #region Layer masks
    public LayerMask boatMask;
    public LayerMask pillarMask;
    public LayerMask waterMask;
    public LayerMask landMask;
    #endregion

    #region Decision conditionals
    public bool isPillarDown = false;
    public bool isGoingDown = false;
    #endregion

    #region keys
    public const string k_Up = "Up";
    public const string k_Down = "Down";
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 3.0f;
        //State machine set up
        stateMachine = new PanamaStateMachine(this);

        //States set up
        MoveBoat = new MoveBoat(stateMachine, this);
        PillarUp = new PillarUp(stateMachine, this);
        PillarDown = new PillarDown(stateMachine, this);
        WaterUp = new WaterUp(stateMachine, this);
        WaterDown = new WaterDown(stateMachine, this); 
        BoatUp = new BoatUp(stateMachine, this);
        BoatDown = new BoatDown(stateMachine, this);


        stateMachine.Initialize(MoveBoat);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.CurrentState.Update();
    }
}
