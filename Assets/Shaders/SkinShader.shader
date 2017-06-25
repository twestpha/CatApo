Shader "Custom/SkinShader" {
  Properties {
    //_MainTex ("Texture", 2D) = "white" {}
    _SkinColor ("Skin Color", COLOR) = (1,1,1,1)
    _ScatterColor ("Subsurface Scatter Color", COLOR) = (1,1,1,1)
    _Strength ("Subsurface Attenuation", Range(50.0, 0.1)) = 8.0
    _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
  }
  SubShader {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
        #pragma surface surf WrapLambert

        fixed4 _SkinColor;
        fixed4 _ScatterColor;
        half _Strength;
        half _Glossiness;

        half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half parallel = dot(normalize(s.Normal), normalize(lightDir));
            half parallelclamp = parallel * 0.75 + 0.25;
            // scatter = e ^ -strength * (parallel - 0.5)^2
            half scatter = pow(2.71828, -_Strength * pow((parallel - 0.5), 2.0));

            half3 lighttoview = normalize(lightDir + viewDir);
            float reflect = max(0, dot (s.Normal, lighttoview));
            float spec = pow(reflect, 2.0);

            half4 c;
            // (1 minus scatter * (albedo shading + specular highlight)) + (scatter * shaded color)
            c.rgb = ((1.0 - scatter) * ((_SkinColor * _LightColor0.rgb * parallelclamp * atten) + (_LightColor0.rgb * spec * _Glossiness)))
                    + (scatter * _ScatterColor.rgb * _LightColor0.rgb * parallelclamp * atten);
            c.a = s.Alpha;
            return c;
        }

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            //o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            //o.Gloss = _Glossiness;
        }
        ENDCG
  }
  Fallback "Diffuse"
}
