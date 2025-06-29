using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public GameObject _player;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;

    [Header("Stats")]
    [SerializeField] public float DMG = 5f;
    [SerializeField] public float HP = 100f;
    [SerializeField] public float attackSPEED = 100f;
    [SerializeField] public float moveSpeed = 10f;

    [Header("Movement")]
    [SerializeField] public float accSpeed = 10f;
    [SerializeField] public float decSpeed = 8f;
    [SerializeField] public float velPower = 1f;

    [SerializeField] public float jumpPower = 1f;
    [SerializeField] public bool GD;

    public bool isFacingRight = true;

    [Header("Item Multipliers")]
    public float moveSpeedMulti = 1f;
    public float dmgMulti = 1f;
    public float hpMulti = 1f;
    public float attackSpeedMulti = 1f;

    private float moveInput;

    private void FixedUpdate()
    {
        Flip();

        // Get raw horizontal input (-1 BACKWARD, 0 NULL, or 1 FORWARD)
        moveInput = Input.GetAxisRaw("Horizontal"); //ASWD
        GD = IsGrounded();

        #region Running
        float targetSpeed = (moveInput * moveSpeed) * moveSpeedMulti;
        // Calculate the diff between current-velocity and peak speed
        float speedDif = targetSpeed - rb.linearVelocity.x;
        // Choose acce or dec depending on whether the player is moving or stopping
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accSpeed : decSpeed;
        // Apply exponential force scaling for nicer acceleration curves
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce((movement * Vector2.right));
        #endregion
    }

    private void Update()
    {
        Jump();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
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

    public void Flip()
    {
        if (isFacingRight && moveInput < 0 || !isFacingRight && moveInput > 0)
        {
            // Flip the character's local scale on the X axis
            Vector3 tempScale = _player.transform.localScale;
            tempScale.x *= -1;

            _player.transform.localScale = tempScale;

            isFacingRight = !isFacingRight;
        }
    }
}
