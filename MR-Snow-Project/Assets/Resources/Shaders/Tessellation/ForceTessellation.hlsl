#include "TessellationLogic.hlsl"

#pragma hull hull
#pragma domain domain
#pragma require tessellation tessHW

void ForceTess_float(in float4 VertexColor, in float2 UV, in float3 WorldPos, in float3 WorldNormal, out float Output)
{
    //Output is not needed, this function 
    //is used to force hull, tessellator, and domain shader stages
    //to run in shader graph
    Output = 1.0;
}
