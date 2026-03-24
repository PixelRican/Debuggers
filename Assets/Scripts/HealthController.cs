using System;
using UnityEngine;

public sealed class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    public int MaxHealth
    {
        get => maxHealth;
    }

    public int Health
    {
        get => health;
    }

    public event EventHandler<EventArgs> HealthDepleted;

    public void TakeDamage(int damage)
    {
        int healthRemaining = health - damage;

        if (healthRemaining > 0)
        {
            health = healthRemaining;
        }
        else
        {
            health = 0;
            HealthDepleted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Start()
    {
        health = maxHealth;
    }
}
