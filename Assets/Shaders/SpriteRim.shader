// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33567,y:31933,varname:node_3138,prsc:2|emission-3387-OUT,alpha-8193-OUT;n:type:ShaderForge.SFN_Sin,id:5513,x:31799,y:32143,varname:node_5513,prsc:2|IN-5657-OUT;n:type:ShaderForge.SFN_Time,id:1491,x:31193,y:31805,varname:node_1491,prsc:2;n:type:ShaderForge.SFN_Pi,id:334,x:31468,y:32160,varname:node_334,prsc:2;n:type:ShaderForge.SFN_Fmod,id:5657,x:31619,y:32087,varname:node_5657,prsc:2|A-4674-OUT,B-334-OUT;n:type:ShaderForge.SFN_Tex2d,id:7066,x:32003,y:32171,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_7066,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:0000000000000000f000000000000000,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1343,x:32005,y:31615,ptovrint:False,ptlb:GlowMask,ptin:_GlowMask,varname:node_1343,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5faa1ad60b528434b9a8ab5492619aa1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:607,x:32003,y:31976,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_607,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:1240,x:32005,y:31787,ptovrint:False,ptlb:RimMask,ptin:_RimMask,varname:node_1240,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c85cb2e7a3daa2542bb32b8c93af2df1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4674,x:31446,y:31895,varname:node_4674,prsc:2|A-1491-T,B-2550-OUT;n:type:ShaderForge.SFN_Slider,id:2550,x:31036,y:31951,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_2550,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:3;n:type:ShaderForge.SFN_Vector1,id:220,x:32003,y:32344,varname:node_220,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:36,x:32328,y:32190,varname:node_36,prsc:2|A-220-OUT,B-7066-A;n:type:ShaderForge.SFN_Subtract,id:96,x:32504,y:32076,varname:node_96,prsc:2|A-7066-RGB,B-36-OUT;n:type:ShaderForge.SFN_Add,id:8193,x:32714,y:32188,varname:node_8193,prsc:2|A-8966-OUT,B-7066-A;n:type:ShaderForge.SFN_Multiply,id:8966,x:32368,y:31689,varname:node_8966,prsc:2|A-1343-R,B-1240-R,C-5513-OUT;n:type:ShaderForge.SFN_Multiply,id:6852,x:32609,y:31780,varname:node_6852,prsc:2|A-8966-OUT,B-607-RGB;n:type:ShaderForge.SFN_Add,id:3387,x:32983,y:31849,varname:node_3387,prsc:2|A-6852-OUT,B-8429-OUT;n:type:ShaderForge.SFN_Clamp01,id:8429,x:32659,y:32044,varname:node_8429,prsc:2|IN-96-OUT;proporder:7066-1343-607-1240-2550;pass:END;sub:END;*/

Shader "Shader Forge/SpriteRim" {
    Properties {
        [PerRendererData]_Texture ("Texture", 2D) = "white" {}
        _GlowMask ("GlowMask", 2D) = "white" {}
        _RimColor ("RimColor", Color) = (1,0,0,1)
        _RimMask ("RimMask", 2D) = "white" {}
        _Speed ("Speed", Range(0, 3)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _GlowMask; uniform float4 _GlowMask_ST;
            uniform float4 _RimColor;
            uniform sampler2D _RimMask; uniform float4 _RimMask_ST;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _GlowMask_var = tex2D(_GlowMask,TRANSFORM_TEX(i.uv0, _GlowMask));
                float4 _RimMask_var = tex2D(_RimMask,TRANSFORM_TEX(i.uv0, _RimMask));
                float4 node_1491 = _Time + _TimeEditor;
                float node_8966 = (_GlowMask_var.r*_RimMask_var.r*sin(fmod((node_1491.g*_Speed),3.141592654)));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = ((node_8966*_RimColor.rgb)+saturate((_Texture_var.rgb-(1.0-_Texture_var.a))));
                float3 finalColor = emissive;
                return fixed4(finalColor,(node_8966+_Texture_var.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
