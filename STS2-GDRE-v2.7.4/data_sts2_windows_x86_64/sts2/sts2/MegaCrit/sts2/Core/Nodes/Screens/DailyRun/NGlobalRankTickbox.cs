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
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x02000333 RID: 819
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NGlobalRankTickbox.cs")]
	public class NGlobalRankTickbox : NTickbox
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x00149F6E File Offset: 0x0014816E
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.peek };
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x00149F83 File Offset: 0x00148183
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("Label");
			base.IsTicked = false;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x00149FA8 File Offset: 0x001481A8
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.25).SetEase(1L).SetTrans(9L);
			this._labelTween.TweenProperty(this._label, "self_modulate", Colors.White, 0.05);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0014A058 File Offset: 0x00148258
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0014A0C8 File Offset: 0x001482C8
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0014A13C File Offset: 0x0014833C
		protected override void OnPress()
		{
			base.OnPress();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._labelTween.TweenProperty(this._label, "self_modulate", StsColors.lightGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0014A1EC File Offset: 0x001483EC
		[NullableContext(1)]
		public void SetLabel(string text)
		{
			this._label.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DEC RID: 11756 RVA: 0x0014A1FC File Offset: 0x001483FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NGlobalRankTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalRankTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalRankTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalRankTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalRankTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalRankTickbox.MethodName.SetLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DED RID: 11757 RVA: 0x0014A350 File Offset: 0x00148550
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGlobalRankTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalRankTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalRankTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalRankTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalRankTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalRankTickbox.MethodName.SetLabel && args.Count == 1)
			{
				this.SetLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002DEE RID: 11758 RVA: 0x0014A454 File Offset: 0x00148654
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGlobalRankTickbox.MethodName._Ready || (ref method) == NGlobalRankTickbox.MethodName.OnRelease || (ref method) == NGlobalRankTickbox.MethodName.OnFocus || (ref method) == NGlobalRankTickbox.MethodName.OnUnfocus || (ref method) == NGlobalRankTickbox.MethodName.OnPress || (ref method) == NGlobalRankTickbox.MethodName.SetLabel || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002DEF RID: 11759 RVA: 0x0014A4C2 File Offset: 0x001486C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGlobalRankTickbox.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NGlobalRankTickbox.PropertyName._labelTween)
			{
				this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002DF0 RID: 11760 RVA: 0x0014A504 File Offset: 0x00148704
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGlobalRankTickbox.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NGlobalRankTickbox.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NGlobalRankTickbox.PropertyName._labelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DF1 RID: 11761 RVA: 0x0014A57C File Offset: 0x0014877C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGlobalRankTickbox.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalRankTickbox.PropertyName._labelTween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NGlobalRankTickbox.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DF2 RID: 11762 RVA: 0x0014A5F3 File Offset: 0x001487F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGlobalRankTickbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NGlobalRankTickbox.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
		}

		/// <inheritdoc />
		// Token: 0x06002DF3 RID: 11763 RVA: 0x0014A628 File Offset: 0x00148828
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGlobalRankTickbox.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGlobalRankTickbox.PropertyName._labelTween, ref variant2))
			{
				this._labelTween = variant2.As<Tween>();
			}
		}

		// Token: 0x04001193 RID: 4499
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04001194 RID: 4500
		[Nullable(2)]
		private Tween _labelTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B1 RID: 5553
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005351 RID: 21329
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005352 RID: 21330
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005353 RID: 21331
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005354 RID: 21332
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005355 RID: 21333
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'SetLabel' method.
			/// </summary>
			// Token: 0x04005356 RID: 21334
			public static readonly StringName SetLabel = "SetLabel";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B2 RID: 5554
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005357 RID: 21335
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005358 RID: 21336
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_labelTween' field.
			/// </summary>
			// Token: 0x04005359 RID: 21337
			public static readonly StringName _labelTween = "_labelTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B3 RID: 5555
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
