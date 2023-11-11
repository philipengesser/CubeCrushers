Shader "Custom/InvisibleMeshWithShadows_URP" // Change the shader name and category as needed
{
    Properties
    {
        // Add any properties you might need here
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Overlay" // Use Overlay queue for transparency in URP
        }

        Pass
        {
            Name "SHADOWRECEIVER"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            ZWrite On
            ZTest LEqual
            ColorMask 0 // Do not write to the color buffer

            HLSLPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3 n3ds wiiu webgl switch

            struct appdata
            {
                float4 vertex : POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            ENDHLSL
        }
    }

        Fallback "Diffuse" // Use a suitable fallback shader for unsupported platforms
}