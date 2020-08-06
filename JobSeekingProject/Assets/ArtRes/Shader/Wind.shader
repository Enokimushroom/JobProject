Shader "Custom/Wind"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _alphaValue("alphavalue",range(0,1))=0.5
        _Color("Color",Color) = (1,1,1,1)
        _TimeScale("TimeScale",float) = 1
    }
    SubShader
    {
        Tags {"DisableBatching" = "True"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord :TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _Color;
            fixed _alphaValue;
            float _TimeScale;
            

            v2f vert (appdata v)
            {
                v2f o;

                float4 offset = float4(0,0,0,0);
                offset.x = sin(3.1416 * _Time * _TimeScale)*0.5;
                o.pos = UnityObjectToClipPos(v.vertex + offset* clamp(v.texcoord.y-0.5, 0, 1));
                o.uv = v.texcoord;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _alphaValue);

                return col;
            }
            ENDCG
        }
    }
}
