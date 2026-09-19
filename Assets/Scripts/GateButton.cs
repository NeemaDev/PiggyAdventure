using System;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private string buttonId;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    private bool state = false;
    private SpriteRenderer spriteRenderer;

    public static event Action<string, bool> OnButtonStateChanged;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            state = !state;
            OnButtonStateChanged?.Invoke(buttonId, state);

            ChangeSprite();
        }
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void ChangeSprite()
    {
        if (state)
        {
            spriteRenderer.sprite = activeSprite;
        }
        else
        {
            spriteRenderer.sprite = inactiveSprite;
        }
    }
}
