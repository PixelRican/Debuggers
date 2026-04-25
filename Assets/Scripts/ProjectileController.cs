using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    private int _damage;

    public void SetTarget(GameObject target, int damage)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0.0f;
        GetComponent<Rigidbody>().linearVelocity = direction.normalized * speed;
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out HealthController health))
            {
                health.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}
