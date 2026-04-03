using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.XR.ARFoundation;

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

        private PermissionCallbacks _callbacks;

        private void OnEnable()
        {
            bool hasUserAuthorizedPermission =
                UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission);

            if (!hasUserAuthorizedPermission)
            {
                _callbacks = new UnityEngine.Android.PermissionCallbacks();

                _callbacks.PermissionGranted += OnGranted;
                _callbacks.PermissionDenied += OnDenied;

                UnityEngine.Android.Permission.RequestUserPermission(spatialPermission);
            }
            else
            {
                StartCoroutine(WaitUntilSessionState());
            }
        }

        private void OnDisable()
        {
            _callbacks.PermissionGranted -= OnGranted;
            _callbacks.PermissionDenied -= OnDenied;
        }

        private void OnGranted(string obj)
        {
            //Invoke(nameof(EnableARPlaneManager), enableDelay);
            StartCoroutine(WaitUntilSessionState());
        }

        private IEnumerator WaitUntilSessionState()
        {
            while (ARSession.state < ARSessionState.SessionTracking)
                yield return null;

            yield return new WaitForSeconds(0.5f);
            Debug.Log("Enabling Plane Manager!");
            OnPermissionGranted?.Invoke();
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