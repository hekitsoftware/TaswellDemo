using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    public SpriteRenderer rend;
    public CapsuleCollider2D zone;
    public AudioSource aSrc;

    public ItemManager playerItemMan;
    public ChestScript chestScript;

    [Header("Game Objects")]
    public GameObject chest;
    public GameObject player;

    public void Awake()
    {
        if (chestScript != null && chestScript.chestItem != null)
        {
            item = chestScript.chestItem;
            if (rend != null)
                rend.sprite = item.itemSprite;
        }
        else
        {
            Debug.LogWarning("ChestScript or chestItem is missing!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something touched the item");

        if (collision.CompareTag("Player") && playerItemMan != null)
        {
            playerItemMan = collision.GetComponent<ItemManager>();
            aSrc.Play();
            playerItemMan._items.Add(item);
            playerItemMan.CompileItems();
            Destroy(gameObject);
        }
    }
}
