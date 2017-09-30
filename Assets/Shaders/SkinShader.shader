Shader "Custom/SkinShader" {
  Properties {
    _SkinColor ("Skin Color", COLOR) = (1,1,1,1)
    _ScatterColor ("Subsurface Scatter Color", COLOR) = (1,1,1,1) // can we automate scatter color?
    _Strength ("Subsurface Attenuation", Range(100.0, 0.1)) = 8.0
    _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
    _MainTex ("Clothing Texture", 2D) = "white" {}
  }
  SubShader {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
        #pragma surface surf WrapLambert

        sampler2D _MainTex;
        fixed4 _SkinColor;
        fixed4 _ScatterColor;
        half _Strength;
        half _Glossiness;

        half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half parallel = dot(normalize(s.Normal), normalize(lightDir));
            half parallelclamp = parallel * 0.75 + 0.25;
            half diff = max(0, dot(s.Normal, lightDir));

            // scatter = e ^ -strength * (parallel - 0.5)^2
            half scatter = pow(2.71828, -_Strength * pow((parallel - 0.5), 2.0));

            half3 lighttoview = normalize(lightDir + viewDir);
            float reflect = max(0, dot (s.Normal, lighttoview));
            float spec = pow(reflect, 6.0);

            half4 c;
            half3 skinColor = ((1.0 - scatter) * _SkinColor) + (scatter * _ScatterColor.rgb);
            skinColor.rgb = ((1.0 - s.Gloss) * skinColor) + (s.Gloss * s.Albedo.rgb);
            c.rgb = ((skinColor * _LightColor0.rgb * parallelclamp) + (_LightColor0.rgb * spec * _Glossiness)) * (atten + UNITY_LIGHTMODEL_AMBIENT);
            //c.rgb += UNITY_LIGHTMODEL_AMBIENT * _SkinColor;
            return c;
        }

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            o.Gloss =  tex2D (_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
  }
  Fallback "Diffuse"
}
