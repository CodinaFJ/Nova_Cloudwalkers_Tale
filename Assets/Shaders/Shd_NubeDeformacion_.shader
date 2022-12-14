Shader "Shd_Nube_"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTexture", 2D) = "white" {}
        [NoScaleOffset]Texture2D_b61294243a2e4633bc60eb2b46f26c21("TexturaDistorsión", 2D) = "white" {}
        Vector1_f52276ee0c144c88841da46cba7861f5("VelocidadDistorisión", Float) = 0.01
        Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd("Fuerza Distorsión", Float) = 0.015
        Vector2_36058a0d6b9840ebb800d3b8952bda6d("Tiling Deformación", Vector) = (0.5, 0.5, 0, 0)
        Vector2_396de5782b34402bb9ae518aadc37dd3("Mov Nube", Vector) = (0, 0.03, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "DisableBatching" = "True"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITELIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            output.interp2.xyzw =  input.screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            output.screenPosition = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 Texture2D_b61294243a2e4633bc60eb2b46f26c21_TexelSize;
        float Vector1_f52276ee0c144c88841da46cba7861f5;
        float Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
        float2 Vector2_36058a0d6b9840ebb800d3b8952bda6d;
        float2 Vector2_396de5782b34402bb9ae518aadc37dd3;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Clamp);
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
        SAMPLER(samplerTexture2D_b61294243a2e4633bc60eb2b46f26c21);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9499453deb424003a34c3bc36b12d418_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_adf10e54ed2747debf903ef036e9d5a3_Out_0 = IN.uv0;
            UnityTexture2D _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
            float4 _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.tex, _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_R_4 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.r;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_G_5 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.g;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_B_6 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.b;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_A_7 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.a;
            float2 _Property_0c139cd12109479f994daba3054a3f44_Out_0 = Vector2_36058a0d6b9840ebb800d3b8952bda6d;
            float _Property_fe8462d772c14bb98d165ff094df5916_Out_0 = Vector1_f52276ee0c144c88841da46cba7861f5;
            float _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_fe8462d772c14bb98d165ff094df5916_Out_0, _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2);
            float2 _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2.xx), _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3);
            float _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3, 10, _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2);
            float2 _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3;
            Unity_TilingAndOffset_float((_SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.xy), _Property_0c139cd12109479f994daba3054a3f44_Out_0, (_GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2.xx), _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3);
            float _Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0 = Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
            float2 _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2;
            Unity_Multiply_float(_TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3, (_Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0.xx), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2);
            float2 _Add_722f9063426e42fdb870bfc00a749c78_Out_2;
            Unity_Add_float2((_UV_adf10e54ed2747debf903ef036e9d5a3_Out_0.xy), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2, _Add_722f9063426e42fdb870bfc00a749c78_Out_2);
            float2 _Property_28c88f9028bb408c9c304133479df4e1_Out_0 = Vector2_396de5782b34402bb9ae518aadc37dd3;
            float2 _Multiply_6b554f447b10424592e322cf010e87b6_Out_2;
            Unity_Multiply_float(_Property_28c88f9028bb408c9c304133479df4e1_Out_0, (IN.TimeParameters.y.xx), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2);
            float2 _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0, 0), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3);
            float2 _Add_a0584206f0174474879b4024a569da17_Out_2;
            Unity_Add_float2(_Add_722f9063426e42fdb870bfc00a749c78_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float4 _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9499453deb424003a34c3bc36b12d418_Out_0.tex, UnityBuildSamplerStateStruct(SamplerState_Linear_Clamp).samplerstate, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_R_4 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.r;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_G_5 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.g;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_B_6 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.b;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITENORMAL
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 Texture2D_b61294243a2e4633bc60eb2b46f26c21_TexelSize;
        float Vector1_f52276ee0c144c88841da46cba7861f5;
        float Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
        float2 Vector2_36058a0d6b9840ebb800d3b8952bda6d;
        float2 Vector2_396de5782b34402bb9ae518aadc37dd3;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Clamp);
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
        SAMPLER(samplerTexture2D_b61294243a2e4633bc60eb2b46f26c21);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9499453deb424003a34c3bc36b12d418_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_adf10e54ed2747debf903ef036e9d5a3_Out_0 = IN.uv0;
            UnityTexture2D _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
            float4 _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.tex, _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_R_4 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.r;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_G_5 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.g;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_B_6 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.b;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_A_7 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.a;
            float2 _Property_0c139cd12109479f994daba3054a3f44_Out_0 = Vector2_36058a0d6b9840ebb800d3b8952bda6d;
            float _Property_fe8462d772c14bb98d165ff094df5916_Out_0 = Vector1_f52276ee0c144c88841da46cba7861f5;
            float _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_fe8462d772c14bb98d165ff094df5916_Out_0, _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2);
            float2 _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2.xx), _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3);
            float _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3, 10, _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2);
            float2 _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3;
            Unity_TilingAndOffset_float((_SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.xy), _Property_0c139cd12109479f994daba3054a3f44_Out_0, (_GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2.xx), _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3);
            float _Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0 = Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
            float2 _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2;
            Unity_Multiply_float(_TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3, (_Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0.xx), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2);
            float2 _Add_722f9063426e42fdb870bfc00a749c78_Out_2;
            Unity_Add_float2((_UV_adf10e54ed2747debf903ef036e9d5a3_Out_0.xy), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2, _Add_722f9063426e42fdb870bfc00a749c78_Out_2);
            float2 _Property_28c88f9028bb408c9c304133479df4e1_Out_0 = Vector2_396de5782b34402bb9ae518aadc37dd3;
            float2 _Multiply_6b554f447b10424592e322cf010e87b6_Out_2;
            Unity_Multiply_float(_Property_28c88f9028bb408c9c304133479df4e1_Out_0, (IN.TimeParameters.y.xx), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2);
            float2 _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0, 0), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3);
            float2 _Add_a0584206f0174474879b4024a569da17_Out_2;
            Unity_Add_float2(_Add_722f9063426e42fdb870bfc00a749c78_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float4 _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9499453deb424003a34c3bc36b12d418_Out_0.tex, UnityBuildSamplerStateStruct(SamplerState_Linear_Clamp).samplerstate, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_R_4 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.r;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_G_5 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.g;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_B_6 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.b;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 Texture2D_b61294243a2e4633bc60eb2b46f26c21_TexelSize;
        float Vector1_f52276ee0c144c88841da46cba7861f5;
        float Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
        float2 Vector2_36058a0d6b9840ebb800d3b8952bda6d;
        float2 Vector2_396de5782b34402bb9ae518aadc37dd3;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Clamp);
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
        SAMPLER(samplerTexture2D_b61294243a2e4633bc60eb2b46f26c21);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9499453deb424003a34c3bc36b12d418_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_adf10e54ed2747debf903ef036e9d5a3_Out_0 = IN.uv0;
            UnityTexture2D _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_b61294243a2e4633bc60eb2b46f26c21);
            float4 _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.tex, _Property_cc0096310864421bb761dbc62cb3dc6d_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_R_4 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.r;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_G_5 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.g;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_B_6 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.b;
            float _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_A_7 = _SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.a;
            float2 _Property_0c139cd12109479f994daba3054a3f44_Out_0 = Vector2_36058a0d6b9840ebb800d3b8952bda6d;
            float _Property_fe8462d772c14bb98d165ff094df5916_Out_0 = Vector1_f52276ee0c144c88841da46cba7861f5;
            float _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_fe8462d772c14bb98d165ff094df5916_Out_0, _Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2);
            float2 _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_d2136e76e1974e25a7177aa92eb6a96a_Out_2.xx), _TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3);
            float _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_1765f98861474853a8c19106f66df15d_Out_3, 10, _GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2);
            float2 _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3;
            Unity_TilingAndOffset_float((_SampleTexture2D_9c1d5568280444529a5febc1b19fdd97_RGBA_0.xy), _Property_0c139cd12109479f994daba3054a3f44_Out_0, (_GradientNoise_69e6ac7a66194d0493996633d66c8e55_Out_2.xx), _TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3);
            float _Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0 = Vector1_5c30e52eccbc4abc9197f0a9bb4f64fd;
            float2 _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2;
            Unity_Multiply_float(_TilingAndOffset_ae2663e3795143968decb3fb12c3187c_Out_3, (_Property_5b0a6a1099fb43e2a7c2154f6c50a386_Out_0.xx), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2);
            float2 _Add_722f9063426e42fdb870bfc00a749c78_Out_2;
            Unity_Add_float2((_UV_adf10e54ed2747debf903ef036e9d5a3_Out_0.xy), _Multiply_95aa86e6518b4c959b9e2d2b944c58e4_Out_2, _Add_722f9063426e42fdb870bfc00a749c78_Out_2);
            float2 _Property_28c88f9028bb408c9c304133479df4e1_Out_0 = Vector2_396de5782b34402bb9ae518aadc37dd3;
            float2 _Multiply_6b554f447b10424592e322cf010e87b6_Out_2;
            Unity_Multiply_float(_Property_28c88f9028bb408c9c304133479df4e1_Out_0, (IN.TimeParameters.y.xx), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2);
            float2 _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0, 0), _Multiply_6b554f447b10424592e322cf010e87b6_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3);
            float2 _Add_a0584206f0174474879b4024a569da17_Out_2;
            Unity_Add_float2(_Add_722f9063426e42fdb870bfc00a749c78_Out_2, _TilingAndOffset_3df5049eff7240e38cb4d81ebd0830ee_Out_3, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float4 _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9499453deb424003a34c3bc36b12d418_Out_0.tex, UnityBuildSamplerStateStruct(SamplerState_Linear_Clamp).samplerstate, _Add_a0584206f0174474879b4024a569da17_Out_2);
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_R_4 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.r;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_G_5 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.g;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_B_6 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.b;
            float _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7 = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_983b309786aa4691a1e1ac86aa1c23c4_A_7;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}