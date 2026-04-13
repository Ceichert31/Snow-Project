using UnityEngine;

/// <summary>
/// Used by settings UI to control VFX values
/// </summary>
public class UpdateSettingsValues : MonoBehaviour
{
    /// <summary>
    /// Called by slider
    /// </summary>
    public void UpdateSpawnRate(float spawnRate)
    {
        SettingsManager.Instance.SpawnRate = (int)spawnRate;
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
        SettingsManager.Instance.WindForce = (int)windRate;
    }

    /// <summary>
    /// Called by button
    /// </summary>
    public void SpawnCube()
    {
        //Call event to spawn cube in front of player
        EventBus<SpawnCubeEvent>.Raise(new SpawnCubeEvent());
    }
}