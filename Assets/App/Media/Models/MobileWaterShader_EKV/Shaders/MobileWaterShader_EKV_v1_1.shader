Shader "Mobile/MobileWaterShaderEKV_v2_URP_Fixed"
{
    Properties
    {
        [Toggle(LIGHTING)] _SECTIONLIGHTING("---- LIGHTING ------------------------", Float) = 1
        [PowerSlider(5.0)] _Shininess("Shininess", Range(0.01, 2)) = 0.1
        [PowerSlider(5.0)] _Brightness("Brightness", Range(0.01, 100)) = 1.0
        [PowerSlider(5.0)] _Attenuation("Attenuation", Range(0.001, 1)) = 0.5
        _Color("Color Tint",COLOR) = (0.5,0.5,0.5,1.0)

        [Toggle(TEXTURES)]_SECTIONTEXTURES("---- TEXTURES ------------------------", Float) = 1
        _MainTex("Texture A", 2D) = "black" {}
        _MainTexRot("Texture A Rotation", Range(0 , 360)) = 0

        _DiffTex("Texture B", 2D) = "black" {}
        _DiffTexRot("Texture B Rotation", Range(0 , 360)) = 0

        [Toggle(WAVES)] _SECTIONWAVES("---- WAVES AND FLOW ------------------------", Float) = 1
        [NoScaleOffset] _DerivHeightMap("Wave Height Map", 2D) = "black" {}
        _Tiling("Tiling", Float) = 3
        [PowerSlider(1.0)] _Speed("Speed", Range(0.001, 1)) = 0.1
        [PowerSlider(1.0)] _FlowStrength("Flow Strength", Range(-1, 1)) = 0.1
        [PowerSlider(1.0)] _FlowOffset("Flow Offset", Range(-1, 1)) = 0.25
        [PowerSlider(1.0)] _HeightScale("Height Scale, Constant", Range(-1, 1)) = 0.5
        [PowerSlider(1.0)] _HeightScaleModulated("Height Scale, Modulated", Range(-5, 5)) = 4

        [Toggle(FOAM)] _FOAM("---- EDGE FOAM ------------------------", Float) = 1
        _FoamTex("Foam Texture", 2D) = "white" {}
        _FoamSpeed("Foam Speed", Range(0, 1)) = 0.1
        _FoamIntensity("Foam Intensity", Range(0, 10)) = 4.5
        _FoamWidth("Foam width", Range(0, 1)) = 0.05
        _FoamDir("Foam strength", Range(-1, 1)) = 0.05
        _FoamTexRot("Foam Rotation", Range(0 , 360)) = 0
        [Toggle(INVERT)] _Invert("Show foam on center", Float) = 1
        [Toggle(SQUARED)] _Squared("Show foam as square", Float) = 1

        [Toggle(REFLECTION)] _Reflection("---- CUBEMAP REFLECTION ------------------------", Float) = 1
        [Slider] _RefStrength("Reflection Strength", Range(0, 2)) = 1
        _Cube("Cubemap", CUBE) = "" {}

        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 5 // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 10 // OneMinusSrcAlpha
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        LOD 600

        Blend [_SrcBlend] [_DstBlend]
        ZWrite Off
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma shader_feature LIGHTING
            #pragma shader_feature TEXTURES
            #pragma shader_feature WAVES
            #pragma shader_feature REFLECTION
            #pragma shader_feature FOAM
            #pragma shader_feature INVERT
            #pragma shader_feature SQUARED

            // Required for URP 14.0.11
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_DiffTex : TEXCOORD1;
                float2 uv_FoamTex : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
                float4 tangentWS : TEXCOORD4;
                float3 positionWS : TEXCOORD5;
                float4 screenPos : TEXCOORD6;
                float3 viewDirWS : TEXCOORD7;
                float fogFactor : TEXCOORD8;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Brightness;
                float _RefStrength;
                float _Attenuation;
                float _MainTexRot;
                float _DiffTexRot;
                float _FoamTexRot;
                
                float _Speed, _FlowStrength, _FlowOffset, _Tiling;
                float _HeightScale, _HeightScaleModulated;
                
                half _Shininess;
                half _FoamSpeed;
                half _FoamIntensity;
                half _FoamWidth;
                half _FoamDir;
            CBUFFER_END

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_DiffTex); SAMPLER(sampler_DiffTex);
            TEXTURE2D(_DerivHeightMap); SAMPLER(sampler_DerivHeightMap);
            TEXTURE2D(_FoamTex); SAMPLER(sampler_FoamTex);
            TEXTURECUBE(_Cube); SAMPLER(sampler_Cube);

            float3 FlowUVW(float2 uv, float2 flowVector, float2 jump, float flowOffset, float tiling, float time, bool flowB)
            {
                float phaseOffset = flowB ? 0.5 : 0;
                float progress = frac(time + phaseOffset);
                float3 uvw;
                uvw.xy = uv - flowVector * (progress + flowOffset);
                uvw.xy *= tiling;
                uvw.xy += phaseOffset;
                uvw.xy += (time - progress) * jump;
                uvw.z = 1 - abs(1 - 2 * progress);
                return uvw;
            }

            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                Rotation = Rotation * (3.1415926f / 180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
                float2x2 rMatrix = float2x2(c, -s, s, c);
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
                Out = UV;
            }

            float3 UnpackDerivativeHeight(float4 textureData)
            {
                float3 dh = textureData.agb;
                dh.xy = dh.xy * 2 - 1;
                return dh;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                
                OUT.positionCS = vertexInput.positionCS;
                OUT.positionWS = vertexInput.positionWS;
                OUT.normalWS = normalInput.normalWS;
                OUT.tangentWS = float4(normalInput.tangentWS.xyz, IN.tangentOS.w);
                OUT.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                
                // Rotate UVs if needed
                float2 center = float2(0.5, 0.5);
                Unity_Rotate_Degrees_float(IN.texcoord, center, _MainTexRot, OUT.uv_MainTex);
                Unity_Rotate_Degrees_float(IN.texcoord, center, _DiffTexRot, OUT.uv_DiffTex);
                OUT.uv_FoamTex = IN.texcoord;
                
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                OUT.fogFactor = ComputeFogFactor(OUT.positionCS.z);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample textures
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv_MainTex) * _Color;
                half4 tex2 = SAMPLE_TEXTURE2D(_DiffTex, sampler_DiffTex, IN.uv_DiffTex) * _Color;
                
                // Initialize normal
                half3 normalWS = IN.normalWS;
                half3 normalTS = half3(0, 0, 1);
                half finalHeightScale = 0;
                half noise = 0;
                half time = 0;
                
                #ifdef WAVES
                    // Flow calculation
                    half3 flow = SAMPLE_TEXTURE2D(_DiffTex, sampler_DiffTex, IN.uv_MainTex).rgb;
                    flow.xy = flow.xy * 2 - 1;
                    flow *= _FlowStrength;
                    noise = SAMPLE_TEXTURE2D(_DiffTex, sampler_DiffTex, IN.uv_MainTex).a;
                    time = _Time.y * _Speed + noise;
                    
                    half3 uvwA = FlowUVW(IN.uv_MainTex, flow.xy, 0, _FlowOffset, _Tiling, time, false);
                    half3 uvwB = FlowUVW(IN.uv_MainTex, flow.xy, 0, _FlowOffset, _Tiling, time, true);
                    
                    finalHeightScale = flow.z * _HeightScaleModulated + _HeightScale;
                    
                    half3 dhA = UnpackDerivativeHeight(SAMPLE_TEXTURE2D(_DerivHeightMap, sampler_DerivHeightMap, uvwA.xy)) * (uvwA.z * finalHeightScale);
                    half3 dhB = UnpackDerivativeHeight(SAMPLE_TEXTURE2D(_DerivHeightMap, sampler_DerivHeightMap, uvwB.xy)) * (uvwB.z * finalHeightScale);
                    
                    normalTS = normalize(half3(-(dhA.xy + dhB.xy), 1));
                    
                    // Transform normal from tangent to world space
                    half3 tangentWS = IN.tangentWS.xyz;
                    half3 bitangentWS = cross(IN.normalWS, tangentWS) * IN.tangentWS.w;
                    half3x3 TBN = half3x3(tangentWS, bitangentWS, IN.normalWS);
                    normalWS = normalize(mul(normalTS, TBN));
                #endif
                
                // Combine textures
                half3 albedo = _Color.rgb;
                half alpha = _Color.a;
                
                #ifdef TEXTURES
                    albedo = tex.rgb + tex2.rgb;
                    alpha = tex.a + tex2.a;
                #endif
                
                // Reflection
                #ifdef REFLECTION
                    half3 viewDirWS = normalize(IN.viewDirWS);
                    half3 reflectVector = reflect(-viewDirWS, normalWS);
                    half3 reflection = SAMPLE_TEXTURECUBE(_Cube, sampler_Cube, reflectVector).rgb;
                    albedo += reflection * _RefStrength;
                #endif
                
                // Foam
                #ifdef FOAM
                    half2 circleCenter = half2(0.5, 0.5);
                    half distanceToCenter = length(IN.uv_FoamTex - circleCenter);
                    half foamMask = 1;
                    
                    #ifdef SQUARED
                        #ifdef INVERT
                            half2 uvFromCenter = abs(IN.uv_FoamTex - circleCenter);
                            half foamMaskX = 1 - smoothstep(0.0, _FoamWidth, uvFromCenter.x);
                            half foamMaskY = 1 - smoothstep(0.0, _FoamWidth, uvFromCenter.y);
                            foamMask = min(foamMaskX, foamMaskY);
                        #else
                            half foamMask1 = 1 - smoothstep(0.0, _FoamWidth, IN.uv_FoamTex.x) * smoothstep(0.0, _FoamWidth, IN.uv_FoamTex.y);
                            half foamMask2 = 1 - smoothstep(1.0, 1 - _FoamWidth, IN.uv_FoamTex.y) * smoothstep(1.0, 1 - _FoamWidth, IN.uv_FoamTex.x);
                            foamMask = foamMask1 + foamMask2;
                        #endif
                    #else
                        #ifdef INVERT
                            foamMask = 1 - smoothstep(0.0, _FoamWidth, distanceToCenter);
                        #else
                            foamMask = smoothstep(1.0 - _FoamWidth, 1.0, distanceToCenter);
                        #endif
                    #endif
                    
                    // Rotate foam UV
                    half2 foamUV = IN.uv_FoamTex;
                    Unity_Rotate_Degrees_float(foamUV, circleCenter, _FoamTexRot, foamUV);
                    
                    // Animate foam
                    noise = SAMPLE_TEXTURE2D(_FoamTex, sampler_FoamTex, foamUV).a;
                    time = _Time.y * _Speed + noise;
                    half2 toCenter = circleCenter - IN.uv_FoamTex;
                    toCenter /= (distanceToCenter + 0.0001); // Avoid division by zero
                    half3 uvF = FlowUVW(foamUV, toCenter, 0, 0, 0, time, true);
                    foamUV += (_Time.y * _FoamSpeed) * _FoamDir;
                    
                    half4 foamColor = SAMPLE_TEXTURE2D(_FoamTex, sampler_FoamTex, foamUV);
                    half finalFoamMask = (foamMask * _FoamIntensity);
                    
                    #ifdef TEXTURES
                        albedo = lerp(albedo, albedo * _FoamIntensity, foamMask);
                    #else
                        albedo = lerp(albedo, albedo * _FoamIntensity, foamMask);
                    #endif
                    
                    albedo += foamColor.rgb * finalFoamMask;
                #endif
                
                // Lighting
                InputData inputData = (InputData)0;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = SafeNormalize(IN.viewDirWS);
                
                #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
                    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                
                inputData.fogCoord = IN.fogFactor;
                inputData.vertexLighting = half3(0, 0, 0);
                inputData.bakedGI = SampleSH(normalWS);
                
                SurfaceData surfaceData;
                surfaceData.albedo = albedo;
                surfaceData.metallic = 0.0;
                surfaceData.specular = half3(0.0, 0.0, 0.0);
                surfaceData.smoothness = _Shininess;
                surfaceData.normalTS = normalTS;
                surfaceData.occlusion = 1.0;
                surfaceData.emission = half3(0, 0, 0);
                surfaceData.alpha = alpha;
                surfaceData.clearCoatMask = 0.0;
                surfaceData.clearCoatSmoothness = 0.0;
                
                #ifdef LIGHTING
                    half4 color = UniversalFragmentBlinnPhong(inputData, surfaceData);
                    color.rgb *= _Brightness * _Attenuation;
                #else
                    half4 color = half4(albedo, alpha);
                #endif
                
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return color;
            }
            ENDHLSL
        }
        
        // Shadow caster pass for receiving shadows
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float3 _LightDirection;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionCS = TransformWorldToHClip(ApplyShadowBias(vertexInput.positionWS, normalWS, _LightDirection));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Simple Lit"
}