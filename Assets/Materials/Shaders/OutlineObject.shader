Shader "Custom/OutlineObject"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _Outline ("Outline", Color) = ( 1, 1, 1, 1 )
        _Scale ("Scale", Range(0, 100)) = 5
        _DepthThreshold ("Depth threshold", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

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

            uniform sampler2D _ObjectDepth;
            float4 _MainTex_TexelSize;
            float _Scale; 
            float _DepthThreshold;
            float4 _Outline;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // taken from https://roystan.net/articles/outline-shader.html

                // sample adjacent pixels and compare values
                // if very different, draw edge
                // using depth buffer
                // sample in X shape

                // alternatively incr by 1 as scale incr by 1
                // can incr edge 1 px at a time
                float halfScaleFloor = floor(_Scale * 0.5);
                float halfScaleCeil = ceil(_Scale * 0.5);

                float2 bottomLeftUV = i.uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
                float2 bottomRightUV = i.uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

                // sample depth texture using UVs
                // CALCULATE MY OWN DEPTH
                float depth0 = tex2D(_ObjectDepth, bottomLeftUV).r;
                float depth1 = tex2D(_ObjectDepth, topRightUV).r;
                float depth2 = tex2D(_ObjectDepth, bottomRightUV).r;
                float depth3 = tex2D(_ObjectDepth, topLeftUV).r;

                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;

                // roberts cross algorithm
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * _Scale;

                // scale to distance from camera
                float scaledEdgeDepth = _DepthThreshold * depth0;

                return edgeDepth > scaledEdgeDepth ? _Outline : 0;
            }
            ENDCG
        }
    }
}
