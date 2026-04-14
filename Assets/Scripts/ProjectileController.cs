using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rigidbody;
    private int _damage;

    public void SetTarget(GameObject target, int damage)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0.0f;
        _rigidbody.linearVelocity = direction.normalized * speed;
        _damage = damage;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out HealthController health))
        {
            health.TakeDamage(_damage);
        }

        Debug.Log(other.gameObject);
        Destroy(gameObject);
    }
}
