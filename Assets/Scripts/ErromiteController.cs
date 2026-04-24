using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class ErromiteController : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackWindup;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float minimumAttackRange;
    [SerializeField] private float maximumAttackRange;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private GameObject soulPrefab;
    private IEnumerator<WaitForSeconds> _attackCoroutine;
    private Collider _patchGeneratorCollider;
    private Collider _playerCollider;
    private NavMeshAgent _agent;
    private GameObject _target;

    protected abstract void Attack(GameObject target, int damage);

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void Start()
    {
        _patchGeneratorCollider = GameObject.FindWithTag("PatchGenerator").GetComponent<Collider>();
        _playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
    }

    private void Update()
    {
        // Target the player when detected. Otherwise, target the patch generator.
        GameObject target = playerDetectionRadius > 0.0f ? GetPlayerIfDetected() : _patchGeneratorCollider.gameObject;

        // ゴゴゴ Stare down the target menacingly ゴゴゴ
        transform.LookAt(target.transform);
        transform.Rotate(0, 90.0f, 0);
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Determine whether the erromite is currently attacking its target.
        if (_attackCoroutine is null)
        {
            // If not attacking, then determine whether its target is within attack range.
            if (distance <= minimumAttackRange)
            {
                // Target is within attack range, stop moving and attack.
                _agent.isStopped = true;
                _agent.velocity = Vector2.zero;
                _target = target;
                _attackCoroutine = GetAttackCoroutine();
                StartCoroutine(_attackCoroutine);
                return;
            }
        }
        else if (distance > maximumAttackRange)
        {
            // Target is outside of attack range, stop attacking and chase.
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
            _target = null;
            _agent.isStopped = false;
        }
        else
        {
            // Target is still within attack range, do not move yet.
            _target = target;
            return;
        }

        // Chase down target.
        _agent.SetDestination(target.transform.position);
    }

    private GameObject GetPlayerIfDetected()
    {
        int pointsHidden = 0;
        Bounds bounds = _playerCollider.bounds;
        NativeArray<Vector3> points = new NativeArray<Vector3>(8, Allocator.Temp);
        points[0] = bounds.min;
        points[1] = bounds.max;
        points[2] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        points[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        points[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        points[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        points[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        points[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

        // Determine whether any part of the player's collider is visible within detection range.
        foreach (Vector3 point in points)
        {
            Vector3 origin = transform.position;

            // Cast a ray at point and determine whether an obstacle is in the way.
            if (Physics.Raycast(origin, point - origin, out RaycastHit hit, playerDetectionRadius) &&
                Vector3.Distance(origin, hit.point) < Vector3.Distance(origin, point))
            {
                // Count the number of points that are blocked by obstacles.
                pointsHidden++;
            }
        }

        // Manually free the memory allocated.
        points.Dispose();

        // Target the player when they are within line of sight; otherwise, target the patch generator.
        return (pointsHidden < points.Length ? _playerCollider : _patchGeneratorCollider).gameObject;
    }

    private void OnDestroy()
    {
        GetComponent<HealthController>().HealthDepleted -= OnHealthDepleted;
    }

    private void OnHealthDepleted(HealthController sender)
    {
        if (sender.Health == 0)
        {
            GameObject obj = Instantiate(soulPrefab, transform.position, Quaternion.identity);
            FloatToTarget mover = obj.GetComponent<FloatToTarget>();
            mover.target = _patchGeneratorCollider.gameObject.transform;
            Destroy(gameObject);
        }
    }

    private IEnumerator<WaitForSeconds> GetAttackCoroutine()
    {
        yield return new WaitForSeconds(attackWindup);

        WaitForSeconds cooldownTime = new WaitForSeconds(attackCooldown);

        while (true)
        {
            Attack(_target, attackDamage);
            yield return cooldownTime;
        }
    }
}
