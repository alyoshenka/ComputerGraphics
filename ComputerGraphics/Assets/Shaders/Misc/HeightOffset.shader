// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/HeightOffset"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _HeightTex ("Height map", 2D) = "white" {}
        _PeakColor ("Peak color", Color) = (1, 1, 1, 1)
        _HeightScale ("Height scale", Range(0.01, 10)) = 0.01
        _YOffset ("Y offset", Range(-5, 5)) = 0.0
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
                float2 vars : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _HeightTex;
            float _HeightScale;
            float4 _PeakColor;
            float _ScrollForward;
            float _ScrollSideways;
            float _YOffset;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {     
                float2 scrollUV = v.uv;
                scrollUV.x += _ScrollForward;  
                scrollUV.y -= _ScrollSideways;
                float newY = tex2Dlod(_HeightTex, float4(scrollUV.xy, 0, 0)).x * _HeightScale - _HeightScale / 2 + _YOffset;         
                v2f o;  
                o.vars.x = (1 + newY) / 2;       
                v.vertex.y += newY;
                o.vertex = UnityObjectToClipPos(v.vertex);  
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = lerp(tex2D(_MainTex, i.uv), _PeakColor, i.vars.x);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
