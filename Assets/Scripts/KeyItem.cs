using UnityEngine;

public class KeyItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Colluision detected.");
            PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
            if(interactor != null)
            {
                interactor.PickUpKey();
                Destroy(gameObject); // Destory the key
            }
        }
    }
}
