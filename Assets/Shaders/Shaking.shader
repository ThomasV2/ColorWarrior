// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32551,y:32729,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7113-OUT;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5983-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32915,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1749,x:33025,y:32818,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A;n:type:ShaderForge.SFN_TexCoord,id:3910,x:32156,y:32675,varname:node_3910,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:3702,x:31447,y:32826,varname:node_3702,prsc:2;n:type:ShaderForge.SFN_Sin,id:166,x:31796,y:32826,varname:node_166,prsc:2|IN-1417-OUT;n:type:ShaderForge.SFN_Append,id:6045,x:32156,y:32826,varname:node_6045,prsc:2|A-5554-OUT,B-6095-OUT;n:type:ShaderForge.SFN_Vector1,id:6095,x:31974,y:32965,varname:node_6095,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:7113,x:32379,y:32729,varname:node_7113,prsc:2|A-3910-UVOUT,B-6045-OUT;n:type:ShaderForge.SFN_Multiply,id:5554,x:31974,y:32826,varname:node_5554,prsc:2|A-166-OUT,B-1841-OUT;n:type:ShaderForge.SFN_Multiply,id:1417,x:31638,y:32826,varname:node_1417,prsc:2|A-3702-TTR,B-7118-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2702,x:31626,y:32985,ptovrint:False,ptlb:Amount,ptin:_Amount,varname:node_2702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Divide,id:1841,x:31796,y:32985,varname:node_1841,prsc:2|A-2702-OUT,B-312-OUT;n:type:ShaderForge.SFN_Vector1,id:312,x:31626,y:33061,varname:node_312,prsc:2,v1:1000;n:type:ShaderForge.SFN_ValueProperty,id:7118,x:31447,y:32985,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_7118,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:15;proporder:4805-5983-2702-7118;pass:END;sub:END;*/

Shader "Shader Forge/Shaking" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Amount ("Amount", Float ) = 0.5
        _Speed ("Speed", Float ) = 15
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
            uniform float4 _Color;
            uniform float _Amount;
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
                float4 node_3702 = _Time + _TimeEditor;
                float2 node_7113 = (i.uv0+float2((sin((node_3702.a*_Speed))*(_Amount/1000.0)),0.0));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_7113, _MainTex));
                float node_603 = (_MainTex_var.a*_Color.a); // A
                float3 emissive = ((_MainTex_var.rgb*_Color.rgb)*node_603);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_603);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
