using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of the erromite wave spawner
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public class ErromiteWaveController : MonoBehaviour
{
    [SerializeField] private float waveDowntime;
    [SerializeField] private float spawnRate;
    [SerializeField] private WaveEntry[] waves;
    private HealthController _patchGeneratorHealth;
    private IEnumerator<object> _spawnCoroutine;
    private WaveEntry _currentWave;
    private int _waveIndex;
    private int _spawnIndex;
    private int _erromitesRemaining;
    private int _erromiteWaveSize;

    /// <summary>
    /// Starts the erromite spawn wave
    /// </summary>
    public bool Started
    {
        get => _spawnCoroutine != null;
    }  // End of Started

    /// <summary>
    /// Returns the number of erromites alive remaing
    /// </summary>
    public int ErromitesRemaining
    {
        get => _erromitesRemaining;
    }  // End of ErromitesRemaing

    /// <summary>
    /// Returns the number of erromites in the current wave
    /// </summary>
    public int ErromiteWaveSize
    {
        get => _erromiteWaveSize;
    }  // End of ErromiteWaveSize

    public event Action<ErromiteWaveController> WaveInitiated;

    public event Action<ErromiteWaveController> WaveCompleted;

    public event Action<ErromiteWaveController> ErromiteDestroyed;

    public event Action<ErromiteWaveController> GameCompleted;

    /// <summary>
    /// When the erromite spawner is enabled, initialize class
    /// </summary>
    private void OnEnable()
    {
        _currentWave.Spawns = Array.Empty<SpawnEntry>();
        _waveIndex = -1;
        _spawnIndex = -1;
        _patchGeneratorHealth = GameObject.FindWithTag("PatchGenerator").GetComponent<HealthController>();
        _patchGeneratorHealth.HealthChanged += OnPatchGeneratorDamaged;
        _spawnCoroutine = GetSpawnCoroutine();
        StartCoroutine(_spawnCoroutine);
    }  // End OnEnable

    /// <summary>
    /// When the erromite spawner is disable, reset class
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
        _patchGeneratorHealth.HealthChanged -= OnPatchGeneratorDamaged;
        _patchGeneratorHealth = null;
        _erromitesRemaining = 0;
        _erromiteWaveSize = 0;
    }  // End of OnDisable

    /// <summary>
    /// Checks if the patch generator still has help and should keep spawning erromites
    /// </summary>
    /// <param name="sender">Container of patch generator's current health</param>
    private void OnPatchGeneratorDamaged(HealthController sender)
    {
        // If the patch generator has run out of health, stop spawning erromites
        enabled = sender.Health > 0;
    }  // End of OnPatchGeneratorDamaged

    /// <summary>
    /// Checks if an erromite has been killed and if the wave has been completed
    /// </summary>
    /// <param name="sender">Container of erromite's current health</param>
    private void OnErromiteDamaged(HealthController sender)
    {
        // If an erromite has lost all its health, remove it from the remaing erromite count
        if (sender.Health == 0)
        {
            int erromitesRemaining = --_erromitesRemaining;
            sender.HealthChanged -= OnErromiteDamaged;
            ErromiteDestroyed?.Invoke(this);

            // If no erromites remain, progress the wave
            if (erromitesRemaining == 0)
            {
                WaveCompleted?.Invoke(this);
            }
        }
    }  // End of OnErromiteDamaged

    /// <summary>
    /// Handles the erromite spawning routine
    /// </summary>
    private IEnumerator<object> GetSpawnCoroutine()
    {
        WaitForSeconds waveTime = new WaitForSeconds(waveDowntime);
        WaitForSeconds spawnTime = new WaitForSeconds(spawnRate);
        WaitUntil nextWaveCondition = new WaitUntil(() => _erromitesRemaining == 0);

        while (true)
        {
            WaveEntry wave = _currentWave;
            int nextSpawnIndex = _spawnIndex + 1;

            // Check if all erromite of a wave have been spawned to move onto the next wave
            if (nextSpawnIndex >= _currentWave.Spawns.Length)
            {
                yield return nextWaveCondition;

                int nextWaveIndex = _waveIndex + 1;

                // Check if all waves have been beaten
                if (nextWaveIndex >= waves.Length)
                {
                    GameCompleted?.Invoke(this);
                    yield break;
                }

                nextSpawnIndex = 0;
                wave = _currentWave = waves[nextWaveIndex];
                _erromitesRemaining = _erromiteWaveSize = wave.Spawns.Length;
                _waveIndex = nextWaveIndex;

                // Wait between spawns is different between inter and intra wave spawns
                if (nextWaveIndex > 0)
                {
                    yield return waveTime;
                }
                else
                {
                    yield return spawnTime;
                }
            }

            // Spawn in an erromite and add its methods to controller
            GameObject erromite = wave.Spawns[nextSpawnIndex].Spawn();
            erromite.GetComponent<HealthController>().HealthChanged += OnErromiteDamaged;
            _spawnIndex = nextSpawnIndex;

            if (nextSpawnIndex == 0)
            {
                WaveInitiated?.Invoke(this);
            }

            yield return spawnTime;
        }
    }  // End of GetSpawnCoroutine

    /// <summary>
    /// Represents a spawn place for erromites
    /// </summary>
    [Serializable]
    private struct SpawnEntry
    {
        public Transform Location;
        public GameObject Prefab;

        public SpawnEntry(Transform location, GameObject prefab)
        {
            Location = location;
            Prefab = prefab;
        }

        public GameObject Spawn()
        {
            return Instantiate(Prefab, Location.position, Location.rotation);
        }
    }  // End of SpawnEntry

    /// <summary>
    /// Represents a wave of erromites
    /// </summary>
    [Serializable]
    private struct WaveEntry
    {
        public SpawnEntry[] Spawns;

        public WaveEntry(SpawnEntry[] spawns)
        {
            Spawns = spawns;
        }
    }  // End of WaveEntry
}  // End of ErromiteWaveController