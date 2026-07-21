using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int enemiesToSpawn = 5;

    [Header("Spawn Locations")]
    public Vector2[] spawns;

    private int spawnedEnemies = 0;
    private int attempts = 0;
    private int maxAttempts = 1000;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (spawns == null || spawns.Length == 0)
        {
            throw new System.NullReferenceException("No spawn points defined.");
        }

        while (spawnedEnemies < enemiesToSpawn && attempts <= maxAttempts)
        {
            int randomIndex = Random.Range(0, spawns.Length);
            Vector2 spawnLocation = spawns[randomIndex];

            Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            spawnedEnemies++;

            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.Log("Max enemy spawning attempts reached.");
        }
    }
}
