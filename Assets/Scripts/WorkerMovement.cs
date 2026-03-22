using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MonoBehaviour
{
    NavMeshAgent enemy;
    GameObject destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        destination = GameObject.FindWithTag("PatchGenerator");
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(destination.transform.position);
    }
}
