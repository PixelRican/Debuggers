using UnityEngine;
using UnityEngine.InputSystem;

public sealed class DebuggingToolController : MonoBehaviour
{
    private const string ActionPath = "XRI Right Interaction/Activate";
    
    [SerializeField] private InputActionAsset action;
    [SerializeField] private float strength;

    private void Start()
    {
        action[ActionPath].performed += OnPerformed;
    }

    private void OnDestroy()
    {
        action[ActionPath].performed -= OnPerformed;
    }

    private void OnPerformed(InputAction.CallbackContext obj)
    {
        Transform transform = this.transform;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            Destroy(hit.transform.gameObject);
        }
    }
}
