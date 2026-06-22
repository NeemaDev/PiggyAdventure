using System;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private string buttonId;
     private bool state = false;
     private SpriteRenderer spriteRenderer;

    public static event Action<string,bool> OnButtonStateChanged;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            state = !state;
            OnButtonStateChanged?.Invoke(buttonId, state);

            // Change Color.
            if (state)
            {
                ChangeColor(Color.green);
            }
            else
            {
                ChangeColor(Color.red);
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void ChangeColor(Color newColor)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }
}
