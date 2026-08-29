Shader "Hidden/MixEight"
{
    Properties
    {
        _mix0 ("Mix 0", Range(0, 1)) = 0
        _tex0 ("Texture 0", 2D) = "white" {}
        _mix1 ("Mix 1", Range(0, 1)) = 0
        _tex1 ("Texture 1 1", 2D) = "white" {}
        _mix2 ("Mix 2", Range(0, 1)) = 0
        _tex2 ("Texture 2", 2D) = "white" {}
        _mix3 ("Mix 3", Range(0, 1)) = 0
        _tex3 ("Texture 3", 2D) = "white" {}
        _mix4 ("Mix 4", Range(0, 1)) = 0
        _tex4 ("Texture 4", 2D) = "white" {}
        _mix5 ("Mix 5", Range(0, 1)) = 0
        _tex5 ("Texture 5", 2D) = "white" {}
        _mix6 ("Mix 6", Range(0, 1)) = 0
        _tex6 ("Texture 6", 2D) = "white" {}
        _mix7 ("Mix 7", Range(0, 1)) = 0
        _tex7 ("Texture 7", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float1 _mix0;
            sampler2D _tex0;
            fixed4 _tex0_ST;

            float1 _mix1;
            sampler2D _tex1;
            fixed4 _tex1_ST;

            float1 _mix2;
            sampler2D _tex2;
            fixed4 _tex2_ST;

            float1 _mix3;
            sampler2D _tex3;
            fixed4 _tex3_ST;

            float1 _mix4;
            sampler2D _tex4;
            fixed4 _tex4_ST;

            float1 _mix5;
            sampler2D _tex5;
            fixed4 _tex5_ST;

            float1 _mix6;
            sampler2D _tex6;
            fixed4 _tex6_ST;

            float1 _mix7;
            sampler2D _tex7;
            fixed4 _tex7_ST;

            fixed4 frag(v2f i) : SV_Target {


                const float2 uv0 = TRANSFORM_TEX(i.uv, _tex0);
                const float2 uv1 = TRANSFORM_TEX(i.uv, _tex1);
                const float2 uv2 = TRANSFORM_TEX(i.uv, _tex2);
                const float2 uv3 = TRANSFORM_TEX(i.uv, _tex3);
                const float2 uv4 = TRANSFORM_TEX(i.uv, _tex4);
                const float2 uv5 = TRANSFORM_TEX(i.uv, _tex5);
                const float2 uv6 = TRANSFORM_TEX(i.uv, _tex6);
                const float2 uv7 = TRANSFORM_TEX(i.uv, _tex7);

                const float1 i0 = step(uv0.x, 1) * step(0, uv0.x) * step(uv0.y, 1) * step(0, uv0.y);
                const float1 i1 = step(uv1.x, 1) * step(0, uv1.x) * step(uv1.y, 1) * step(0, uv1.y);
                const float1 i2 = step(uv2.x, 1) * step(0, uv2.x) * step(uv2.y, 1) * step(0, uv2.y);
                const float1 i3 = step(uv3.x, 1) * step(0, uv3.x) * step(uv3.y, 1) * step(0, uv3.y);
                const float1 i4 = step(uv4.x, 1) * step(0, uv4.x) * step(uv4.y, 1) * step(0, uv4.y);
                const float1 i5 = step(uv5.x, 1) * step(0, uv5.x) * step(uv5.y, 1) * step(0, uv5.y);
                const float1 i6 = step(uv6.x, 1) * step(0, uv6.x) * step(uv6.y, 1) * step(0, uv6.y);
                const float1 i7 = step(uv7.x, 1) * step(0, uv7.x) * step(uv7.y, 1) * step(0, uv7.y);

                const float1 total =
                    i0 * _mix0
                    + i1 * _mix1
                    + i2 * _mix2
                    + i3 * _mix3
                    + i4 * _mix4
                    + i5 * _mix5
                    + i6 * _mix6
                    + i7 * _mix7;

                const fixed4 col0 = (i0 * _mix0 / total) * tex2D(_tex0, uv0);
                const fixed4 col1 = (i1 * _mix1 / total) * tex2D(_tex1, uv1);
                const fixed4 col2 = (i2 * _mix2 / total) * tex2D(_tex2, uv2);
                const fixed4 col3 = (i3 * _mix3 / total) * tex2D(_tex3, uv3);
                const fixed4 col4 = (i4 * _mix4 / total) * tex2D(_tex4, uv4);
                const fixed4 col5 = (i5 * _mix5 / total) * tex2D(_tex5, uv5);
                const fixed4 col6 = (i6 * _mix6 / total) * tex2D(_tex6, uv6);
                const fixed4 col7 = (i7 * _mix7 / total) * tex2D(_tex7, uv7);

                return col0 + col1 + col2 + col3 + col4 + col5 + col6 + col7;
            }
            ENDCG
        }
    }
}