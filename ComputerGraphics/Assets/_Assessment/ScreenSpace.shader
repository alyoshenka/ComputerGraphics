Shader "Unlit/ScreenSpace"
{
	Properties
	{
		_ZOffset("ZOffset", Float) = 2.0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		ZTest Always
		ZWrite Off
		Cull Off

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

			fixed _ZOffset;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = v.vertex;
				o.vertex.z += _ZOffset;
				o.uv = v.uv;
				return o;
			}

			sampler2D _CameraDepthTexture;

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 depthTex = tex2D(_CameraDepthTexture, i.uv);
				return Linear01Depth(depthTex.r);
			}
			ENDCG
		}
	}
}