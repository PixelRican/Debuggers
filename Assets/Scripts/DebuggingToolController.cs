using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the behavior of the debugging tool, the weapon the player uses to attack enemies
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class DebuggingToolController : MonoBehaviour
{
    private const string ActionPath = "XRI Right Interaction/Activate";

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private ParticleSystem laserParticles;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private InputActionAsset action;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask ignoreMask;

    /// <summary>
    /// When the debugging tool is enabled, add members to input action controller
    /// </summary>
    private void OnEnable()
    {
        action[ActionPath].performed += OnPerformed;
    }  // End of OnEnable

    /// <summary>
    /// When the debugging tool is disable, remove members from input action controller
    /// </summary>
    private void OnDisable()
    {
        action[ActionPath].performed -= OnPerformed;
    }  // End of OnDisable

    /// <summary>
    /// Handles the firing of the debugging tool
    /// </summary>
    /// <param name="obj">Not used</param>
    private void OnPerformed(InputAction.CallbackContext obj)
    {
        Transform muzzle = muzzleTransform;
        Vector3 start = muzzle.position;
        Vector3 end;

        // Check if laser hit anything
        if (Physics.Raycast(start, muzzle.forward, out RaycastHit hit, 100.0f, ~ignoreMask))
        {
            GameObject target = hit.collider.gameObject;

            // If what the laser hit was an enemy, make that enemy take damage
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<HealthController>().TakeDamage(damage);
            }

            // Set endpoint of laser at thing it hit
            end = hit.point;
        }

        // If the laser does not hit anything, end it after 100 units of length
        else
        {
            end = start + muzzle.forward * 100.0f;
        }

        // Create laser object with particle effect, then destroy the laser after 0.1 seconds
        Instantiate(laserParticles, end, Quaternion.identity);
        GameObject laser = Instantiate(laserPrefab, (start + end) * 0.5f, muzzle.rotation * Quaternion.Euler(90, 0, 0));
        Vector3 scale = laser.transform.localScale;
        scale.y = (start - end).magnitude * 0.5f;
        laser.transform.localScale = scale;
        Destroy(laser, 0.1f);
    }  // End of OnPerformed
}  // End of DebuggingToolController