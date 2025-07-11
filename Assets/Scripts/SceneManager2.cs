using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager2 : MonoBehaviour
{
    [SerializeField] GameObject EntrancePrefab;
    [SerializeField] GameObject ExitPrefab;
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Battery;
    [SerializeField] public MusicManager mManager;

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

    private void Start()
    {
        fadeIn = false;
        fadeOut = false;

        //Grab the scripts
        elevScript = EntrancePrefab.GetComponent<ElevScript>();
        exitElevScript = ExitPrefab.GetComponent<ElevScript>();
        _iManager = Player.GetComponent<ItemManager>();
        batteryScript = Battery.GetComponent<BatteryScript>();

        //Initialise all level-variables
        exitElevatorIsCharged = false;

        isLeaving = false;

        exitElevatorIsOpen = false;
        exitElevScript.isExitClosing = true;

        EnterStage();
    }

    public bool fadeOut;
    public bool fadeIn;
    public float TimeToFade = 3;

    private void Update()
    {
        //Wait for Elevator to Open
        if (isEntering && elevScript.anim.GetCurrentAnimatorStateInfo(0).IsName("Elev_Open"))
        {
            isEntering = false;
            ElevatorIsOpen = true;
            playerIsLocked = false;
        }

        if (!isLeaving && exitElevatorIsCharged && exitElevScript.hasPassenger && Input.GetKey(KeyCode.E))
        {
            LeaveStage();
        }

        if (batteryScript.isActive)
        {
            exitElevatorIsCharged = true;
            if (!isLeaving)
            {
                exitElevScript.isExitClosing = false;
            }
        }
        else { exitElevatorIsCharged = false; }
    }

    public void EnterStage()
    {
        isEntering = true;
        playerIsLocked = true;
        elevScript.isClosing = false;
    }

    private void LeaveStage()
    {
        playerIsLocked = true;
        isLeaving = true;
        exitElevScript.isExitClosing = true;
        SceneManager.LoadScene(3);
        mManager.PlayMusic("WinTrack", (1 / 2));
    }

    public void DieSequence()
    {
        SceneManager.LoadScene(2);
        mManager.PlayMusic("LoseTrack", (1 / 2));
    }
}
