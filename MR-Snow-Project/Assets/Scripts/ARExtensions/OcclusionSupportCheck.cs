using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARExtensions
{
    [RequireComponent(typeof(AROcclusionManager))]
    public class OcclusionSupportCheck : MonoBehaviour
    {
        private AROcclusionManager manager;

        void Awake()
        {
            manager = GetComponent<AROcclusionManager>();
        }

        void Start()
        {
            if (LoaderUtility
                    .GetActiveLoader()?
                    .GetLoadedSubsystem<XROcclusionSubsystem>() != null)
            {
                // XROcclusionSubsystem was loaded. The platform supports occlusion.
                Debug.Log("Occlusion is supported!");
                CheckForOptionalFeatureSupport();
            }
            else
            {
                Debug.LogWarning("Occlusion is not supported!");
            }
        }

        void CheckForOptionalFeatureSupport()
        {
            if (manager.descriptor.environmentDepthImageSupported == Supported.Supported)
            {
                Debug.Log("Environment depth image is supported!");
            }
            else
            {
                Debug.LogWarning("Environment depth image is not supported!");
            }

            if (manager.descriptor.humanSegmentationStencilImageSupported
                == Supported.Supported)
            {
                Debug.Log("Human Segmentation stencil is supported!");
            }
            else
            {
                Debug.LogWarning("Human Segmentation stencil is not supported!");
            }
        }
    }
}