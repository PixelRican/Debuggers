using System;
using UnityEngine;

public sealed class PatchGeneratorController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
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
