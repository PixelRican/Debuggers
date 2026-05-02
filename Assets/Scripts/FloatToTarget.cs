using UnityEngine;

/// <summary>
/// Controls the movement of erroite souls
/// </summary>
/// <author>Roberto Mercado & Jack Wooldridge</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public class FloatToTarget : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0.2f;
    private Transform _target;

    /// <summary>
    /// Set target of erromite soul
    /// </summary>
    /// <param name="target"> End destination of object </param>
    public void SetTarget(GameObject target)
    {
        _target = target.transform;
        gameObject.transform.LookAt(target.transform);
    }  // End of SetTarget

    /// <summary>
    /// Move erromite soul towards target
    /// </summary>
    private void Update()
    {
        if (_target is null) return;

        // Move soul
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            speed * Time.deltaTime
        );

        // Once soul has reached target, destroy it
        if (Vector3.Distance(transform.position, _target.position) <= stopDistance)
        {
            Destroy(gameObject);
        }
    }  // End of Update
}  // End of FloatToTarget
