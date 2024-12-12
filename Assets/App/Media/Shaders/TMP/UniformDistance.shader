Shader "Unlit/UniformDistance"
{
        Properties
    {
        _FaceColor ("Face Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.2
        _FaceTex ("Face Texture", 2D) = "white" {}
        _Size ("Size on Screen", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Name "TextMeshPro/ConstantSize"
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _FaceTex;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _Size;

            v2f vert (appdata_t v)
            {
                v2f o;

                // Получаем мировую позицию вершины
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Получаем расстояние до камеры
                float3 cameraPos = _WorldSpaceCameraPos;
                float distance = length(worldPos - cameraPos);

                // Вычисляем масштаб
                float scale = _Size / distance;

                // Масштабируем вершину
                float4 scaledVertex = v.vertex;
                scaledVertex.xy *= scale;

                o.vertex = UnityObjectToClipPos(scaledVertex);
                o.uv = v.texcoord;
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Рендер текста с использованием Face Texture
                fixed4 col = tex2D(_FaceTex, i.uv) * i.color;
                col.a *= _FaceColor.a; // Учет альфа-канала
                return col;
            }
            ENDCG
        }
    }

    FallBack "TextMeshPro/Distance Field"
}
