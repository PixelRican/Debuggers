using UnityEngine;

public sealed class MeleeErromiteController : ErromiteController
{
    [SerializeField] private ParticleSystem attackParticles;

    protected override void Attack(GameObject target, int damage)
    {
        Instantiate(attackParticles, target.transform.position, Quaternion.identity);
        target.GetComponent<HealthController>().TakeDamage(damage);
    }
}
