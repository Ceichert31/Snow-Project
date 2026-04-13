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

    /// <summary>
    /// Called by slider
    /// </summary>
    public void UpdateSpawnRate(float spawnRate)
    {
        SettingsManager.Instance.SpawnRate = spawnRate;
    }

    /// <summary>
    /// Called by toggle button
    /// </summary>
    public void UpdateSnowToggle(bool isEnabled)
    {
        SettingsManager.Instance.IsSnowEnabled = isEnabled;
    }

    /// <summary>
    /// Called by slider
    /// </summary>
    public void UpdateWindRate(float windRate)
    {
        SettingsManager.Instance.WindForce = windRate;
    }
}