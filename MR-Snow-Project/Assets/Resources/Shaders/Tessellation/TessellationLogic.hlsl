///Contains data needed for 
///each triangle for tessellation 
struct TessellationData
{
    float triangleEdge[3] : SV_TessFactor;
    float triangleCenter : SV_InsideTessFactor;
};

//Run hull stage on triangles
[domain("tri")]
[outputcontrolpoints(3)]
[outputtopology("triangle_cw")]
[partitioning("fractional_odd")]
[patchconstantfunc("PatchConstantFunction")]
PackedVaryings hull(InputPatch<PackedVaryings, 3> patch, uint id : SV_OutputControlPointID)
{
    return patch[id];
}

float CalcDistanceTessFactor(float3 worldPosition)
{
    const float minDist = 2;
    float dist = distance(worldPosition, _WorldSpaceCameraPos.xyz);
    float factor = clamp(1 - (dist - minDist) / (_MaxTessellationDist - minDist), 0.01, 1);

    return clamp(factor * _Tessellation, 0, _Tessellation);
}

TessellationData CalcTriEdgeTessFactors(float3 vertexTessFactors)
{
    TessellationData tess;

    tess.triangleEdge[0] = 0.5 * (vertexTessFactors.y + vertexTessFactors.z);
    tess.triangleEdge[1] = 0.5 * (vertexTessFactors.x + vertexTessFactors.z);
    tess.triangleEdge[2] = 0.5 * (vertexTessFactors.x + vertexTessFactors.y);
    tess.triangleCenter = (vertexTessFactors.x + vertexTessFactors.y + vertexTessFactors.z) / 3.0f;

    return tess;
}

TessellationData DistanceBasedTess(PackedVaryings vertex0, PackedVaryings vertex1, PackedVaryings vertex2)
{
    float3 vertexTessFactors;

    vertexTessFactors.x = CalcDistanceTessFactor(vertex0.positionWS);
    vertexTessFactors.y = CalcDistanceTessFactor(vertex1.positionWS);
    vertexTessFactors.z = CalcDistanceTessFactor(vertex2.positionWS);

    return CalcTriEdgeTessFactors(vertexTessFactors);
}

TessellationData PatchConstantFunction(InputPatch<PackedVaryings, 3> patch)
{
    return DistanceBasedTess(patch[0], patch[1], patch[2]);
}

void vert(inout PackedVaryings IN)
{
    //No vertex logic needs to execute here
    float3 posInObjectSpace = TransformWorldToObject(IN.positionWS);
    IN.positionCS = TransformObjectToHClip(posInObjectSpace);
}

#define INTERPOLATE(fieldName) data.fieldName = \
		        patch[0].fieldName * barycentricCoordinates.x + \
		        patch[1].fieldName * barycentricCoordinates.y + \
		        patch[2].fieldName * barycentricCoordinates.z;

[domain("tri")]
PackedVaryings domain(TessellationData factors, OutputPatch<PackedVaryings, 3> patch,
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
