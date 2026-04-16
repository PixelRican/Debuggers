using System;
using System.Collections.Generic;
using UnityEngine;

public class ErromiteWaveController : MonoBehaviour
{
    [SerializeField] private float waveDowntime;
    [SerializeField] private float spawnRate;
    [SerializeField] private WaveEntry[] waves;
    private IEnumerator<WaitForSeconds> spawnCoroutine;
    private WaveEntry currentWave;
    private int waveIndex;
    private int spawnIndex;

    private void OnEnable()
    {
        currentWave.Spawns = Array.Empty<SpawnEntry>();
        waveIndex = -1;
        spawnIndex = -1;
        spawnCoroutine = GetSpawnCoroutine();
        StartCoroutine(spawnCoroutine);
    }

    private void OnDisable()
    {
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }

    private IEnumerator<WaitForSeconds> GetSpawnCoroutine()
    {
        WaitForSeconds waveTime = new WaitForSeconds(waveDowntime);
        WaitForSeconds spawnTime = new WaitForSeconds(spawnRate);

        while (true)
        {
            WaveEntry wave = currentWave;
            int nextSpawnIndex = spawnIndex + 1;

            if (nextSpawnIndex >= currentWave.Spawns.Length)
            {
                int nextWaveIndex = waveIndex + 1;

                if (nextWaveIndex >= waves.Length)
                {
                    yield break;
                }

                nextSpawnIndex = 0;
                wave = currentWave = waves[nextWaveIndex];
                waveIndex = nextWaveIndex;

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
            spawnIndex = nextSpawnIndex;
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
