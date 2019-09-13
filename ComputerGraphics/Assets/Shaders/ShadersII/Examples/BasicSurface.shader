// The name provided below is what will appear in the editor
//   Use '/' to organize shaders into specific categories (or create your own)
Shader "Examples/BasicSurface"
{
	// Defines shader properties that can be exposed to the inspector
	//   These are often defined ahead of time in your materials
	Properties
	{
		// Convention is as follows:
		//   _PropertyName ("InspectorName", Type) = default-value
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

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
			LOD 200

			// MARKS THE START OF THE CGPROGRAM
			CGPROGRAM

			// Declare/define any preprocessor things here
			//   Pragmas allow you to set flags
			// 
			// A list of different directives you can provide can be found online:
			//   https://docs.unity3d.com/Manual/SL-SurfaceShaders.html

			// This is the default set of pragmas in a Standard Surface shader.
			// Physically based Standard lighting model, and enable shadows on all light types
			// 
			// Let's break this down...
			//   'surface' allows us to provide the identifier for your surface shader's function
			//     In this case, it's called the 'surf' function, which follows the above keyword
			//
			//   'Standard` is the lighting model that we're using
			//     This also affects the parameters that your surface function will accept
			//     You may declare your own lighting model. See:
			//     https://docs.unity3d.com/Manual/SL-SurfaceShaderLighting.html
			//
			//   'fullforwardshadows' allows us to provide support for shadows in a forward rendering context
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			//
			// Most desktop
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
			  float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				// This "surf" function is our "surface" shader.
				//  This works similarly to how UE4's Material System or Unity's Shader1
				void surf(Input IN, inout SurfaceOutputStandard o)
				{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			  }
			  ENDCG
		}
			FallBack "Diffuse"
}
