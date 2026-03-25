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

    private static void OnHealthDepleted(object sender, EventArgs args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
