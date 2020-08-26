Shader "Hidden/InjuredFlash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint",Color)=(1,1,1,1)
        _FlashColor("FlashColor",Color) = (1,1,1,1)
        _FlashAmount("FlashAmount",Range(0,1))=0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
		}
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;
            fixed4 _FlashColor;
            float _FlashAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.rgb = lerp(col.rgb,_FlashColor.rgb,_FlashAmount);
                col.rgb *= col.a;
                clip(col.a - 0.5);
                return col;
            }
            ENDCG
        }
    }
}
