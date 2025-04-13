Shader "Unlit/ScrollingTexture"
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
            "IgnoreProjector"="True"
        }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        CGPROGRAM
        #pragma surface surf Lambert alpha
        
        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _GlowColor;
        float _GlowPower;
        float _ScrollSpeed;
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Зацикленные UV с движением
            float2 scrolledUV = IN.uv_MainTex;
            scrolledUV.x = frac(IN.uv_MainTex.x + _Time.y * _ScrollSpeed);
            
            fixed4 texColor = tex2D(_MainTex, scrolledUV);
            fixed4 col = texColor * _Color;
            
            // Свечение только по альфа-каналу
            fixed3 glow = _GlowColor.rgb * _GlowPower * texColor.a;
            col.rgb += glow;
            
            o.Albedo = col.rgb;
            o.Alpha = texColor.a * _Color.a;
        }
        ENDCG
    }
    FallBack "Transparent"
}
