using System;
using UnityEngine;

/// <summary>
/// Controls health of the player, enemies, and patch generator
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    /// <summary>
    /// Returns max health of object
    /// </summary>
    public int MaxHealth
    {
        get => maxHealth;
    }  // End of MaxHealth

    /// <summary>
    /// Returns current health of object
    /// </summary>
    public int Health
    {
        get => health;
    }  // End of Health

    public event Action<HealthController> HealthChanged;

    /// <summary>
    /// Resets object health to its max health
    /// </summary>
    public void Reset()
    {
        health = maxHealth;
        HealthChanged?.Invoke(this);
    }  // End of Reset

    /// <summary>
    /// Reduces current health by damage taken
    /// </summary>
    /// <param name="damage">Damage taken</param>
    public void TakeDamage(int damage)
    {
        // Reduce health, but not below zero
        health = Math.Max(health - damage, 0);
        HealthChanged?.Invoke(this);
    }  // End of TakeDamage

    /// <summary>
    /// Resets health upon creation of object
    /// </summary>
    private void Awake()
    {
        Reset();
    }  // End of Awake
}  // End of HealthController
