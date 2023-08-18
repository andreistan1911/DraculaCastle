#ifndef MAINLIGHT_INCLUDED
#define MAINLIGHT_INCLUDED

half4 TransformWorldToShadowCoord(float3 worldPos)
{
    float4 homogenousPos = float4(worldPos, 1.0);
    float4 shadowCoord;
    shadowCoord.xyz = worldPos.xyz * 0.5 + 0.5;
    shadowCoord.w = 1.0;
    return half4(shadowCoord);
}

void MainLight_float(in float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
    Direction = half3(0, 1, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#ifdef SHADERGRAPH_PREVIEW
    Direction = half3(0, 1, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
#if SHADOWS_SCREEN
    half4 clipPos = TransformWorldToHClip(WorldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif

#if defined(UNIVERSAL_LIGHTING_INCLUDED)
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
#endif
}

#endif
