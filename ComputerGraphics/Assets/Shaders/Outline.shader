// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PostProcessing/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline ("Outline", Color) = ( 1, 1, 1, 1 )
        _Debug ("Debug", Color) = ( 0, 0, 0, 1 )
        _Other ("Other", Color) = ( 1, 1, 1, 1)
        _Scale ("Scale", Range(-10, 100)) = 5
        _DepthThreshold ("Depth threshold", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;
            float4 _MainTex_TexelSize; // automatically tied to texture
            float _Scale; 
            float _DepthThreshold;
            float4 _Outline;
            float4 _Debug;
            float4 _Other;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz; // https://forum.unity.com/threads/world-space-normal.58810/
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = 0;
                c.rgb = i.normal * 0.5 + 0.5;
                // return c;

                fixed4 col = (i.normal.x, i.normal.y, i.normal.z, 1.0);
                // return col;




                // taken from https://roystan.net/articles/outline-shader.html

                // sample adjacent pixels and compare values
                // if very different, draw edge
                // using depth buffer
                // sample in X shape

                // alternatively incr by 1 as scale incr by 1
                // can incr edge 1 px at a time
                float halfScaleFloor = floor(_Scale * 0.5);
                float halfScaleCeil = ceil(_Scale * 0.5);

                float2 bottomLeftUV = i.uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
                float2 bottomRightUV = i.uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

                // sample depth texture using UVs
                // CALCULATE MY OWN DEPTH
                float depth0 = tex2D(_CameraDepthTexture, bottomLeftUV).r;
                float depth1 = tex2D(_CameraDepthTexture, topRightUV).r;
                float depth2 = tex2D(_CameraDepthTexture, bottomRightUV).r;
                float depth3 = tex2D(_CameraDepthTexture, topLeftUV).r;

                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;

                // roberts cross algorithm
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * _Scale;

                // scale to distance from camera
                float scaledEdgeDepth = _DepthThreshold * depth0;

                edgeDepth = edgeDepth > scaledEdgeDepth ? 1 : 0;

                return edgeDepth;

                // same process with normals
                // 'flat' angles will not be picked up using the scaled distance

                float3 normal0 = tex2D(_CameraNormalsTexture, bottomLeftUV).rgb;
                float3 normal1 = tex2D(_CameraNormalsTexture, topRightUV).rgb;
                float3 normal2 = tex2D(_CameraNormalsTexture, bottomRightUV).rgb;
                float3 normal3 = tex2D(_CameraNormalsTexture, topLeftUV).rgb;
            }
            ENDCG
        }
    }
}

