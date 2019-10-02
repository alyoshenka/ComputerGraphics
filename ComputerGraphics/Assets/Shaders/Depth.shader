Shader "Custom/PostProcessing/Depth"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [Header(Wave)]
        _WaveDistance("Distance from the player", Range(0, 20)) = 5
        _WaveTrail("Length of the trail", Range(0, 5)) = 1
        _WaveColor("Color of the wave", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CameraDepthTexture;

            float _WaveDistance;
            float _WaveTrail;
            float4 _WaveColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // get depth from camera texture
                float depth = tex2D(_CameraDepthTexture, i.uv).r;

                // make depth linear - values closer to camera are skewed with greater precision
                depth = Linear01Depth(depth);

                // change depth to distance from camera
                depth = depth * _ProjectionParams.z;

                                // get source color
                fixed4 source = tex2D(_MainTex, i.uv);

                // return if at skybox
                if(depth >= _ProjectionParams.z)
                {
                    return source;
                }

                // get the wave distance
                float waveFront = step(depth, _WaveDistance);

                // get the wave trail
                float waveTrail = smoothstep(_WaveDistance - _WaveTrail, _WaveDistance, depth);

                // get the wave
                float wave = waveFront * waveTrail;

                // add in scene color
                fixed4 col = lerp(source, _WaveColor, wave);

                return col;
            }
            ENDCG
        }
    }
}
