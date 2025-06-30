using UnityEngine;

public class ChestSpawnManager : MonoBehaviour
{
    public GameObject chestPrefab;
    public int numberOfChests = 4;

    public BoxCollider2D spawnArea;

    public float groundCheckDistance;
    public LayerMask groundLayer;

    private void Awake()
    {
        SpawnChests();
    }

    void SpawnChests()
    {
        //Check a spawn-area has been assigned
        if (spawnArea != null)
        {
            //Setting the limits of our area based on the Trigger Box
            Bounds bounds = spawnArea.bounds;
            Vector2 areaMin = bounds.min;
            Vector2 areaMax = bounds.max;

            //Reset the int to keep track of our spawned chests
            int spawned = 0;
            int maxAttempts = numberOfChests * 10;

            //for every attempt...
            for (int i = 0; i < maxAttempts && spawned < numberOfChests; i++)
            {
                float randomX = Random.Range(areaMin.x, areaMax.x);
                Vector2 origin = new Vector2(randomX, areaMax.y);

                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
                if (hit.collider != null)
                {
                    Vector2 chestPos = hit.point + Vector2.up * 0.1f; // Slightly above ground
                    Instantiate(chestPrefab, chestPos, Quaternion.identity);
                    spawned++;
                }
            }

            Debug.Log($"Spawned {spawned} chests.");
        }

    }
}
