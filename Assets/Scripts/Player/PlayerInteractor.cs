using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private PlayerStats stats;
    private IInteractable nearbyInteractable;

    public void PickUpKey()
    {
        if (stats != null)
        {
            stats.hasKey = true;
        }
    }

    public void OnInteract()
    {
        Debug.Log("Try call Interact on nearbyinteractable");
        nearbyInteractable?.Interact();
    }
    
    public void SetNearbyInteractable(IInteractable interactable) => nearbyInteractable = interactable;
    public void ClearNearbyInteractable() => nearbyInteractable = null;

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();

        if (stats == null)
        {
            Debug.Log("No parent stats found!");
        }
    }


}
