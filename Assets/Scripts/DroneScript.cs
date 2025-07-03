using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneScript : MonoBehaviour
{
    //This script was based on the "Bloodhound" drone pathfinding system by 'Imphenzia'

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
    }

}
