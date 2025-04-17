Shader "Unlit/ScrollingTextureURP"
{
    Properties
    {
        _MainTex ("Base Texture (RGB + Alpha)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowPower ("Glow Power", Range(0, 5)) = 1
        _ScrollSpeed ("Scroll Speed", Float) = 0.5
    }

    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                half4 _GlowColor;
                half _GlowPower;
                float _ScrollSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Scrolling UV with looping
                float2 scrolledUV = IN.uv;
                scrolledUV.x = frac(IN.uv.x + _Time.y * _ScrollSpeed);
                
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV);
                half4 col = texColor * _Color;
                
                // Glow effect based on alpha channel
                half3 glow = _GlowColor.rgb * _GlowPower * texColor.a;
                col.rgb += glow;
                
                return half4(col.rgb, texColor.a * _Color.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}