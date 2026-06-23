using UnityEngine;

public class PlayerMechanics : MonoBehaviour, IDrainable, IKillable
{
    PlayerStats stats;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void DrainBravery(float amount)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();

        if(stats == null)
        {
            Debug.Log("Error while loading player stats. No stats available.");
        }
    }

}
