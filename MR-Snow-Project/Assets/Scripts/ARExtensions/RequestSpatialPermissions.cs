using UnityEngine;
using UnityEngine.Events;

namespace ARExtensions
{
    /// <summary>
    /// Requests spatial data from user, if denied just generate a large plane as the ground plane
    /// </summary>
    public class RequestSpatialPermissions : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPermissionGranted;

        [SerializeField] private UnityEvent OnPermissionDenied;
        const string spatialPermission = "com.oculus.permission.USE_SCENE";

        private const float enableDelay = 0.5f;

        private void OnEnable()
        {
            bool hasUserAuthorizedPermission =
                UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission);

            if (!hasUserAuthorizedPermission)
            {
                var callbacks = new UnityEngine.Android.PermissionCallbacks();

                callbacks.PermissionGranted += OnGranted;
                callbacks.PermissionDenied += OnDenied;

                UnityEngine.Android.Permission.RequestUserPermission(spatialPermission);
            }
        }

        private void OnDisable()
        {
            bool hasUserAuthorizedPermission =
                UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission);

            if (!hasUserAuthorizedPermission)
            {
                var callbacks = new UnityEngine.Android.PermissionCallbacks();

                callbacks.PermissionGranted -= OnGranted;
                callbacks.PermissionDenied -= OnDenied;
            }
        }

        private void OnGranted(string obj)
        {
            Invoke(nameof(EnableARPlaneManager), enableDelay);
        }

        private void EnableARPlaneManager()
        {
            OnPermissionGranted?.Invoke();
        }

        private void OnDenied(string obj)
        {
            OnPermissionDenied?.Invoke();
        }
    }
}