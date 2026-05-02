using UnityEngine;

/// <summary>
/// Increase the scale of explosion prticle effects
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public sealed class ScaleIncreaser : MonoBehaviour
{
    [SerializeField] private float growthRate;

    /// <summary>
    /// Increase scale of object
    /// </summary>
    private void Update()
    {
        transform.localScale += new Vector3(growthRate, growthRate, growthRate) *  Time.deltaTime;
    }  // End of Update
}  // ScaleIncreaser
