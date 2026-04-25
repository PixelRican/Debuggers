using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PatchGeneratorController : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    [SerializeField] private ErromiteWaveController erromiteWaveController;
    [SerializeField] private ParticleSystem energyParticles;
    [SerializeField] private GameObject destructionExplosionEffect;
    [SerializeField] private GameObject victoryExplosionEffect;
    [SerializeField] private float energyParticleBuildUp;

    private void OnEnable()
    {
        healthController.HealthChanged += OnHealthChanged;
        erromiteWaveController.WaveCompleted += OnErromiteWaveCompleted;
        erromiteWaveController.GameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        healthController.HealthChanged -= OnHealthChanged;
        erromiteWaveController.WaveCompleted -= OnErromiteWaveCompleted;
        erromiteWaveController.GameCompleted -= OnGameCompleted;
    }

    private void OnHealthChanged(HealthController sender)
    {
        if (sender.Health == 0)
        {
            CreateExplosion(destructionExplosionEffect);
            gameObject.active = false;
        }
    }

    private void OnErromiteWaveCompleted(ErromiteWaveController sender)
    {
        healthController.Reset();
        ParticleSystem.EmissionModule emission = energyParticles.emission;
        emission.rateOverTimeMultiplier += energyParticleBuildUp;
    }

    private void OnGameCompleted(ErromiteWaveController sender)
    {
        CreateExplosion(victoryExplosionEffect);
    }

    private void CreateExplosion(GameObject effect)
    {
        GameObject explosion = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(explosion, 5.0f);
        Invoke(nameof(RestartGame), 5.0f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
