using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HealItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject itemPrefab;
    public int itemsToSpawn = 7;

    [Header("Spawn Locations")]
    public Vector2[] spawns;

    [Header("Physics")]
    public LayerMask obstacleLayer;
    public LayerMask itemLayer;

    private void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        if (spawns.Count() <= 0)
        {
            throw new System.NullReferenceException("No spawn areas defined");
        }

        int spawnedItems = 0;
        int maxAttemps = 1000;
        int attemps = 0;

        while (spawnedItems < itemsToSpawn && attemps <= maxAttemps)
        {
            attemps++;

            int index = Random.Range(0, spawns.Count());
            Vector2 position = spawns[index];

            // Check wall collision.
            Collider2D hit = Physics2D.OverlapCircle(position, 0.3f, obstacleLayer);
            // Check item collision.
            Collider2D itemHit = Physics2D.OverlapCircle(position, 1.5f, itemLayer);

            if (hit == null && itemHit == null)
            {
                Instantiate(itemPrefab, position, Quaternion.identity);
                spawnedItems++;
                Debug.Log($"Spawned item at {position.x}, {position.y}");
            }
        }

        if (attemps == maxAttemps)
        {
            Debug.Log("Reached max spawn attemps.");
        }
    }
}
