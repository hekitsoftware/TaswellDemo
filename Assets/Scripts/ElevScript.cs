using UnityEngine;

public enum ElevatorType
{
    Entrance,
    Exit
}

public enum Direction
{
    Up,
    Down,
}

public class ElevScript : MonoBehaviour
{
    [Header("Dependant Variables")]
    [SerializeField] public ElevatorType eType;
    [SerializeField] public Direction eDirection;
    [SerializeField] public GameObject self;
    [SerializeField] public BoxCollider2D playerDetection;

    public Animator anim;
    public SceneManager sceneManager;

    public bool hasPassenger;
    public bool isTransitioning;

    public bool isClosing;
    public bool isExitClosing;

    private void Update()
    {
        if(sceneManager.isEntering || sceneManager.isLeaving)
        {
            isTransitioning = true;
        } else { isTransitioning = false; }

        switch(eType){
            case ElevatorType.Entrance:
                anim.SetBool("IsClosed", isClosing);
                break;
            case ElevatorType.Exit:
                anim.SetBool("IsClosed", isExitClosing);
                break;
        }

        if (isTransitioning)
        {
            self.transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
        } else { self.transform.position = new Vector3(transform.position.x, transform.position.y, 1f); }
    }
}
