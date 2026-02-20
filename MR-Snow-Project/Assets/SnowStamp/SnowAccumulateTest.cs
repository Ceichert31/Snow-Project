using UnityEngine;

namespace SnowStamp
{
    public class SnowAccumulateTest : MonoBehaviour
    {
        [SerializeField]
        private RenderTexture splatMap;
        [SerializeField]
        private ComputeShader compute;
        [SerializeField]
        private Transform groundPlane;

        [SerializeField]
        private float startLevel = 0.5f;
        [SerializeField]
        private float stepSize = 0.1f;
        [SerializeField]
        private float stampRadius = 0.1f;
        private int fillKernel;

        private RenderTexture readRT;
        private int stampKernel;

        private void Start()
        {
            fillKernel = compute.FindKernel("CSFill");
            stampKernel = compute.FindKernel("CSStamp");

            if (!splatMap.IsCreated()) splatMap.Create();

            readRT = new RenderTexture(splatMap.width, splatMap.height, 0, splatMap.graphicsFormat);
            readRT.Create();

            groundPlane.GetComponent<Renderer>().material.SetTexture("_SplatMap", readRT);

            Fill(startLevel);
            Publish();
        }

        private void Update()
        {
            Restore();

            if (Input.GetKeyDown(KeyCode.Space)) StampCenter(-stepSize);

            if (Input.GetKeyDown(KeyCode.R)) StampCenter(+stepSize);

            Publish();
        }

        private void OnDestroy()
        {
            if (readRT != null)
            {
                readRT.Release();
                Destroy(readRT);
            }
        }

        private void Restore()
        {
            Graphics.CopyTexture(readRT, splatMap);
        }

        private void Publish()
        {
            Graphics.CopyTexture(splatMap, readRT);
        }

        private void Fill(float level)
        {
            compute.SetTexture(fillKernel, "_SplatMap", splatMap);
            compute.SetFloat("_TexSize", splatMap.width);
            compute.SetFloat("_FillValue", level);
            var groups = Mathf.CeilToInt(splatMap.width / 8f);
            compute.Dispatch(fillKernel, groups, groups, 1);
        }

        private void StampCenter(float intensity)
        {
            var center = splatMap.width * 0.5f;
            var radius = splatMap.width * stampRadius;

            compute.SetTexture(stampKernel, "_SplatMap", splatMap);
            compute.SetFloat("_TexSize", splatMap.width);
            compute.SetFloats("_StampCenter", center, center);
            compute.SetFloat("_StampRadius", radius);
            compute.SetFloat("_StampIntensity", intensity);

            var diameter = Mathf.CeilToInt(radius * 2f);
            var groups = Mathf.Max(1, Mathf.CeilToInt(diameter / 8f));
            compute.Dispatch(stampKernel, groups, groups, 1);
        }
    }
}