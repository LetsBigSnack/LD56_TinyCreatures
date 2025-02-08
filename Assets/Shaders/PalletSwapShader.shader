Shader "Custom/PaletteSwap" {
    // Exposed properties for texture and replacement colors.
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}  // The sprite texture.
        _Grey1 ("Color of Grey1", Color) = (1,0,0,1)      // Color to detect for Grey1.
        _Grey2 ("Color of Grey2", Color) = (1,0,0,1)      // Color to detect for Grey1.
        _Grey3 ("Color of Grey3", Color) = (1,0,0,1)      // Color to detect for Grey1.
        _Grey4 ("Color of Grey4", Color) = (1,0,0,1)      // Color to detect for Grey1.
        
        _Accent1 ("Color of Accent1", Color) = (1,0,0,1)      // Color to detect for Grey1.
        _Accent2 ("Color of Accent2", Color) = (1,0,0,1)      // Color to detect for Grey1.
        _Accent2 ("Color of Accent3", Color) = (1,0,0,1)      // Color to detect for Grey1.
        
        _Color1 ("Color for Grey1", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color2 ("Color for Grey2", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color3 ("Color for Grey3", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color4 ("Color for Grey4", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color5 ("Color for Grey5", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color6 ("Color for Grey6", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        _Color7 ("Color for Grey7", Color) = (1,0,0,1)      // Replacement for detected Grey1 – e.g., red.
        
        _Tolerance ("Color Tolerance", Range(0,0.1)) = 0.01 // Tolerance for color comparisons.
    }
    SubShader {
        // Tags to handle rendering order and transparency.
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100  // Simple shader.

        // Standard blending for transparency.
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            // Specify vertex and fragment shader entry points.
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Structure for vertex input.
            struct appdata {
                float4 vertex : POSITION; // Vertex position in object space.
                float2 uv : TEXCOORD0;    // Texture coordinates.
            };

            // Structure for data passed from vertex to fragment shader.
            struct v2f {
                float2 uv : TEXCOORD0;      // UV for texture sampling.
                float4 vertex : SV_POSITION; // Transformed vertex position.
            };

       
            sampler2D _MainTex;  
            float4 _Grey1;
            float4 _Grey2;
            float4 _Grey3;
            float4 _Grey4;
            float4 _Accent1;
            float4 _Accent2;
            float4 _Accent3;
            
            float4 _Color1;      
            float4 _Color2;  
            float4 _Color3;     
            float4 _Color4;
            float4 _Color5;
            float4 _Color6;
            float4 _Color7;  
            float _Tolerance;    

            // Vertex shader: transforms vertex positions and passes UVs.
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
           
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
