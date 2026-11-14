using UnityEngine;

public class TreasureChestInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject treasureLidOpen;
    [SerializeField] private GameObject treasureLidClosed;

    public void Interact()
    {
        Debug.Log("Switching treasure lid states");
        SetLidState(true, false);
    }

    private void Start()
    {
        SetLidState(false, true);
    }

    private void SetLidState(bool openLidActive, bool closedLidActive)
    {
        if (treasureLidClosed?.activeSelf != closedLidActive)
        {
            treasureLidClosed.SetActive(closedLidActive);
        }
        else
        {
            Debug.Log("Could not change Closed Lid state. Reference is null or its already in the passed state");
        }

        if (treasureLidOpen?.activeSelf != openLidActive)
        {
            treasureLidOpen.SetActive(openLidActive);
        }
        else
        {
            Debug.Log("Could not change Open Lid state. Reference is null or its already in the passed state");
        }
    }

}
