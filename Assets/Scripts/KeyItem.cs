using UnityEngine;

public class KeyItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
            PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
            if (interactor != null)
            {
                interactor.PickUpKey();
                Destroy(gameObject); // Destory the key
            }
    }
}
