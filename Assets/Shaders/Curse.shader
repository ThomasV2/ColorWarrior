// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-2442-OUT,alpha-9774-OUT;n:type:ShaderForge.SFN_TexCoord,id:274,x:31682,y:32731,varname:node_274,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:4537,x:31856,y:32731,varname:node_4537,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-274-UVOUT;n:type:ShaderForge.SFN_OneMinus,id:5525,x:32439,y:32801,varname:node_5525,prsc:2|IN-3940-OUT;n:type:ShaderForge.SFN_Multiply,id:2442,x:32947,y:32712,varname:node_2442,prsc:2|A-9779-OUT,B-5525-OUT;n:type:ShaderForge.SFN_Floor,id:9779,x:32439,y:32643,varname:node_9779,prsc:2|IN-3940-OUT;n:type:ShaderForge.SFN_Time,id:7947,x:31322,y:32794,varname:node_7947,prsc:2;n:type:ShaderForge.SFN_Length,id:1451,x:32030,y:32731,varname:node_1451,prsc:2|IN-4537-OUT;n:type:ShaderForge.SFN_RemapRange,id:3940,x:32205,y:32731,varname:node_3940,prsc:2,frmn:0,frmx:1,tomn:-0.7,tomx:1.4|IN-1451-OUT;n:type:ShaderForge.SFN_Slider,id:5729,x:31165,y:32972,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_5729,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1.5,max:3;n:type:ShaderForge.SFN_Multiply,id:5263,x:31519,y:32900,varname:node_5263,prsc:2|A-7947-T,B-5729-OUT;n:type:ShaderForge.SFN_Sin,id:4714,x:31935,y:32989,varname:node_4714,prsc:2|IN-719-OUT;n:type:ShaderForge.SFN_Fmod,id:719,x:31779,y:32989,varname:node_719,prsc:2|A-5263-OUT,B-7189-OUT;n:type:ShaderForge.SFN_Pi,id:7189,x:31535,y:33048,varname:node_7189,prsc:2;n:type:ShaderForge.SFN_Subtract,id:7611,x:32753,y:32971,varname:node_7611,prsc:2|A-5525-OUT,B-2733-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:9774,x:32983,y:33068,varname:node_9774,prsc:2|IN-7611-OUT,IMIN-80-OUT,IMAX-115-OUT,OMIN-80-OUT,OMAX-2781-OUT;n:type:ShaderForge.SFN_Vector1,id:115,x:32753,y:33158,varname:node_115,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:80,x:32753,y:33107,varname:node_80,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4123,x:32311,y:33248,varname:node_4123,prsc:2,v1:10;n:type:ShaderForge.SFN_Divide,id:2781,x:32753,y:33221,varname:node_2781,prsc:2|A-4123-OUT,B-8272-OUT;n:type:ShaderForge.SFN_OneMinus,id:2486,x:32311,y:33107,varname:node_2486,prsc:2|IN-2733-OUT;n:type:ShaderForge.SFN_Multiply,id:8272,x:32519,y:33107,varname:node_8272,prsc:2|A-2486-OUT,B-4123-OUT;n:type:ShaderForge.SFN_RemapRange,id:2733,x:32092,y:32989,varname:node_2733,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:0.75|IN-4714-OUT;proporder:5729;pass:END;sub:END;*/

Shader "Shader Forge/Curse" {
    Properties {
        _Speed ("Speed", Range(1, 3)) = 1.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
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
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float node_3940 = (length((i.uv0*2.0+-1.0))*2.1+-0.7);
                float node_5525 = (1.0 - node_3940);
                float node_2442 = (floor(node_3940)*node_5525);
                float3 emissive = float3(node_2442,node_2442,node_2442);
                float3 finalColor = emissive;
                float4 node_7947 = _Time + _TimeEditor;
                float node_2733 = (sin(fmod((node_7947.g*_Speed),3.141592654))*0.375+0.375);
                float node_80 = 0.0;
                float node_4123 = 10.0;
                return fixed4(finalColor,(node_80 + ( ((node_5525-node_2733) - node_80) * ((node_4123/((1.0 - node_2733)*node_4123)) - node_80) ) / (1.0 - node_80)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
