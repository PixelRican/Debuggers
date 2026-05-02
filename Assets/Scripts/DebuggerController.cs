using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

/// <summary>
/// Controls the behavior of the player
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public class DebuggerController : MonoBehaviour
{
    [SerializeField] private XROrigin origin;
    [SerializeField] private HealthController healthController;
    [SerializeField] private Transform spawnTransform;

    /// <summary>
    /// On start of the game, move the player to the spawn location
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        TeleportToSpawn();
    }  // End of Start

    /// <summary>
    /// When the player is enabled, add members to controllers
    /// </summary>
    private void OnEnable()
    {
        healthController.HealthChanged += OnHealthChanged;
    }  // End of OnEnable

    /// <summary>
    /// When the player is disabled, remove members from controllers
    /// </summary>
    private void OnDisable()
    {
        healthController.HealthChanged -= OnHealthChanged;
    }  // End of OnDisable

    /// <summary>
    /// Checks if the player has run out of health
    /// </summary>
    /// <param name="sender"></param>
    private void OnHealthChanged(HealthController sender)
    {
        // If the player has run out of health, reset their health and return them to the spawn point
        if (sender.Health == 0)
        {
            sender.Reset();
            TeleportToSpawn();
        }
    }  // End of OnHealthChanged

    /// <summary>
    /// Moves the player to the spawn location
    /// </summary>
    private void TeleportToSpawn()
    {
        Transform originTransform = origin.transform;
        Vector3 originPosition = originTransform.position;
        Vector3 headOffset = origin.Camera.transform.position - originPosition;
        originTransform.position = spawnTransform.position - headOffset;
    }  // End of TeleportToSpawn
}  // End of DebuggerController