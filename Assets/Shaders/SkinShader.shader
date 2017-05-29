Shader "Custom/Test" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Subsurface Scatter Color", COLOR) = (1,1,1,1)
    _Strength ("Subsurface Attenuation", Range(0.0, 500.0)) = 0.5
    _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
  }
  SubShader {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
        #pragma surface surf WrapLambert

        fixed4 _Color;
        half _Strength;
        half _Glossiness;

        half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half NormalDotLightDir = dot (normalize(s.Normal), normalize(lightDir));
            half diff = NormalDotLightDir * 0.5 + 0.5;
            half scatter = pow(2.71828, -_Strength * pow((NormalDotLightDir - 0.5), 2.0));

            half3 h = normalize (lightDir + viewDir);
            half diff2 = max (0, dot (s.Normal, lightDir));
            float nh = max (0, dot (s.Normal, h));
            float spec = pow (nh, 32.0);
            half3 specCol = spec * _Glossiness;

            half4 c;
            // (1 minus scatter * normal albedo shading) + (scatter * shaded color)
            c.rgb = ((1.0 - scatter) * ((s.Albedo * _LightColor0.rgb * diff * atten) + (_LightColor0.rgb * specCol)))
                    + (scatter * _Color.rgb * _LightColor0.rgb * diff * atten);
            c.a = s.Alpha;
            return c;
        }

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
            void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            o.Gloss = _Glossiness;
        }
        ENDCG
  }
  Fallback "Diffuse"
}
