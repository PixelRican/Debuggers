using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebuggerController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void OnDestroy()
    {
        GetComponent<HealthController>().HealthDepleted -= OnHealthDepleted;
    }

    private static void OnHealthDepleted(HealthController sender)
    {
        if (sender.Health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
