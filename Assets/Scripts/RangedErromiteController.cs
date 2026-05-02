using UnityEngine;

/// <summary>
/// Controls ranged enemy attacks
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class RangedErromiteController : ErromiteController
{
    [SerializeField] private GameObject projectilePrefab;

    /// <summary>
    /// Handles the creation,  movement, and destruction of enemy projectiles
    /// </summary>
    /// <param name="target">Target the projectile is fired at</param>
    /// <param name="damage">Damage of projectile on collision</param>
    protected override void Attack(GameObject target, int damage)
    {
        // Create projectile slightly in front of enemy firing it
        GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(0.0f, 0.25f, 0.0f), Quaternion.identity);

        // Set target of projectile
        projectile.GetComponent<ProjectileController>().SetTarget(target, damage);

        // Destroy projectile after 10 seconds
        Destroy(projectile, 10.0f);
    }  // End of Attack
}  // End of RangedErromiteController