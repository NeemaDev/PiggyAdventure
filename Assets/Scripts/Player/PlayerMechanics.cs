using Unity.VisualScripting;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour, IDrainable, IKillable
{
    PlayerStats stats;

    public void Die()
    {
        Destroy(gameObject,0.05f);
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

    public void RestoreBravery(float amount)
    {
        if(stats != null && stats.bravery < stats.maxBravery)
        {
            stats.bravery += amount;
            Debug.Log($"Bravery restored by {amount}. New Bravery: {stats.bravery}");
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
