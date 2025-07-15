using UnityEngine;
using System.Collections.Generic;

public class DroneScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float velocity = 9f;
    [SerializeField] float acceleration = 100f;

    [Header("Debugging Visualiser")]
    [SerializeField] Material debugMatOrange;
    [SerializeField] Material debugMatGreen;
    [SerializeField] Mesh debugMesh;
    [SerializeField] bool debugEnabled = false;

    //Shooting
    public GameObject bullet;
    public GameObject player;
    public Transform bulletPos;

    private Vector3 TargetPosition => target != null ? target.position + Vector3.up : transform.position;
    private List<Vector3> wayPoints = new List<Vector3>();

    public Transform target;
    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private HealthScript ownHealth;

    private float stuckTimer = -1f;
    private Vector3 lastPosition;
    private bool wiggleWindowExists;
    private float orgDrag;
    private LineRenderer debugLineRenderer;
    private int worldLayerMask;
    private int playerLayerId;

    public bool isUnstable;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        worldLayerMask = LayerMask.GetMask("Ground");
        playerLayerId = LayerMask.NameToLayer("Player");
        ownHealth = GetComponent<HealthScript>();
        audioSource = GetComponent<AudioSource>();
        orgDrag = rb.linearDamping;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public float timer2 = 0f;

    public void StabilizeSpin()
    {
        if (isUnstable)
        {
            timer += Time.deltaTime;

            if (timer2 < 2)
            {
                float threshold = 50f;
                float damping = 5f;

                if (Mathf.Abs(rb.angularVelocity) > threshold)
                {
                    // Apply opposite torque to reduce spin
                    float oppositeTorque = -Mathf.Sign(rb.angularVelocity) * damping;
                    rb.AddTorque(oppositeTorque);
                }
            }
            else { isUnstable = false; }
        }
        else { return; }
    }

    public void Death()
    {
        anim.SetBool("IsDead", true);
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Exit"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == playerLayerId)
        {
            collision.collider.gameObject.GetComponent<HealthScript>()?.Damage(5);
        }
    }

    //Fire timer
    public float timer = 2f;

    public void Fire()
    {
            timer += Time.deltaTime;

        if (timer > 2)
            {
                timer = 0;
                Instantiate(bullet, bulletPos.position, Quaternion.identity);
            }
    }

    private void HandleAudio()
    {
        float dist = Vector3.Distance(TargetPosition, transform.position);
        float mul = (1f - Mathf.Clamp01(dist / 10f));
        audioSource.volume = Mathf.Clamp01(rb.linearVelocity.magnitude / velocity * 0.1f * mul) + 0.02f;
        audioSource.pitch = Mathf.Clamp(rb.linearVelocity.magnitude * mul * 1f + 0.5f, 0, 1);
    }

    private void Point()
    {
        if (target != null)
        {
            Vector2 direction = (TargetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle + 180f), Time.deltaTime * rotationSpeed);
        }
    }

    private void Update()
    {
        if (target == null) return;

        HandleAudio();
        Point();
        Fire();

        Vector2 targetDir = (TargetPosition - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, TargetPosition);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, targetDir, distance, worldLayerMask);
        bool blocked = hit.collider != null || Physics2D.OverlapCircle(transform.position + (Vector3)targetDir, 0.5f, worldLayerMask);

        if (!blocked)
        {
            wayPoints.Clear();
            wayPoints.Add(TargetPosition);
        }

        if (wayPoints.Count > 0 && Vector3.Distance(wayPoints[wayPoints.Count - 1], TargetPosition) > 1f)
        {
            wayPoints.Add(TargetPosition);
        }

        if (wayPoints.Count == 0) return;

        if (Vector3.Distance(transform.position, wayPoints[0]) < 2f)
        {
            wayPoints.RemoveAt(0);
            wiggleWindowExists = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, 0f));

        if (wayPoints.Count == 0)
        {
            rb.linearDamping = 1f;
            return;
        }

        rb.linearDamping = orgDrag;

        Vector2 moveDir = (wayPoints[0] - transform.position).normalized;
        rb.AddForce(moveDir * acceleration);

        if (rb.linearVelocity.magnitude > velocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocity;
        }

        float disTravelled = Vector3.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        if (disTravelled < Time.fixedDeltaTime)
        {
            if (stuckTimer < 0) stuckTimer = Time.time;
        }

        if (Time.time > stuckTimer + 1)
        {
            Vector2 randomInCircle = Random.insideUnitCircle.normalized * 4;
            Vector2 wigglePosition = rb.position + randomInCircle;

            if (!Physics2D.OverlapCircle(wigglePosition, 0.5f, worldLayerMask))
            {
                if (!wiggleWindowExists)
                {
                    wayPoints.Insert(0, wigglePosition);
                    wiggleWindowExists = true;
                }
                else
                {
                    wayPoints[0] = wigglePosition;
                }

                stuckTimer = -1;
            }
        }
    }
}
