using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ErromiteController : MonoBehaviour
{
    [SerializeField] private string preferredTargetTag;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    private Transform target;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(preferredTargetTag).transform;
    }

    private void Update()
    {
        transform.LookAt(target);
        if (Vector3.Distance(transform.position, target.transform.position) > range)  // If range is zero, always move to melee
        {
            agent.SetDestination(target.position);
        }
        else if (agent.Raycast(target.position, out NavMeshHit hit))  // Check for line of sight
        {
            agent.SetDestination(target.position);  // Player not visible, keep moving
        }
        else  // If can see the player and within range, stop to attack
        {
            agent.ResetPath();
            // StartCoroutine(RangedAttack());
        }
    }

    /*public IEnumerator RangedAttack()
    {
        yield return new WaitForSeconds(0.5f);
    }*/

    private void OnDestroy()
    {
        GetComponent<HealthController>().HealthDepleted -= OnHealthDepleted;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject victim = other.gameObject;

        if (victim.CompareTag(preferredTargetTag))
        {
            victim.GetComponent<HealthController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnHealthDepleted(object sender, EventArgs args)
    {
        Destroy(gameObject);
    }
}
