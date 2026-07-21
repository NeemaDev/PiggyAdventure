using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public KeyItem key;

    private List<Vector2> spawnLocations = new List<Vector2>()
    {
        new Vector2(-8.3f, 6.5f),
        new Vector2(-8.45f, 0f),
        new Vector2(-0.75f, 4.25f)
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (key == null)
        {
            throw new System.NullReferenceException("key is null");
        }
        else
        {
            int randomIndex = Random.Range(0, spawnLocations.Count);
            Vector2 randomPosition = spawnLocations[randomIndex];

            key.transform.position = randomPosition;
            key.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
