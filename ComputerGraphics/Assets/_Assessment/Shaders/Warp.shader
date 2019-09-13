Shader "Custom/Warp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Period ("Wave period", Range(0.01, 10)) = 0.5
        _Amplitude ("Wave amplitude", Range(0, 0.001)) = .0005
        _Ripple ("Ripple", Range(0, 5000)) = 1000
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Period;
            float _Amplitude;
            float _Ripple;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.x += _Amplitude * sin(_Period * _Time.w + v.vertex.z * _Ripple);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
