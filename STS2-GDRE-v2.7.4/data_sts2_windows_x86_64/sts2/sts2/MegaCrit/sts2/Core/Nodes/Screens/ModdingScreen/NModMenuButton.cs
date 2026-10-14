using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen
{
	/// <summary>
	/// Ping button. Flies in from off-screen after a short delay when the local player is ready but remote players are not.
	/// </summary>
	// Token: 0x020002E7 RID: 743
	[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModMenuButton.cs")]
	public class NModMenuButton : NButton
	{
		// Token: 0x060027E3 RID: 10211 RVA: 0x00110E04 File Offset: 0x0010F004
		public override void _Ready()
		{
			this.ConnectSignals();
			this._visuals = base.GetNode<Control>("Visuals");
			this._image = base.GetNode<TextureRect>("Visuals/Image");
			this._label = base.GetNode<MegaLabel>("Visuals/Label");
			this._viewport = base.GetViewport();
			this._hsv = (ShaderMaterial)this._image.Material;
			LocString locString = new LocString("gameplay_ui", "PING_BUTTON");
			this._label.SetTextAutoSize(locString.GetFormattedText());
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x00110E9C File Offset: 0x0010F09C
		protected override void OnRelease()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NModMenuButton._v), base.IsFocused ? 1.5f : 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._label, "modulate", base.IsEnabled ? StsColors.cream : StsColors.gray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00110FB4 File Offset: 0x0010F1B4
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hsv.SetShaderParameter(NModMenuButton._v, 1.5);
			this._visuals.Position = new Vector2(0f, -2f);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00111010 File Offset: 0x0010F210
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NModMenuButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._label, "modulate", base.IsEnabled ? StsColors.cream : StsColors.gray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00111118 File Offset: 0x0010F318
		protected override void OnPress()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NModMenuButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", new Vector2(0f, 4f), 0.5).SetEase(1L).SetTrans(7L);
			this._hoverTween.TweenProperty(this._label, "modulate", Colors.DarkGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x0011121A File Offset: 0x0010F41A
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NModMenuButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027E9 RID: 10217 RVA: 0x00111234 File Offset: 0x0010F434
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NModMenuButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027EA RID: 10218 RVA: 0x00111388 File Offset: 0x0010F588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModMenuButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060027EB RID: 10219 RVA: 0x0011148C File Offset: 0x0010F68C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NModMenuButton.MethodName._Ready || (ref method) == NModMenuButton.MethodName.OnRelease || (ref method) == NModMenuButton.MethodName.OnFocus || (ref method) == NModMenuButton.MethodName.OnUnfocus || (ref method) == NModMenuButton.MethodName.OnPress || (ref method) == NModMenuButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060027EC RID: 10220 RVA: 0x001114FC File Offset: 0x0010F6FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NModMenuButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060027ED RID: 10221 RVA: 0x001115B4 File Offset: 0x0010F7B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NModMenuButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NModMenuButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027EE RID: 10222 RVA: 0x0011168C File Offset: 0x0010F88C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NModMenuButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuButton.PropertyName._hoverTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027EF RID: 10223 RVA: 0x00111768 File Offset: 0x0010F968
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NModMenuButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NModMenuButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NModMenuButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NModMenuButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NModMenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NModMenuButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x060027F0 RID: 10224 RVA: 0x00111800 File Offset: 0x0010FA00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NModMenuButton.PropertyName._visuals, ref variant))
			{
				this._visuals = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NModMenuButton.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NModMenuButton.PropertyName._label, ref variant3))
			{
				this._label = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NModMenuButton.PropertyName._viewport, ref variant4))
			{
				this._viewport = variant4.As<Viewport>();
			}
			Variant variant5;
			if (info.TryGetProperty(NModMenuButton.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NModMenuButton.PropertyName._hoverTween, ref variant6))
			{
				this._hoverTween = variant6.As<Tween>();
			}
		}

		// Token: 0x04000ECE RID: 3790
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000ECF RID: 3791
		[Nullable(1)]
		private Control _visuals;

		// Token: 0x04000ED0 RID: 3792
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000ED1 RID: 3793
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000ED2 RID: 3794
		[Nullable(1)]
		private Viewport _viewport;

		// Token: 0x04000ED3 RID: 3795
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04000ED4 RID: 3796
		[Nullable(2)]
		private Tween _hoverTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148A RID: 5258
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DEB RID: 19947
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004DEC RID: 19948
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004DED RID: 19949
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004DEE RID: 19950
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004DEF RID: 19951
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004DF0 RID: 19952
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148B RID: 5259
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x04004DF1 RID: 19953
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004DF2 RID: 19954
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004DF3 RID: 19955
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04004DF4 RID: 19956
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004DF5 RID: 19957
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004DF6 RID: 19958
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148C RID: 5260
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
