using UnityEngine;
using Random = UnityEngine.Random;

public class ErromiteWaveController : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] erromitePrefabs;

    private void OnEnable()
    {
        InvokeRepeating(nameof(SpawnErromite), 5.0f, 5.0f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnErromite));
    }

    private void SpawnErromite()
    {
        Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject erromitePrefab = erromitePrefabs[Random.Range(0, erromitePrefabs.Length)];
        Instantiate(erromitePrefab, spawnLocation.position, spawnLocation.rotation);
    }
}
