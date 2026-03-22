using UnityEngine;
using UnityEngine.AI;

public class SoldierMovement : MonoBehaviour
{
    NavMeshAgent enemy;
    GameObject destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        destination = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(destination.transform.position);
    }
}
