using UnityEngine;
using UnityEngine.Rendering;

namespace OrthoSnowSplat
{
    public class SplatCaptureTest : MonoBehaviour
    {
        [Header("Required")] [SerializeField] private Camera splatCamera;
        [SerializeField] private RenderTexture splatRT;
        [SerializeField] private CustomRenderTexture snowAccumulationCRT;

        [Header("Debug Display")] [SerializeField]
        private int previewSize = 256;

        [SerializeField] private bool showDebug = true;

        private static readonly int IsSplatPassID = Shader.PropertyToID("_IsSplatPass");
        private static readonly int SplatInputID = Shader.PropertyToID("_SplatInput");

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCamera;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
        }

        private void Awake()
        {
            //Prevent render texture from auto-dispatching
            if (snowAccumulationCRT != null)
            {
                snowAccumulationCRT.updateMode = CustomRenderTextureUpdateMode.OnDemand;
            }
            else
            {
                Debug.LogWarning("Snow Accumulation CRT Render Texture Missing!");
            }
        }

        private void Start()
        {
            if (splatRT == null)
            {
                Debug.LogWarning("Splat Render Texture is Missing!");
            }

            if (splatCamera != null)
            {
                splatCamera.targetTexture = splatRT;
            }
            else
            {
                Debug.LogWarning("Splat Camera Missing!");
            }

            //Update texture and reset update mode
            if (snowAccumulationCRT != null && snowAccumulationCRT.material != null)
            {
                snowAccumulationCRT.material.SetTexture(SplatInputID, splatRT);
                snowAccumulationCRT.Initialize();
                snowAccumulationCRT.updateMode = CustomRenderTextureUpdateMode.Realtime;
            }
            else
            {
                Debug.LogWarning("Snow Accumulation CRT material Missing!");
            }
        }

        private void OnBeginCamera(ScriptableRenderContext ctx, Camera cam)
        {
            //Prevent this being called from XR camera
            if (cam.cameraType == CameraType.Game && cam.stereoEnabled)
                return;

            Shader.SetGlobalFloat(IsSplatPassID, cam == splatCamera ? 1f : 0f);
        }

        private void OnGUI()
        {
            if (!showDebug) return;

            int pad = 10;
            int y = Screen.height - previewSize - pad;

            if (splatRT != null)
            {
                GUI.Label(new Rect(pad, y - 20, previewSize, 20), "Splat RT");
                GUI.DrawTexture(new Rect(pad, y, previewSize, previewSize), splatRT);
            }

            if (snowAccumulationCRT != null)
            {
                float x = pad + previewSize + pad;
                GUI.Label(new Rect(x, y - 20, previewSize, 20), "CRT");
                GUI.DrawTexture(new Rect(x, y, previewSize, previewSize), snowAccumulationCRT);
            }
        }
    }
}