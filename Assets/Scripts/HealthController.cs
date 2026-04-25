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

    public event Action<HealthController> HealthChanged;

    public void Reset()
    {
        health = maxHealth;
        HealthChanged?.Invoke(this);
    }

    public void TakeDamage(int damage)
    {
        health = Math.Max(health - damage, 0);
        HealthChanged?.Invoke(this);
    }

    private void Awake()
    {
        Reset();
    }
}
