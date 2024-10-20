Shader "UI/CRT_Shader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0, 2)) = 1.0
        _Size("Scanline Size", Float) = 100.0
        _Speed("Speed", Float) = 1.0
        _Opacity("Opacity", Range(0, 1)) = 1.0
        _Color("Color Tint", Color) = (1, 1, 1, 1) // New color tint
        _BlurAmount("Blur Amount", Range(0, 5)) = 1.0 // New blur amount
    }
        SubShader
        {
            Tags {"RenderType" = "Transparent" "Queue" = "Overlay"}
            LOD 100

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Brightness;
                float _Size;
                float _Speed;
                float _Opacity;
                float4 _Color; // New color tint
                float _BlurAmount; // New blur amount

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    return o;
                }

                fixed4 Blur(sampler2D tex, float2 uv, float amount)
                {
                    float2 offset = amount / 500.0;
                    fixed4 color = tex2D(tex, uv) * 0.36;
                    color += tex2D(tex, uv + float2(-offset.x, -offset.y)) * 0.14;
                    color += tex2D(tex, uv + float2(offset.x, offset.y)) * 0.14;
                    color += tex2D(tex, uv + float2(-offset.x, offset.y)) * 0.14;
                    color += tex2D(tex, uv + float2(offset.x, -offset.y)) * 0.14;
                    return color;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the original UI image texture with blur
                    fixed4 col = Blur(_MainTex, i.uv, _BlurAmount);

                // Apply CRT scanline effect using sine waves based on UV.y
                float scanline = 0.5 + 0.5 * sin(i.uv.y * _Size + _Time.y * _Speed);

                // Multiply the brightness
                col.rgb *= _Brightness;

                // Darken the image using the scanline effect
                col.rgb *= scanline;

                // Apply color tint
                col.rgb *= _Color.rgb;

                // Apply opacity
                col.a *= _Opacity;

                return col;
            }
            ENDCG
        }
        }
            FallBack "UI/Default"
}
