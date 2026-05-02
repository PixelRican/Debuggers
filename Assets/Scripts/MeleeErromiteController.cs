using UnityEngine;

/// <summary>
/// Controls melee enemy attacks
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class MeleeErromiteController : ErromiteController
{
    [SerializeField] private ParticleSystem attackParticles;

    /// <summary>
    /// Handles particle effects and damage calculations of attack
    /// </summary>
    /// <param name="target">Object that is being attacked</param>
    /// <param name="damage">Damage of attack</param>
    protected override void Attack(GameObject target, int damage)
    {
        Instantiate(attackParticles, target.transform.position, Quaternion.identity);
        target.GetComponent<HealthController>().TakeDamage(damage);
    }  // End of Attack
}  // End of MeleeErromiteController