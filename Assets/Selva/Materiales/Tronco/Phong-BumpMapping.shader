Shader "TemasSelectos/Phong-BumpMapping"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal", 2D) = "white" {}
        _Kao ("Ambient Color", Color) = (1,1,1,1)
        _Kdo ("Diffuse Color", Color) = (1,1,1,1)
        _Kso ("Specular Color", Color) = (1, 1, 1, 1)
        _Q ("q", Float) = 10 //Shininess
        _DeltaTime ("Delta Time", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 n_uv : TEXCOORD2;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 n_uv : TEXCOORD2;
                float4 posWorld : TEXCOORD1;
            };

            uniform float4 _LightColor0;
            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float  _DeltaTime;
            float4 _Kao;
            float4 _Kdo;
            float4 _Kso;
            float _Q;

            v2f vert (appdata v)
            {
                v2f o;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
    
                o.n_uv = TRANSFORM_TEX(v.n_uv, _NormalMap);
                o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;

                i.normal = tex2D(_NormalMap, i.n_uv).xyz;
                float3 N = normalize(i.normal);
                float3 V = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

                // Ambient component
                float4 Ka = UNITY_LIGHTMODEL_AMBIENT * _Kao;

                // Diffuse component
                float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                float oneOverDistance = 1.0 / length(vert2LightSource);
                float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w); 
                float3 L = _WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w;
                float3 Kd = attenuation * _LightColor0.rgb * _Kdo.rgb * max(0.0, dot(N, L));

                // Specular component
                float3 Ks;
                if (dot(i.normal, L) < 0.0) 
                {
                    Ks = float3(0.0, 0.0, 0.0);
                }
                else
                {
                    Ks = attenuation * _LightColor0.rgb * _Kso.rgb * pow(max(0.0, dot(reflect(-L, N), V)), _Q);
                }

                col = (Ka + float4(Kd,1.0)+ float4(Ks,1.0)) * tex2D(_MainTex, i.uv) ;

                return col;
            }
            ENDCG
        }
    }
}
