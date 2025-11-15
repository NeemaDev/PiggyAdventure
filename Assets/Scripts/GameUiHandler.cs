using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUiHandler : MonoBehaviour
{
    public Player Player;
    public UIDocument UiDoc;

    private Label healthLabel;
    private VisualElement healthBarMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        healthLabel = UiDoc.rootVisualElement.Q<Label>("HealthLabel");
        healthBarMask = UiDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");

        Player.HealthChanged += UpdateHealth;
        var health = Player.GetHealth();
        UpdateHealth(health.current, health.max);
    }
    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        Debug.Log($"Health: {currentHealth}/{maxHealth}");
        healthLabel.text = $"{currentHealth} / {maxHealth}";
        float healthRatio = (float)currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(0, 100, healthRatio);
        healthBarMask.style.width = Length.Percent(healthPercent);
    }
}
