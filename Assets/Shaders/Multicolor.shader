// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-2046-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32454,y:32680,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-3359-OUT;n:type:ShaderForge.SFN_Color,id:5983,x:31822,y:32814,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:1749,x:33025,y:32818,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-4805-A;n:type:ShaderForge.SFN_Color,id:8924,x:31822,y:33162,ptovrint:False,ptlb:Color4,ptin:_Color4,varname:node_8924,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:4710,x:31822,y:32653,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_4710,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.6470588,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:8809,x:31822,y:32993,ptovrint:False,ptlb:Color3,ptin:_Color3,varname:node_8809,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:1,c4:1;n:type:ShaderForge.SFN_Time,id:4342,x:30739,y:32615,varname:node_4342,prsc:2;n:type:ShaderForge.SFN_Slider,id:5097,x:30660,y:32794,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_5097,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Multiply,id:1802,x:31038,y:32707,varname:node_1802,prsc:2|A-4342-T,B-5097-OUT,C-2357-OUT;n:type:ShaderForge.SFN_Multiply,id:4534,x:32054,y:32663,varname:node_4534,prsc:2|A-4710-RGB,B-1739-OUT;n:type:ShaderForge.SFN_Multiply,id:9321,x:32054,y:32824,varname:node_9321,prsc:2|A-5983-RGB,B-4268-OUT;n:type:ShaderForge.SFN_Multiply,id:1233,x:32054,y:32993,varname:node_1233,prsc:2|A-8809-RGB,B-1072-OUT;n:type:ShaderForge.SFN_Multiply,id:2406,x:32054,y:33162,varname:node_2406,prsc:2|A-8924-RGB,B-1805-OUT;n:type:ShaderForge.SFN_Add,id:5321,x:32377,y:32918,varname:node_5321,prsc:2|A-4534-OUT,B-9321-OUT,C-1233-OUT,D-2406-OUT;n:type:ShaderForge.SFN_Clamp01,id:3359,x:32560,y:32918,varname:node_3359,prsc:2|IN-5321-OUT;n:type:ShaderForge.SFN_Sin,id:318,x:31459,y:32663,varname:node_318,prsc:2|IN-1802-OUT;n:type:ShaderForge.SFN_Vector1,id:2357,x:30817,y:32884,varname:node_2357,prsc:2,v1:10;n:type:ShaderForge.SFN_Sin,id:7624,x:31459,y:32834,varname:node_7624,prsc:2|IN-123-OUT;n:type:ShaderForge.SFN_Sin,id:6231,x:31459,y:33013,varname:node_6231,prsc:2|IN-9188-OUT;n:type:ShaderForge.SFN_Sin,id:5465,x:31459,y:33207,varname:node_5465,prsc:2|IN-7686-OUT;n:type:ShaderForge.SFN_Pi,id:2673,x:30659,y:33005,varname:node_2673,prsc:2;n:type:ShaderForge.SFN_Add,id:123,x:31283,y:32834,varname:node_123,prsc:2|A-9198-OUT,B-1802-OUT;n:type:ShaderForge.SFN_Add,id:9188,x:31283,y:33013,varname:node_9188,prsc:2|A-2673-OUT,B-1802-OUT;n:type:ShaderForge.SFN_Add,id:7686,x:31283,y:33207,varname:node_7686,prsc:2|A-6309-OUT,B-1802-OUT;n:type:ShaderForge.SFN_Multiply,id:6309,x:31077,y:33246,varname:node_6309,prsc:2|A-9198-OUT,B-9714-OUT;n:type:ShaderForge.SFN_Vector1,id:9714,x:30878,y:33297,varname:node_9714,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:8884,x:30643,y:33123,varname:node_8884,prsc:2,v1:2;n:type:ShaderForge.SFN_Divide,id:9198,x:30838,y:33079,varname:node_9198,prsc:2|A-2673-OUT,B-8884-OUT;n:type:ShaderForge.SFN_Clamp01,id:1739,x:31652,y:32663,varname:node_1739,prsc:2|IN-318-OUT;n:type:ShaderForge.SFN_Clamp01,id:4268,x:31652,y:32834,varname:node_4268,prsc:2|IN-7624-OUT;n:type:ShaderForge.SFN_Clamp01,id:1072,x:31652,y:33013,varname:node_1072,prsc:2|IN-6231-OUT;n:type:ShaderForge.SFN_Clamp01,id:1805,x:31652,y:33207,varname:node_1805,prsc:2|IN-5465-OUT;n:type:ShaderForge.SFN_Slider,id:1230,x:32403,y:33130,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_1230,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:2046,x:33025,y:32972,varname:node_2046,prsc:2|A-4805-A,B-1258-OUT;n:type:ShaderForge.SFN_OneMinus,id:1258,x:32812,y:33055,varname:node_1258,prsc:2|IN-1230-OUT;proporder:4805-5983-8809-5097-4710-8924-1230;pass:END;sub:END;*/

Shader "Shader Forge/Multicolor" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color2 ("Color2", Color) = (1,0,0,1)
        _Color3 ("Color3", Color) = (0,0,1,1)
        _Speed ("Speed", Range(0, 2)) = 1
        _Color1 ("Color1", Color) = (1,0.6470588,0,1)
        _Color4 ("Color4", Color) = (0,1,0,1)
        _Opacity ("Opacity", Range(0, 1)) = 1
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color2;
            uniform float4 _Color4;
            uniform float4 _Color1;
            uniform float4 _Color3;
            uniform float _Speed;
            uniform float _Opacity;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 node_4342 = _Time + _TimeEditor;
                float node_1802 = (node_4342.g*_Speed*10.0);
                float node_2673 = 3.141592654;
                float node_9198 = (node_2673/2.0);
                float3 emissive = ((_MainTex_var.rgb*saturate(((_Color1.rgb*saturate(sin(node_1802)))+(_Color2.rgb*saturate(sin((node_9198+node_1802))))+(_Color3.rgb*saturate(sin((node_2673+node_1802))))+(_Color4.rgb*saturate(sin(((node_9198*3.0)+node_1802)))))))*_MainTex_var.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*(1.0 - _Opacity)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
