Shader "Custom/TextureCoordinates/RichShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;

            fixed4 frag(v2f_img i) : SV_Target 
			{
				float2 ruv = i.uv;
				
				//ruv -= 0.5;

				const float PI = 3.14159;
				const float distort = 1.0;

				float d = length(ruv.xy);
				//const float max_radius = 0.70711;
				//float modified = sin((d/max_radius/2.5)*PI)/1.55;
				//ruv += ruv*(d-modified);
				//ruv*=(2.2-distort);
 
				//ruv += 0.5;
				

				float interlaceIntensity = 1.0f;
				float flickIntensity = 0.9f;

				float darklevel = clamp((1-d)*1.4,0,1);
				float4 color = tex2D(_MainTex, i.uv);
				float modifier = (ruv.x>=0)*(ruv.y>=0)*(ruv.x<=1)*(ruv.y<=1);


				//float interlace = 1+clamp(sin(ruv.y*PI*interlaceIntensity)*0.25,0.0,0.25)*interlaceIntensity;
				float interlace = 0.8f;
				float flicks = 2.0f;
				flicks += sin((ruv.y+ _Time.y*5)*PI*3.5)*0.2;
				flicks += sin((ruv.y+ _Time.y)*PI*2)*0.2;
				flicks *= flickIntensity;
				return color*modifier*interlace*flicks;
            }
            ENDCG
        }
    }
}