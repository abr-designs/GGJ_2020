﻿Shader "Unlit/UnlitDraw"

{

	Properties

	{

		_MainTex("Texture", 2D) = "white" {}
	_MainTex2("Albedo 2 (RGB)", 2D) = "white" {}
	_Blend("Texture Blend", Range(0,1)) = 0.0
		_Coordinate("Coordinate", Vector) = (0,0,0,0)
		
		_Color("Draw Color", Color) = (1,1,1,0)



		_Brush("Brush Size", Range(0.0, 50.0)) = 25.0

	}

		SubShader

		{

			Tags { "RenderType" = "Opaque" }

			LOD 100



			Pass

			{

				CGPROGRAM

				#pragma vertex vert

				#pragma fragment frag



				#include "UnityCG.cginc"



				struct appdata

				{

					float4 vertex : POSITION;

					float2 uv : TEXCOORD0;
					float2 uv2: TEXCOORD1;

				};



				struct v2f

				{

					float2 uv : TEXCOORD0;

					float4 vertex : SV_POSITION;

				};



				sampler2D _MainTex;

				float4 _MainTex_ST;

				fixed4 _Coordinate, _Color;

				float _Brush;



				v2f vert(appdata v)

				{

					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					return o;

				}



				fixed4 frag(v2f i) : SV_Target

				{

					// sample the texture

					fixed4 col = tex2D(_MainTex, i.uv);



					float draw = pow(saturate(1 - distance(i.uv, _Coordinate.xy)), 50 - _Brush);

					fixed4 drawCol = _Color * (draw * 1);



					return saturate(col + drawCol);



					//return col;

				}

				ENDCG

			}

		}

}