using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;

    public event Action OnDamaged;
    public event Action OnDeath;
    
    public int CurrentHealth { get => currentHealth; }
    public int MaxHealth { get => maxHealth; }

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Curenthealth initialized: {currentHealth}");        

    }

    public void ChangeHealth(int amount)
    {
        Debug.Log($"Changing health {currentHealth} by {amount}");
        currentHealth += amount;

        Debug.Log($"CurrentHealth {currentHealth}");
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            Debug.Log($"Invoking Death.");
            OnDeath?.Invoke();
        }else if(amount < 0)
        {
            Debug.Log($"Invoking Damage.");
            OnDamaged?.Invoke();
        }        
    }
}
