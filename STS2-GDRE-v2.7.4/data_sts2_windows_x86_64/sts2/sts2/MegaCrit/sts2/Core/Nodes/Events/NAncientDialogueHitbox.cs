using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	/// <summary>
	/// Hitbox button for Ancient events to continue through any ancient dialogue.
	/// </summary>
	// Token: 0x020003B8 RID: 952
	[ScriptPath("res://src/Core/Nodes/Events/NAncientDialogueHitbox.cs")]
	public class NAncientDialogueHitbox : NButton
	{
		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x001B073C File Offset: 0x001AE93C
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x001B0751 File Offset: 0x001AE951
		[NullableContext(2)]
		public string GetHotkey()
		{
			return this.Hotkeys.FirstOrDefault<string>();
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x001B0760 File Offset: 0x001AE960
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("%Label");
			this._label.SelfModulate = StsColors.transparentWhite;
			this._label.Text = string.Empty;
			this._arrow = base.GetNode<TextureRect>("%Arrow");
			this._arrow.SelfModulate = StsColors.transparentWhite;
			this._loopTween = base.CreateTween().SetParallel(true).SetLoops(0);
			this._loopTween.TweenProperty(this._arrow, "position:x", this._arrow.Position.X + 4f, 0.4).SetEase(2L).SetTrans(1L);
			this._loopTween.Chain().TweenProperty(this._arrow, "position:x", this._arrow.Position.X - 4f, 0.6).SetEase(2L)
				.SetTrans(1L);
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenProperty(this._label, "self_modulate:a", 1f, 1.0).SetDelay(0.5);
			tween.TweenProperty(this._arrow, "self_modulate:a", 1f, 1.0).SetDelay(0.5);
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x001B0908 File Offset: 0x001AEB08
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween loopTween = this._loopTween;
			if (loopTween != null)
			{
				loopTween.Play();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._arrow, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x001B09CC File Offset: 0x001AEBCC
		protected override void OnPress()
		{
			base.OnPress();
			Tween loopTween = this._loopTween;
			if (loopTween != null)
			{
				loopTween.Pause();
			}
			this._label.PivotOffset = this._label.Size * 0.5f;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "scale", Vector2.One * 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._arrow, "scale", Vector2.One * 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._isAnimating = false;
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x001B0AC8 File Offset: 0x001AECC8
		protected override void OnFocus()
		{
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003933 RID: 14643 RVA: 0x001B0ACC File Offset: 0x001AECCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAncientDialogueHitbox.MethodName.GetHotkey, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueHitbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueHitbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueHitbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueHitbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003934 RID: 14644 RVA: 0x001B0BCC File Offset: 0x001AEDCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientDialogueHitbox.MethodName.GetHotkey && args.Count == 0)
			{
				string hotkey = this.GetHotkey();
				ret = VariantUtils.CreateFrom<string>(ref hotkey);
				return true;
			}
			if ((ref method) == NAncientDialogueHitbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueHitbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueHitbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueHitbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003935 RID: 14645 RVA: 0x001B0CA4 File Offset: 0x001AEEA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientDialogueHitbox.MethodName.GetHotkey || (ref method) == NAncientDialogueHitbox.MethodName._Ready || (ref method) == NAncientDialogueHitbox.MethodName.OnRelease || (ref method) == NAncientDialogueHitbox.MethodName.OnPress || (ref method) == NAncientDialogueHitbox.MethodName.OnFocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003936 RID: 14646 RVA: 0x001B0D04 File Offset: 0x001AEF04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientDialogueHitbox.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._arrow)
			{
				this._arrow = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._loopTween)
			{
				this._loopTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._isAnimating)
			{
				this._isAnimating = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003937 RID: 14647 RVA: 0x001B0DA0 File Offset: 0x001AEFA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientDialogueHitbox.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._arrow)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._arrow);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._loopTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._loopTween);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NAncientDialogueHitbox.PropertyName._isAnimating)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAnimating);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003938 RID: 14648 RVA: 0x001B0E78 File Offset: 0x001AF078
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NAncientDialogueHitbox.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueHitbox.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueHitbox.PropertyName._arrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueHitbox.PropertyName._loopTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueHitbox.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAncientDialogueHitbox.PropertyName._isAnimating, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003939 RID: 14649 RVA: 0x001B0F54 File Offset: 0x001AF154
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientDialogueHitbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NAncientDialogueHitbox.PropertyName._arrow, Variant.From<TextureRect>(ref this._arrow));
			info.AddProperty(NAncientDialogueHitbox.PropertyName._loopTween, Variant.From<Tween>(ref this._loopTween));
			info.AddProperty(NAncientDialogueHitbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NAncientDialogueHitbox.PropertyName._isAnimating, Variant.From<bool>(ref this._isAnimating));
		}

		/// <inheritdoc />
		// Token: 0x0600393A RID: 14650 RVA: 0x001B0FD8 File Offset: 0x001AF1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._arrow, ref variant2))
			{
				this._arrow = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._loopTween, ref variant3))
			{
				this._loopTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._isAnimating, ref variant5))
			{
				this._isAnimating = variant5.As<bool>();
			}
		}

		// Token: 0x0400163B RID: 5691
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x0400163C RID: 5692
		[Nullable(1)]
		private TextureRect _arrow;

		// Token: 0x0400163D RID: 5693
		[Nullable(2)]
		private Tween _loopTween;

		// Token: 0x0400163E RID: 5694
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400163F RID: 5695
		private bool _isAnimating;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C1 RID: 6081
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'GetHotkey' method.
			/// </summary>
			// Token: 0x04005C7C RID: 23676
			public static readonly StringName GetHotkey = "GetHotkey";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C7D RID: 23677
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005C7E RID: 23678
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005C7F RID: 23679
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005C80 RID: 23680
			public new static readonly StringName OnFocus = "OnFocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C2 RID: 6082
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005C81 RID: 23681
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005C82 RID: 23682
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_arrow' field.
			/// </summary>
			// Token: 0x04005C83 RID: 23683
			public static readonly StringName _arrow = "_arrow";

			/// <summary>
			/// Cached name for the '_loopTween' field.
			/// </summary>
			// Token: 0x04005C84 RID: 23684
			public static readonly StringName _loopTween = "_loopTween";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005C85 RID: 23685
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_isAnimating' field.
			/// </summary>
			// Token: 0x04005C86 RID: 23686
			public static readonly StringName _isAnimating = "_isAnimating";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C3 RID: 6083
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
