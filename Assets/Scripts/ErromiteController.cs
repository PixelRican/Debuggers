using System;
using UnityEngine;
using UnityEngine.AI;

public class ErromiteController : MonoBehaviour
{
    [SerializeField] private string preferredTargetTag;
    [SerializeField] private int damage;
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
        agent.SetDestination(target.position);
    }

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
