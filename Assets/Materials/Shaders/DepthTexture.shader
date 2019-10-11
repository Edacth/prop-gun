Shader "Debug/DepthTexture"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DepthLevel("Depth Level", Range(1, 3)) = 1
	}
		SubShader
		{
			Pass
			{
				ZWrite Off
				ZTest Always
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform sampler2D _CameraDepthTexture;
				uniform fixed _DepthLevel;
				uniform half4 _MainTex_TexelSize;

				struct v2f {
					//float4 	vertex 		: POSITION 	;//The position of the vertex in local space(model space)
					float3 	normal 		: NORMAL 	;//The normal of the vertex
					float2 	texcoord 	: TEXCOORD0 ;//	The texcoord coordinate for that vertex.Mesh being rendered must have at least one texture coordinate.The third and fourth floats in the vector represent a 3rd texcoord dimension and a scale factor, and are rarely if ever used.
					float4 	texcoord1 	: TEXCOORD1 ;//	A second set of texcoord coordinates for the vertex. (Only two are supported.) Always present, but often not used.
					float4 	tangent 	: TANGENT 	;//All meshes have either calculated or imported tangents.
					float4 	color 		: COLOR 	;//The color value of this vertex specifically.Mesh must have colors defined, otherwise they default to <TODO>.
				};

				v2f vert(
					float4 vertex : POSITION, // vertex position input
					float2 texcoord : TEXCOORD0, // texture coordinate input
					out float4 outpos : SV_POSITION // clip space position output
				)
				{
					v2f o;
					o.texcoord = texcoord;
					outpos = UnityObjectToClipPos(vertex);
					o.texcoord1 = UnityObjectToClipPos(vertex);
					return o;
				}

				fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
				{
					
					// screenPos.xy will contain pixel integer coordinates.
					// use them to implement a checkerboard pattern that skips rendering
					// 4x4 blocks of pixels

					// checker value will be negative for 4x4 blocks of pixels
					// in a checkerboard pattern
					screenPos.xy = floor(screenPos.xy * 0.25) * 0.5;
					float checker = -frac(screenPos.r + screenPos.g);

					// clip HLSL instruction stops rendering a pixel if value is negative
					//clip(checker);

					// for pixels that were kept, read the texture and output it
					float depth = tex2D(_CameraDepthTexture, screenPos.xy / _ScreenParams.xy * 8.0f);
					fixed4 c = float4(depth, depth, depth,1);
					return c;
				}

				//fixed4 frag(v2f o, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
				//{
				//	
				//	depth = Linear01Depth(depth);
				//	return screenPos;//depth*1000;
				//}

				ENDCG
			}
		}
}