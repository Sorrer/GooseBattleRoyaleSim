Shader "Custom/ScreenColored"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

        float _FlipMidpoint;
		float4 _FlipUpColor;
		float4 _FlipDownColor;


        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 scenePixel =  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float average = (scenePixel.r + scenePixel.g + scenePixel.b) / 3;

				if(scenePixel.a == 0){
					return scenePixel;
				}

				
				if(average > _FlipMidpoint){
					return _FlipDownColor;			
				}else{
					return _FlipUpColor;		
				}

				return float4(average, average, average, 1);	
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}