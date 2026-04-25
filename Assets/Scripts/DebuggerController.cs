using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public class DebuggerController : MonoBehaviour
{
    [SerializeField] private XROrigin origin;
    [SerializeField] private HealthController healthController;
    [SerializeField] private Transform spawnTransform;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        TeleportToSpawn();
    }

    private void OnEnable()
    {
        healthController.HealthDepleted += OnHealthDepleted;
    }

    private void OnDisable()
    {
        healthController.HealthDepleted -= OnHealthDepleted;
    }

    private void OnHealthDepleted(HealthController sender)
    {
        if (sender.Health == 0)
        {
            sender.Reset();
            TeleportToSpawn();
        }
    }

    private void TeleportToSpawn()
    {
        Transform originTransform = origin.transform;
        Vector3 originPosition = originTransform.position;
        Vector3 headOffset = origin.Camera.transform.position - originPosition;
        originTransform.position = spawnTransform.position - headOffset;
    }
}
