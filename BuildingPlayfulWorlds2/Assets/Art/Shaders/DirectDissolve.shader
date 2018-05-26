Shader "Custom/DirectDissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_StartingVector("Starting point of the dissolve", Vector) = (0, 0, 0, 0)
		_DissolveSize("Size of the dissolve", float) = 0.0
		_DissolveTexture("Texture of the dissolve", 2D) = "white" {}
		_SliceAmount("Slice shit", Range(0,1)) = 0.0

		_ColorEmission("Color emission", Color) = (1,1,1,1)

		_StartingY("Starting point", float) = 0.0
		_DissolveY("Size dissolve", float) = 0.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DissolveTexture;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float2 uv_DissolveTexture;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float3 _StartingVector;
		float _DissolveSize;
		float _SliceAmount;

		float _StartingY;
		float _DissolveY;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			/* float DiffuseY = ((IN.worldPos.y - (_StartingVector.y - _DissolveSize)) * ((_StartingVector.y + _DissolveSize) - IN.worldPos.y) * -1);
			float DiffuseX = ((IN.worldPos.x - (_StartingVector.x - _DissolveSize)) * ((_StartingVector.x + _DissolveSize) - IN.worldPos.x) * -1);
			float DiffuseZ = ((IN.worldPos.z - (_StartingVector.z - _DissolveSize)) * ((_StartingVector.z + _DissolveSize) - IN.worldPos.z) * -1);

			float DiffuseYDouble = ((IN.worldPos.y - (_StartingVector.y - _DissolveSize * 2)) * ((_StartingVector.y + _DissolveSize * 2) - IN.worldPos.y) * -1);
			float DiffuseXDouble = ((IN.worldPos.x - (_StartingVector.x - _DissolveSize * 2)) * ((_StartingVector.x + _DissolveSize * 2) - IN.worldPos.x) * -1);
			float DiffuseZDouble = ((IN.worldPos.z - (_StartingVector.z - _DissolveSize * 2)) * ((_StartingVector.z + _DissolveSize * 2) - IN.worldPos.z) * -1); */
			//clip(DiffuseY);
			//clip(DiffuseX);
			//clip(DiffuseZ);
			//float DiffuseDouble = max(DiffuseYDouble, DiffuseXDouble);

			//float DiffuseONE = tex2D(_DissolveTexture, IN.uv_DissolveTexture).rgb - _SliceAmount;
			//clip(DiffuseONE);

			float _DissolveTextureCalc = (tex2D(_DissolveTexture, IN.uv_DissolveTexture) * _DissolveSize) + _SliceAmount;

			float transition = _StartingVector.y - IN.worldPos.y;
			float transitionX = _StartingVector.x - IN.worldPos.x;
			
			float trans1 = _StartingY + (transition + _DissolveTextureCalc);
			float trans2 = _StartingY + (transitionX + _DissolveTextureCalc);
			float trans3 = _StartingY - (transition - _DissolveTextureCalc);
			float trans4 = _StartingY - (transitionX - _DissolveTextureCalc);

			float trans5 = min(trans1, trans3) * -1;
			float trans6 = min(trans2, trans4) * -1;

			clip(min(trans5, trans6));

			//clip(_StartingY + (transitionX + (tex2D(_DissolveTexture, IN.uv_DissolveTexture)) * _DissolveSize) + _SliceAmount);


			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
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
