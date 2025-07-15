using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
using static UnityEditor.Progress;
=======
>>>>>>> Stashed changes

public class ChestScript : MonoBehaviour
{
    [SerializeField] public Item chestItem;
    [SerializeField] public GameObject chest;
    [SerializeField] public Animator chestAnim;

    [SerializeField] public BoxCollider2D triggerBox;

    [SerializeField] public bool hasBeenActivated;
    public bool isOpen;
    public AudioSource open_sfx;
    public GameObject droppedItemPrefab;
    public GameObject _player;
    public ItemObject itemobj;

    [SerializeField] public List<Item> itemPool = new List<Item>();

    private void OnAwake()
    {
        isOpen = false;
        chestAnim.SetBool("ChestActivated", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.gameObject.name == "PLAYER")
        {
            open_sfx.Play();
            chestAnim.SetBool("ChestActivated", true);
            isOpen = true;
            GetDroppedItem();
            InstatiateItem(transform.position);
        }
    }

    //Roll random number + Choose an item to spit out
    Item GetDroppedItem()
    {
        int randomeNumb = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();
        foreach (Item item in itemPool)
        {
            if(randomeNumb <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null;
    }

    //Spawn a physical version of the item for the player to see/pick-up
    public void InstatiateItem(Vector2 spawnPosition)
    {
        Item droppedItem = GetDroppedItem();
        itemobj.playerItemMan = _player.GetComponent<ItemManager>();
        itemobj.chestScript = this.GetComponent<ChestScript>();

        if (droppedItem != null)
        {
            chestItem = droppedItem;
            GameObject itemGO = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            itemGO.GetComponent<SpriteRenderer>().sprite = droppedItem.itemSprite;

            float dropForce = 1.5f;
            Vector2 dropDir = new Vector2(Random.Range(-1f, 1f), 0);

            itemGO.GetComponent<Rigidbody2D>().AddForce(dropDir * dropForce, ForceMode2D.Impulse);
        }
    }

}
