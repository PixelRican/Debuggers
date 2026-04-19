using UnityEngine;

public class FloatToTarget : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0.2f;

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            Destroy(gameObject);
        }
    }
}