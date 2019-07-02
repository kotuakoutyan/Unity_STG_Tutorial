Shader "Test/Alpha" 
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma shader_feature _EMISSION
		#pragma multi_compile_instancing
		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
		};

		sampler2D _MainTex;

		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			float alpha = abs(dot(IN.viewDir, IN.worldNormal)) * 0.1f;
			fixed4 c =tex2D(_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
			c = c * (1 - alpha) + fixed4(1,1,1,1) * alpha;
			o.Albedo = c.rgb;
			o.Alpha = 1;
			o.Emission = c.rgb;
		}
		ENDCG
	}
		FallBack "Diffuse"
}