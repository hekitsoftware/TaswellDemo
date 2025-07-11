using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public ItemManager _iManager;      // Handles player's item-based stat upgrades
    [SerializeField] public SceneManager2 _sManager;     // Manages scene transitions and player lock states

    [SerializeField] public GameObject _player;          // The main player GameObject
    [SerializeField] public Animator _anim;              // Controls player animations
    [SerializeField] public Rigidbody2D rb;              // Rigidbody for physics-based movement
    [SerializeField] public Transform groundCheck;       // Position used to check if player is grounded
    [SerializeField] public LayerMask groundLayer;       // Ground detection layer

    [Header("Movement")]
    [SerializeField] public float moveSpeed;             // Base movement speed
    [SerializeField] public float accSpeed = 10f;        // Acceleration rate
    [SerializeField] public float decSpeed = 8f;         // Deceleration rate
    [SerializeField] public float velPower = 1f;         // Curve adjustment for acceleration
    private float speedMulti;                            // Movement speed multiplier (from items)

    [SerializeField] public float jumpPower = 1f;        // Jump force multiplier
    [SerializeField] public bool GD; // isGrounded flag
    [SerializeField] public bool IM; // isMoving flag
    [SerializeField] public bool IF; // isFalling flag

    public HealthScript healScript;                      // Reference to the player’s health system

    [SerializeField] public WeaponParent weaponParent;   // Handles player's weapon orientation
    private Vector2 pointerInput, movementInput;         // Used with InputSystem (not fully used here)
    [SerializeField] private InputActionReference movement, attack, pointerPosition;

    public bool isFacingRight = true;                    // Track the direction player is facing

    public AudioSource mouth;                            // Audio source for hurt sound
    public AudioClip ouchySnd;                           // "Ouch" sound clip

    private float moveInput;                             // Raw horizontal input value

    private void FixedUpdate()
    {
        // Only allow movement if the scene manager has not locked the player
        if (!_sManager.playerIsLocked) { Move(); }

        // Get left/right input (-1 to 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Update grounded status
        GD = IsGrounded();
    }

    /// <summary>
    /// Plays hurt sound when the player takes damage.
    /// </summary>
    public void Ouchy()
    {
        mouth.PlayOneShot(ouchySnd, 1);
    }

    /// <summary>
    /// Flips the player's direction to face the side of the screen where the mouse is.
    /// </summary>
    public void FaceMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        float screenCenter = Screen.width / 2;

        bool shouldFaceRight = mousePosition.x >= screenCenter;

        if (shouldFaceRight != isFacingRight)
        {
            Flip(); // Flip sprite
            isFacingRight = shouldFaceRight;
        }

        // Notify weapon which direction the player is facing
        weaponParent.IsFacingRight = isFacingRight;
    }

    /// <summary>
    /// Flips the player's sprite horizontally.
    /// </summary>
    private void Flip()
    {
        Vector3 scale = _player.transform.localScale;
        scale.x *= -1; // Invert X scale
        _player.transform.localScale = scale;
    }

    private float healTimer = 0f; // Timer used for passive healing

    private void Update()
    {
        // Keep health multiplier updated from item manager
        healScript.healthMulti = _iManager.hpMulti;

        // Heal the player over time
        healTimer += Time.deltaTime;
        if (healTimer >= 1f)
        {
            healScript.Heal(1); // Heal 1 HP every second
            healTimer = 0f;
        }

        FaceMouse(); // Continuously face the mouse

        speedMulti = _iManager.moveSpeedMulti; // Get updated speed multiplier

        if (!_sManager.playerIsLocked) { Jump(); }

        // Horizontal animation check
        IM = moveInput != 0;

        // Vertical (falling) animation check
        IF = !GD && rb.linearVelocityY != 0;

        // Update animator states
        _anim.SetBool("IsMoving", IM);
        _anim.SetBool("IsFalling", IF);
    }

    #region Movement

    /// <summary>
    /// Checks if the player is grounded using a small overlap circle.
    /// </summary>
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    /// <summary>
    /// Handles smooth acceleration and deceleration for movement.
    /// </summary>
    public void Move()
    {
        float targetSpeed = (moveInput * moveSpeed) * speedMulti;

        // Calculate difference between current and desired velocity
        float speedDif = targetSpeed - rb.linearVelocity.x;

        // Use acceleration or deceleration depending on movement
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accSpeed : decSpeed;

        // Apply exponential curve for smooth control
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        // Apply force to move the player horizontally
        rb.AddForce((movement * Vector2.right));
    }

    /// <summary>
    /// Handles jumping and variable jump height logic.
    /// </summary>
    public void Jump()
    {
        // Jumping when grounded and spacebar is held
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, (jumpPower * _iManager.jumpForce));
        }
        else
        {
            return;
        }

        // Cut jump short if spacebar is released early
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * 0.5f);
        }
    }

    #endregion
}
