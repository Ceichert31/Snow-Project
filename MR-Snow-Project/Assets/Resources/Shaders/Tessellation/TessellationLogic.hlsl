struct TessellationFactors
{
    float edge[3] : SV_TessFactor;
    float inside : SV_InsideTessFactor;
};

[patchconstantfunc("patchConstantFunction")]

[domain("tri")]
[outputcontrolpoints(3)]
[outputtopology("triangle_cw")]
[partitioning("fractional_odd")]
PackedVaryings hull(InputPatch<PackedVaryings, 3> patch, uint id : SV_OutputControlPointID)
{
    return patch[id];
}

float CalcDistanceTessFactor(float3 worldPosition)
{
    const float minDist = 2;
    float dist = distance(worldPosition, _WorldSpaceCameraPos);
    float factor = clamp(1 - (dist - minDist) / (_MaxTessellationDist - minDist), 0.01, 1);

    return clamp(factor * _Tessellation, 0, _Tessellation);
}

TessellationFactors CalcTriEdgeTessFactors(float3 vertexTessFactors)
{
    TessellationFactors tess;

    tess.edge[0] = 0.5 * (vertexTessFactors.y + vertexTessFactors.z);
    tess.edge[1] = 0.5 * (vertexTessFactors.x + vertexTessFactors.z);
    tess.edge[2] = 0.5 * (vertexTessFactors.x + vertexTessFactors.y);
    tess.inside = (vertexTessFactors.x + vertexTessFactors.y + vertexTessFactors.z) / 3.0f;

    return tess;
}

TessellationFactors DistanceBasedTess(PackedVaryings vertex0, PackedVaryings vertex1, PackedVaryings vertex2)
{
    float3 vertexTessFactors;

    vertexTessFactors.x = CalcDistanceTessFactor(vertex0.positionWS);
    vertexTessFactors.y = CalcDistanceTessFactor(vertex1.positionWS);
    vertexTessFactors.z = CalcDistanceTessFactor(vertex2.positionWS);

    return CalcTriEdgeTessFactors(vertexTessFactors);
}

TessellationFactors patchConstantFunction(InputPatch<PackedVaryings, 3> patch)
{
    return DistanceBasedTess(patch[0], patch[1], patch[2]);
}

void vert(inout PackedVaryings IN)
{
    //float4 heightMap = SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, IN.texCoord0, 0);

    //IN.positionWS.xyz += IN.normalWS;
}

#define INTERPOLATE(fieldName) data.fieldName = \
		        patch[0].fieldName * barycentricCoordinates.x + \
		        patch[1].fieldName * barycentricCoordinates.y + \
		        patch[2].fieldName * barycentricCoordinates.z;

[domain("tri")]
PackedVaryings domain(TessellationFactors factors, OutputPatch<PackedVaryings, 3> patch,
                      float3 barycentricCoordinates : SV_DomainLocation)
{
    PackedVaryings data;
    //Sets all data points equal to zero
    ZERO_INITIALIZE(PackedVaryings, data);


    INTERPOLATE(positionWS)
    INTERPOLATE(positionCS)

    /*#ifndef UNITY_PASS_SHADOWCASTER
    INTERPOLATE(normalWS)
    INTERPOLATE(texCoord0)
    INTERPOLATE(tangentWS)
    #endif*/

    vert(data);
    return data;
}
