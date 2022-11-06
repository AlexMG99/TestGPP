Shader "Stylized/Sky"
{
    Properties
    {
        [Header(Horizon Line)]
        _HorizonLineColor ("Color", Color) = (0.9044118, 0.8872592, 0.7913603, 1)
        _HorizonLineExponent ("Exponent", float) = 4
        _HorizonLineContribution ("Contribution", Range(0, 1)) = 0.25
        
        [Header(Sky Gradient)]
        _SkyGradientTop ("Top", Color) = (0.172549, 0.5686274, 0.6941177, 1)
        _SkyGradientBottom ("Bottom", Color) = (0.764706, 0.8156863, 0.8509805)
        _SkyGradientExponent ("Exponent", float) = 2.5
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Background"
            "Queue" = "Background"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float3 _HorizonLineColor;
            float _HorizonLineExponent;
            float _HorizonLineContribution;

            float3 _SkyGradientTop;
            float3 _SkyGradientBottom;
            float _SkyGradientExponent;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Masks.
                float maskHorizon = dot(normalize(i.worldPosition), float3(0, 1, 0));

                // Horizon line.
                float3 horizonLineColor = _HorizonLineColor * saturate(pow(1 - abs(maskHorizon), _HorizonLineExponent));
                horizonLineColor = lerp(0, horizonLineColor, _HorizonLineContribution);

                // Sky gradient.
                float3 skyGradientColor = lerp(_SkyGradientTop, _SkyGradientBottom, pow(1 - saturate(maskHorizon), _SkyGradientExponent));

                float3 finalColor = saturate(horizonLineColor + skyGradientColor);
                return float4(finalColor, 1);
            }
            ENDCG
        }
    }
}
