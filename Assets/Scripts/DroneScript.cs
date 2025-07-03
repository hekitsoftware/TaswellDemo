using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneScript : MonoBehaviour
{
    //This script was based on the "Bloodhound" drone pathfinding system by 'Imphenzia'
    //This will be for the DR0WN Drones (ball dudes)

    [Header("Movement")]
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float velocity = 9f;
    [SerializeField] float acceleration = 100f;
    [SerializeField] float damage = 10f;

    [Header("Debugging Visualiser")]
    [SerializeField] Material debugMatOrange;
    [SerializeField] Material debugMatGreen;
    [SerializeField] Mesh debugMesh;
    [SerializeField] bool debugEnabled = false;

    //Setting Target Position
    private Vector3 TargetPosition { get => target.position + Vector3.up; }
    private List<Vector3> wayPoints = new List<Vector3>();

    //References
    private Transform target;
    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private HealthScript ownHealth;

    //Memory
    private float stuckTimer = -1;
    private Vector3 lastPosition;
    private bool wiggleWindowExists;
    private float orgDrag;

    //Debugging Lines
    private LineRenderer debugLineRenderer;
    private Material debugLineMaterialGreen;
    private Material debugLineMaterialOrange;

    private int worldLayerMask;
    private int playerLayerId;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        worldLayerMask = LayerMask.GetMask("Ground");
        playerLayerId = LayerMask.NameToLayer("Player");
        ownHealth = GetComponent<HealthScript>();
        audioSource = GetComponent<AudioSource>();
        orgDrag = rb.linearDamping;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If drone has collided with Taswell
        if (collision.collider.gameObject.layer == playerLayerId)
        {
            //Call the damage method from HealthScript
            collision.collider.gameObject.GetComponent<HealthScript>()?.Damage((int)damage, collision.contacts[0].normal);
        }
    }

    public void Fire()
    {
        //SHOOTING HERE
    }

    private void DrawDebug()
    {

    }

    private void HandleAudio()
    {

    }

    private void Update()
    {
        if (debugEnabled) DrawDebug();

        //Wait until the player has been seen
        if (target == null) return;

        HandleAudio();

        //If view is not obstructed
        var targetDir = (TargetPosition - transform.position).normalized;
        if(!Physics.SphereCast(new Ray(transform.position, targetDir), 0.5f, Vector3.Distance(transform.position, TargetPosition), worldLayerMask) &&
            !Physics.CheckSphere(transform.position + targetDir, 0.5f, worldLayerMask))
        {
            //Delete any waypoints
            wayPoints.Clear();

            //Add the target current position as a waypoint
            wayPoints.Add(TargetPosition);
        }

        //If waypoints already exist andthe distance between the last waypoint and target is more than 'n'
        if(wayPoints.Count > 0 && Vector3.Distance(wayPoints[wayPoints.Count - 1], TargetPosition) > 1f)
        {
            //Add target's current position to the list of waypoints
            wayPoints.Add(TargetPosition);
        }

        //If there are no waypoints, don't do anything else
        if (wayPoints.Count == 0) { return; }

        //if the current drone position is close enough to the next waypoint...
        if(Vector3.Distance(transform.position, wayPoints[0]) < 2f)
        {
            //Delete first array entry
            wayPoints.RemoveAt(0);

            //Cancel any attempts to wiggle loose if stuck
            wiggleWindowExists = false;
        }
    }

    private void FixedUpdate()
    {
        //Force the rigidbody Z position to 0 (Layering)
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, 0));

        //If no waypoints
        if (wayPoints.Count == 0)
        {
            //Bring drone towards full stop and return
            rb.linearDamping = 1f;
            return;
        }
        else
        {
            rb.linearDamping = orgDrag;

            //ROTATION SCRIPT WOULD GO HERE
            ///Was having issues with Vector2/3 conversions; kinda gave up and then realised it was better without rotation
        }

        //Add force to accelerate the drone forwards
        rb.AddForce(transform.forward * acceleration, ForceMode2D.Force);

        //Ensure drone doesn't exceed max velocity
        if (rb.linearVelocity.magnitude > velocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocity;
        }

        //Code if the drone gets stuck

        //Check distance travelled since last physics frame
        var disTravelled = Vector3.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        //If distance travelled < 1 unit / sec...
        if (disTravelled < Time.fixedDeltaTime)
        {
            if (stuckTimer < 0) stuckTimer = Time.time;
        }

        //If drone has been stuck for more than 'n' seconds
    }

}
