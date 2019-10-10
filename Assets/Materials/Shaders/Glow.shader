// provides highlight effect for objects
Shader "Custom/Glow"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _BlurSize("Blur size", Range(0, 0.1)) = 0.01
        [IntRange]_Samples("Sample count", Range(2, 30)) = 10 
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 300

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

            float _BlurSize;
            int _Samples;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                for(float j = 0.0; j < _Samples; j++)
                {
                    float2 uv = i.uv + float2(0, (j / (_Samples - 1) - 0.5) * _BlurSize);
                    col += tex2D(_MainTex, uv);
                }
                col /= _Samples;
                return col;
            }
            ENDCG
        }
    }
}
