Shader "Plundertools/Unlit-Extra" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _HueShift ("Hue Shift", Range(0.0, 1.0)) = 0.0
    _Saturation ("Saturation", Float) = 1.0
    _Lightness ("Lightness", Float) = 1.0
    _Contrast ("Contrast", Float) = 1.0
    _Tint ("Tint", Color) = (1,1,1,1)
    [Toggle] _Bypass ("Bypass", Float) = 0.0
}

SubShader {
    LOD 100

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "shared.hlsl"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord*_MainTex_ST.xy + _MainTex_ST.zw);
                // UNITY_APPLY_FOG(i.fogCoord, col);

                col.rgb = adjustColor(col.rgb, _Lightness, _Saturation, _Contrast, _HueShift, _Tint, _Bypass);

                // const float midpoint = pow(0.5, 2.2);
                // col.rgb = (col.rgb - midpoint) * _Contrast + midpoint;

                // col.rgb *= _Lightness;
                // // There is something weird going on here - if _Lightness is exactly equal to 1 it causes some visual artifacts for perfectly white pixels
                // // Setting _Lightness = 1.0001 or 0.9999 works just fine.
                // // Something to do with RGBtoHSL probably?
        
                // float3 hsl = RGBtoHSL(col.rgb);
                // hsl[0] = fmod(hsl[0] + _HueShift, 1.0);
                // hsl[1] *= _Saturation;
                // col.rgb = HSLtoRGB(hsl);

                // col.rgb *= _Tint;
                return col;
            }
        ENDCG
    }
}

}
