using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public bool HasKey = false;
    private BridgeLock bridgeLock = null;

    public void PickUpKey()
    {
        HasKey = true;
    }

    public void OnInteract()
    {
        if(bridgeLock != null)
        {
            bridgeLock.Interact(gameObject);
        }
    }

    public void SetNearbyObject(BridgeLock nearbyLock)
    {
        bridgeLock = nearbyLock;
    }

    public void ClearNearbyObject()
    {
        bridgeLock = null;
    }
}
