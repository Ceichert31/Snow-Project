using UnityEngine;
using UnityEngine.VFX;

public class UpdateVFXValues : MonoBehaviour
{
    private VisualEffect snowEffect;

    private EventBinding<SettingsEvent> settingsBinding;

    private void OnEnable()
    {
        settingsBinding = new(UpdateValues);
        EventBus<SettingsEvent>.Register(settingsBinding);
    }

    private void OnDisable()
    {
        EventBus<SettingsEvent>.Deregister(settingsBinding);
    }

    private void Awake()
    {
        snowEffect = GetComponent<VisualEffect>();
    }

    private void UpdateValues(SettingsEvent ctx)
    {
        snowEffect.enabled = ctx.isSnowEnabled;
        snowEffect.SetInt("SnowSpawnRate", ctx.snowSpawnRate);
    }
}
