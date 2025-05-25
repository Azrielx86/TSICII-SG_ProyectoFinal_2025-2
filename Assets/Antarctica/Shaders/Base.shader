Shader "Antarctica/Base"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 1, 1, 0.5)
        _ScrollSpeed ("Scanline Speed", Float) = 1.0
        _GlowIntensity ("Glow Intensity", Float) = 1.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _ScrollSpeed;
            float _GlowIntensity;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += _Time.y * _ScrollSpeed;

                // Scanline effect
                float scanline = sin(uv.y * 100) * 0.1 + 0.9;

                // Base texture and color
                float4 texColor = tex2D(_MainTex, uv);
                float3 color = texColor.rgb * _Color.rgb * scanline;

                // Emission (adds glow)
                float3 emission = color * _GlowIntensity;

                return float4(emission, _Color.a);
            }
            ENDCG
        }
    }
}
