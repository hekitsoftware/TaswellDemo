using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] GameObject EntrancePrefab;
    [SerializeField] GameObject ExitPrefab;
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Battery;

    public ElevScript elevScript;
    public ElevScript exitElevScript;
    public ItemManager _iManager;
    public BatteryScript batteryScript;

    public bool ElevatorIsCharged;
    public bool ElevatorIsProgressing;
    public bool exitElevatorIsCharged;
    public bool exitElevatorIsProgressing;

    public bool ElevatorIsOpen;
    public bool exitElevatorIsOpen;

    public bool isLeaving;
    public bool isEntering;

    public bool playerIsLocked;

    //Battery Stuff
    public float chargeSpeedMultiplier;

    private void Awake()
    {
        //Grab the scripts
        elevScript = EntrancePrefab.GetComponent<ElevScript>();
        exitElevScript = ExitPrefab.GetComponent<ElevScript>();

        //Initialise all level-variables
        exitElevatorIsCharged = false;

        isEntering = true;
        isLeaving = false;

        exitElevatorIsOpen = false;
        exitElevScript.isExitClosing = true;

        EnterStage();
    }

    private void Update()
    {
        //Wait for Elevator to Open
        if (isEntering && elevScript.anim.GetCurrentAnimatorStateInfo(0).IsName("Elev_Open"))
        {
            isEntering = false;
            ElevatorIsOpen = true;
            playerIsLocked = false;
        }

        LeaveStage();

        if (batteryScript.isActive)
        {
            exitElevatorIsCharged = true;
        } else { exitElevatorIsCharged = false; }
    }

    public void EnterStage()
    {
        playerIsLocked = true;
        elevScript.isClosing = false;
    }

    public void LeaveStage()
    {
        if (exitElevatorIsCharged && exitElevScript.hasPassenger && Input.GetKey(KeyCode.E))
        {
            isLeaving = true;
            exitElevScript.isExitClosing = true;
        }
    }
}
