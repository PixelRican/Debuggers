using UnityEngine;

/// <summary>
/// Teleports the player upon collision with teleporter
/// </summary>
/// <author>Roberto Mercado & Jack Wooldridge</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
public class TeleportScript : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform teleportTarget;  // Empty gameobject used as teleporter destination

    /// <summary>
    /// Teleports the player upon activation of teleporter
    /// </summary>
    /// <param name="other">Object that triggered teleporter</param>
    private void OnTriggerEnter(Collider other)
    {
        // If other object is the player, teleport to destination
        if (other.transform.root == xrOrigin)
        {
            xrOrigin.position = teleportTarget.position;
            xrOrigin.rotation = teleportTarget.rotation;
        }
    }  // End of OnTriggerEnter
}  // End of TeleportScript