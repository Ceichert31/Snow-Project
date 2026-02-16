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
}
