Shader "Custom/StarAR"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _EmissionColor("Color", Color) = (0,0,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 150

        CGPROGRAM
        #pragma shader_feature _EMISSION
        #pragma surface surf Lambert noforwardadd finalcolor:lightEstimation
        
        fixed4 _Color;
        fixed3 _EmissionColor;
        sampler2D _MainTex;
        fixed3 _GlobalColorCorrection;

        struct Input
        {
            float2 uv_MainTex;
        };

        void lightEstimation(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            color.rgb *= _GlobalColorCorrection;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = _EmissionColor;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}
