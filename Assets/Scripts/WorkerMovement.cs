using System;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MonoBehaviour
{
    [SerializeField] private string preferredTargetTag;
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

    private void OnHealthDepleted(object sender, EventArgs args)
    {
        Destroy(gameObject);
    }
}
