using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform teleportTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == xrOrigin)
        {
            xrOrigin.position = teleportTarget.position;
            xrOrigin.rotation = teleportTarget.rotation;
        }
    }
}
