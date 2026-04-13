using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls all the VFX and snow settings
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    #region Properties

    /// <summary>
    /// Rate at which snow spawns
    /// </summary>
    public float SpawnRate
    {
        get => _spawnRate;
        set
        {
            _spawnRate = value;
            OnPropertyChanged?.Invoke();
        }
    }

    private float _spawnRate = 100;

    /// <summary>
    /// Whether snow VFX is enabled
    /// </summary>
    public bool IsSnowEnabled
    {
        get => _isSnowEnabled;
        set
        {
            _isSnowEnabled = value;
            OnPropertyChanged?.Invoke();
        }
    }

    private bool _isSnowEnabled = true;

    /// <summary>
    /// The speed of wind forces on snow particles
    /// </summary>
    /// <remarks>
    /// Could add a particle that goes along with this
    /// </remarks>
    public float WindForce
    {
        get => _windForce;
        set
        {
            _windForce = value;
            OnPropertyChanged?.Invoke();
        }
    }

    private float _windForce = 1;

    #endregion


    public UnityEvent OnPropertyChanged;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}