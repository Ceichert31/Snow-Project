using System;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Controls VFX settings
/// </summary>
public class UpdateVFXValues : MonoBehaviour
{
    private VisualEffect snowEffect;

    private void Awake()
    {
        snowEffect = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        SettingsManager.Instance.OnPropertyChanged += UpdateValues;
    }

    /// <summary>
    /// Updates all settings values
    /// </summary>
    private void UpdateValues()
    {
        snowEffect.SetInt("SnowSpawnRate", SettingsManager.Instance.SpawnRate);
        snowEffect.SetInt("WindRate", SettingsManager.Instance.WindForce);

        snowEffect.enabled = SettingsManager.Instance.IsSnowEnabled;
    }
}