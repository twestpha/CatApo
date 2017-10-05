// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Highlight"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		// No culling or depth
		// Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert(appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MainTex;

            float whiteness(fixed4 input){
                return pow((input.r + input.g + input.b), 0.5) / 1.73;
            }

			fixed4 frag (v2f i) : SV_Target {
				fixed4 fragColor = tex2D(_MainTex, i.uv);
                float onePixelX = 1.0 / _ScreenParams.x;
                float onePixelY = 1.0 / _ScreenParams.y;

                // Ajusted Coordinates
                float xPixelPosition = i.uv.x * _ScreenParams.x;
                float yPixelPosition = i.uv.y * _ScreenParams.y;

                float threshold = 1.0;
                int reach = 16;
                float foundx = 0;
                float foundy = 0;

                for(int x = -reach; x < reach; ++x){
                    float2 sampleX = float2(i.uv.x + onePixelX * x, i.uv.y);
                    fixed4 sampleXFragColor = tex2D(_MainTex, sampleX);

                    if(whiteness(sampleXFragColor) > threshold ){
                        foundx = i.uv.x + onePixelX * x;
                    }
                }

                for(int y = -reach; y < reach; ++y){
                    float2 sampleY = float2(i.uv.x, i.uv.y + onePixelY * y);
                    fixed4 sampleYFragColor = tex2D(_MainTex, sampleY);

                    if(whiteness(sampleYFragColor) > threshold ){
                        foundy = i.uv.y + onePixelY * y;
                    }
                }

                if(foundx != 0){
                    float distanceX = (1.0 - (abs(i.uv.x - foundx) / (onePixelX * reach)));
                    fragColor += fixed4(1.0, 1.0, 1.0, 1.0) * distanceX * distanceX;
                }

                if(foundy != 0){
                    float distanceY = (1.0 - (abs(i.uv.y - foundy) / (onePixelY * reach)));
                    fragColor += fixed4(1.0, 1.0, 1.0, 1.0) * distanceY * distanceY;
                }



				return fragColor;
			}
			ENDCG
		}
	}
}
