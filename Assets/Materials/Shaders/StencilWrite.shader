Shader "Custom/StencilWrite"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [IntRange] _StencilRef ("Stencil reference value", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "Queue" = "Geometry-1" 
        }
        ZTest Off
        LOD 100

        Stencil
        {
            Ref [_StencilRef]
            Comp Always
            Pass Replace
            Fail Zero
        }

        Pass
        {
            Blend Off 
            ZWrite Off

            CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;

			struct appdata{
				float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
			};

			struct v2f{
				float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
            {
				return tex2D(_MainTex, i.uv);
			}

            ENDCG
        }
    }
}
