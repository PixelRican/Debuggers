using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public sealed class XRSpawnController : MonoBehaviour
{
    [SerializeField] private XROrigin origin;
    [SerializeField] private Transform teleportTarget;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        Transform originTransform = origin.transform;
        Vector3 originPosition = originTransform.position;
        Vector3 headOffset = origin.Camera.transform.position - originPosition;
        originTransform.position = teleportTarget.position - headOffset;
        Destroy(this);
    }
}
