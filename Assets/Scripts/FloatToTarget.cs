using UnityEngine;

public class FloatToTarget : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0.2f;
    private Transform _target;

    public void SetTarget(GameObject target)
    {
        _target = target.transform;
        gameObject.transform.LookAt(target.transform);
    }

    private void Update()
    {
        if (_target is null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, _target.position) <= stopDistance)
        {
            Destroy(gameObject);
        }
    }
}
