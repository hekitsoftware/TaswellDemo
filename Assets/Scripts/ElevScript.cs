using UnityEngine;

public class ElevScript : MonoBehaviour
{
    [SerializeField] public bool isExit;
    [SerializeField] public GameObject self;
    [SerializeField] public BoxCollider2D playerDetection;

    public Animator anim;
    public SceneManager sceneManager;
    public bool hasPassenger;

    private void Update()
    {
        //Check if prefab is Entrance or Exit
        if (isExit)
        {
            if (sceneManager.exitElevatorIsCharged)
            {
                anim.SetBool("IsClosed", false);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
            else if (!sceneManager.exitElevatorIsCharged && !hasPassenger)
            {
                anim.SetBool("IsClosed", true);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
            else if (!sceneManager.exitElevatorIsCharged && hasPassenger)
            {
                anim.SetBool("IsClosed", true);
                transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
            }
        }

        if (!isExit)
        {
            if (sceneManager.ElevatorIsCharged)
            {
                anim.SetBool("IsClosed", false);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
            else if (!sceneManager.ElevatorIsCharged && !hasPassenger)
            {
                anim.SetBool("IsClosed", true);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
            else if (!sceneManager.exitElevatorIsCharged && hasPassenger)
            {
                anim.SetBool("IsClosed", true);
                transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hasPassenger = true;
        }
    }
}
