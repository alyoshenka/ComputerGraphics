Shader "Custom/DistanceSelection"
{
    Properties
    {
        _Tex1 ("First texture", 2D) = "white" {}
        _Tex2 ("Second texture", 2D) = "white" {}
        _Orig ("Origin vector", Vector) = (0, 0, 0, 1)
        _Dist ("Distance from origin", Range(-10, 10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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
                float2 useFirst : TEXCOORD1;
            };

            sampler2D _Tex1;
            sampler2D _Tex2;
            float3 _Orig;
            float _Dist;
            float4 _Tex1_ST;
            float4 _Tex2_ST;

            v2f vert (appdata v)
            {
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                bool useFirst = distance(_Orig, worldPos) < _Dist;
                v2f o;
                if(useFirst)
                {
                    o.useFirst.x = 1;
                }
                else
                {
                    o.useFirst.x = -1;
                }
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _Tex1);
                if(useFirst.x > 0)
                {
                    o.uv = TRANSFORM_TEX(v.uv, _Tex1);
                }
                else
                {
                    o.uv = TRANSFORM_TEX(v.uv, _Tex2);
                }
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                // sample the texture
                if(i.useFirst.x > 0)
                {
                    col = tex2D(_Tex1, i.uv);
                }
                else
                {
                    col = tex2D(_Tex2, i.uv);
                }
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }

            ENDCG
        }
    }
}
