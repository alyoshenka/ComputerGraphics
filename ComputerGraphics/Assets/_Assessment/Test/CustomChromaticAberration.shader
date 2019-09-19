Shader "PostProcess/CustomChromaticAberration"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        sampler2D _MainTex_ST;

        float _RX;
        float _RY;

        float _GX;
        float _GY;

        float _BX;
        float _BY;

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

        v2f Vert(appdata v)
        {
            v2f o;
            o.vertex = float4(v.vertex.xy, 0.0, 1.0);
            o.uv = TransformTriangleVertexToUV(v.vertex.xy);
            #if UNITY_UV_STARTS_AT_TOP
                o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
            #endif
            return o;
        };

        float4 Frag(v2f i) : SV_Target
        {
            float r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, 
                float2(i.uv.x + _RX, i.uv.y + _RY)).r;
            float g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, 
                float2(i.uv.x + _GX, i.uv.y + _GY)).g;
            float b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, 
                float2(i.uv.x + _BX, i.uv.y + _BY)).b;

            return float4(r, g, b, 1);
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex Vert
                #pragma fragment Frag

            ENDHLSL
        }
    }
}

// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects

// https://github.com/Unity-Technologies/PostProcessing/blob/v2/PostProcessing/Shaders/StdLib.hlsl

// try for help
// https://stackoverflow.com/questions/49453696/get-world-position-in-shader-of-unitys-new-postprocessing-stack/50309382#50309382
