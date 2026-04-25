using UnityEngine;
using UnityEngine.InputSystem;

public sealed class DebuggingToolController : MonoBehaviour
{
    private const string ActionPath = "XRI Right Interaction/Activate";

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private ParticleSystem laserParticles;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private InputActionAsset action;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask ignoreMask;

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
        Transform muzzle = muzzleTransform;
        Vector3 start = muzzle.position;
        Vector3 end;

        if (Physics.Raycast(start, muzzle.forward, out RaycastHit hit, 100.0f, ~ignoreMask))
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
            end = start + muzzle.forward * 100.0f;
        }

        Instantiate(laserParticles, end, Quaternion.identity);
        GameObject laser = Instantiate(laserPrefab, (start + end) * 0.5f, muzzle.rotation * Quaternion.Euler(90, 0, 0));
        Vector3 scale = laser.transform.localScale;
        scale.y = (start - end).magnitude * 0.5f;
        laser.transform.localScale = scale;
        Destroy(laser, 0.1f);
    }
}
