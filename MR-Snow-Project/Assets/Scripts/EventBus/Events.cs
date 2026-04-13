public interface IEvent
{
}

/// <summary>
/// Sends all settings values to VFX
/// </summary>
public class SettingsEvent : IEvent
{
    public int snowSpawnRate = 100;
    public bool isSnowEnabled = true;
    public int windRate = 1;
}