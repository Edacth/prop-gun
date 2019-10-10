Shader "Unlit/Intersection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
		ZWrite Off
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float2 screenuv : THING;
				float depth : THING2;
            };

            sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) / 2;
				o.screenuv.y = 1 - o.screenuv.y;
				o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture,float2(0.5,0.5)).zw);
				float diff = screenDepth - i.depth;
				float intersect = 0;

				if (diff > 0)
				{
					intersect = 1 - smoothstep(0, _ProjectionParams, diff);
				}
				// sample the texture
                fixed4 col = screenDepth;
                return col;
            }
            ENDCG
        }
    }
}
