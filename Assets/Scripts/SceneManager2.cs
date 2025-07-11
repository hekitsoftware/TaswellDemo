using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Battery Stuff
    public float chargeSpeedMultiplier;

    // Fade variables
    public bool fadeOut;
    public bool fadeIn;
    public float TimeToFade = 3;

    private IEnumerator Start()
    {
        fadeIn = false;
        fadeOut = false;

        // Wait for all GameObjects to finish loading/spawning
        yield return new WaitForSeconds(0.1f);

        // Attempt to find objects if not manually assigned
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Battery == null)
            Battery = GameObject.FindWithTag("Battery");

        if (EntrancePrefab == null || ExitPrefab == null)
        {
            Debug.LogError("Elevator prefabs not assigned!");
            yield break;
        }

        elevScript = EntrancePrefab.GetComponent<ElevScript>();
        exitElevScript = ExitPrefab.GetComponent<ElevScript>();

        if (Player == null)
        {
            Debug.LogError("Player not found in scene.");
            yield break;
        }

        if (Battery == null)
        {
            Debug.LogError("Battery not found in scene.");
            yield break;
        }

        _iManager = Player.GetComponent<ItemManager>();
        batteryScript = Battery.GetComponent<BatteryScript>();

        // Initialize values
        exitElevatorIsCharged = false;
        isLeaving = false;
        exitElevatorIsOpen = false;
        exitElevScript.isExitClosing = true;

        EnterStage();
    }

    private void Update()
    {
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

        if (batteryScript != null && batteryScript.isActive)
        {
            exitElevatorIsCharged = true;
            if (!isLeaving)
                exitElevScript.isExitClosing = false;
        }
        else
        {
            exitElevatorIsCharged = false;
        }
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
        mManager.PlayMusic("WinTrack", 1);
    }

    public void DieSequence()
    {
        SceneManager.LoadScene(2);
        mManager.PlayMusic("LoseTrack", 1);
    }
}
