using Unity.VisualScripting;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour, IDrainable, IKillable
{
    PlayerStats stats;

    public void Die()
    {
        Destroy(gameObject, 0.5f);
    }

    public void DrainBravery(float amount)
    {
        if(stats != null)
        {
            stats.bravery -= amount;
            Debug.Log($"Bravery drained by {amount}. New Bravery: {stats.bravery}");

            if(stats.bravery <= 0)
            {
                Debug.Log("Bravery drained. You Dead.");
                Die();
            }
        }
    }

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();

        if(stats == null)
        {
            throw new System.NullReferenceException("No player stats found.");
        }
    }

}
