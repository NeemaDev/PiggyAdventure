using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField]private float healAmount = 10f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDrainable potentialTarget = collision.GetComponent<IDrainable>();

        if (potentialTarget != null)
        {
            potentialTarget.RestoreBravery(healAmount);
            Destroy(gameObject, 0.1f);
        }
    }
}
