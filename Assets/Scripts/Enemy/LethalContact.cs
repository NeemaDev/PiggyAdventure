using UnityEngine;

public class LethalContact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isKillable = collision.gameObject.TryGetComponent(out IKillable target);

        if (isKillable)
        {
            target.Die();
        }
    }
}
