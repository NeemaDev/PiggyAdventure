using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    private PlayerInteractor playerInteractor;

    private void Start()
    {
        playerInteractor = GetComponentInParent<PlayerInteractor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected.");
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log("Interactable register called");
            playerInteractor.SetNearbyInteractable(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            Debug.Log("Interactable unregister called");
            playerInteractor.ClearNearbyInteractable();
        }
    }
}
