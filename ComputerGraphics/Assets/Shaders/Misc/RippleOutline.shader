Shader "Custom/RippleOutline"{
	//show values to edit in inspector
	Properties{
        _OutlineColor ("Outline color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline thickness", Range(0, 1)) = 0.1

		_Color ("Tint", Color) = (0, 0, 0, 1)

        _Period ("Wave period", Range(0.01, 10)) = 0.5
        _Amplitude ("Wave amplitude", Range(0, 100)) = .0005
        _Ripple ("Ripple", Range(0, 5000)) = 1000
	}

	SubShader{
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

		Pass{
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader
			#pragma vertex vert
			#pragma fragment frag

			//tint of the texture
			fixed4 _Color;

			//the object data that's put into the vertex shader
			struct appdata{
				float4 vertex : POSITION;
			};

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 position : SV_POSITION;
			};

			//the vertex shader
			v2f vert(appdata v){
				v2f o;
				//convert the vertex positions from object space to clip space so they can be rendered
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				return _Color;
			}

			ENDCG
		}

        //The second pass where we render the outlines
        Pass{

            Cull Front

            CGPROGRAM

            //include useful shader functions
            #include "UnityCG.cginc"

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //texture and transforms of the texture
            sampler2D _MainTex;
            float4 _MainTex_ST;

            //tint of the texture
            fixed4 _Color;

            fixed4 _OutlineColor;
            float _OutlineThickness;

            float _Period;
            float _Amplitude;
            float _Ripple;

            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f{
                float4 position : SV_POSITION;
            };

            //the vertex shader
            v2f vert(appdata v){
                v2f o;

                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                float3 position = v.vertex + outlineOffset;

                position.y += _Amplitude * sin(_Period * _Time.w + v.vertex.z * _Ripple);

                o.position = UnityObjectToClipPos(position);

                return o;
            }

            //the fragment shader
            fixed4 frag(v2f i) : SV_TARGET{
                return _OutlineColor;
            }

            ENDCG
        }
	}
}

