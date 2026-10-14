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
	// Token: 0x02000229 RID: 553
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NLowHpBorderVfx.cs")]
	public class NLowHpBorderVfx : ColorRect
	{
		// Token: 0x06001C0B RID: 7179 RVA: 0x000B64EB File Offset: 0x000B46EB
		[NullableContext(2)]
		public static NLowHpBorderVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NLowHpBorderVfx.scenePath).Instantiate<NLowHpBorderVfx>(0L);
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000B650C File Offset: 0x000B470C
		public override void _Ready()
		{
			this._isPlaying = false;
			this._originalMaterial = base.Material;
			this._materialCopy = (ShaderMaterial)this._originalMaterial.Duplicate(true);
			base.Material = this._materialCopy;
			this.SetProperties(1f);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000B655C File Offset: 0x000B475C
		private void SetProperties(float interpolation)
		{
			float num = this._alphaMultiplierCurve.Sample(interpolation);
			Color color = this._gradient.Sample(interpolation);
			this._materialCopy.SetShaderParameter(NLowHpBorderVfx._alphaMultiplierString, num);
			this._materialCopy.SetShaderParameter(NLowHpBorderVfx._mainColorString, color);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000B65AF File Offset: 0x000B47AF
		private void RandomizeInitialOffset()
		{
			this._materialCopy.SetShaderParameter(NLowHpBorderVfx._noiseInitialOffsetString, new Vector2(GD.Randf(), GD.Randf()));
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000B65D5 File Offset: 0x000B47D5
		public void Play()
		{
			if (this._isPlaying)
			{
				this._currentTimer = 0.0;
				return;
			}
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000B65FC File Offset: 0x000B47FC
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._isPlaying = true;
			this._currentTimer = 0.0;
			this.RandomizeInitialOffset();
			this.SetProperties(0f);
			while (this._currentTimer < (double)this._duration)
			{
				float num = (float)(this._currentTimer / (double)this._duration);
				this.SetProperties(num);
				this._currentTimer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			this.SetProperties(1f);
			this._isPlaying = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C11 RID: 7185 RVA: 0x000B6640 File Offset: 0x000B4840
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NLowHpBorderVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("ColorRect"), false), 33L, null, null),
				new MethodInfo(NLowHpBorderVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLowHpBorderVfx.MethodName.SetProperties, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "interpolation", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLowHpBorderVfx.MethodName.RandomizeInitialOffset, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLowHpBorderVfx.MethodName.Play, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C12 RID: 7186 RVA: 0x000B6774 File Offset: 0x000B4974
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLowHpBorderVfx.MethodName.Create && args.Count == 0)
			{
				NLowHpBorderVfx nlowHpBorderVfx = NLowHpBorderVfx.Create();
				ret = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref nlowHpBorderVfx);
				return true;
			}
			if ((ref method) == NLowHpBorderVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLowHpBorderVfx.MethodName.SetProperties && args.Count == 1)
			{
				this.SetProperties(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLowHpBorderVfx.MethodName.RandomizeInitialOffset && args.Count == 0)
			{
				this.RandomizeInitialOffset();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLowHpBorderVfx.MethodName.Play && args.Count == 0)
			{
				this.Play();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000B6858 File Offset: 0x000B4A58
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLowHpBorderVfx.MethodName.Create && args.Count == 0)
			{
				NLowHpBorderVfx nlowHpBorderVfx = NLowHpBorderVfx.Create();
				ret = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref nlowHpBorderVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C14 RID: 7188 RVA: 0x000B6898 File Offset: 0x000B4A98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLowHpBorderVfx.MethodName.Create || (ref method) == NLowHpBorderVfx.MethodName._Ready || (ref method) == NLowHpBorderVfx.MethodName.SetProperties || (ref method) == NLowHpBorderVfx.MethodName.RandomizeInitialOffset || (ref method) == NLowHpBorderVfx.MethodName.Play || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C15 RID: 7189 RVA: 0x000B68F8 File Offset: 0x000B4AF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLowHpBorderVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._alphaMultiplierCurve)
			{
				this._alphaMultiplierCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._noiseOffsetCurve)
			{
				this._noiseOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._gradient)
			{
				this._gradient = VariantUtils.ConvertTo<Gradient>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._originalMaterial)
			{
				this._originalMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._materialCopy)
			{
				this._materialCopy = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._isPlaying)
			{
				this._isPlaying = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._currentTimer)
			{
				this._currentTimer = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C16 RID: 7190 RVA: 0x000B69E8 File Offset: 0x000B4BE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLowHpBorderVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._alphaMultiplierCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._alphaMultiplierCurve);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._noiseOffsetCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._noiseOffsetCurve);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._gradient)
			{
				value = VariantUtils.CreateFrom<Gradient>(ref this._gradient);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._originalMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._originalMaterial);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._materialCopy)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._materialCopy);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._isPlaying)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPlaying);
				return true;
			}
			if ((ref name) == NLowHpBorderVfx.PropertyName._currentTimer)
			{
				value = VariantUtils.CreateFrom<double>(ref this._currentTimer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C17 RID: 7191 RVA: 0x000B6B00 File Offset: 0x000B4D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NLowHpBorderVfx.PropertyName._duration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NLowHpBorderVfx.PropertyName._noiseOffsetCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NLowHpBorderVfx.PropertyName._gradient, 17L, "Gradient", 4102L, true),
				new PropertyInfo(24L, NLowHpBorderVfx.PropertyName._originalMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLowHpBorderVfx.PropertyName._materialCopy, 0L, "", 4096L, false),
				new PropertyInfo(1L, NLowHpBorderVfx.PropertyName._isPlaying, 0L, "", 4096L, false),
				new PropertyInfo(3L, NLowHpBorderVfx.PropertyName._currentTimer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C18 RID: 7192 RVA: 0x000B6C1C File Offset: 0x000B4E1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLowHpBorderVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, Variant.From<Curve>(ref this._alphaMultiplierCurve));
			info.AddProperty(NLowHpBorderVfx.PropertyName._noiseOffsetCurve, Variant.From<Curve>(ref this._noiseOffsetCurve));
			info.AddProperty(NLowHpBorderVfx.PropertyName._gradient, Variant.From<Gradient>(ref this._gradient));
			info.AddProperty(NLowHpBorderVfx.PropertyName._originalMaterial, Variant.From<Material>(ref this._originalMaterial));
			info.AddProperty(NLowHpBorderVfx.PropertyName._materialCopy, Variant.From<ShaderMaterial>(ref this._materialCopy));
			info.AddProperty(NLowHpBorderVfx.PropertyName._isPlaying, Variant.From<bool>(ref this._isPlaying));
			info.AddProperty(NLowHpBorderVfx.PropertyName._currentTimer, Variant.From<double>(ref this._currentTimer));
		}

		/// <inheritdoc />
		// Token: 0x06001C19 RID: 7193 RVA: 0x000B6CE0 File Offset: 0x000B4EE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._duration, ref variant))
			{
				this._duration = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, ref variant2))
			{
				this._alphaMultiplierCurve = variant2.As<Curve>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._noiseOffsetCurve, ref variant3))
			{
				this._noiseOffsetCurve = variant3.As<Curve>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._gradient, ref variant4))
			{
				this._gradient = variant4.As<Gradient>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._originalMaterial, ref variant5))
			{
				this._originalMaterial = variant5.As<Material>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._materialCopy, ref variant6))
			{
				this._materialCopy = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._isPlaying, ref variant7))
			{
				this._isPlaying = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._currentTimer, ref variant8))
			{
				this._currentTimer = variant8.As<double>();
			}
		}

		// Token: 0x04000ABE RID: 2750
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_low_hp_border");

		// Token: 0x04000ABF RID: 2751
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _duration = 1f;

		// Token: 0x04000AC0 RID: 2752
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _alphaMultiplierCurve;

		// Token: 0x04000AC1 RID: 2753
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _noiseOffsetCurve;

		// Token: 0x04000AC2 RID: 2754
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Gradient _gradient;

		// Token: 0x04000AC3 RID: 2755
		[Nullable(2)]
		private Material _originalMaterial;

		// Token: 0x04000AC4 RID: 2756
		[Nullable(2)]
		private ShaderMaterial _materialCopy;

		// Token: 0x04000AC5 RID: 2757
		private bool _isPlaying;

		// Token: 0x04000AC6 RID: 2758
		private double _currentTimer;

		// Token: 0x04000AC7 RID: 2759
		[Nullable(1)]
		private static readonly StringName _alphaMultiplierString = new StringName("alpha_multiplier");

		// Token: 0x04000AC8 RID: 2760
		[Nullable(1)]
		private static readonly StringName _noiseInitialOffsetString = new StringName("noise_initial_offset");

		// Token: 0x04000AC9 RID: 2761
		[Nullable(1)]
		private static readonly StringName _mainColorString = new StringName("main_color");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D3 RID: 4563
		public class MethodName : ColorRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004551 RID: 17745
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004552 RID: 17746
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetProperties' method.
			/// </summary>
			// Token: 0x04004553 RID: 17747
			public static readonly StringName SetProperties = "SetProperties";

			/// <summary>
			/// Cached name for the 'RandomizeInitialOffset' method.
			/// </summary>
			// Token: 0x04004554 RID: 17748
			public static readonly StringName RandomizeInitialOffset = "RandomizeInitialOffset";

			/// <summary>
			/// Cached name for the 'Play' method.
			/// </summary>
			// Token: 0x04004555 RID: 17749
			public static readonly StringName Play = "Play";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D4 RID: 4564
		public class PropertyName : ColorRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004556 RID: 17750
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_alphaMultiplierCurve' field.
			/// </summary>
			// Token: 0x04004557 RID: 17751
			public static readonly StringName _alphaMultiplierCurve = "_alphaMultiplierCurve";

			/// <summary>
			/// Cached name for the '_noiseOffsetCurve' field.
			/// </summary>
			// Token: 0x04004558 RID: 17752
			public static readonly StringName _noiseOffsetCurve = "_noiseOffsetCurve";

			/// <summary>
			/// Cached name for the '_gradient' field.
			/// </summary>
			// Token: 0x04004559 RID: 17753
			public static readonly StringName _gradient = "_gradient";

			/// <summary>
			/// Cached name for the '_originalMaterial' field.
			/// </summary>
			// Token: 0x0400455A RID: 17754
			public static readonly StringName _originalMaterial = "_originalMaterial";

			/// <summary>
			/// Cached name for the '_materialCopy' field.
			/// </summary>
			// Token: 0x0400455B RID: 17755
			public static readonly StringName _materialCopy = "_materialCopy";

			/// <summary>
			/// Cached name for the '_isPlaying' field.
			/// </summary>
			// Token: 0x0400455C RID: 17756
			public static readonly StringName _isPlaying = "_isPlaying";

			/// <summary>
			/// Cached name for the '_currentTimer' field.
			/// </summary>
			// Token: 0x0400455D RID: 17757
			public static readonly StringName _currentTimer = "_currentTimer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D5 RID: 4565
		public class SignalName : ColorRect.SignalName
		{
		}
	}
}
