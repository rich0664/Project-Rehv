// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9943,x:32719,y:32712,varname:node_9943,prsc:2|diffpow-3502-OUT,alpha-7293-OUT;n:type:ShaderForge.SFN_Slider,id:7293,x:32371,y:33080,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7293,prsc:2,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Color,id:459,x:32346,y:32480,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_459,prsc:2,glob:False,c1:0.5367647,c2:0.5367647,c3:0.5367647,c4:1;n:type:ShaderForge.SFN_Slider,id:6054,x:32371,y:32986,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_6054,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ValueProperty,id:3502,x:32543,y:32715,ptovrint:False,ptlb:Power,ptin:_Power,varname:node_3502,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:5696,x:32319,y:32889,ptovrint:False,ptlb:Multiply,ptin:_Multiply,varname:node_5696,prsc:2,glob:False,v1:2;proporder:7293-459-6054-3502-5696;pass:END;sub:END;*/

Shader "Shader Forge/Transparent" {
    Properties {
        _Opacity ("Opacity", Range(0, 1)) = 1
        _Color ("Color", Color) = (0.5367647,0.5367647,0.5367647,1)
        _Emission ("Emission", Range(0, 1)) = 0
        _Power ("Power", Float ) = 1
        _Multiply ("Multiply", Float ) = 2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float _Opacity;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
////// Lighting:
                float3 finalColor = 0;
                return fixed4(finalColor,_Opacity);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
