// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/StandardNonOccluding"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_Radius("Alpha Radius", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5

		_Normal("Normal Map", 2D) = "bump" {}

		_Disabled("Disabled", Range(0.0, 1.0)) = 0.0
	}

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG

    SubShader {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
        CGPROGRAM
            #pragma surface surf Standard alpha:blend

            fixed4 _Color;
            half _Radius;
            sampler2D _MainTex;
            sampler2D _Normal;
            half _Glossiness;
            half _Disabled;

            struct Input {
                float2 uv_MainTex;
                float2 uv_Normal;
                float4 screenPos;
            };

            void surf (Input IN, inout SurfaceOutputStandard o) {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
                o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
                o.Smoothness = _Glossiness;

                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                screenUV.y = (((screenUV.y - 0.5f) *0.5f) + 0.5f);

                //o.Alpha = 0.5f;

                half alpha = length(screenUV - float2(0.5f, 0.5f)) / _Radius;
                alpha = pow(alpha, 1.5f);

                o.Alpha = _Disabled + alpha;
            }
        ENDCG
    }

    Fallback "VertexLit"
}
