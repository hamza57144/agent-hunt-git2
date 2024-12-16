Shader "Custom/Vertex3Blend" {
	Properties {
		_MainTex ("Color r (RGB)", 2D) = "grey" {}
		_MainTex1 ("Color g (RGB)", 2D) = "grey" {}
		_MainTex2 ("Color b (RGB)", 2D) = "grey" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Vertexlit"
}