// The name provided below is what will appear in the editor
//   Use '/' to organize shaders into specific categories (or create your own)
Shader "Examples/BasicUnlit"
{
	// Defines shader properties that can be exposed to the inspector
	//   These are often defined ahead of time in your materials
	Properties
	{
		// Convention is as follows:
		//   _PropertyName ("InspectorName", Type) = default-value
		_MainTex("Texture", 2D) = "white" {}

	// Note that the name of the property (and its "type") must match the name
	// of the field as it is defined in the subshader below.
	}

		// Defines subshaders (like individual shader programs inside of this mega-shader)
		//   Shaders can have multiple subshaders for different needs (aesthetics/performance)
		SubShader
	{
		// Tags provide meta-data to the renderer affect things like render order
		Tags { "RenderType" = "Opaque" }
		// Level-of-detail value for this shader. Denotes complexity.
		//   More complex shaders should have a higher LOD value.
		//   The maximum LOD value can be defined in script.
		//   see: https://docs.unity3d.com/Manual/SL-ShaderLOD.html
		LOD 100

		// Defines a pass for this subshader
		//   Subshaders can have multiple passes that participate at different
		//   stages of the render process.
		Pass
		{
		// MARKS THE START OF THE CGPROGRAM
		CGPROGRAM

		// Declare/define any preprocessor things here
		//   Pragmas allow you to set flags

		//   In this case, we're indicating the name of the vertex and fragment shaders
		//      Our vertex shader is called "vert"
		//      Our fragment shader is called "frag"
		#pragma vertex vert
		#pragma fragment frag
		//   This pragma apparently will "make fog work" accordin' to Unity
		#pragma multi_compile_fog
		//   You can also include files that are chockfull of helper funcs
		#include "UnityCG.cginc"

		// User-defined types with variable/attribute pairs
		//   These structs can be named anything.
		//
		//   The type returned by the vertex shader must match the parameter-type
		//   for the fragment shader.

		// This struct imports data from the vertex for use in the vertex shader.
		struct appdata
		{
		  float4 vertex : POSITION;
		  float2 uv     : TEXCOORD0;
		};

	// This struct is outputted by the vertex shader and is received by the fragment shader.
	struct v2f
	{
	  float4 vertex : SV_POSITION;
	  float2 uv     : TEXCOORD0;
	  UNITY_FOG_COORDS(1)
	};

	// subshader-wide variables
	sampler2D _MainTex;
	float4 _MainTex_ST;

	// vertex shader
	v2f vert(appdata v)
	{
	  v2f o;
	  o.vertex = UnityObjectToClipPos(v.vertex);
	  o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	  UNITY_TRANSFER_FOG(o,o.vertex);
	  return o;
	}

	// fragment shader
	//   - SV_Target[N] refers to the frame buffer that the fragment will be
	//     written to. In this case, the number is omitted, so it defaults to
	//     0.
	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = tex2D(_MainTex, i.uv);
	// apply fog
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
  }

		// MARKS THE END OF THE CGPROGRAM
		ENDCG
	  }
	}
}