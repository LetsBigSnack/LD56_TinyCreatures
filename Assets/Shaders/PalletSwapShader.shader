Shader "Custom/UI/PaletteSwap" {
    Properties{
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Grey1("Color of Grey1", Color) = (1,0,0,1)
        _Grey2("Color of Grey2", Color) = (1,0,0,1)
        _Grey3("Color of Grey3", Color) = (1,0,0,1)
        _Grey4("Color of Grey4", Color) = (1,0,0,1)

        _Accent1("Color of Accent1", Color) = (1,0,0,1)
        _Accent2("Color of Accent2", Color) = (1,0,0,1)
        _Accent3("Color of Accent3", Color) = (1,0,0,1)

        _Color1("Color for Grey1", Color) = (1,0,0,1)
        _Color2("Color for Grey2", Color) = (1,0,0,1)
        _Color3("Color for Grey3", Color) = (1,0,0,1)
        _Color4("Color for Grey4", Color) = (1,0,0,1)
        _Color5("Color for Grey5", Color) = (1,0,0,1)
        _Color6("Color for Grey6", Color) = (1,0,0,1)
        _Color7("Color for Grey7", Color) = (1,0,0,1)

        _Tolerance("Color Tolerance", Range(0,0.1)) = 0.01
    }

        SubShader{
            Tags { "Queue" = "Overlay" "RenderType" = "Transparent" "IgnoreProjector" = "True" "Canvas" = "UI" }
            LOD 100

            // Handles alpha blending properly for UI.
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _Grey1, _Grey2, _Grey3, _Grey4;
                float4 _Accent1, _Accent2, _Accent3;
                float4 _Color1, _Color2, _Color3, _Color4;
                float4 _Color5, _Color6, _Color7;
                float _Tolerance;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);

                    if (distance(col.rgb, _Grey1.rgb) < _Tolerance) {
                        col.rgb = _Color1.rgb;
                    }
                    if (distance(col.rgb, _Grey2.rgb) < _Tolerance) {
                        col.rgb = _Color2.rgb;
                    }
                    if (distance(col.rgb, _Grey3.rgb) < _Tolerance) {
                        col.rgb = _Color3.rgb;
                    }
                    if (distance(col.rgb, _Grey4.rgb) < _Tolerance) {
                        col.rgb = _Color4.rgb;
                    }
                    if (distance(col.rgb, _Accent1.rgb) < _Tolerance) {
                        col.rgb = _Color5.rgb;
                    }
                    if (distance(col.rgb, _Accent2.rgb) < _Tolerance) {
                        col.rgb = _Color6.rgb;
                    }
                    if (distance(col.rgb, _Accent3.rgb) < _Tolerance) {
                        col.rgb = _Color7.rgb;
                    }

                    return col;
                }
                ENDCG
            }
        }
}
