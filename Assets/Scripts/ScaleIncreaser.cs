using UnityEngine;

public sealed class ScaleIncreaser : MonoBehaviour
{
    [SerializeField] private float growthRate;

    private void Update()
    {
        transform.localScale += new Vector3(growthRate, growthRate, growthRate) *  Time.deltaTime;
    }
}
