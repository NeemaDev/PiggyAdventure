using UnityEngine;

public class ScareZone : MonoBehaviour
{
    private IDrainable target = null;
    private EnemyStats stats;

    private void Awake()
    {
        stats = GetComponentInParent<EnemyStats>();

        if(stats == null)
        {
            Debug.Log("No parent stats found!");
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
            Debug.Log("Draining..");
            target.DrainBravery(stats.braveryDrainPerSecond * Time.deltaTime);
        }
    }
}
