﻿// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Voodoo/GameOps/SliderRampUltimateURP"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		_RampThreshold ("Threshold", Range(0,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularToonSize ("Toon Size", Range(0,1)) = 0.25
		_SpecularToonSmoothness ("Toon Smoothness", Range(0.001,0.5)) = 0.05
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]

		[TCP2HeaderHelp(Reflections)]
		[Toggle(TCP2_REFLECTIONS)] _UseReflections ("Enable Reflections", Float) = 0
		
		[NoScaleOffset] _Cube ("Reflection Cubemap", Cube) = "black" {}
		[TCP2ColorNoAlpha] _ReflectionCubemapColor ("Color", Color) = (1,1,1,1)
		_ReflectionCubemapRoughness ("Cubemap Roughness", Range(0,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(MatCap)]
		[Toggle(TCP2_MATCAP)] _UseMatCap ("Enable MatCap", Float) = 0
		[NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "black" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[Toggle(_NORMALMAP)] _UseNormalMap ("Enable Normal Mapping", Float) = 0
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1
		[TCP2Separator]
		
		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Opaque"
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#if UNITY_VERSION >= 202020
			#define URP_10_OR_NEWER
		#endif

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		// Uniforms

		// Shader Properties
		sampler2D _BumpMap;
		sampler2D _BaseMap;

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float4 _BumpMap_ST;
			float _BumpScale;
			float4 _BaseMap_ST;
			fixed4 _BaseColor;
			fixed4 _MatCapColor;
			float _RampThreshold;
			float _RampSmoothing;
			float _RimMin;
			float _RimMax;
			fixed4 _RimColor;
			float _SpecularToonSize;
			float _SpecularToonSmoothness;
			fixed4 _SpecularColor;
			fixed4 _SColor;
			fixed4 _HColor;
			float _ReflectionCubemapRoughness;
			fixed4 _ReflectionCubemapColor;
			samplerCUBE _Cube;
			sampler2D _MatCapTex;
		CBUFFER_END

		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		Pass
		{
			Name "Main"
			Tags
			{
				"LightMode"="UniversalForward"
			}

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			#pragma shader_feature _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			// -------------------------------------

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature TCP2_SPECULAR
			#pragma shader_feature TCP2_RIM_LIGHTING
			#pragma shader_feature TCP2_REFLECTIONS
			#pragma shader_feature TCP2_MATCAP
			#pragma shader_feature _NORMALMAP

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex output / fragment input
			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float4 worldPosAndFog : TEXCOORD0;
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord    : TEXCOORD1; // compute shadow coord per-vertex for the main light
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				half3 vertexLights : TEXCOORD2;
			#endif
				float3 pack0 : TEXCOORD3; /* pack0.xyz = tangent */
				float3 pack1 : TEXCOORD4; /* pack1.xyz = bitangent */
				float4 pack2 : TEXCOORD5; /* pack2.xy = texcoord0  pack2.zw = matcap */
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings Vertex(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				// Texture Coordinates
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal, input.tangent);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// normal
				output.normal = NormalizeNormalPerVertex(vertexNormalInput.normalWS);

				// tangent
				output.pack0.xyz = vertexNormalInput.tangentWS;
				output.pack1.xyz = vertexNormalInput.bitangentWS;

				// clip position
				output.positionCS = vertexInput.positionCS;

				#if defined(TCP2_RIM_LIGHTING)
				#endif
				#if defined(TCP2_MATCAP)
				//MatCap
				float3 worldNorm = normalize(unity_WorldToObject[0].xyz * input.normal.x + unity_WorldToObject[1].xyz * input.normal.y + unity_WorldToObject[2].xyz * input.normal.z);
				worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
				output.pack2.zw = worldNorm.xy * 0.5 + 0.5;
				#endif

				return output;
			}

			half4 Fragment(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = NormalizeNormalPerPixel(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half3 tangentWS = input.pack0.xyz;
				half3 bitangentWS = input.pack1.xyz;
				#if defined(_NORMALMAP)
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);
				#endif

				// Shader Properties Sampling
				float4 __normalMap = ( tex2D(_BumpMap, input.pack2.xy * _BumpMap_ST.xy + _BumpMap_ST.zw).rgba );
				float __bumpScale = ( _BumpScale );
				float4 __albedo = ( tex2D(_BaseMap, input.pack2.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __ambientIntensity = ( 1.0 );
				float3 __matcapColor = ( _MatCapColor.rgb );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float __rimMin = ( _RimMin );
				float __rimMax = ( _RimMax );
				float3 __rimColor = ( _RimColor.rgb );
				float __rimStrength = ( 1.0 );
				float __specularToonSize = ( _SpecularToonSize );
				float __specularToonSmoothness = ( _SpecularToonSmoothness );
				float3 __specularColor = ( _SpecularColor.rgb );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float __reflectionCubemapRoughness = ( _ReflectionCubemapRoughness );
				float3 __reflectionCubemapColor = ( _ReflectionCubemapColor.rgb );

				#if defined(_NORMALMAP)
				half4 normalMap = half4(0,0,0,0);
				normalMap = __normalMap;
				half3 normalTS = UnpackNormalScale(normalMap, __bumpScale);
				normalWS = normalize( mul(normalTS, tangentToWorldMatrix) );
				#endif

				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				
				albedo *= __mainColor.rgb;

				// main light: direction, color, distanceAttenuation, shadowAttenuation
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord = input.shadowCoord;
			#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
			#else
				float4 shadowCoord = float4(0, 0, 0, 0);
			#endif

			#if defined(URP_10_OR_NEWER)
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
					half4 shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
				#elif !defined (LIGHTMAP_ON)
					half4 shadowMask = unity_ProbesOcclusion;
				#else
					half4 shadowMask = half4(1, 1, 1, 1);
				#endif

				Light mainLight = GetMainLight(shadowCoord, positionWS, shadowMask);
			#else
				Light mainLight = GetMainLight(shadowCoord);
			#endif

				// ambient or lightmap
				// Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
				// are also defined in case you want to sample some terms per-vertex.
				half3 bakedGI = SampleSH(normalWS);
				half occlusion = 1;

				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;

				//MatCap
				#if defined(TCP2_MATCAP)
				fixed3 matcap = tex2D(_MatCapTex, input.pack2.zw).rgb * __matcapColor;
				emission += matcap;
				#endif

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				half3 ramp;
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				ndl = saturate(ndl);
				ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);

				// apply attenuation
				ramp *= atten;

				half3 color = half3(0,0,0);
				// Rim Lighting
				#if defined(TCP2_RIM_LIGHTING)
				half rim = 1 - ndvRaw;
				rim = ( rim );
				half rimMin = __rimMin;
				half rimMax = __rimMax;
				rim = smoothstep(rimMin, rimMax, rim);
				half3 rimColor = __rimColor;
				half rimStrength = __rimStrength;
				emission.rgb += rim * rimColor * rimStrength;
				#endif
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

				#if defined(TCP2_SPECULAR)
				//Blinn-Phong Specular
				half3 h = normalize(lightDir + viewDirWS);
				float ndh = max(0, dot (normalWS, h));
				float spec = smoothstep(__specularToonSize + __specularToonSmoothness, __specularToonSize - __specularToonSmoothness,1 - (ndh / (1+__specularToonSmoothness)));
				spec *= ndl;
				spec *= atten;
				
				//Apply specular
				emission.rgb += spec * lightColor.rgb * __specularColor;
				#endif

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				uint additionalLightsCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
				{
					#if defined(URP_10_OR_NEWER)
						Light light = GetAdditionalLight(lightIndex, positionWS, shadowMask);
					#else
						Light light = GetAdditionalLight(lightIndex, positionWS);
					#endif
					half atten = light.shadowAttenuation * light.distanceAttenuation;
					half3 lightDir = light.direction;
					half3 lightColor = light.color.rgb;

					half ndl = dot(normalWS, lightDir);
					half3 ramp;
					
					ndl = saturate(ndl);
					ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColors += ramp * lightColor.rgb;

					#if defined(TCP2_SPECULAR)
					//Blinn-Phong Specular
					half3 h = normalize(lightDir + viewDirWS);
					float ndh = max(0, dot (normalWS, h));
					float spec = smoothstep(__specularToonSize + __specularToonSmoothness, __specularToonSize - __specularToonSmoothness,1 - (ndh / (1+__specularToonSmoothness)));
					spec *= ndl;
					spec *= atten;
					
					//Apply specular
					emission.rgb += spec * lightColor.rgb * __specularColor;
					#endif
				}
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				accumulatedRamp = saturate(accumulatedRamp);
				half3 shadowColor = (1 - accumulatedRamp.rgb) * __shadowColor;
				accumulatedRamp = accumulatedColors.rgb * __highlightColor + shadowColor;
				color += albedo * accumulatedRamp;

				// apply ambient
				color += indirectDiffuse;

				half3 reflections = half3(0, 0, 0);
				#if defined(TCP2_REFLECTIONS)
				// Reflection cubemap
				half3 reflectVector = reflect(-viewDirWS, normalWS);
				reflections.rgb += texCUBElod(_Cube, half4(reflectVector.xyz, __reflectionCubemapRoughness * 10.0)).rgb;
				reflections.rgb *= __reflectionCubemapColor;
				#endif
				color.rgb += reflections;

				color += emission;

				return half4(color, alpha);
			}
			ENDHLSL
		}

		// Depth & Shadow Caster Passes
		HLSLINCLUDE

		#if defined(SHADOW_CASTER_PASS) || defined(DEPTH_ONLY_PASS)

			#define fixed half
			#define fixed2 half2
			#define fixed3 half3
			#define fixed4 half4

			float3 _LightDirection;

			struct Attributes
			{
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float3 pack0 : TEXCOORD1; /* pack0.xyz = positionWS */
				float4 pack1 : TEXCOORD2; /* pack1.xy = texcoord0  pack1.zw = matcap */
			#if defined(DEPTH_ONLY_PASS)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			#endif
			};

			float4 GetShadowPositionHClip(Attributes input)
			{
				float3 positionWS = TransformObjectToWorld(input.vertex.xyz);
				float3 normalWS = TransformObjectToWorldNormal(input.normal);

				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				return positionCS;
			}

			Varyings ShadowDepthPassVertex(Attributes input)
			{
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				float3 worldNormalUv = mul(unity_ObjectToWorld, float4(input.normal, 1.0)).xyz;

				// Texture Coordinates
				output.pack1.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
				output.normal = NormalizeNormalPerVertex(worldNormalUv);
				output.pack0.xyz = vertexInput.positionWS;

				#if defined(DEPTH_ONLY_PASS)
					output.positionCS = TransformObjectToHClip(input.vertex.xyz);
				#elif defined(SHADOW_CASTER_PASS)
					output.positionCS = GetShadowPositionHClip(input);
				#else
					output.positionCS = float4(0,0,0,0);
				#endif

				return output;
			}

			half4 ShadowDepthPassFragment(Varyings input) : SV_TARGET
			{
				#if defined(DEPTH_ONLY_PASS)
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				#endif

				float3 positionWS = input.pack0.xyz;
				float3 normalWS = NormalizeNormalPerPixel(input.normal);

				// Shader Properties Sampling
				float4 __albedo = ( tex2D(_BaseMap, input.pack1.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );

				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);

				return 0;
			}

		#endif
		ENDHLSL

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile SHADOW_CASTER_PASS

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "DepthOnly"
			Tags
			{
				"LightMode" = "DepthOnly"
			}

			ZWrite On
			ColorMask 0

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			ENDHLSL
		}

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2019.4.22f1";ver:"2.7.2";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","SPEC_LEGACY","SPECULAR","SPECULAR_TOON","RIM","SPECULAR_SHADER_FEATURE","RIM_SHADER_FEATURE","REFLECTION_CUBEMAP","REFLECTION_SHADER_FEATURE","MATCAP_ADD","MATCAP","MATCAP_SHADER_FEATURE","BUMP","BUMP_SHADER_FEATURE","REFL_ROUGH","BUMP_SCALE","TEMPLATE_LWRP"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[,,,,sp(name:"Ramp Threshold";imps:list[imp_mp_range(def:0.5;min:0;max:1;prop:"_RampThreshold";md:"";gbv:False;custom:False;refs:"";guid:"314d49f5-81f7-41c8-9cea-7bdd6f9e2061";op:Multiply;lbl:"Threshold";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Ramp Smoothing";imps:list[imp_mp_range(def:0.1;min:0.001;max:1;prop:"_RampSmoothing";md:"";gbv:False;custom:False;refs:"";guid:"0e926a6d-4108-4db1-b4d9-0b29a501ce77";op:Multiply;lbl:"Smoothing";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,,,,,,,,,sp(name:"Normal Map";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"bump";locked_uv:False;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_BumpMap";md:"";gbv:False;custom:False;refs:"";guid:"b8774ef2-b5cd-4257-85d3-973cda9b6e79";op:Multiply;lbl:"Normal Map";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 9f536a0bee427e705ea9de83210166ba */
