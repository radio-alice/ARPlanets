Shader "Custom/CustomAr" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emission ("Emission", Color) = (1,1,1,1)
        _Strength ("Emission Strength", Float) = .5
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
        fixed3 _GlobalColorCorrection;

		struct Input {
			float2 uv_MainTex;
		};
        
		fixed4 _Color;
        fixed4 _Emission;
        float _Strength;

         void lightEstimation(Input IN, SurfaceOutput o, inout fixed4 _Color)
        {
            _Color.rgb *= _GlobalColorCorrection;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = _Emission * _Strength; 
            o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
