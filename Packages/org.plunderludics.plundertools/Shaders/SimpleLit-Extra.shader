// TODO: in principle i think this should be merged w mstandard (and that should be cleaned up and renamed (Standard-Extra?))
// to make a mega-shader with customizable params and also post-processing
Shader "Plundertools/SimpleLit-Extra"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        // Pre-shading color adjustment
        _HueShift ("Hue Shift", Range(0.0, 1.0)) = 0.0
        _Saturation ("Saturation", Float) = 1.0
        _Lightness ("Lightness", Float) = 1.0
        _Contrast ("Contrast", Float) = 1.0
        _Tint ("Tint", Color) = (1,1,1,1)
        // Post-shading color adjustment
        _SaturationPost ("Saturation (Post-shading)", Float) = 1.0
        _LightnessPost ("Lightness (Post-shading)", Float) = 1.0
        _ContrastPost ("Contrast (Post-shading)", Float) = 1.0
        [Toggle] _Bypass ("Bypass", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows finalcolor:finalcolor

        #include "shared.hlsl"

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _EmissionColor;

        float _HueShift;
        float _Saturation;
        float _Contrast;
        float _Lightness;
        float4 _Tint;
        float _Bypass;

        float _SaturationPost;
        float _ContrastPost;
        float _LightnessPost;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color; // [_Color is made redundant my _Tint below]

            o.Albedo = adjustColor(c.rgb, _Lightness, _Saturation, _Contrast, _HueShift, _Tint, _Bypass);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = _EmissionColor;
        }

        void finalcolor (Input IN, SurfaceOutputStandard o, inout fixed4 col)
        {
            col.rgb = adjustColor(col.rgb, _LightnessPost, _SaturationPost, _ContrastPost, 0, 1, _Bypass);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
