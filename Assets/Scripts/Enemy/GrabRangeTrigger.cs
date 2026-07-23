using UnityEngine;

public class GrabRangeTrigger : MonoBehaviour
{
    [SerializeField] private DesertEnemy parent;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.StartGrabPlayer(collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.CancelGrabPlayer();
        }
    }
}
