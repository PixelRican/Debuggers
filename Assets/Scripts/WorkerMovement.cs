using System;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MonoBehaviour
{
    NavMeshAgent enemy;
    GameObject destination;

    private void Awake()
    {
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        destination = GameObject.FindWithTag("PatchGenerator");
    }

    private void Update()
    {
        enemy.SetDestination(destination.transform.position);
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
