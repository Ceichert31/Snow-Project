using UnityEngine;

/// <summary>
/// Removes XR Device Simulator in builds
/// </summary>
public class XRDeviceSimulatorExclude : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor)
            return;

        DestroyImmediate(gameObject);
    }
}
