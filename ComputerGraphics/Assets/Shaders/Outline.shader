// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PostProcessing/Outline"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _Outline ("Outline", Color) = ( 1, 1, 1, 1 )
        _Scale ("Scale", Range(0, 0.05)) = 0.002
        _DepthThreshold ("Depth threshold", Range(0, 0.0005)) = 0.00022
        _NormalThreshold("Normal threshold", Range(0, 3)) = 0.63
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
            // https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/CGIncludes/UnityCG.cginc

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

                float2 stereo : TEXCOORD1; // alphablend function
                float3 viewSpaceDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CameraDepthNormalsTexture;
            float4 _MainTex_TexelSize; // automatically tied to texture
            float _Scale; 
            float _DepthThreshold;
            float _NormalThreshold;
            float4 _Outline;
            float4x4 _ClipToView;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.stereo = TransformStereoScreenSpaceTex(o.uv, 1.0);
                o.viewSpaceDir = mul(_ClipToView, o.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // taken from https://roystan.net/articles/outline-shader.html

                // sample adjacent pixels and compare values
                // if very different, draw edge
                // using depth buffer
                // sample in X shape

                // alternatively incr by 1 as scale incr by 1
                // can incr edge 1 px at a time

                // get all the values

                float halfScaleFloor = floor(_Scale * 0.5);
                float halfScaleCeil = ceil(_Scale * 0.5);

                float2 bottomLeftUV = i.uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
                float2 bottomRightUV = i.uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

                float4 depthNormal0 = tex2D(_CameraDepthNormalsTexture, bottomLeftUV);
                float4 depthNormal1 = tex2D(_CameraDepthNormalsTexture, topRightUV);
                float4 depthNormal2 = tex2D(_CameraDepthNormalsTexture, bottomRightUV);
                float4 depthNormal3 = tex2D(_CameraDepthNormalsTexture, topLeftUV);

                float depth0, depth1, depth2, depth3;
                float3 normal0, normal1, normal2, normal3;

                DecodeDepthNormal(depthNormal0, depth0, normal0);
                DecodeDepthNormal(depthNormal1, depth1, normal1);
                DecodeDepthNormal(depthNormal2, depth2, normal2);
                DecodeDepthNormal(depthNormal3, depth3, normal3);

                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;

                // roberts cross algorithm
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * _Scale;

                // scale to distance from camera
                float scaledEdgeDepth = _DepthThreshold * depth0;

                edgeDepth = edgeDepth > scaledEdgeDepth ? 1 : 0;

                // same process with normals
                // 'flat' angles will not be picked up using the scaled distance

                float3 normalFiniteDifference0 = normal1 - normal0;
                float3 normalFiniteDifference1 = normal3 - normal2;

                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0)
                    + dot(normalFiniteDifference1, normalFiniteDifference1));
                edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;

                float edge = max(edgeDepth, edgeNormal);

                // return edge == 1 ? edge : tex2D(_MainTex, i.uv);


                // now we need to get rid of surface artifacts

                // edges with angles similar to that of the camera view angle
                // will be shaded because the distance between pixels
                // falls within the threshold

                // -> modulate depth threshold by surface normal

                // to get camera direction in view space (same as given normals)
                // multiply camera direction in clip space (given) by 
                // inverse prijection matrix (clip to view)
                
                return float4(i.viewSpaceDir, 1);
            }
            ENDCG
        }
    }
}

