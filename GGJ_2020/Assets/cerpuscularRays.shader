Shader "cerpuscularRays"

{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Decay("Decay", Range(0,1)) = 1.0
		_Density("Density", Range(0,1)) = 1.0
		_Weight("Weight", Range(0,1)) = 1.0
		_Exposure("Exposure", Range(0, 1)) = 1.0

	}
		SubShader
		{
			// No culling or depth
		   // Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag

				#include "UnityCG.cginc"



				uniform sampler2D _MainTex;
				uniform sampler2D _CameraDepthTexture;
				float3 _LightPos;
				float _Density, _Weight, _Exposure, _Decay;
				const int NUM_SAMPLES = 150;

				float4 frag(v2f_img i) :COLOR
				{
					float4 light = float4(_LightPos.xyz, 1);
				fixed4 Out;
				half2 deltaTexCoord = i.uv - _LightPos.xy;

				deltaTexCoord *= 1.0f / NUM_SAMPLES * _Density;

				Out.xyz = tex2D(_MainTex, i.uv);
				half depth = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r);
				half illuminationDecay = 1.0f;
				for (int j = 0; j < NUM_SAMPLES; j++)
				{
					// Step sample location along ray.
					i.uv -= deltaTexCoord;
					// Retrieve sample at new location.
					half3 sample = tex2D(_MainTex, i.uv);
					half depth2 = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r);
					// Apply sample attenuation scale/decay factors.
					sample *= illuminationDecay * _Weight;
					// Accumulate combined color.
					Out.xyz += sample;
					// Update exponential decay factor.
					illuminationDecay *= _Decay;
				}





				Out.xyz *= _Exposure;
				Out.w = 1;
				return Out;
				}
				ENDCG
			}
		}
}
