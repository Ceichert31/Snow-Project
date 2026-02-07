using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace UnityEngine.XR.OpenXR.Features.Meta.Tests
{
    public class PermissionsCheck : MonoBehaviour
    {
        const string k_DefaultPermissionId = "com.oculus.permission.USE_SCENE";

        [SerializeField]
        string m_PermissionId = k_DefaultPermissionId;

        [SerializeField]
        UnityEvent<string> m_PermissionDenied;

        [SerializeField]
        UnityEvent<string> m_PermissionGranted;

        // Add reference to AR Plane Manager
        [SerializeField]
        ARPlaneManager m_ARPlaneManager;

#if UNITY_ANDROID
        void Start()
        {
            // Disable AR Plane Manager until permission is granted
            if (m_ARPlaneManager != null)
                m_ARPlaneManager.enabled = false;

            Debug.Log($"[PermissionsCheck] Checking permission: {m_PermissionId}");

            if (Permission.HasUserAuthorizedPermission(m_PermissionId))
            {
                Debug.Log($"[PermissionsCheck] Permission already granted");
                OnPermissionGranted(m_PermissionId);
            }
            else
            {
                Debug.Log($"[PermissionsCheck] Requesting permission...");
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += OnPermissionDenied;
                callbacks.PermissionGranted += OnPermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += OnPermissionDeniedAndDontAskAgain;

                Permission.RequestUserPermission(m_PermissionId, callbacks);
            }
        }

        void OnPermissionDenied(string permission)
        {
            Debug.LogError($"[PermissionsCheck] Permission DENIED: {permission}");
            m_PermissionDenied?.Invoke(permission);
        }

        void OnPermissionGranted(string permission)
        {
            Debug.Log($"[PermissionsCheck] Permission GRANTED: {permission}");

            // Enable AR Plane Manager after permission is granted
            if (m_ARPlaneManager != null)
            {
                m_ARPlaneManager.enabled = true;
                Debug.Log("[PermissionsCheck] AR Plane Manager enabled");
            }

            m_PermissionGranted?.Invoke(permission);
        }

        void OnPermissionDeniedAndDontAskAgain(string permission)
        {
            Debug.LogError($"[PermissionsCheck] Permission DENIED (Don't Ask Again): {permission}");
            m_PermissionDenied?.Invoke(permission);
        }
#else
        void Start()
        {
            Debug.LogWarning(
                "[PermissionsCheck] Not running on Android - skipping permission check"
            );
        }
#endif
    }
}
