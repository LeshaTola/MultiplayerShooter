Shader "MTE/Surface/4 Textures/Bumped (URP)"
{
    Properties
    {
        _Control ("Control (RGBA)", 2D) = "red" {}
        _Splat0 ("Layer 1", 2D) = "white" {}
        _Splat1 ("Layer 2", 2D) = "white" {}
        _Splat2 ("Layer 3", 2D) = "white" {}
        _Splat3 ("Layer 4", 2D) = "white" {}

        _Normal0 ("Normalmap 1", 2D) = "bump" {}
        _Normal1 ("Normalmap 2", 2D) = "bump" {}
        _Normal2 ("Normalmap 3", 2D) = "bump" {}
        _Normal3 ("Normalmap 4", 2D) = "bump" {}
        
        [Toggle(ENABLE_NORMAL_INTENSITY)] _EnableNormalIntensity ("Normal Intensity", Float) = 0
        _NormalIntensity0 ("Normal Intensity 0", Range(0.01, 10)) = 1.0
        _NormalIntensity1 ("Normal Intensity 1", Range(0.01, 10)) = 1.0
        _NormalIntensity2 ("Normal Intensity 2", Range(0.01, 10)) = 1.0
        _NormalIntensity3 ("Normal Intensity 3", Range(0.01, 10)) = 1.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry-99"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

        TEXTURE2D(_Control); SAMPLER(sampler_Control);
        TEXTURE2D(_Splat0); SAMPLER(sampler_Splat0);
        TEXTURE2D(_Splat1); SAMPLER(sampler_Splat1);
        TEXTURE2D(_Splat2); SAMPLER(sampler_Splat2);
        TEXTURE2D(_Splat3); SAMPLER(sampler_Splat3);
        TEXTURE2D(_Normal0); SAMPLER(sampler_Normal0);
        TEXTURE2D(_Normal1); SAMPLER(sampler_Normal1);
        TEXTURE2D(_Normal2); SAMPLER(sampler_Normal2);
        TEXTURE2D(_Normal3); SAMPLER(sampler_Normal3);

        float4 _Control_ST;
        float4 _Splat0_ST;
        float4 _Splat1_ST;
        float4 _Splat2_ST;
        float4 _Splat3_ST;

        #ifdef ENABLE_NORMAL_INTENSITY
            float _NormalIntensity0;
            float _NormalIntensity1;
            float _NormalIntensity2;
            float _NormalIntensity3;
        #endif

        struct Attributes
        {
            float4 positionOS   : POSITION;
            float2 texcoord     : TEXCOORD0;
            float3 normalOS    : NORMAL;
            float4 tangentOS    : TANGENT;
        };

        struct Varyings
        {
            float2 uv_Control   : TEXCOORD0;
            float4 uv_Splat01   : TEXCOORD1;
            float4 uv_Splat23   : TEXCOORD2;
            float3 normalWS     : TEXCOORD3;
            float3 tangentWS    : TEXCOORD4;
            float3 bitangentWS  : TEXCOORD5;
            float4 positionHCS  : SV_POSITION;
        };

        // Normal intensity function
        float3 ApplyNormalIntensity(float3 normal, float intensity)
        {
            return float3(normal.xy * intensity, normal.z);
        }

        Varyings MTE_SplatmapVert(Attributes input)
        {
            Varyings output;
            
            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
            output.positionHCS = vertexInput.positionCS;
            
            output.uv_Control = TRANSFORM_TEX(input.texcoord, _Control);
            output.uv_Splat01.xy = TRANSFORM_TEX(input.texcoord, _Splat0);
            output.uv_Splat01.zw = TRANSFORM_TEX(input.texcoord, _Splat1);
            output.uv_Splat23.xy = TRANSFORM_TEX(input.texcoord, _Splat2);
            output.uv_Splat23.zw = TRANSFORM_TEX(input.texcoord, _Splat3);
            
            VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
            output.normalWS = normalInput.normalWS;
            output.tangentWS = normalInput.tangentWS;
            output.bitangentWS = normalInput.bitangentWS;
            
            return output;
        }

        void MTE_SplatmapMix(Varyings input, out half weight, out half4 mixedDiffuse, out half3 mixedNormal)
        {
            half4 splat_control = SAMPLE_TEXTURE2D(_Control, sampler_Control, input.uv_Control);
            weight = dot(splat_control, half4(1, 1, 1, 1));
            splat_control /= (weight + 1e-3f);

            mixedDiffuse = 0.0h;
            mixedDiffuse += splat_control.r * SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, input.uv_Splat01.xy);
            mixedDiffuse += splat_control.g * SAMPLE_TEXTURE2D(_Splat1, sampler_Splat1, input.uv_Splat01.zw);
            mixedDiffuse += splat_control.b * SAMPLE_TEXTURE2D(_Splat2, sampler_Splat2, input.uv_Splat23.xy);
            mixedDiffuse += splat_control.a * SAMPLE_TEXTURE2D(_Splat3, sampler_Splat3, input.uv_Splat23.zw);
            
            #ifdef ENABLE_NORMAL_INTENSITY
                half3 nrm0 = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, input.uv_Splat01.xy), _NormalIntensity0);
                half3 nrm1 = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal1, sampler_Normal1, input.uv_Splat01.zw), _NormalIntensity1);
                half3 nrm2 = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal2, sampler_Normal2, input.uv_Splat23.xy), _NormalIntensity2);
                half3 nrm3 = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal3, sampler_Normal3, input.uv_Splat23.zw), _NormalIntensity3);
                nrm0 = splat_control.r * nrm0;
                nrm1 = splat_control.g * nrm1;
                nrm2 = splat_control.b * nrm2;
                nrm3 = splat_control.a * nrm3;
                mixedNormal = normalize(nrm0 + nrm1 + nrm2 + nrm3);
            #else
                half4 nrm = 0.0h;
                nrm += splat_control.r * SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, input.uv_Splat01.xy);
                nrm += splat_control.g * SAMPLE_TEXTURE2D(_Normal1, sampler_Normal1, input.uv_Splat01.zw);
                nrm += splat_control.b * SAMPLE_TEXTURE2D(_Normal2, sampler_Normal2, input.uv_Splat23.xy);
                nrm += splat_control.a * SAMPLE_TEXTURE2D(_Normal3, sampler_Normal3, input.uv_Splat23.zw);
                mixedNormal = UnpackNormal(nrm);
            #endif
            
            // Transform normal from tangent to world space
            float3x3 tangentToWorld = float3x3(input.tangentWS, input.bitangentWS, input.normalWS);
            mixedNormal = mul(mixedNormal, tangentToWorld);
        }
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature ENABLE_NORMAL_INTENSITY
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct FragInput
            {
                float2 uv_Control   : TEXCOORD0;
                float4 uv_Splat01   : TEXCOORD1;
                float4 uv_Splat23   : TEXCOORD2;
                float3 normalWS     : TEXCOORD3;
                float3 tangentWS    : TEXCOORD4;
                float3 bitangentWS  : TEXCOORD5;
                float4 positionHCS  : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                return MTE_SplatmapVert(input);
            }

            half4 frag(FragInput input) : SV_Target
            {
                // Get surface data
                half weight;
                half4 mixedDiffuse;
                half3 mixedNormal;
                MTE_SplatmapMix(input, weight, mixedDiffuse, mixedNormal);
                
                // Setup surface data
                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = GetCameraRelativePositionWS(TransformObjectToWorld(input.positionHCS.xyz));
                lightingInput.normalWS = normalize(mixedNormal);
                lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(lightingInput.positionWS);
                lightingInput.shadowCoord = TransformWorldToShadowCoord(lightingInput.positionWS);
                
                SurfaceData surfaceData;
                surfaceData.albedo = mixedDiffuse.rgb;
                surfaceData.specular = 0.0h;
                surfaceData.metallic = 0.0h;
                surfaceData.smoothness = 0.5h;
                surfaceData.normalTS = mixedNormal;
                surfaceData.emission = 0.0h;
                surfaceData.occlusion = 1.0h;
                surfaceData.alpha = 1.0h;
                surfaceData.clearCoatMask = 0.0h;
                surfaceData.clearCoatSmoothness = 0.0h;
                
                // Calculate lighting
                half4 color = UniversalFragmentPBR(lightingInput, surfaceData);
                return color;
            }
            ENDHLSL
        }
    }
    
    Fallback "Universal Render Pipeline/Lit"
    CustomEditor "MTE.MTEShaderGUI"
}