using UnityEngine;
using UnityEngine.SceneManagement;

public class DebuggerController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<HealthController>().HealthDepleted += OnHealthDepleted;
    }

    private void OnDisable()
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
