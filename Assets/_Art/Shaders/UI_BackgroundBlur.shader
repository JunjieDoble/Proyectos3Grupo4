Shader "Custom/UI/BackgroundBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _BlurSize ("Blur Size", Range(0, 20)) = 5
        _BlurIter ("Blur Iterations", Range(1, 10)) = 3
        
        // Required for UI Masking
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 positionNDC: TEXCOORD0;
                float2 uv         : TEXCOORD1;
                float4 color      : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            // Camera Opaque Texture from URP
            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);
            float4 _CameraOpaqueTexture_TexelSize;

            float4 _Color;
            float _BlurSize;
            float _BlurIter;

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionNDC = vertexInput.positionNDC;
                output.uv = input.uv;
                output.color = input.color * _Color;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float2 screenUV = input.positionNDC.xy / input.positionNDC.w;
                
                // Sample the UI sprite mask/texture
                float4 mainTexColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // The alpha channel of the sprite or vertex color controls the intensity of the blur/gradient
                float alpha = mainTexColor.a * input.color.a;

                // Simple box/gaussian blur kernel
                float4 col = float4(0, 0, 0, 0);
                float totalWeight = 0;
                
                int iterations = (int)_BlurIter;
                float2 blurRadius = _BlurSize * _CameraOpaqueTexture_TexelSize.xy;

                for (int x = -iterations; x <= iterations; x++)
                {
                    for (int y = -iterations; y <= iterations; y++)
                    {
                        float2 offset = float2(x, y) * blurRadius;
                        col += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV + offset);
                        totalWeight += 1.0;
                    }
                }
                
                col /= totalWeight;

                // Tint the blur effect with the UI color and apply alpha masking
                float4 finalColor = float4(col.rgb * input.color.rgb, alpha);
                return finalColor;
            }
            ENDHLSL
        }
    }
}
