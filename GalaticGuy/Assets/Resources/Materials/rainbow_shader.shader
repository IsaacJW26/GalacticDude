Shader "Unlit/rainbow_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _color0 ("BaseColor", Color ) = (1,1,1)
        _color1 ("Tint1", Color ) = (0,0,0)
        _color2 ("Tint2", Color ) = (0,0,0)
        _color3 ("Tint3", Color ) = (0,0,0)

        _ScrollSpeed ("ScrollSpeed", Float) = 1
        _BandDist ("Band Distance", Range(1,100)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _color0;
            float4 _color1;
            float4 _color2;
            float4 _color3;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            float _BandDist;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float centreDist = i.vertex;
                col = col * _color0;

                col = col * (1-(sin((_Time.g  * _ScrollSpeed) + i.vertex.y / _BandDist)+1) / 3) +
                    (col * _color1 * (sin((_Time.g  * _ScrollSpeed) + i.vertex.y / _BandDist)+2) / 3);

                col = col * (1-(sin((_Time.g  * _ScrollSpeed* 1.5) + i.vertex.y / _BandDist)+1) / 2) +
                    ( col * _color2 * (sin((_Time.g  * _ScrollSpeed* 1.5) + i.vertex.y / _BandDist)+2) / 3);
                // col = col * _color2 * (sin((_Time.g  * _ScrollSpeed * 1.5) + i.vertex.y / _BandDist)+1) / 2;

                col = col * (1-(sin((_Time.g  * _ScrollSpeed*-1.3) + i.vertex.y / _BandDist)+1) / 3) +
                    ( col * _color3 * (sin((_Time.g  * _ScrollSpeed*-1.3) + i.vertex.y / _BandDist)+2) / 3);
                // col = col * _color3 * (sin((_Time.g  * _ScrollSpeed * -1.3) + i.vertex.y / _BandDist)+1) / 2;

                return col;
            }
            ENDCG
        }
    }
}
