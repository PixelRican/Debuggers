using UnityEngine;
using UnityEngine.InputSystem;

public sealed class DebuggingToolController : MonoBehaviour
{
    private const string ActionPath = "XRI Right Interaction/Activate";

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private InputActionAsset action;
    [SerializeField] private int damage;

    private void OnEnable()
    {
        action[ActionPath].performed += OnPerformed;
    }

    private void OnDisable()
    {
        action[ActionPath].performed -= OnPerformed;
    }

    private void OnPerformed(InputAction.CallbackContext obj)
    {
        Transform transform = this.transform;
        Vector3 start = transform.position;
        Vector3 end;

        if (Physics.Raycast(start, transform.forward, out RaycastHit hit))
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<HealthController>().TakeDamage(damage);
            }

            end = hit.point;
        }
        else
        {
            end = start + transform.forward * 100.0f;
        }

        GameObject laser = Instantiate(laserPrefab, (start + end) * 0.5f, transform.rotation * Quaternion.Euler(90, 0, 0));
        Vector3 scale = laser.transform.localScale;
        scale.y = (start - end).magnitude * 0.5f;
        laser.transform.localScale = scale;
        Destroy(laser, 0.1f);
    }
}
