Shader "Custom/ChromA"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Offset", Vector) = (0.01, 0, -0.01, 1)
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
            // make fog work
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float r = tex2D(_MainTex, float2(i.uv.x + _Offset.x, i.uv.y + _Offset.x)).r;
                float g = tex2D(_MainTex, float2(i.uv.x + _Offset.y, i.uv.y + _Offset.y)).g;
                float b = tex2D(_MainTex, float2(i.uv.x + _Offset.z, i.uv.y + _Offset.z)).b;
                fixed4 col = fixed4(r, g, b, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

// https://halisavakis.com/my-take-on-shaders-chromatic-aberration-introduction-to-image-effects-part-iv/
