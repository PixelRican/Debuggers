using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ParticleSystem collisionParticles;
    [SerializeField] private float speed;
    private int _damage;

    public void SetTarget(GameObject target, int damage)
    {
        Vector3 direction = target.transform.position - transform.position;
        GetComponent<Rigidbody>().linearVelocity = direction.normalized * speed;
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out HealthController health))
        {
            health.TakeDamage(_damage);
        }

        Instantiate(collisionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
