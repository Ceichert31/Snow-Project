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

    [SerializeField]
    private float particleLifetime = 4;

    [SerializeField]
    private float spawnRadius = 3f;

    private float particleLifetimeMax;

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

            pos = Random.insideUnitCircle * spawnRadius;

            particleArray[i].position = pos;

            particleArray[i].velocity = Vector3.zero;

            //Random values for testing purposes
            particleArray[i].lifetime = Random.Range(1, particleLifetime);
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
        computeShader.SetBuffer(kernelId, "_ParticleBuffer", particleBuffer);

        //Set buffer on particle material
        particleMaterial.SetBuffer("_ParticleBuffer", particleBuffer);

        //Set the maximum lifetime of a particle for lerp calculations
        particleLifetimeMax = 1.0f / particleLifetime;
        particleMaterial.SetFloat("_MaxLifetime", particleLifetimeMax);

        renderParams = new RenderParams(particleMaterial);
        renderParams.worldBounds = new Bounds(Vector3.zero, 10000 * Vector3.one);
    }

    private void Update()
    {
        computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        computeShader.SetVector("_TargetDirection", transform.forward);
        computeShader.SetVector("_SpawnPosition", transform.position);
        computeShader.SetFloat("_SpawnRadius", spawnRadius);
        computeShader.SetFloat("_Lifetime", particleLifetime);

        //Run the compute shader
        computeShader.Dispatch(kernelId, groupSizeX, 1, 1);

        //Draw call for our particles
        Graphics.RenderPrimitives(renderParams, MeshTopology.Points, 1, particleCount);
    }

    private void OnDestroy()
    {
        particleBuffer?.Release();
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.green);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
