using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public sealed class XRSpawnController : MonoBehaviour
{
    [SerializeField] private XROrigin origin;
    [SerializeField] private Vector3 offset;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        Transform originTransform = origin.transform;
        Vector3 originPosition = originTransform.position;
        Vector3 headOffset = origin.Camera.transform.position - originPosition;
        originTransform.position = offset - headOffset;
        Destroy(this);
    }
}
