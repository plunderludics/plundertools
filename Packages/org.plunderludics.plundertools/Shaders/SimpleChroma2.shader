Shader "Plundertools/SimpleChroma"
{
    Properties
    {
        // These are drawn by custom inspector so hide them in default inspector
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _KeyCount ("Key Count", Int) = 0
        [HideInInspector] _Key0 ("Key Color 0", Color) = (1,1,1,1)
        [HideInInspector] _Key1 ("Key Color 1", Color) = (1,1,1,1)
        [HideInInspector] _Key2 ("Key Color 2", Color) = (1,1,1,1)
        [HideInInspector] _Key3 ("Key Color 3", Color) = (1,1,1,1)
        [HideInInspector] _Key4 ("Key Color 4", Color) = (1,1,1,1)
        [HideInInspector] _Key5 ("Key Color 5", Color) = (1,1,1,1)
        [HideInInspector] _Key6 ("Key Color 6", Color) = (1,1,1,1)
        [HideInInspector] _Key7 ("Key Color 7", Color) = (1,1,1,1)
        [HideInInspector] _HSLTolerance0 ("HSL Tolerance 0", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance1 ("HSL Tolerance 1", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance2 ("HSL Tolerance 2", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance3 ("HSL Tolerance 3", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance4 ("HSL Tolerance 4", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance5 ("HSL Tolerance 5", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance6 ("HSL Tolerance 6", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _HSLTolerance7 ("HSL Tolerance 7", Vector) = (0.1,0.1,0.1,0)
        [HideInInspector] _RGBTolerance0 ("RGB Tolerance 0", Float) = 0.01
        [HideInInspector] _RGBTolerance1 ("RGB Tolerance 1", Float) = 0.01
        [HideInInspector] _RGBTolerance2 ("RGB Tolerance 2", Float) = 0.01
        [HideInInspector] _RGBTolerance3 ("RGB Tolerance 3", Float) = 0.01
        [HideInInspector] _RGBTolerance4 ("RGB Tolerance 4", Float) = 0.01
        [HideInInspector] _RGBTolerance5 ("RGB Tolerance 5", Float) = 0.01
        [HideInInspector] _RGBTolerance6 ("RGB Tolerance 6", Float) = 0.01
        [HideInInspector] _RGBTolerance7 ("RGB Tolerance 7", Float) = 0.01
        [HideInInspector] _Enable0 ("Enable 0", Float) = 1
        [HideInInspector] _Enable1 ("Enable 1", Float) = 1
        [HideInInspector] _Enable2 ("Enable 2", Float) = 1
        [HideInInspector] _Enable3 ("Enable 3", Float) = 1
        [HideInInspector] _Enable4 ("Enable 4", Float) = 1
        [HideInInspector] _Enable5 ("Enable 5", Float) = 1
        [HideInInspector] _Enable6 ("Enable 6", Float) = 1
        [HideInInspector] _Enable7 ("Enable 7", Float) = 1
        [HideInInspector] _Invert ("Invert", Float) = 0
        [HideInInspector] _Enable ("Enable", Float) = 1

        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Int) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Int) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend [_BlendSrc] [_BlendDst], SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "shared.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Invert;
            float _Enable;

            #define MAX_KEY_COUNT 8
            int _KeyCount;

            float4 _Key0;
            float4 _Key1;
            float4 _Key2;
            float4 _Key3;
            float4 _Key4;
            float4 _Key5;
            float4 _Key6;
            float4 _Key7;

            float _Enable0;
            float _Enable1;
            float _Enable2;
            float _Enable3;
            float _Enable4;
            float _Enable5;
            float _Enable6;
            float _Enable7;

            float4 _HSLTolerance0;
            float4 _HSLTolerance1;
            float4 _HSLTolerance2;
            float4 _HSLTolerance3;
            float4 _HSLTolerance4;
            float4 _HSLTolerance5;
            float4 _HSLTolerance6;
            float4 _HSLTolerance7;

            float _RGBTolerance0;
            float _RGBTolerance1;
            float _RGBTolerance2;
            float _RGBTolerance3;
            float _RGBTolerance4;
            float _RGBTolerance5;
            float _RGBTolerance6;
            float _RGBTolerance7;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                if (_Enable <= 0) {
                    return col; // if not enabled, return the original color
                }

                // convert to hsl
                float3 col_hsl = RGBtoHSL(col.rgb);

                bool inv = (_Invert > 0);
                bool includePixel = !inv;
                // calculate the distance to each key color using the new Key properties
                for (int j = 0; j < _KeyCount; j++)
                {
                    float3 keyColor;
                    if (j == 0) keyColor = _Key0.rgb;
                    else if (j == 1) keyColor = _Key1.rgb;
                    else if (j == 2) keyColor = _Key2.rgb;
                    else if (j == 3) keyColor = _Key3.rgb;
                    else if (j == 4) keyColor = _Key4.rgb;
                    else if (j == 5) keyColor = _Key5.rgb;
                    else if (j == 6) keyColor = _Key6.rgb;
                    else if (j == 7) keyColor = _Key7.rgb;
                    else break;

                    float enable;
                    if (j == 0) enable = _Enable0;
                    else if (j == 1) enable = _Enable1;
                    else if (j == 2) enable = _Enable2;
                    else if (j == 3) enable = _Enable3;
                    else if (j == 4) enable = _Enable4;
                    else if (j == 5) enable = _Enable5;
                    else if (j == 6) enable = _Enable6;
                    else if (j == 7) enable = _Enable7;
                    else break;
                    if (enable <= 0) continue; // skip if not enabled

                    float3 key_hsl = RGBtoHSL(keyColor);

                    float3 delta = hsl_delta(col_hsl, key_hsl);
                    float3 hslTolerance;
                    if (j == 0) hslTolerance = _HSLTolerance0.rgb;
                    else if (j == 1) hslTolerance = _HSLTolerance1.rgb;
                    else if (j == 2) hslTolerance = _HSLTolerance2.rgb;
                    else if (j == 3) hslTolerance = _HSLTolerance3.rgb;
                    else if (j == 4) hslTolerance = _HSLTolerance4.rgb;
                    else if (j == 5) hslTolerance = _HSLTolerance5.rgb;
                    else if (j == 6) hslTolerance = _HSLTolerance6.rgb;
                    else if (j == 7) hslTolerance = _HSLTolerance7.rgb;
                    else break;

                    float rgbTolerance;
                    if (j == 0) rgbTolerance = _RGBTolerance0;
                    else if (j == 1) rgbTolerance = _RGBTolerance1;
                    else if (j == 2) rgbTolerance = _RGBTolerance2;
                    else if (j == 3) rgbTolerance = _RGBTolerance3;
                    else if (j == 4) rgbTolerance = _RGBTolerance4;
                    else if (j == 5) rgbTolerance = _RGBTolerance5;
                    else if (j == 6) rgbTolerance = _RGBTolerance6;
                    else if (j == 7) rgbTolerance = _RGBTolerance7;
                    else break;

                    float rgbDistance = length(col.rgb - keyColor);

                    bool withinTolerance = rgbDistance < rgbTolerance || (delta.x < hslTolerance.x && delta.y < hslTolerance.y && delta.z < hslTolerance.z);
                    if (withinTolerance) {
                        includePixel = inv; // normal mode, exclude matched pixel; invert mode, exclude non-matched pixel
                        break;
                    }
                }

                col.a = includePixel ? 1 : 0; // set alpha to 1 if within tolerance, otherwise 0
                // return the color
                return col;
            }
            ENDCG
        }
    }
    
    CustomEditor "SimpleChroma2GUI"
}
