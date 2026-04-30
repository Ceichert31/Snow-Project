using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

namespace ARExtensions
{
    public class RequestDepthPermissions : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPermissionGranted;

        [SerializeField] private UnityEvent OnPermissionDenied;
        const string DepthPermission = "android.permission.SCENE_UNDERSTANDING_FINE";

        private PermissionCallbacks _callbacks;

        private void OnEnable()
        {
            bool hasUserAuthorizedPermission =
                UnityEngine.Android.Permission.HasUserAuthorizedPermission(DepthPermission);

            if (!hasUserAuthorizedPermission)
            {
                _callbacks = new UnityEngine.Android.PermissionCallbacks();

                _callbacks.PermissionGranted += OnGranted;
                _callbacks.PermissionDenied += OnDenied;

                UnityEngine.Android.Permission.RequestUserPermission(DepthPermission);
            }
            else
            {
                StartCoroutine(WaitUntilSessionState());
            }
        }

        private void OnDisable()
        {
            if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(DepthPermission)) return;

            _callbacks.PermissionGranted -= OnGranted;
            _callbacks.PermissionDenied -= OnDenied;
        }

        private void OnGranted(string obj)
        {
            StartCoroutine(WaitUntilSessionState());
        }

        private IEnumerator WaitUntilSessionState()
        {
            while (ARSession.state < ARSessionState.SessionTracking)
                yield return null;

            yield return new WaitForSeconds(0.5f);
            Debug.Log("Enabling Occlussion Manager!");
            OnPermissionGranted?.Invoke();
        }

        private void OnDenied(string obj)
        {
            OnPermissionDenied?.Invoke();
        }
    }
}