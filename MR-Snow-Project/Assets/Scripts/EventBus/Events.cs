public interface IEvent { }

/// <summary>
/// Sends all settings values to VFX
/// </summary>
public class SettingsEvent : IEvent
{
    public int snowSpawnRate;
    public bool isSnowEnabled;
}
