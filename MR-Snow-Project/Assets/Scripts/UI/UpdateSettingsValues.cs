using System;
using UnityEngine;
using UnityEngine.UI;


public class UpdateSettingsValues : MonoBehaviour
{
    [Header("UI References")] [SerializeField]
    private Slider spawnRateSlider;

    [SerializeField] private Slider windRateSlider;

    [SerializeField] private Toggle snowEnabledToggle;

    private SettingsEvent settingsEvent = new();

    private void OnEnable()
    {
        spawnRateSlider.onValueChanged.AddListener(UpdateSpawnRate);
        windRateSlider.onValueChanged.AddListener(UpdateWindRate);
        snowEnabledToggle.onValueChanged.AddListener(UpdateSnowToggle);
    }

    private void OnDisable()
    {
        spawnRateSlider.onValueChanged.RemoveListener(UpdateSpawnRate);
        windRateSlider.onValueChanged.RemoveListener(UpdateWindRate);
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

    private void UpdateWindRate(float windRate)
    {
        settingsEvent.windRate = (int)windRate;
        EventBus<SettingsEvent>.Raise(settingsEvent);
    }
}