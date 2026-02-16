using UnityEngine;

public class ComputeParticleSystem : MonoBehaviour
{
    /// <summary>
    /// Primary struct used for all particle data passed to compute
    /// </summary>
    struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float lifetime;
    }

    //For allocating space in memory
    const int ParticleSize = 7 * sizeof(float);

    [Header("Particle Settings")]
    [SerializeField]
    private int particleCount = 3000;

    [SerializeField]
    private Material particleMaterial;

    [SerializeField]
    private ComputeShader computeShader;

    private int kernelId;

    private ComputeBuffer particleBuffer;

    private int groupSizeX;

    private RenderParams renderParams;

    private void Start()
    {
        Particle[] particleArray = new Particle[particleCount];

        //Initialize particles
        for (int i = 0; i < particleCount; ++i)
        {
            Vector3 pos = new Vector3();
            //Keep within homogenous coordinate space, between -1 and 1
            //assuming w = 1
            pos.x = Random.value * 2 - 1.0f;
            pos.y = Random.value * 2 - 1.0f;
            pos.z = Random.value * 2 - 1.0f;
            pos.Normalize();

            pos *= Random.value * 5;

            particleArray[i].position = pos;

            particleArray[i].velocity = Vector3.zero;

            //Random values for testing purposes
            particleArray[i].lifetime = Random.Range(1, 7);
        }

        //Create new compute buffer
        particleBuffer = new ComputeBuffer(particleCount, ParticleSize);

        //Set data to our initialized array
        particleBuffer.SetData(particleArray);

        kernelId = computeShader.FindKernel("CSMain");

        //Only need x thread
        computeShader.GetKernelThreadGroupSizes(kernelId, out uint threadsX, out _, out _);

        //Calculate group size from x thread count
        groupSizeX = Mathf.CeilToInt((float)particleCount / (float)threadsX);

        //Set buffer on compute shader
        computeShader.SetBuffer(kernelId, "particleBuffer", particleBuffer);

        //Set buffer on particle material
        particleMaterial.SetBuffer("particleBuffer", particleBuffer);

        renderParams = new RenderParams(particleMaterial);
    }
}
