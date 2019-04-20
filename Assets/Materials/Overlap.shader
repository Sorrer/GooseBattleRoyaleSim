// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Grayscale"
{
	Properties
	{
		[MaterialToggle] _OnlyInvert("Only Invert", float) = 0
		_FlipMidpoint("Flip Midpoint", Range (0.0, 1.0)) = 0.5
		_FlipUpColor("Flip Up Color", Color) = (1, 1, 1, 1)
		_FlipDownColor("Flip Down Color", Color) = (0, 0, 0, 1)
	}
	SubShader
	{
		Tags{ "Queue" = "Overlay" "RenderType" = "Opaque" }
		ZWrite Off
		//Blend One One


		GrabPass
	{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
	}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform float _FlipMidpoint;
			uniform float _OnlyInvert;
			uniform float4 _FlipUpColor;
			uniform float4 _FlipDownColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 UV : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _GrabTexture;
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);



				o.uv.xy = (float2(o.vertex.x, -o.vertex.y) + o.vertex.w) * 0.5;
				o.uv.zw = o.vertex.zw;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 scenePixel = tex2Dproj(_GrabTexture, i.uv);
				float average = (scenePixel.r + scenePixel.g + scenePixel.b) / 3;

				if(_OnlyInvert == 1){
					return float4(1.0 - scenePixel.r, 1.0 - scenePixel.g, 1.0 - scenePixel.b, scenePixel.a);
				}else{
					if(average > _FlipMidpoint){
						return _FlipDownColor;			
					}else{
						return _FlipUpColor;		
					}
				}

				

				return float4(average, average, average, 1);				
			}
			ENDCG
		}
	}
}