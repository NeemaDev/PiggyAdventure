using System;
using UnityEngine;

public class ScareZone : MonoBehaviour
{
    private IDrainable target = null;
    private EnemyStats stats;
    private float drainRate = 0f;
    [SerializeField] private float drainRateMultiplier = 2f;

    private void Awake()
    {
        stats = GetComponentInParent<EnemyStats>();

        if(stats == null)
        {
            throw new System.NullReferenceException("No enemy stats found.");
        }
        else
        {
            drainRate = stats.braveryDrain * stats.enemyLevel;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDrainable potentialTarget = collision.GetComponent<IDrainable>();

        if(potentialTarget != null)
        {
            target = potentialTarget;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(target != null  && collision.GetComponent<IDrainable>() == target)
        {
            target = null;
        }
    }

    private void Update()
    {
        if(target != null)
        {
            target.DrainBravery(drainRate * Time.deltaTime * drainRateMultiplier);
        }
    }
}
