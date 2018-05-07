// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MyShaders/meshDisplacerShader"
{
    Properties
    {
		_BaseColor("Base Color", Color) = (0,1,1,1)
		_MainTex ("NoiseA", 2D) = "white" {}
		_Noise("NoiseB", 2D) = "white" {}
		_Displacement ("Displacement", Range(0, 1)) = 0.0 
    }
    SubShader
    {
        Tags
        { 
			 "RenderType"="Opaque" 
        }
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
            struct appdata_t
            {
                float4 vertex   : POSITION;
				float3 normal: NORMAL;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
				float4 vertex   : SV_POSITION;
                half2 texcoord  : TEXCOORD0;
            };

			sampler2D _MainTex;
			sampler2D _Noise;
			half4 _BaseColor;
			float _Displacement;

            v2f vert(appdata_t IN)
            {
				v2f OUT;
				
				half4 noise = tex2D(_Noise, IN.texcoord + _Time[0]);

				half2 uv = half2(
					noise.r * 0.01f,
					noise.r * 0.01f);
				
				half4 texcol = tex2D(_MainTex, IN.texcoord + uv);
				texcol = pow(texcol, sin(_Time[0] * 20) + 2);



				IN.vertex.x  += (IN.normal.x * sin(_Time[0] * 30 + IN.vertex.y * 10) * 0.3f) * _Displacement;
				IN.vertex.y  += (IN.normal.y * sin(_Time[0] * 30 + IN.vertex.z * 10) * 0.3f) * _Displacement;
				IN.vertex.z  += (IN.normal.z * sin(_Time[0] * 30 + IN.vertex.x * 10) * 0.3f) * _Displacement;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
                return OUT;
            }



			fixed4 frag(v2f IN) : COLOR
			{
				half4 noise = tex2D(_Noise, IN.texcoord + _Time[0]);

				half2 uv = half2(
					noise.r * 0.01f,
					noise.r * 0.01f);
				
				half4 texcol = tex2D(_MainTex, IN.texcoord + uv);
				texcol = pow(texcol, sin(_Time[0] * 20) + 2);
				return texcol * _BaseColor;
			}
        ENDCG
        }
    }
    Fallback "Standard"
}
