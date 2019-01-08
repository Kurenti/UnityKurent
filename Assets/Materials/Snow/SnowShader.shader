Shader "Custom/SnowShader" {
	Properties {
		_HeightMap("Height Map", 2D) = "white" {}
		_HeightMapScale("Height", Float) = 1
		_SnowColor ("SnowColor", Color) = (1,1,1,1)
		_SnowTex ("Snow (RGB)", 2D) = "white" {}
		_GroundColor("GroundColor", Color) = (1,1,1,1)
		_GroundTex("Ground (RGB)", 2D) = "white" {}
		_Splat("SplatMap", 2D) = "black" {}
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:disp

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.6

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		sampler2D _HeightMap;
		sampler2D _Splat;
		half _HeightMapScale;
		float _Displacement;

		sampler2D _GroundTex;
		fixed4 _GroundColor;
		sampler2D _SnowTex;
		fixed4 _SnowColor;

		struct Input {
			float2 uv_GroundTex;
			float2 uv_SnowTex;
			float2 uv_Splat;
		};

		half _Glossiness;
		half _Metallic;

		void disp(inout appdata v)
		{
			float d_Displaced = _Displacement * (1 - tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r);
			float d_HeightMap = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0)).r * _HeightMapScale;

			v.vertex.y += d_HeightMap + d_Displaced;
		}

		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half lerpAmount = tex2Dlod(_Splat, float4(IN.uv_Splat, 0, 0)).r;
			fixed4 c = lerp(tex2D(_SnowTex, IN.uv_SnowTex) * _SnowColor, tex2D(_GroundTex, IN.uv_GroundTex) * _GroundColor, lerpAmount);

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
