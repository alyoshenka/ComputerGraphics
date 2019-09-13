Shader "Custom/HeightOffset"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _HeightTex("Height map", 2D) = "white" {}
        _HeightScale("Height scale", Range(0.0, 5.0)) = 0.0
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
            sampler2D _HeightTex;
            float _HeightScale;
            float _ScrollValue;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {     
                float2 scrollUV = v.uv;
                scrollUV.x += _ScrollValue;  
                float newY = tex2Dlod(_HeightTex, float4(scrollUV.xy, 0, 0)).x * _HeightScale;         
                v2f o;             
                v.vertex.y += newY;
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
