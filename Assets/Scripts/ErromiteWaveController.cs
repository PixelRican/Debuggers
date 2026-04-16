using System;
using System.Collections.Generic;
using UnityEngine;

public class ErromiteWaveController : MonoBehaviour
{
    [SerializeField] private float waveDowntime;
    [SerializeField] private float spawnRate;
    [SerializeField] private WaveEntry[] waves;
    private HealthController _patchGeneratorHealth;
    private IEnumerator<WaitForSeconds> _spawnCoroutine;
    private WaveEntry _currentWave;
    private int _waveIndex;
    private int _spawnIndex;

    private void Start()
    {
        _patchGeneratorHealth = GameObject.FindWithTag("PatchGenerator").GetComponent<HealthController>();
    }

    private void OnEnable()
    {
        _currentWave.Spawns = Array.Empty<SpawnEntry>();
        _waveIndex = -1;
        _spawnIndex = -1;
        _spawnCoroutine = GetSpawnCoroutine();
        StartCoroutine(_spawnCoroutine);
        _patchGeneratorHealth.HealthDepleted += OnPatchGeneratorDestroyed;
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
        _patchGeneratorHealth.HealthDepleted -= OnPatchGeneratorDestroyed;
    }

    private void OnPatchGeneratorDestroyed(object sender, EventArgs e)
    {
        enabled = false;
    }

    private IEnumerator<WaitForSeconds> GetSpawnCoroutine()
    {
        WaitForSeconds waveTime = new WaitForSeconds(waveDowntime);
        WaitForSeconds spawnTime = new WaitForSeconds(spawnRate);

        while (true)
        {
            WaveEntry wave = _currentWave;
            int nextSpawnIndex = _spawnIndex + 1;

            if (nextSpawnIndex >= _currentWave.Spawns.Length)
            {
                int nextWaveIndex = _waveIndex + 1;

                if (nextWaveIndex >= waves.Length)
                {
                    yield break;
                }

                nextSpawnIndex = 0;
                wave = _currentWave = waves[nextWaveIndex];
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

            wave.Spawns[nextSpawnIndex].Spawn();
            _spawnIndex = nextSpawnIndex;
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
