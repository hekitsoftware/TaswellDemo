using UnityEngine;

public class ElevScript : MonoBehaviour
{
    [SerializeField] public bool isExit;
    [SerializeField] public GameObject self;
    [SerializeField] public BoxCollider2D playerDetection;

    public Animator anim;
    public SceneManager sceneManager;
    public bool hasPassenger;

    public bool isLitirallyClosed;

    private void Update()
    {
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Elev_Closed"))
        {
            isLitirallyClosed = true;
            playerDetection.enabled = false;
        }

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Elev_Open"))
        {
            isLitirallyClosed = false;
            playerDetection.enabled = true;
        }

        if (hasPassenger)
        {
            self.transform.position = new Vector3(0, 0, -3);
        }
        else { self.transform.position = new Vector3(0, 0, 0); }

        if (isExit)
        {
            if (sceneManager.exitElevatorIsCharged)
            {
                anim.SetBool("IsClosed", false);
            }
            else { anim.SetBool("IsClosed", true); }
        }
        if (!isExit)
        {
            if (sceneManager.ElevatorIsCharged)
            {
                anim.SetBool("IsClosed", false);
            }
            else { anim.SetBool("IsClosed", true); }
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
