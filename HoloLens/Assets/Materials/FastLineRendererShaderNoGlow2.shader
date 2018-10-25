Shader "Custom/FastLineRendererShaderNoGlow2"
{
	Properties
	{
		_MainTex("Line Texture (RGB) Alpha (A)", 2D) = "white" {}
		_MainTexStartCap("Line Texture Start Cap (RGB) Alpha (A)", 2D) = "transparent" {}
		_MainTexEndCap("Line Texture End Cap (RGB) Alpha (A)", 2D) = "transparent" {}
		_MainTexRoundJoin("Line Texture Round Join (RGB) Alpha (A)", 2D) = "transparent" {}
		_TintColor("Tint Color (RGB)", Color) = (1, 1, 1, 1)
		_AnimationSpeed("Animation Speed (Float)", Float) = 0
		_UVXScale("UV X Scale (Float)", Float) = 1.0
		_UVYScale("UV Y Scale (Float)", Float) = 1.0

		_AnimationSpeedGlow("Glow Animation Speed (Float)", Float) = 0
		_UVXScaleGlow("Glow UV X Scale (Float)", Float) = 1.0
		_UVYScaleGlow("Glow UV Y Scale (Float)", Float) = 1.0

		_InvFade("Soft Particles Factor", Range(0.01, 3.0)) = 1.0
		_JitterMultiplier("Jitter Multiplier (Float)", Float) = 0.0
		_ScreenRadiusMultiplier("Screen Radius Multiplier (Float)", Float) = 0.0
    }

    SubShader
	{
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent+10" "LightMode"="Always" "PreviewType"="Plane"}
		UsePass "Custom/FastLineRendererShader/LINEPASS"
    }
}