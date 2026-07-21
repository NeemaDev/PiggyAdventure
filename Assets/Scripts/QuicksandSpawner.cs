using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class QuicksandSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject quickSandPrefab;
    [SerializeField] private int spawnCount = 5;

    [Header("Spawn Area")]
    [SerializeField] private Rect spawnArea = new Rect(-9f, 12.25f, 18f, 5.55f);

    [Header("Safe Zones")]
    [SerializeField]
    private Rect[] safeZones = new Rect[]
    {
        new Rect(-9f,12.25f, 2f,2.75f),
        new Rect(7f,14.5f, 2f,2.75f),
        new Rect(-1f,14f, 2f,2f)
    };

    private float diameter = 3f;
    private float maxOverlap = 1f;
    private int maxAttemptsPerObject = 500;
    private Rect effectiveArea;
    private readonly List<Vector2> placedPositions = new List<Vector2>();

    private float Radius => diameter * 0.5f;
    private float MinCenterDistance => diameter - maxOverlap;

    private void Awake()
    {
        BuildGeometry();
        SpawnObjects();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(spawnArea.center.x, spawnArea.center.y, 0f), new Vector3(spawnArea.width, spawnArea.height, 0f));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(effectiveArea.center.x, effectiveArea.center.y, 0f), new Vector3(effectiveArea.width, effectiveArea.height, 0f));

        Gizmos.color = Color.hotPink;
        foreach (Rect zone in safeZones)
        {
            Gizmos.DrawWireCube(new Vector3(zone.center.x, zone.center.y, 0f), new Vector3(zone.width, zone.height, 0f));
        }

        Gizmos.color = Color.cyan;
        foreach (Vector2 point in placedPositions)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x, point.y, 0f), Radius);
        }
    }

    private void BuildGeometry()
    {
        float xMin = spawnArea.xMin + Radius;
        float xMax = spawnArea.xMax - Radius;
        float yMin = spawnArea.yMin + Radius;
        float yMax = spawnArea.yMax - Radius;

        if (xMin <= xMax && yMin <= yMax)
        {
            effectiveArea = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }
        else
        {
            throw new System.Exception("Quicksand Spawner: spawn area too small.");
        }
    }

    private void SpawnObjects()
    {
        placedPositions.Clear();
        List<GameObject> spawned = new List<GameObject>();

        for (int i = 0; i < spawnCount; i++)
        {
            if (TryFindValidPosition(out Vector2 pos))
            {
                placedPositions.Add(pos);
                GameObject go = Instantiate(quickSandPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
                spawned.Add(go);
            }
            else
            {
                Debug.LogWarning($"Quicksand Spawner: only placed {spawned.Count}/{spawnCount} objects after {maxAttemptsPerObject} attempts.");
                break;
            }
        }
    }

    private bool TryFindValidPosition(out Vector2 position)
    {
        for (int attempts = 0; attempts < maxAttemptsPerObject; attempts++)
        {
            float x = Random.Range(effectiveArea.xMin, effectiveArea.xMax);
            float y = Random.Range(effectiveArea.yMin, effectiveArea.yMax);
            Vector2 candidate = new Vector2(x, y);

            if (IsValidPosition(candidate))
            {
                position = candidate;
                return true;
            }
        }

        position = Vector2.zero;
        return false;
    }

    private bool IsValidPosition(Vector2 candidate)
    {
        // Check if collider overlaps with any safezone.
        foreach (Rect zone in safeZones)
        {
            if (QuickSandOverlapsSafezone(candidate, Radius, zone))
            {
                return false;
            }
        }

        foreach (Vector2 other in placedPositions)
        {
            if (Vector2.Distance(candidate, other) < MinCenterDistance)
            {
                return false;
            }
        }

        return true;
    }

    private static bool QuickSandOverlapsSafezone(Vector2 candidatePos, float radius, Rect zone)
    {
        // Get the closest point of the safezone to the center of the quicksand position candidate.
        float closestX = Mathf.Clamp(candidatePos.x, zone.xMin, zone.xMax);
        float closestY = Mathf.Clamp(candidatePos.y, zone.yMin, zone.yMax);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        // Check if the distance between both points is smaller than the radius --> circle is not overlapping.
        return (closestPoint - candidatePos).magnitude < radius;
    }
}
