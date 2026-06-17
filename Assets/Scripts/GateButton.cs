using System;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private string buttonId;

    public static event Action<string,bool> OnButtonStateChanged;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log($"Collision with Player on {buttonId} Button!");
            OnButtonStateChanged?.Invoke(buttonId, true);
        }
    }
}
