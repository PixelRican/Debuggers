using UnityEngine;

/// <summary>
/// Controls projectiles
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ParticleSystem collisionParticles;
    [SerializeField] private float speed;
    private int _damage;

    /// <summary>
    /// Sets the velocity of projectile
    /// </summary>
    /// <param name="target">Target the projectile is fired at</param>
    /// <param name="damage">Damage of projectile on collision</param>
    public void SetTarget(GameObject target, int damage)
    {
        Vector3 direction = target.transform.position - transform.position;
        GetComponent<Rigidbody>().linearVelocity = direction.normalized * speed;
        _damage = damage;
    }  // End of SetTarget

    /// <summary>
    /// Handles collisions of projectiles
    /// </summary>
    /// <param name="other">The object the projectile collided with</param>
    private void OnCollisionEnter(Collision other)
    {
        // If the other object has health, make it take damage
        if (other.gameObject.TryGetComponent(out HealthController health))
        {
            health.TakeDamage(_damage);
        }

        // Create particle effects and destroy projectile
        Instantiate(collisionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }  // End of OnCollisionEnter
}  // End of ProjectileController
