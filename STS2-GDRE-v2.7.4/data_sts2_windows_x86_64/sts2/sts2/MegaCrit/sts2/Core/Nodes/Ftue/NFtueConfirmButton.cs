using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003AE RID: 942
	[ScriptPath("res://src/Core/Nodes/Ftue/NFtueConfirmButton.cs")]
	public class NFtueConfirmButton : NButton
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x001AC9AB File Offset: 0x001AABAB
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x001AC9C0 File Offset: 0x001AABC0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._outline = base.GetNode<TextureRect>("Outline");
			this._hsv = (ShaderMaterial)base.Material;
			this._label = base.GetNode<MegaLabel>("%Label");
			this._label.SetTextAutoSize(new LocString("ftues", "CONFIRM_BUTTON").GetRawText());
			this._outlineTween = base.CreateTween().SetLoops(0);
			this._outlineTween.TweenProperty(this._outline, "modulate:a", 1f, 0.6);
			this._outlineTween.TweenProperty(this._outline, "modulate:a", 0.25f, 0.6);
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x001ACAA0 File Offset: 0x001AACA0
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween outlineTween = this._outlineTween;
			if (outlineTween != null)
			{
				outlineTween.Kill();
			}
			this._outline.Modulate = StsColors.gold;
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05).SetEase(1L).SetTrans(5L);
			this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NFtueConfirmButton._v), 1.4f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x001ACB84 File Offset: 0x001AAD84
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NFtueConfirmButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			Tween outlineTween = this._outlineTween;
			if (outlineTween != null)
			{
				outlineTween.Kill();
			}
			this._outlineTween = base.CreateTween().SetLoops(0);
			this._outlineTween.TweenProperty(this._outline, "modulate:a", 0.25f, 0.6);
			this._outlineTween.TweenProperty(this._outline, "modulate:a", 1f, 0.6).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x001ACCC9 File Offset: 0x001AAEC9
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NFtueConfirmButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038B8 RID: 14520 RVA: 0x001ACCE4 File Offset: 0x001AAEE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFtueConfirmButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFtueConfirmButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFtueConfirmButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFtueConfirmButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038B9 RID: 14521 RVA: 0x001ACDDC File Offset: 0x001AAFDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFtueConfirmButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFtueConfirmButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFtueConfirmButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFtueConfirmButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060038BA RID: 14522 RVA: 0x001ACE94 File Offset: 0x001AB094
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFtueConfirmButton.MethodName._Ready || (ref method) == NFtueConfirmButton.MethodName.OnFocus || (ref method) == NFtueConfirmButton.MethodName.OnUnfocus || (ref method) == NFtueConfirmButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038BB RID: 14523 RVA: 0x001ACEE4 File Offset: 0x001AB0E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFtueConfirmButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._outlineTween)
			{
				this._outlineTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060038BC RID: 14524 RVA: 0x001ACF80 File Offset: 0x001AB180
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFtueConfirmButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._outlineTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._outlineTween);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NFtueConfirmButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038BD RID: 14525 RVA: 0x001AD058 File Offset: 0x001AB258
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NFtueConfirmButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFtueConfirmButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFtueConfirmButton.PropertyName._outlineTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFtueConfirmButton.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFtueConfirmButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFtueConfirmButton.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038BE RID: 14526 RVA: 0x001AD134 File Offset: 0x001AB334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFtueConfirmButton.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NFtueConfirmButton.PropertyName._outlineTween, Variant.From<Tween>(ref this._outlineTween));
			info.AddProperty(NFtueConfirmButton.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NFtueConfirmButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NFtueConfirmButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x060038BF RID: 14527 RVA: 0x001AD1B8 File Offset: 0x001AB3B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFtueConfirmButton.PropertyName._outline, ref variant))
			{
				this._outline = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFtueConfirmButton.PropertyName._outlineTween, ref variant2))
			{
				this._outlineTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFtueConfirmButton.PropertyName._scaleTween, ref variant3))
			{
				this._scaleTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NFtueConfirmButton.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
			Variant variant5;
			if (info.TryGetProperty(NFtueConfirmButton.PropertyName._label, ref variant5))
			{
				this._label = variant5.As<MegaLabel>();
			}
		}

		// Token: 0x040015FB RID: 5627
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040015FC RID: 5628
		[Nullable(1)]
		private TextureRect _outline;

		// Token: 0x040015FD RID: 5629
		[Nullable(2)]
		private Tween _outlineTween;

		// Token: 0x040015FE RID: 5630
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x040015FF RID: 5631
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001600 RID: 5632
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A5 RID: 6053
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C27 RID: 23591
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005C28 RID: 23592
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005C29 RID: 23593
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005C2A RID: 23594
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A6 RID: 6054
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005C2B RID: 23595
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005C2C RID: 23596
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_outlineTween' field.
			/// </summary>
			// Token: 0x04005C2D RID: 23597
			public static readonly StringName _outlineTween = "_outlineTween";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x04005C2E RID: 23598
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005C2F RID: 23599
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005C30 RID: 23600
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A7 RID: 6055
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
