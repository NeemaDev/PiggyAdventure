using UnityEngine;

public class ScareZone : MonoBehaviour
{
    private IDrainable target = null;
    private EnemyStats stats;
    private float drainRate = 0f;
    [SerializeField] private float drainRateMultiplier = 2f;

    public LayerMask WallLayer;

    private void Awake()
    {
        stats = GetComponentInParent<EnemyStats>();

        if(stats == null)
        {
            throw new System.NullReferenceException("No enemy stats found.");
        }
        else
        {
            drainRate = stats.BraveryDrain * stats.EnemyLevel;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.hotPink;
        Gizmos.DrawWireSphere(transform.position, Mathf.Round(transform.localScale.x / 2));       
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
            // Check if the target is in line-of-sight via raycast.
            Vector2 targetPosition = target.Position;
            Vector2 heading = targetPosition - (Vector2)transform.position;
            float distance = heading.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, heading.normalized, distance, WallLayer);

            if(hit.collider != null)
            {
                // Hit a wall.
                Debug.DrawRay(transform.position, heading, Color.red);
            }
            else
            {
                // Hit the player, go drain them.
                target.DrainBravery(drainRate * Time.deltaTime * drainRateMultiplier);
            }

        }
    }
}
