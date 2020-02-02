// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GroundSurface"
{
	Properties
	{
		_Forest("Forest", 2D) = "white" {}
		_ForestNormal("ForestNormal", 2D) = "bump" {}
		_WastelandNormal("WastelandNormal", 2D) = "bump" {}
		_Wasteland("Wasteland", 2D) = "white" {}
		_Splatter("Splatter", 2D) = "white" {}
		_Metalness0("Metalness 0", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_NormalStrength("Normal Strength", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _WastelandNormal;
		uniform float4 _WastelandNormal_ST;
		uniform float _NormalStrength;
		uniform sampler2D _ForestNormal;
		uniform float4 _ForestNormal_ST;
		uniform sampler2D _Splatter;
		uniform float4 _Splatter_ST;
		uniform sampler2D _Wasteland;
		uniform float4 _Wasteland_ST;
		uniform sampler2D _Forest;
		uniform float4 _Forest_ST;
		uniform float _Metalness0;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_WastelandNormal = i.uv_texcoord * _WastelandNormal_ST.xy + _WastelandNormal_ST.zw;
			float2 uv_ForestNormal = i.uv_texcoord * _ForestNormal_ST.xy + _ForestNormal_ST.zw;
			float2 uv_Splatter = i.uv_texcoord * _Splatter_ST.xy + _Splatter_ST.zw;
			float4 tex2DNode4 = tex2D( _Splatter, uv_Splatter );
			float3 lerpResult19 = lerp( UnpackNormal( tex2D( _WastelandNormal, uv_WastelandNormal ) ) , UnpackScaleNormal( tex2D( _ForestNormal, uv_ForestNormal ), _NormalStrength ) , tex2DNode4.r);
			o.Normal = lerpResult19;
			float2 uv_Wasteland = i.uv_texcoord * _Wasteland_ST.xy + _Wasteland_ST.zw;
			float2 uv_Forest = i.uv_texcoord * _Forest_ST.xy + _Forest_ST.zw;
			float4 lerpResult6 = lerp( tex2D( _Wasteland, uv_Wasteland ) , tex2D( _Forest, uv_Forest ) , tex2DNode4.r);
			o.Albedo = lerpResult6.rgb;
			o.Metallic = _Metalness0;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1391;524;1476;835;-323.7993;171.4413;1.6;True;True
Node;AmplifyShaderEditor.RangedFloatNode;16;1116.163,228.5235;Inherit;False;Property;_NormalStrength;Normal Strength;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;1254.149,842.2446;Inherit;True;Property;_Splatter;Splatter;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;875.5181,580.0872;Inherit;True;Property;_Wasteland;Wasteland;4;0;Create;True;0;0;False;0;-1;None;8daf317ad25d492449d9063d4ba1edfb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;860.7531,819.5726;Inherit;True;Property;_Forest;Forest;0;0;Create;True;0;0;False;0;-1;None;c2b430947a9549a4e94285ce6af3b459;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;1368.1,81.49845;Inherit;True;Property;_ForestNormal;ForestNormal;2;0;Create;True;0;0;False;0;-1;None;83213d04d9868424e9b9fbe66517cabf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;1302.999,-117.8412;Inherit;True;Property;_WastelandNormal;WastelandNormal;3;0;Create;True;0;0;False;0;-1;None;83213d04d9868424e9b9fbe66517cabf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;6;1603.801,381.9955;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;1901.225,73.01985;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;1194.956,85.59866;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;1701.074,656.5615;Inherit;False;Property;_Metalness0;Metalness 0;6;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;1676.328,740.5616;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;17;1811.545,923.4769;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;1053.606,366.6122;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;1683.799,-105.8412;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1899.147,288.3442;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;GroundSurface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;7;2294.811,1258.563;Inherit;False;765.1592;493.9802;Created by The Four Headed Cat @fourheadedcat - www.twitter.com/fourheadedcat;0;;1,1,1,1;0;0
WireConnection;8;5;16;0
WireConnection;6;0;3;0
WireConnection;6;1;5;0
WireConnection;6;2;4;1
WireConnection;19;0;18;0
WireConnection;19;1;8;0
WireConnection;19;2;4;1
WireConnection;0;0;6;0
WireConnection;0;1;19;0
WireConnection;0;3;12;0
WireConnection;0;4;13;0
ASEEND*/
//CHKSM=7D574F79FE430E8DAA307761ED780753A31C678D