using UnityEngine;
using UnityEngine.UI;

public class UpdateSettingsValues : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Slider spawnRateSlider;

    [SerializeField]
    private Toggle snowEnabledToggle;

    private SettingsEvent settingsEvent = new();

    private void OnEnable()
    {
        spawnRateSlider.onValueChanged.AddListener(UpdateSpawnRate);
        snowEnabledToggle.onValueChanged.AddListener(UpdateSnowToggle);
    }

    private void OnDisable()
    {
        spawnRateSlider.onValueChanged.RemoveListener(UpdateSpawnRate);
        snowEnabledToggle.onValueChanged.RemoveListener(UpdateSnowToggle);
    }

    private void UpdateSpawnRate(float spawnRate)
    {
        settingsEvent.snowSpawnRate = (int)spawnRate;
        EventBus<SettingsEvent>.Raise(settingsEvent);
    }

    private void UpdateSnowToggle(bool isEnabled)
    {
        settingsEvent.isSnowEnabled = isEnabled;
        EventBus<SettingsEvent>.Raise(settingsEvent);
    }
}
