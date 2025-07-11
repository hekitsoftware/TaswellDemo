using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : MonoBehaviour
{
    // References to key scene objects
    [SerializeField] GameObject EntrancePrefab;    // Elevator entrance
    [SerializeField] GameObject ExitPrefab;        // Elevator exit
    [SerializeField] public GameObject Player;     // Player GameObject
    [SerializeField] public GameObject Battery;    // Battery GameObject
    [SerializeField] public MusicManager mManager; // Reference to music manager

    // Scripts on the elevator prefabs and player
    public ElevScript elevScript;           // Entrance elevator logic
    public ElevScript exitElevScript;       // Exit elevator logic
    public ItemManager _iManager;           // Player's item manager
    public BatteryScript batteryScript;     // Logic for the battery behavior

    // Elevator and player state tracking
    public bool ElevatorIsCharged;
    public bool ElevatorIsProgressing;
    public bool exitElevatorIsCharged;
    public bool exitElevatorIsProgressing;

    public bool ElevatorIsOpen;
    public bool exitElevatorIsOpen;

    public bool isLeaving;      // Is the player currently leaving the stage?
    public bool isEntering;     // Is the player currently entering the stage?

    public bool playerIsLocked; // Prevents player movement during transitions

    // Charging speed control
    public float chargeSpeedMultiplier;

    // Fade transition variables
    public bool fadeOut;
    public bool fadeIn;
    public float TimeToFade = 3;

    private IEnumerator Start()
    {
        // Initialize fade settings
        fadeIn = false;
        fadeOut = false;

        // Wait briefly to ensure all scene objects have loaded
        yield return new WaitForSeconds(0.1f);

        // Auto-find player and battery in the scene if not assigned in inspector
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Battery == null)
            Battery = GameObject.FindWithTag("Battery");

        // Ensure elevator prefabs are assigned
        if (EntrancePrefab == null || ExitPrefab == null)
        {
            Debug.LogError("Elevator prefabs not assigned!");
            yield break; // Stop further execution if missing
        }

        // Get elevator scripts from prefabs
        elevScript = EntrancePrefab.GetComponent<ElevScript>();
        exitElevScript = ExitPrefab.GetComponent<ElevScript>();

        // Validate player and battery were found
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

        // Initialize references to other scripts on the player and battery
        _iManager = Player.GetComponent<ItemManager>();
        batteryScript = Battery.GetComponent<BatteryScript>();

        // Set initial elevator and player states
        exitElevatorIsCharged = false;
        isLeaving = false;
        exitElevatorIsOpen = false;
        exitElevScript.isExitClosing = true;

        // Begin entrance animation/stage logic
        EnterStage();
    }

    private void Update()
    {
        // Check if player has finished entering the elevator (based on animation)
        if (isEntering && elevScript.anim.GetCurrentAnimatorStateInfo(0).IsName("Elev_Open"))
        {
            isEntering = false;
            ElevatorIsOpen = true;
            playerIsLocked = false; // Allow player movement again
        }

        // If exit elevator is charged and player presses E, initiate leave sequence
        if (!isLeaving && exitElevatorIsCharged && exitElevScript.hasPassenger && Input.GetKey(KeyCode.E))
        {
            LeaveStage();
        }

        // Check if battery is active to determine if the exit elevator is powered
        if (batteryScript != null && batteryScript.isActive)
        {
            exitElevatorIsCharged = true;
            if (!isLeaving)
                exitElevScript.isExitClosing = false; // Keep elevator open if not leaving
        }
        else
        {
            exitElevatorIsCharged = false;
        }
    }

    /// <summary>
    /// Begins stage entry sequence — locks player, starts entrance elevator animation.
    /// </summary>
    public void EnterStage()
    {
        isEntering = true;
        playerIsLocked = true;
        elevScript.isClosing = false; // Open the entrance elevator
    }

    /// <summary>
    /// Handles the logic for exiting the stage: locking player, transitioning scenes, playing music.
    /// </summary>
    private void LeaveStage()
    {
        playerIsLocked = true;
        isLeaving = true;
        exitElevScript.isExitClosing = true; // Close exit elevator
        SceneManager.LoadScene(2);           // Load the next level/scene
        mManager.PlayMusic("WinTrack", 1);   // Play victory music
    }

    /// <summary>
    /// Called when player dies. Reloads the current level and plays loss music.
    /// </summary>
    public void DieSequence()
    {
        SceneManager.LoadScene(1);           // Reload the level
        mManager.PlayMusic("LoseTrack", 1);  // Play game-over music
    }
}
