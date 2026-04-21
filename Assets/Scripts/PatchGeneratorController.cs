using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnHealthDepleted(HealthController sender)
    {
        if (sender.Health == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
