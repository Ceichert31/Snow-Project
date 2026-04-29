using UnityEngine;
using UnityEngine.VFX;

namespace OrthoSnowSplat
{
using UnityEngine;
using UnityEngine.VFX;

namespace OrthoSnowSplat
{
    [RequireComponent(typeof(VisualEffect))]
    public class SnowObstacleProvider : MonoBehaviour
    {
        [SerializeField] private VisualEffect snowVFX;
        [SerializeField] private int initialCapacity = 16;

        private static readonly int ObstaclesID     = Shader.PropertyToID("Obstacles");
        private static readonly int ObstacleCountID = Shader.PropertyToID("ObstacleCount");

        private const int MatrixStrideBytes = 64;

        private GraphicsBuffer obstacleBuffer;
        private Matrix4x4[] cpuScratch;
        private int currentCapacity;

        private void Awake()
        {
            if (snowVFX == null) snowVFX = GetComponent<VisualEffect>();
            EnsureCapacity(Mathf.Max(1, initialCapacity));
        }

        private void OnDisable()
        {
            obstacleBuffer?.Release();
            obstacleBuffer = null;
            currentCapacity = 0;
        }

        private void LateUpdate()
        {
            if (snowVFX == null) return;

            var list = SnowObstacleRegistry.Obstacles;
            int registered = list.Count;

            EnsureCapacity(Mathf.Max(1, registered));

            int validCount = 0;
            for (int i = 0; i < registered; i++)
            {
                BoxCollider b = list[i].Box;
                if (b == null) continue;

                Matrix4x4 boxToLocal = Matrix4x4.TRS(b.center, Quaternion.identity, b.size);
                Matrix4x4 worldToBox = (b.transform.localToWorldMatrix * boxToLocal).inverse;

                cpuScratch[validCount++] = worldToBox;
            }

            obstacleBuffer.SetData(cpuScratch, 0, 0, Mathf.Max(1, validCount));

            snowVFX.SetGraphicsBuffer(ObstaclesID, obstacleBuffer);
            snowVFX.SetUInt(ObstacleCountID, (uint)validCount);
        }

        private void EnsureCapacity(int needed)
        {
            if (obstacleBuffer != null && currentCapacity >= needed) return;

            obstacleBuffer?.Release();
            currentCapacity = Mathf.NextPowerOfTwo(needed);
            obstacleBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                currentCapacity,
                MatrixStrideBytes);
            cpuScratch = new Matrix4x4[currentCapacity];
        }
    }
}
}
