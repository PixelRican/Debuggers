using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PatchGeneratorController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void OnDisable()
    {
        GetComponent<HealthController>().HealthDepleted -= OnHealthDepleted;
    }

    private void OnHealthDepleted(HealthController sender)
    {
        if (sender.Health == 0)
        {
            enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
