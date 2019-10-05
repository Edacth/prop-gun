Shader "Custom/OutlineObject"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]_ObjectTex("Object Texture", 2D) = "blue" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_ST;

    sampler2D _ObjectTex;

    struct frag_collect_out
    {
        half4 dest0 : SV_Target0;
        half4 dest1 : SV_Target1;
    };

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

    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        return o;
    }

    frag_collect_out frag_collect (v2f i) : SV_Target1
    {
        frag_collect_out o;
        o.dest1 = tex2D(_ObjectTex, i.uv);
        o.dest0 = tex2D(_MainTex, i.uv);
        return o;
    }

    fixed4 frag_combine (v2f i) : SV_Target
    {
        return lerp(tex2D(_ObjectTex, i.uv), tex2D(_MainTex, i.uv), 0.5);


        fixed4 c = tex2D(_ObjectTex, i.uv);
        if(0 != c.r)
        {
            return c;
        }
        else
        {
            return tex2D(_MainTex, i.uv);
        }
    }

    ENDCG

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        LOD 300

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_collect          
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_combine          
            ENDCG
        }
    }
}
