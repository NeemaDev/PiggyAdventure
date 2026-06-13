using System;
using UnityEngine;

public class BridgeLock : MonoBehaviour
{
    public GameObject Bridge;

    public void Interact(GameObject player)
    {
        PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

        if (interactor != null && interactor.HasKey)
        {
            Unlock();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.SetNearbyObject(this);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.ClearNearbyObject();
        }
    }

    private void Unlock()
    {
        if (Bridge != null)
        {
            Bridge.SetActive(true);
        }

        var interactors = FindObjectsByType<PlayerInteractor>(FindObjectsSortMode.None);
        if(interactors.Length > 0)
        {
            PlayerInteractor interactor = interactors[0];
            interactor.ClearNearbyObject();
        }

        Destroy(gameObject);
    }
}
