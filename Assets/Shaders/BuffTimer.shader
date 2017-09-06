// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32390,y:32552,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32651,y:32641,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5983-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32390,y:32738,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1749,x:32986,y:32681,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32809,y:32866,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A,C-1985-OUT;n:type:ShaderForge.SFN_TexCoord,id:615,x:31442,y:33023,varname:node_615,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:7661,x:31617,y:33023,varname:node_7661,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-615-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:3302,x:31780,y:33023,varname:node_3302,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7661-OUT;n:type:ShaderForge.SFN_ArcTan2,id:2115,x:31975,y:33023,varname:node_2115,prsc:2,attp:2|A-3302-G,B-3302-R;n:type:ShaderForge.SFN_Ceil,id:9161,x:32339,y:32913,varname:node_9161,prsc:2|IN-501-OUT;n:type:ShaderForge.SFN_Subtract,id:501,x:32170,y:32913,varname:node_501,prsc:2|A-2416-OUT,B-2115-OUT;n:type:ShaderForge.SFN_Slider,id:6926,x:31562,y:32839,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_6926,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_OneMinus,id:2416,x:31939,y:32840,varname:node_2416,prsc:2|IN-6926-OUT;n:type:ShaderForge.SFN_Add,id:1985,x:32526,y:32962,varname:node_1985,prsc:2|A-9161-OUT,B-4378-OUT;n:type:ShaderForge.SFN_Vector1,id:4378,x:32227,y:33124,varname:node_4378,prsc:2,v1:0.6;proporder:4805-5983-6926;pass:END;sub:END;*/

Shader "Shader Forge/BuffTimer" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Opacity ("Opacity", Range(0, 1)) = 0
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
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
                float2 node_3302 = (i.uv0*2.0+-1.0).rg;
                float node_603 = (_MainTex_var.a*_Color.a*(ceil(((1.0 - _Opacity)-((atan2(node_3302.g,node_3302.r)/6.28318530718)+0.5)))+0.6)); // A
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
