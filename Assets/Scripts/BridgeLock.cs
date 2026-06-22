using System;
using UnityEngine;

public class BridgeLock : MonoBehaviour, IInteractable
{
    public GameObject Bridge;

    public void Interact()
    {
        Unlock();
    }

    private void Unlock()
    {
        if (Bridge != null)
        {
            Bridge.SetActive(true);
        }
        else
        {
            Debug.Log("Can't activate bridge. Bridge is null.");
        }

        var parent = transform.parent.gameObject;
        if(parent != null)
        {
            Destroy(parent);
        }
        else
        {
            Debug.Log("Can't destroy Bridge Lock. Parent is null.");
        }
    }
}
