Shader "Custom/StandardScrolling" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _TuningScale ("Tuning Scale", float) = 1.0
        _TuningOffset ("Tuning Offset", float) = 0.0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
            #pragma surface surf Lambert

            struct Input {
                float2 uv_MainTex;
                float2 uv_BumpMap;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _TuningScale;
            float _TuningOffset;

            void surf (Input IN, inout SurfaceOutput o) {
                o.Albedo = (tex2D(_MainTex, IN.uv_MainTex).rgb * _TuningScale) + _TuningOffset;
                o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            }
        ENDCG
    }
  Fallback "Diffuse"
}
