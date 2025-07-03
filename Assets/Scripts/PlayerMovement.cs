using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public ItemManager _iManager;
    [SerializeField] public SceneManager _sManager;

    [SerializeField] public GameObject _player;
    [SerializeField] public Animator _anim;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float accSpeed = 10f;
    [SerializeField] public float decSpeed = 8f;
    [SerializeField] public float velPower = 1f;
    private float speedMulti;

    [SerializeField] public float jumpPower = 1f;
    [SerializeField] public bool GD; //isGrounded
    [SerializeField] public bool IM; //IsMoving
    [SerializeField] public bool IF; //isFalling

    [SerializeField] public WeaponParent weaponParent;
    private Vector2 pointerInput, movementInput;
    [SerializeField] private InputActionReference movement, attack, pointerPosition;

    public bool isFacingRight = true;

    private float moveInput;

    private void FixedUpdate()
    {
        if (!_sManager.playerIsLocked) { Move(); }

        // Get raw horizontal input (-1 BACKWARD, 0 NULL, or 1 FORWARD)
        moveInput = Input.GetAxisRaw("Horizontal"); //ASWD

        GD = IsGrounded();
    }

    //Detect which side of the screen the Mouse is

    public void FaceMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        float screenCenter = Screen.width / 2;

        bool shouldFaceRight = mousePosition.x >= screenCenter;

        if (shouldFaceRight != isFacingRight)
        {
            Flip();
            isFacingRight = shouldFaceRight;
        }

        // Pass facing direction to weapon
        weaponParent.IsFacingRight = isFacingRight;
    }

    private void Flip()
    {
        Vector3 scale = _player.transform.localScale;
        scale.x *= -1;
        _player.transform.localScale = scale;
    }

    private void Update()
    {
        FaceMouse();

        speedMulti = _iManager.moveSpeedMulti;
        if (!_sManager.playerIsLocked) { Jump(); }

        //X
        if (moveInput != 0)
        {
            IM = true;
        }
        else if (moveInput == 0) { IM = false; }

        //Y
        if (!GD && rb.linearVelocityY != 0)
        {
            IF = true;
        }
        else { IF = false; }

        _anim.SetBool("IsMoving", IM);
        _anim.SetBool("IsFalling", IF);

    }

    #region Movement
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Move()
    {
        #region Running
        float targetSpeed = (moveInput * moveSpeed) * speedMulti;
        // Calculate the diff between current-velocity and peak speed
        float speedDif = targetSpeed - rb.linearVelocity.x;
        // Choose acce or dec depending on whether the player is moving or stopping
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accSpeed : decSpeed;
        // Apply exponential force scaling for nicer acceleration curves
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce((movement * Vector2.right));
        #endregion
    }

    public void Jump()
    {
            if (Input.GetKey(KeyCode.Space) && IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, (jumpPower * _iManager.jumpForce));
            }
            else
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocityY > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * 0.5f);
            }
    }

    #endregion
}
