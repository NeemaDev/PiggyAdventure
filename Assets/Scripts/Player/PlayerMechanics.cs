using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour, IDrainable, IKillable
{
    PlayerStats stats;

    public Vector2 Position => transform.position;

    public void Die()
    {
        if (stats != null && !stats.isGodMode)
        {
            Destroy(gameObject, 0.05f);
        }
    }

    public void DrainBravery(float amount)
    {
        if (stats != null && !stats.isGodMode)
        {
            stats.bravery -= amount;
            Debug.Log($"Bravery drained by {amount}. New Bravery: {stats.bravery}");

            if (stats.bravery <= 0)
            {
                Debug.Log("Bravery drained. You Dead.");
                Die();
            }
        }
    }

    public void RestoreBravery(float amount)
    {
        if (stats != null && stats.bravery < stats.maxBravery)
        {
            stats.bravery = Mathf.Min(stats.bravery + amount, stats.maxBravery);
            Debug.Log($"Bravery restored by {amount}. New Bravery: {stats.bravery}");
        }
    }

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();

        if (stats == null)
        {
            throw new System.NullReferenceException("No player stats found.");
        }
    }

}
