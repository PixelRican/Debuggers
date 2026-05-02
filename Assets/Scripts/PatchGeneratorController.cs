using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the behavior of the patch generator
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class PatchGeneratorController : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    [SerializeField] private ErromiteWaveController erromiteWaveController;
    [SerializeField] private ParticleSystem energyParticles;
    [SerializeField] private GameObject destructionExplosionEffect;
    [SerializeField] private GameObject victoryExplosionEffect;
    [SerializeField] private float energyParticleBuildUp;

    /// <summary>
    /// When the patch generator is enabled, add members to controllers
    /// </summary>
    private void OnEnable()
    {
        healthController.HealthChanged += OnHealthChanged;
        erromiteWaveController.WaveCompleted += OnErromiteWaveCompleted;
        erromiteWaveController.GameCompleted += OnGameCompleted;
    }  // End of OnEnable

    /// <summary>
    /// When the patch generator is disable, remove members from controllers
    /// </summary>
    private void OnDisable()
    {
        healthController.HealthChanged -= OnHealthChanged;
        erromiteWaveController.WaveCompleted -= OnErromiteWaveCompleted;
        erromiteWaveController.GameCompleted -= OnGameCompleted;
    }  // End of OnDisable

    /// <summary>
    /// Checks if patch generator was destroyed upon taking damage
    /// </summary>
    /// <param name="sender">Container of patch generator's current health</param>
    private void OnHealthChanged(HealthController sender)
    {
        // If patch generator health is zero, create explosion particle effect and disable the patch generator
        if (sender.Health == 0)
        {
            CreateExplosion(destructionExplosionEffect);
            gameObject.active = false;
        }
    }  // End of OnHealthChanged

    /// <summary>
    /// When a wave of erromites is completed, reset health of patch generator and emit more particles from generator
    /// </summary>
    /// <param name="sender">Not used</param>
    private void OnErromiteWaveCompleted(ErromiteWaveController sender)
    {
        healthController.Reset();
        ParticleSystem.EmissionModule emission = energyParticles.emission;
        emission.rateOverTimeMultiplier += energyParticleBuildUp;
    }  // End of OnErromiteWaveCompleted

    /// <summary>
    /// Executes when the game is won, creates the patch generator explosion that wipes out the erromites
    /// </summary>
    /// <param name="sender">Not used</param>
    private void OnGameCompleted(ErromiteWaveController sender)
    {
        CreateExplosion(victoryExplosionEffect);
    }  // End of OnGameCompleted

    /// <summary>
    /// Creates a particle effect explosion and then restarts the game
    /// </summary>
    /// <param name="effect">Explosion particle effect</param>
    private void CreateExplosion(GameObject effect)
    {
        GameObject explosion = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(explosion, 5.0f);
        Invoke(nameof(RestartGame), 5.0f);
    }  // End of CreateExplosion

    /// <summary>
    /// Restarts the game to the default state
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }  // End of RestartGame
}  // End of PatchGeneratorController