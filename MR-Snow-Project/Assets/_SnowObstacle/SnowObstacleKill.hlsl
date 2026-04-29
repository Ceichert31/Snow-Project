void KillInsideObstacles(
    inout VFXAttributes attributes,
    StructuredBuffer<float4x4> Obstacles,
    uint ObstacleCount)
{
    for (uint i = 0u; i < ObstacleCount; i++)
    {
        float3 localP = mul(Obstacles[i], float4(attributes.position, 1.0)).xyz;

        if (all(abs(localP) < 0.5))
        {
            attributes.alive = false;
            return;
        }
    }
}
