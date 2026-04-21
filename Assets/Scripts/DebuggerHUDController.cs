using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebuggerHUDController : MonoBehaviour
{
    private const string ActionPath = "XRI Left Interaction/Activate";

    [SerializeField] private InputActionAsset action;
    [SerializeField] private HealthController debuggerHealthController;
    [SerializeField] private HealthController patchGeneratorHealthController;
    [SerializeField] private ErromiteWaveController erromiteWaveController;
    [SerializeField] private Slider debuggerHealthSlider;
    [SerializeField] private Slider patchGeneratorHealthSlider;
    [SerializeField] private Slider erromiteWaveSlider;

    private void Awake()
    {
        action[ActionPath].performed += OnActiveChange;
        action[ActionPath].canceled += OnActiveChange;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        action[ActionPath].performed -= OnActiveChange;
        action[ActionPath].canceled -= OnActiveChange;
    }

    private void OnEnable()
    {
        debuggerHealthController.HealthDepleted += UpdateDebuggerHealthSlider;
        patchGeneratorHealthController.HealthDepleted += UpdatePatchGeneratorHealthSlider;
        erromiteWaveController.ErromiteDestroyed += UpdateErromiteWaveSlider;
        UpdateDebuggerHealthSlider(debuggerHealthController);
        UpdatePatchGeneratorHealthSlider(patchGeneratorHealthController);
        UpdateErromiteWaveSlider(erromiteWaveController);
    }

    private void OnDisable()
    {
        debuggerHealthController.HealthDepleted -= UpdateDebuggerHealthSlider;
        patchGeneratorHealthController.HealthDepleted -= UpdatePatchGeneratorHealthSlider;
        erromiteWaveController.ErromiteDestroyed -= UpdateErromiteWaveSlider;
    }

    private void UpdateDebuggerHealthSlider(HealthController sender)
    {
        debuggerHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }

    private void UpdatePatchGeneratorHealthSlider(HealthController sender)
    {
        patchGeneratorHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }

    private void UpdateErromiteWaveSlider(ErromiteWaveController sender)
    {
        erromiteWaveSlider.value = (float)sender.ErromitesRemaining / sender.ErromiteWaveSize;
    }

    private void OnActiveChange(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(obj.performed);
    }
}
