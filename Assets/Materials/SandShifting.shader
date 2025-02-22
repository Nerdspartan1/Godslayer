﻿Shader "Custom/SandShifting"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		[Normal]
		_BumpMap("Normal", 2D) = "white" {}
		_NormalIntensity ("Normal Intensity", Range(0,1)) = 1
		_ShiftTex("Shift Pattern", 2D) = "white" {}
		_Intensity("Intensity", Range(0,1)) = 0.2
		_Speed("Speed", Vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _ShiftTex;
		sampler2D _BumpMap;
		float _NormalIntensity;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		
		float _Intensity;
		float2 _Speed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 w = tex2D(_ShiftTex, IN.uv_MainTex + _Speed.xy * _Time.yy);

            o.Albedo = c.rgb + _Intensity*w.rgb;
			o.Normal += _NormalIntensity * UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
