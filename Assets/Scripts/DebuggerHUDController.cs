using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior of the player HUD
/// </summary>
/// <author>Roberto Mercado</author>
/// <remarks>Commented by Jack Wooldridge</remarks>
/// <date>2025-05-02</date>
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

    /// <summary>
    /// When the patch game starts, add members to action controllers
    /// </summary>
    private void Awake()
    {
        action[ActionPath].performed += OnActiveChange;
        action[ActionPath].canceled += OnActiveChange;
        gameObject.SetActive(false);
    }  // End of Awake

    /// <summary>
    /// When the program ends, remove members from action controllers
    /// </summary>
    private void OnDestroy()
    {
        action[ActionPath].performed -= OnActiveChange;
        action[ActionPath].canceled -= OnActiveChange;
    }  // End of OnDestroy

    /// <summary>
    /// When the player HUD is enabled, add members to controllers
    /// </summary>
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
    }  // End of OnEnable

    /// <summary>
    /// When the player's HUD is disable, remove members from controllers
    /// </summary>
    private void OnDisable()
    {
        debuggerHealthController.HealthChanged -= UpdateDebuggerHealthSlider;
        patchGeneratorHealthController.HealthChanged -= UpdatePatchGeneratorHealthSlider;
        erromiteWaveController.ErromiteDestroyed -= UpdateErromiteWaveProgressSlider;
        erromiteWaveController.WaveInitiated -= UpdateErromiteWaveProgressSlider;
    }  // End of OnDisable

    /// <summary>
    /// Updates the player's health on the player's HUD
    /// </summary>
    /// <param name="sender">Container of player's health information</param>
    private void UpdateDebuggerHealthSlider(HealthController sender)
    {
        debuggerHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }  // End of UpdateDebuggerHealthSlider

    /// <summary>
    /// Updates the patch generator's health on the player's HUD
    /// </summary>
    /// <param name="sender">Container of patch generator's health information</param>
    private void UpdatePatchGeneratorHealthSlider(HealthController sender)
    {
        patchGeneratorHealthSlider.value = (float)sender.Health / sender.MaxHealth;
    }  // End of UpdatePatchGeneratorHealthSlider

    /// <summary>
    /// Update the number of remaining waves on the player's HUD
    /// </summary>
    /// <param name="sender">Container of erromite wave system information</param>
    private void UpdateErromiteWaveProgressSlider(ErromiteWaveController sender)
    {
        erromiteWaveProgressSlider.value = 1.0f - (float)sender.ErromitesRemaining / sender.ErromiteWaveSize;
    }  // End of UpdateErromiteWaveProgressSlider

    /// <summary>
    /// When the player's HUD changes, update it visually
    /// </summary>
    /// <param name="obj">Contains information about change to HUD</param>
    private void OnActiveChange(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(obj.performed);
    }  // End of OnActiveChange
}  // End of DebuggerHUDController