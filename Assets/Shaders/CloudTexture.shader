Shader "UI/CloudTexture"
{
    Properties
    {
        _Speed("Speed", Vector) = (0.1, 0, 0.1, 0)
        _MainTex("Main Texture", 2D) = "white" {}
        _MainTex2("Secondary Texture", 2D) = "white" {}
        _Opacity("Opacity", Range(0, 1)) = 1.0
        _Color("Cloud Color", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off

            Pass
            {
                Name "CloudTexture"

                CGPROGRAM
                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float4 _Speed;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _MainTex2;
                float4 _MainTex2_ST;
                float _Opacity;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                float4 frag(v2f IN) : SV_Target
                {
                    float2 uv = IN.uv;

                    // Sample the first cloud texture
                    float4 cloud = tex2D(_MainTex, uv + frac(_Time * _Speed.xy));

                    // Sample the second cloud texture
                    float4 cloud2 = tex2D(_MainTex2, uv + frac(_Time * _Speed.zw));

                    // Combine the textures and apply the color and opacity
                    float4 result = cloud * cloud2 * _Color;
                    result.a *= saturate(_Opacity); // Control alpha with the _Opacity property

                    return result;
                }
                ENDCG
            }
        }

            FallBack "UI/Default"
}
