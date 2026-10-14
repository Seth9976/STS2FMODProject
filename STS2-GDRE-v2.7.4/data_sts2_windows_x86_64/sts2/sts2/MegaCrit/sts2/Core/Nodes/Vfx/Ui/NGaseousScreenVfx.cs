using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui
{
	// Token: 0x02000228 RID: 552
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NGaseousScreenVfx.cs")]
	public class NGaseousScreenVfx : AspectRatioContainer
	{
		// Token: 0x06001BFB RID: 7163 RVA: 0x000B59ED File Offset: 0x000B3BED
		[NullableContext(2)]
		public static NGaseousScreenVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NGaseousScreenVfx._scenePath).Instantiate<NGaseousScreenVfx>(0L);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000B5A10 File Offset: 0x000B3C10
		public override void _Ready()
		{
			this._originalMaterial = this._gfx.Material;
			this._materialCopy = (ShaderMaterial)this._originalMaterial.Duplicate(true);
			this._gfx.Material = this._materialCopy;
			this.SetProperties(1f);
			this.Play();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000B5A68 File Offset: 0x000B3C68
		private void SetProperties(float interpolation)
		{
			float num = this._alphaMultiplierCurve.Sample(interpolation);
			float num2 = this._minBaseAlphaCurve.Sample(interpolation);
			float num3 = this._noiseAOffsetCurve.Sample(interpolation);
			float num4 = this._noiseBOffsetCurve.Sample(interpolation);
			float num5 = this._erosionCurve.Sample(interpolation);
			this._materialCopy.SetShaderParameter(NGaseousScreenVfx._alphaMultiplierString, num);
			this._materialCopy.SetShaderParameter(NGaseousScreenVfx._minBaseAlphaString, num2);
			this._materialCopy.SetShaderParameter(NGaseousScreenVfx._noiseAOffsetString, new Vector2(num3, this._noiseAOffsetY));
			this._materialCopy.SetShaderParameter(NGaseousScreenVfx._noiseBOffsetString, new Vector2(num4, this._noiseBOffsetY));
			this._materialCopy.SetShaderParameter(NGaseousScreenVfx._erosionString, num5);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000B5B3C File Offset: 0x000B3D3C
		private void Play()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000B5B4C File Offset: 0x000B3D4C
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._noiseAOffsetY = GD.Randf();
			this._noiseBOffsetY = GD.Randf();
			double timer = 0.0;
			this.SetProperties(0f);
			while (timer < (double)this._duration)
			{
				float num = (float)(timer / (double)this._duration);
				this.SetProperties(num);
				timer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			this.SetProperties(1f);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C00 RID: 7168 RVA: 0x000B5B90 File Offset: 0x000B3D90
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGaseousScreenVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("AspectRatioContainer"), false), 33L, null, null),
				new MethodInfo(NGaseousScreenVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGaseousScreenVfx.MethodName.SetProperties, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "interpolation", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGaseousScreenVfx.MethodName.Play, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C01 RID: 7169 RVA: 0x000B5C94 File Offset: 0x000B3E94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGaseousScreenVfx.MethodName.Create && args.Count == 0)
			{
				NGaseousScreenVfx ngaseousScreenVfx = NGaseousScreenVfx.Create();
				ret = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref ngaseousScreenVfx);
				return true;
			}
			if ((ref method) == NGaseousScreenVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGaseousScreenVfx.MethodName.SetProperties && args.Count == 1)
			{
				this.SetProperties(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGaseousScreenVfx.MethodName.Play && args.Count == 0)
			{
				this.Play();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000B5D54 File Offset: 0x000B3F54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGaseousScreenVfx.MethodName.Create && args.Count == 0)
			{
				NGaseousScreenVfx ngaseousScreenVfx = NGaseousScreenVfx.Create();
				ret = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref ngaseousScreenVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C03 RID: 7171 RVA: 0x000B5D94 File Offset: 0x000B3F94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGaseousScreenVfx.MethodName.Create || (ref method) == NGaseousScreenVfx.MethodName._Ready || (ref method) == NGaseousScreenVfx.MethodName.SetProperties || (ref method) == NGaseousScreenVfx.MethodName.Play || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C04 RID: 7172 RVA: 0x000B5DE4 File Offset: 0x000B3FE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGaseousScreenVfx.PropertyName._gfx)
			{
				this._gfx = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._alphaMultiplierCurve)
			{
				this._alphaMultiplierCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._minBaseAlphaCurve)
			{
				this._minBaseAlphaCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._erosionCurve)
			{
				this._erosionCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseAOffsetCurve)
			{
				this._noiseAOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseBOffsetCurve)
			{
				this._noiseBOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._originalMaterial)
			{
				this._originalMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._materialCopy)
			{
				this._materialCopy = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseAOffsetY)
			{
				this._noiseAOffsetY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseBOffsetY)
			{
				this._noiseBOffsetY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C05 RID: 7173 RVA: 0x000B5F24 File Offset: 0x000B4124
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGaseousScreenVfx.PropertyName._gfx)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._gfx);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._alphaMultiplierCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._alphaMultiplierCurve);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._minBaseAlphaCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._minBaseAlphaCurve);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._erosionCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._erosionCurve);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseAOffsetCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._noiseAOffsetCurve);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseBOffsetCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._noiseBOffsetCurve);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._originalMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._originalMaterial);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._materialCopy)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._materialCopy);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseAOffsetY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._noiseAOffsetY);
				return true;
			}
			if ((ref name) == NGaseousScreenVfx.PropertyName._noiseBOffsetY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._noiseBOffsetY);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C06 RID: 7174 RVA: 0x000B609C File Offset: 0x000B429C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._gfx, 34L, "ColorRect", 4102L, true),
				new PropertyInfo(3L, NGaseousScreenVfx.PropertyName._duration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._erosionCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._originalMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGaseousScreenVfx.PropertyName._materialCopy, 0L, "", 4096L, false),
				new PropertyInfo(3L, NGaseousScreenVfx.PropertyName._noiseAOffsetY, 0L, "", 4096L, false),
				new PropertyInfo(3L, NGaseousScreenVfx.PropertyName._noiseBOffsetY, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C07 RID: 7175 RVA: 0x000B6220 File Offset: 0x000B4420
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGaseousScreenVfx.PropertyName._gfx, Variant.From<ColorRect>(ref this._gfx));
			info.AddProperty(NGaseousScreenVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, Variant.From<Curve>(ref this._alphaMultiplierCurve));
			info.AddProperty(NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, Variant.From<Curve>(ref this._minBaseAlphaCurve));
			info.AddProperty(NGaseousScreenVfx.PropertyName._erosionCurve, Variant.From<Curve>(ref this._erosionCurve));
			info.AddProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, Variant.From<Curve>(ref this._noiseAOffsetCurve));
			info.AddProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, Variant.From<Curve>(ref this._noiseBOffsetCurve));
			info.AddProperty(NGaseousScreenVfx.PropertyName._originalMaterial, Variant.From<Material>(ref this._originalMaterial));
			info.AddProperty(NGaseousScreenVfx.PropertyName._materialCopy, Variant.From<ShaderMaterial>(ref this._materialCopy));
			info.AddProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetY, Variant.From<float>(ref this._noiseAOffsetY));
			info.AddProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetY, Variant.From<float>(ref this._noiseBOffsetY));
		}

		/// <inheritdoc />
		// Token: 0x06001C08 RID: 7176 RVA: 0x000B6328 File Offset: 0x000B4528
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._gfx, ref variant))
			{
				this._gfx = variant.As<ColorRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._duration, ref variant2))
			{
				this._duration = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, ref variant3))
			{
				this._alphaMultiplierCurve = variant3.As<Curve>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, ref variant4))
			{
				this._minBaseAlphaCurve = variant4.As<Curve>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._erosionCurve, ref variant5))
			{
				this._erosionCurve = variant5.As<Curve>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, ref variant6))
			{
				this._noiseAOffsetCurve = variant6.As<Curve>();
			}
			Variant variant7;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, ref variant7))
			{
				this._noiseBOffsetCurve = variant7.As<Curve>();
			}
			Variant variant8;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._originalMaterial, ref variant8))
			{
				this._originalMaterial = variant8.As<Material>();
			}
			Variant variant9;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._materialCopy, ref variant9))
			{
				this._materialCopy = variant9.As<ShaderMaterial>();
			}
			Variant variant10;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetY, ref variant10))
			{
				this._noiseAOffsetY = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetY, ref variant11))
			{
				this._noiseBOffsetY = variant11.As<float>();
			}
		}

		// Token: 0x04000AAD RID: 2733
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_gaseous_screen");

		// Token: 0x04000AAE RID: 2734
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private ColorRect _gfx;

		// Token: 0x04000AAF RID: 2735
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _duration = 1f;

		// Token: 0x04000AB0 RID: 2736
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _alphaMultiplierCurve;

		// Token: 0x04000AB1 RID: 2737
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _minBaseAlphaCurve;

		// Token: 0x04000AB2 RID: 2738
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _erosionCurve;

		// Token: 0x04000AB3 RID: 2739
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _noiseAOffsetCurve;

		// Token: 0x04000AB4 RID: 2740
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _noiseBOffsetCurve;

		// Token: 0x04000AB5 RID: 2741
		[Nullable(2)]
		private Material _originalMaterial;

		// Token: 0x04000AB6 RID: 2742
		[Nullable(2)]
		private ShaderMaterial _materialCopy;

		// Token: 0x04000AB7 RID: 2743
		private float _noiseAOffsetY;

		// Token: 0x04000AB8 RID: 2744
		private float _noiseBOffsetY;

		// Token: 0x04000AB9 RID: 2745
		[Nullable(1)]
		private static readonly StringName _alphaMultiplierString = new StringName("alpha_multiplier");

		// Token: 0x04000ABA RID: 2746
		[Nullable(1)]
		private static readonly StringName _minBaseAlphaString = new StringName("min_base_alpha");

		// Token: 0x04000ABB RID: 2747
		[Nullable(1)]
		private static readonly StringName _noiseAOffsetString = new StringName("noise_a_static_offset");

		// Token: 0x04000ABC RID: 2748
		[Nullable(1)]
		private static readonly StringName _noiseBOffsetString = new StringName("noise_b_static_offset");

		// Token: 0x04000ABD RID: 2749
		[Nullable(1)]
		private static readonly StringName _erosionString = new StringName("erosion_base");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011CF RID: 4559
		public class MethodName : AspectRatioContainer.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400453D RID: 17725
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400453E RID: 17726
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetProperties' method.
			/// </summary>
			// Token: 0x0400453F RID: 17727
			public static readonly StringName SetProperties = "SetProperties";

			/// <summary>
			/// Cached name for the 'Play' method.
			/// </summary>
			// Token: 0x04004540 RID: 17728
			public static readonly StringName Play = "Play";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D0 RID: 4560
		public class PropertyName : AspectRatioContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_gfx' field.
			/// </summary>
			// Token: 0x04004541 RID: 17729
			public static readonly StringName _gfx = "_gfx";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004542 RID: 17730
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_alphaMultiplierCurve' field.
			/// </summary>
			// Token: 0x04004543 RID: 17731
			public static readonly StringName _alphaMultiplierCurve = "_alphaMultiplierCurve";

			/// <summary>
			/// Cached name for the '_minBaseAlphaCurve' field.
			/// </summary>
			// Token: 0x04004544 RID: 17732
			public static readonly StringName _minBaseAlphaCurve = "_minBaseAlphaCurve";

			/// <summary>
			/// Cached name for the '_erosionCurve' field.
			/// </summary>
			// Token: 0x04004545 RID: 17733
			public static readonly StringName _erosionCurve = "_erosionCurve";

			/// <summary>
			/// Cached name for the '_noiseAOffsetCurve' field.
			/// </summary>
			// Token: 0x04004546 RID: 17734
			public static readonly StringName _noiseAOffsetCurve = "_noiseAOffsetCurve";

			/// <summary>
			/// Cached name for the '_noiseBOffsetCurve' field.
			/// </summary>
			// Token: 0x04004547 RID: 17735
			public static readonly StringName _noiseBOffsetCurve = "_noiseBOffsetCurve";

			/// <summary>
			/// Cached name for the '_originalMaterial' field.
			/// </summary>
			// Token: 0x04004548 RID: 17736
			public static readonly StringName _originalMaterial = "_originalMaterial";

			/// <summary>
			/// Cached name for the '_materialCopy' field.
			/// </summary>
			// Token: 0x04004549 RID: 17737
			public static readonly StringName _materialCopy = "_materialCopy";

			/// <summary>
			/// Cached name for the '_noiseAOffsetY' field.
			/// </summary>
			// Token: 0x0400454A RID: 17738
			public static readonly StringName _noiseAOffsetY = "_noiseAOffsetY";

			/// <summary>
			/// Cached name for the '_noiseBOffsetY' field.
			/// </summary>
			// Token: 0x0400454B RID: 17739
			public static readonly StringName _noiseBOffsetY = "_noiseBOffsetY";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D1 RID: 4561
		public class SignalName : AspectRatioContainer.SignalName
		{
		}
	}
}
