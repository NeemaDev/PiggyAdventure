using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int enemiesToSpawn = 5;

    [Header("Spawn Locations")]
    public Vector2[] spawns;

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

        int actualAmountToSpawn = Mathf.Min(enemiesToSpawn, spawns.Length);

        List<int> availableSpawnIndices = new List<int>();
        for (int index = 0; index < spawns.Length; index++)
        {
            availableSpawnIndices.Add(index);
        }

        int spawnedEnemies = 0;

        while (spawnedEnemies < actualAmountToSpawn)
        {
            Debug.Log($"Spawned:{spawnedEnemies}, ToSpawn: {actualAmountToSpawn}");

            int randomIndexOfAvilableIndices = Random.Range(0, availableSpawnIndices.Count);
            int actualSpawnIndex = availableSpawnIndices[randomIndexOfAvilableIndices];
            Vector2 spawnLocation = spawns[actualSpawnIndex];

            Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);

            availableSpawnIndices.RemoveAt(randomIndexOfAvilableIndices);
            spawnedEnemies++;
        }
    }
}
