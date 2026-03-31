using System;
using UnityEngine;

public class ErromiteWaveController : MonoBehaviour
{
    [SerializeField] private WaveEntry[] waves;
    private WaveEntry currentWave;
    private int waveIndex;
    private int spawnIndex;

    private void OnEnable()
    {
        currentWave.Spawns = Array.Empty<SpawnEntry>();
        waveIndex = -1;
        spawnIndex = -1;
        InvokeRepeating(nameof(SpawnErromite), 5.0f, 5.0f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnErromite));
    }

    private void SpawnErromite()
    {
        WaveEntry wave = currentWave;
        int nextSpawnIndex = spawnIndex + 1;

        if (nextSpawnIndex >= currentWave.Spawns.Length)
        {
            int nextWaveIndex = waveIndex + 1;

            if (nextWaveIndex >= waves.Length)
            {
                return;
            }

            nextSpawnIndex = 0;
            wave = currentWave = waves[nextWaveIndex];
            waveIndex = nextWaveIndex;
        }

        wave.Spawns[nextSpawnIndex].Spawn();
        spawnIndex = nextSpawnIndex;
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
