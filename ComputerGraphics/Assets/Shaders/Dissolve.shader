Shader "Custom/Dissolve"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DissolveTex("DissolveMap", 2D) = "white" {}
		_DissolveVal("DissolvedValue", Range(0.0, 1.0)) = 1.0
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _DissolveTex;
			float4 _MainTex_ST;
			float _DissolveVal;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 sampleA = tex2D(_DissolveTex, i.uv);
				if (sampleA.x < _DissolveVal) { discard; }
				return col;
			}

			ENDCG
        }
	}
}
