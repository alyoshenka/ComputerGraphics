Shader "Custom/HeightColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LevelColor("Level color", Color) = (1, 1, 1, 1)
        _ValleyColor("Valley color", Color) = (1, 0, 0, 1)
        _PeakColor("Peak color", Color) = (0, 0, 1, 1)
        _ValleyRange("Valley range", Range(-5, 0)) = -1.0
        _PeakRange("Peak range", Range(0, 5)) = 1.0
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

                float2 offset : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _LevelColor;
            float4 _ValleyColor;
            float4 _PeakColor;
            float _ValleyRange;
            float _PeakRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.offset = v.vertex;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 samp = tex2D(_MainTex, i.uv);

                fixed4 tint;
                if(i.offset.y < 0)
                {
                    tint = lerp(_LevelColor, _ValleyColor, i.offset.y / _ValleyRange);
                }
                else if(i.offset.y > 0)
                {
                    tint = lerp(_LevelColor, _PeakColor, i.offset.y / _PeakRange);
                }
                else
                {
                    tint = _LevelColor;
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return samp * tint;
            }
            ENDCG
        }
    }
}
