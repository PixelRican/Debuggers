using System;
using UnityEngine;
using UnityEngine.AI;

public sealed class ErromiteController : MonoBehaviour
{
    [SerializeField] private string initialTargetTag;
    [SerializeField] private string preferredTargetTag;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    private NavMeshAgent agent;
    private Transform preferredTarget;
    private Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void Start()
    {
        target = GameObject.FindWithTag(initialTargetTag).transform;
        preferredTarget = GameObject.FindWithTag(preferredTargetTag).transform;
    }

    private void Update()
    {
        // Check for line of sight with preferred target.
        if (target != preferredTarget)
        {
            Vector3 origin = transform.position;
            Vector3 direction = preferredTarget.position - origin;

            // Chase after preferred target when found.
            if (Physics.Raycast(origin, direction, out RaycastHit hit) && hit.transform == preferredTarget)
            {
                target = preferredTarget;
            }
            
            Debug.DrawRay(origin, direction, Color.yellow);
            Debug.Log(hit.transform.gameObject);
        }

        // ゴゴゴ Stare down the target menacingly ゴゴゴ
        transform.LookAt(target);

        // Check distance between self and target.
        if (Vector3.Distance(transform.position, target.transform.position) > range)
        {
            // Target is not close enough, keep moving.
            agent.SetDestination(target.position);
        }
        else
        {
            // Target is within range, stop to attack.
            agent.ResetPath();
        }
    }

    private void OnDestroy()
    {
        GetComponent<HealthController>().HealthDepleted -= OnHealthDepleted;
    }

    private void OnHealthDepleted(object sender, EventArgs args)
    {
        Destroy(gameObject);
    }
}
