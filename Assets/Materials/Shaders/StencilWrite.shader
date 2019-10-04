Shader "Custom/StencilWrite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [IntRange] _StencilRef ("Stencil reference value", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry - 1" }
        LOD 100

        Stencil
        {
            Ref [_StencilRef]
            Comp Always
            Pass Replace
        }

        Pass
        {
            Blend Zero One // ignore color returned by shader
            ZWrite Off // don't occlude

            CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct appdata{
				float4 vertex : POSITION;
			};

			struct v2f{
				float4 position : SV_POSITION;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				return 0;
			}
            ENDCG
        }
    }
}
