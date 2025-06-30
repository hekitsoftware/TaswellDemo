using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] public Item chestItem;
    [SerializeField] public GameObject chest;
    [SerializeField] public Animator chestAnim;

    [SerializeField] public BoxCollider2D triggerBox;

    [SerializeField] public bool hasBeenActivated;

    private void OnAwake()
    {
        chestAnim.SetBool("ChestActivated", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER")
        {
            chestAnim.SetBool("ChestActivated", true);
        }
    }


}
