// [Some stuff that i don't really understand copied from https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/DefaultResourcesExtra/UI/UI-Default.shader]
Shader "Plundertools/UI-Extra" {
Properties {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    
    [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
    [HideInInspector] _Stencil ("Stencil ID", Float) = 0
    [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
    [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    [HideInInspector] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

    _Alpha  ("Alpha", Float) = 1.0

    // Processing:
    _HueShift ("Hue Shift", Range(0.0, 1.0)) = 0.0
    _Saturation ("Saturation", Float) = 1.0
    _Lightness ("Lightness", Float) = 1.0
    _Contrast ("Contrast", Float) = 1.0
    _Tint ("Tint", Color) = (1,1,1,1)
    [Toggle] _Bypass ("Bypass", Float) = 0.0
}

SubShader {
    Tags
    {
        "Queue"="Transparent"
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
        "PreviewType"="Plane"
        "CanUseSpriteAtlas"="True"
    }

    Stencil
    {
        Ref [_Stencil]
        Comp [_StencilComp]
        Pass [_StencilOp]
        ReadMask [_StencilReadMask]
        WriteMask [_StencilWriteMask]
    }

    Cull Off
    Lighting Off
    ZWrite Off
    ZTest [unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha, Zero One
    ColorMask [_ColorMask]

    Pass {
        Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "shared.hlsl"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float _Alpha;

            float _HueShift;
            float _Saturation;
            float _Contrast;
            float _Lightness;
            float4 _Tint;
            float _Bypass;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord*_MainTex_ST.xy + _MainTex_ST.zw) + _TextureSampleAdd;
                
                #ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (col.a - 0.001);
                #endif
                
                col.rgb = adjustColor(col.rgb, _Lightness, _Saturation, _Contrast, _HueShift, _Tint, _Bypass);
                col.rgb = clamp(0, col.rgb, 1); // Fixes weird issue where black pixels don't get occluded properly sometimes
                
                col.a *= _Alpha;
                return col;
            }
        ENDCG
    }
}

}
