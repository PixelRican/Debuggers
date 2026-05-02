using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the behavior of the erromites
/// </summary>
/// <author>Roberto Mercado & Jack Wooldridge</author>
/// <remarks>Commented by Roberto Mercado & Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public abstract class ErromiteController : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackWindup;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float minimumAttackRange;
    [SerializeField] private float maximumAttackRange;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private GameObject energyOrbPrefab;
    [SerializeField] private LayerMask ignoreMask;
    private IEnumerator<WaitForSeconds> _attackCoroutine;
    private Collider _patchGeneratorCollider;
    private Collider _playerCollider;
    private NavMeshAgent _agent;
    private GameObject _target;

    protected abstract void Attack(GameObject target, int damage);

    /// <summary>
    /// create erromite, add methods to controller
    /// </summary>
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthController>().HealthChanged += OnHealthChanged;
    }  // End of Awake

    /// <summary>
    /// Get colliders for the player and patch generator
    /// </summary>
    private void Start()
    {
        _patchGeneratorCollider = GameObject.FindWithTag("PatchGenerator").GetComponent<Collider>();
        _playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
    }  // End of Start

    /// <summary>
    /// Handles movement of erromite and attack range
    /// </summary>
    private void Update()
    {
        // Target the player when detected. Otherwise, target the patch generator.
        GameObject target = GetTarget();

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
    }  // End of Update

    /// <summary>
    /// Determines whether the erromite will target the player or the patch generator
    /// </summary>
    /// <returns></returns>
    private GameObject GetTarget()
    {
        // If player is too far away from erromite, target the patch generator
        if (Vector3.Distance(transform.position, _playerCollider.transform.position) > playerDetectionRadius)
        {
            return _patchGeneratorCollider.gameObject;
        }

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
            if (Physics.Raycast(origin, point - origin, out RaycastHit hit, playerDetectionRadius, ~ignoreMask) &&
                (Vector3.Distance(origin, hit.point) < Vector3.Distance(origin, point)))
            {
                // Count the number of points that are blocked by obstacles.
                pointsHidden++;
            }
        }

        // Manually free the memory allocated.
        points.Dispose();

        // Target the player when they are within line of sight; otherwise, target the patch generator.
        if (pointsHidden < points.Length)
        {
            return _playerCollider.gameObject;
        }

        return _patchGeneratorCollider.gameObject;
    }  // End of GetTarget

    /// <summary>
    /// When the erromite is destroyed, remove members from controllers
    /// </summary>
    private void OnDestroy()
    {
        GetComponent<HealthController>().HealthChanged -= OnHealthChanged;
    }  // End of OnDestroy

    /// <summary>
    /// Checks if erromite was killed after taking damage
    /// </summary>
    /// <param name="sender">Container of erroite's current health</param>
    private void OnHealthChanged(HealthController sender)
    {
        // If erromite health has reached zero, create erromite soul, then destroy the erromit object
        if (sender.Health == 0)
        {
            GameObject obj = Instantiate(energyOrbPrefab, transform.position, Quaternion.identity);
            FloatToTarget mover = obj.GetComponent<FloatToTarget>();
            mover.SetTarget(GameObject.FindGameObjectWithTag("EnergyOrb"));  // "EnergyOrb" refers to energy orb of patch generator
            Destroy(gameObject);
        }
    }  // End of OnHealthChanged

    /// <summary>
    /// Handles attack windups and attack cooldowns of erromites
    /// </summary>
    private IEnumerator<WaitForSeconds> GetAttackCoroutine()
    {
        // Pause for windup
        yield return new WaitForSeconds(attackWindup);

        WaitForSeconds cooldownTime = new WaitForSeconds(attackCooldown);

        // Repeatedly attack between attack cooldowns
        while (true)
        {
            Attack(_target, attackDamage);
            yield return cooldownTime;
        }
    }  // End of GetAttackCoroutine
}  // End of ErromiteController