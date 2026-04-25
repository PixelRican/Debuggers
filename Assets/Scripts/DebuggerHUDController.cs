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
    [SerializeField] private Slider erromiteWaveProgressSlider;

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
        debuggerHealthController.HealthChanged += UpdateDebuggerHealthSlider;
        patchGeneratorHealthController.HealthChanged += UpdatePatchGeneratorHealthSlider;
        erromiteWaveController.WaveInitiated += UpdateErromiteWaveProgressSlider;
        erromiteWaveController.ErromiteDestroyed += UpdateErromiteWaveProgressSlider;

        if (erromiteWaveController.Started)
        {
            UpdateDebuggerHealthSlider(debuggerHealthController);
            UpdatePatchGeneratorHealthSlider(patchGeneratorHealthController);
            UpdateErromiteWaveProgressSlider(erromiteWaveController);
        }
    }

    private void OnDisable()
    {
        debuggerHealthController.HealthChanged -= UpdateDebuggerHealthSlider;
        patchGeneratorHealthController.HealthChanged -= UpdatePatchGeneratorHealthSlider;
        erromiteWaveController.ErromiteDestroyed -= UpdateErromiteWaveProgressSlider;
        erromiteWaveController.WaveInitiated -= UpdateErromiteWaveProgressSlider;
    }

    private void UpdateDebuggerHealthSlider(HealthController sender)
    {
        debuggerHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }

    private void UpdatePatchGeneratorHealthSlider(HealthController sender)
    {
        patchGeneratorHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }

    private void UpdateErromiteWaveProgressSlider(ErromiteWaveController sender)
    {
        erromiteWaveProgressSlider.value = 1.0f - (float)sender.ErromitesRemaining / sender.ErromiteWaveSize;
    }

    private void OnActiveChange(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(obj.performed);
    }
}
