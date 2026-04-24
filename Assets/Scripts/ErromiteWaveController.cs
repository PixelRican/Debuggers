using System;
using System.Collections.Generic;
using UnityEngine;

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

    public bool Started
    {
        get => _spawnCoroutine != null;
    }

    public int ErromitesRemaining
    {
        get => _erromitesRemaining;
    }

    public int ErromiteWaveSize
    {
        get => _erromiteWaveSize;
    }

    public event Action<ErromiteWaveController> WaveInitiated;

    public event Action<ErromiteWaveController> ErromiteDestroyed;

    public event Action<ErromiteWaveController> GameCompleted;

    private void OnEnable()
    {
        _currentWave.Spawns = Array.Empty<SpawnEntry>();
        _waveIndex = -1;
        _spawnIndex = -1;
        _patchGeneratorHealth = GameObject.FindWithTag("PatchGenerator").GetComponent<HealthController>();
        _patchGeneratorHealth.HealthDepleted += OnPatchGeneratorDamaged;
        _spawnCoroutine = GetSpawnCoroutine();
        StartCoroutine(_spawnCoroutine);
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
        _patchGeneratorHealth.HealthDepleted -= OnPatchGeneratorDamaged;
        _patchGeneratorHealth = null;
        _erromitesRemaining = 0;
        _erromiteWaveSize = 0;
    }

    private void OnPatchGeneratorDamaged(HealthController sender)
    {
        enabled = sender.Health > 0;
    }

    private void OnErromiteDamaged(HealthController sender)
    {
        if (sender.Health == 0)
        {
            _erromitesRemaining--;
            sender.HealthDepleted -= OnErromiteDamaged;
            ErromiteDestroyed?.Invoke(this);
        }
    }

    private IEnumerator<object> GetSpawnCoroutine()
    {
        WaitForSeconds waveTime = new WaitForSeconds(waveDowntime);
        WaitForSeconds spawnTime = new WaitForSeconds(spawnRate);
        WaitUntil nextWaveCondition = new WaitUntil(() => _erromitesRemaining == 0);

        while (true)
        {
            WaveEntry wave = _currentWave;
            int nextSpawnIndex = _spawnIndex + 1;

            if (nextSpawnIndex >= _currentWave.Spawns.Length)
            {
                yield return nextWaveCondition;

                int nextWaveIndex = _waveIndex + 1;

                if (nextWaveIndex >= waves.Length)
                {
                    GameCompleted?.Invoke(this);
                    yield break;
                }

                nextSpawnIndex = 0;
                wave = _currentWave = waves[nextWaveIndex];
                _erromitesRemaining = _erromiteWaveSize = wave.Spawns.Length;
                _waveIndex = nextWaveIndex;

                if (nextWaveIndex > 0)
                {
                    yield return waveTime;
                }
                else
                {
                    yield return spawnTime;
                }
            }

            GameObject erromite = wave.Spawns[nextSpawnIndex].Spawn();
            erromite.GetComponent<HealthController>().HealthDepleted += OnErromiteDamaged;
            _spawnIndex = nextSpawnIndex;

            if (nextSpawnIndex == 0)
            {
                WaveInitiated?.Invoke(this);
            }

            yield return spawnTime;
        }
    }

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
    }

    [Serializable]
    private struct WaveEntry
    {
        public SpawnEntry[] Spawns;

        public WaveEntry(SpawnEntry[] spawns)
        {
            Spawns = spawns;
        }
    }
}
