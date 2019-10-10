// combines scene view with highlighted objects
Shader "Custom/Combine"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]_ObjectTex("Object texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _ObjectTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 object = tex2D(_ObjectTex, i.uv);
                fixed4 base = tex2D(_MainTex, i.uv);

                // if object texture is black or white
                if(object.a == 0)
                {
                    return base;
                }
                else
                {
                    return object;
                }
            }
            ENDCG
        }
    }
}
