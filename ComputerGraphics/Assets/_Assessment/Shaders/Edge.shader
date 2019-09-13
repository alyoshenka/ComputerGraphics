Shader "Custom/Edge"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline ("Outline", Color) = ( 1, 1, 1, 1 )
        _Debug ("Debug", Color) = ( 0, 0, 0, 1 )
        _Other ("Other", Color) = ( 1, 1, 1, 1)
        _Scale ("Scale", Range(-10, 100)) = 5
        _DepthThreshold ("Depth threshold", Range(-100, 100)) = 0.2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  "LightMode" = "ShadowCaster" }
		Blend SrcAlpha OneMinusSrcAlpha
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
                //float2 depth : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CameraDepthTexture;
            sampler2D _LastCameraDepthTexture;
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
                UNITY_TRANSFER_FOG(o, o.vertex);
                //UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //return fixed4(1,1,1,1);
                return Linear01Depth(tex2D(_LastCameraDepthTexture , i.uv).r);

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

                // scale to depth
                float scaledEdgeDepth = _DepthThreshold * depth0;

                edgeDepth = edgeDepth > scaledEdgeDepth ? 1 : -1; // clamp

                // same process with normals

                // put it all into a color
                // float4 edgeColor = float4(_Color.rgb, _Color.a * edge);
                fixed4 col;
                if(edgeDepth > 0)
                {
                    // col = fixed4(_Outline.rgb, _Outline.a * edgeDepth);
                    col = _Outline;
                }
                else if(edgeDepth < 0)
                {
                    col = tex2D(_MainTex, i.uv);
                }
                else
                {
                    col = _Debug;
                }

                float depth = tex2D(_CameraDepthTexture, i.uv).r;

                if(depth > 0)
                {
                    col = _Outline;
                }
                else if(depth < 0)
                {
                    col = _Debug;
                }
                else{
                    col = _Other;
                }

                return col;               

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
