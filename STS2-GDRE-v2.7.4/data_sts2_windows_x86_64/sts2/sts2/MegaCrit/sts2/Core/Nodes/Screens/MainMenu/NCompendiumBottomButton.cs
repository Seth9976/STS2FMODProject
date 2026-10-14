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
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// The submenu buttons in the bottom section in the Compendium screen.
	/// </summary>
	// Token: 0x020002FC RID: 764
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NCompendiumBottomButton.cs")]
	public class NCompendiumBottomButton : NButton
	{
		// Token: 0x060029BB RID: 10683 RVA: 0x00122CDA File Offset: 0x00120EDA
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCompendiumBottomButton))
			{
				throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x00122D04 File Offset: 0x00120F04
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._bgPanel = base.GetNode<Control>("BgPanel");
			this._hsv = (ShaderMaterial)this._bgPanel.Material;
			this._icon = base.GetNode<TextureRect>("Icon");
			this._label = base.GetNode<MegaLabel>("Label");
			this._defaultV = (float)this._hsv.GetShaderParameter(NCompendiumBottomButton._v);
			this._focusV = this._defaultV + 0.2f;
			this._pressV = this._defaultV - 0.2f;
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00122DAF File Offset: 0x00120FAF
		[NullableContext(1)]
		public void SetLocalization(string locKeyPrefix)
		{
			this._locKeyPrefix = locKeyPrefix;
			this.RefreshLabels();
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x00122DBE File Offset: 0x00120FBE
		public override void _Notification(int what)
		{
			if ((long)what == 2010L && this._locKeyPrefix != null && base.IsNodeReady())
			{
				this.RefreshLabels();
			}
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x00122DE0 File Offset: 0x00120FE0
		private void RefreshLabels()
		{
			LocString locString = new LocString("main_menu_ui", this._locKeyPrefix + ".title");
			this._label.SetTextAutoSize(locString.GetFormattedText());
			this._label.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x00122E2A File Offset: 0x0012102A
		protected override void OnEnable()
		{
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x00122E2C File Offset: 0x0012102C
		protected override void OnDisable()
		{
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x00122E30 File Offset: 0x00121030
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NCompendiumBottomButton._v), this._focusV, 0.05);
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x00122EDC File Offset: 0x001210DC
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NCompendiumBottomButton._v), this._defaultV, 0.3);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.3);
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00122FB0 File Offset: 0x001211B0
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.2).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this, "modulate", StsColors.lightGray, 0.2);
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x00123052 File Offset: 0x00121252
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NCompendiumBottomButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029C6 RID: 10694 RVA: 0x0012306C File Offset: 0x0012126C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NCompendiumBottomButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.SetLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locKeyPrefix", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCompendiumBottomButton.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.RefreshLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumBottomButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029C7 RID: 10695 RVA: 0x001232F8 File Offset: 0x001214F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCompendiumBottomButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.SetLocalization && args.Count == 1)
			{
				this.SetLocalization(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.RefreshLabels && args.Count == 0)
			{
				this.RefreshLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumBottomButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060029C8 RID: 10696 RVA: 0x001234D0 File Offset: 0x001216D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCompendiumBottomButton.MethodName._Ready || (ref method) == NCompendiumBottomButton.MethodName.ConnectSignals || (ref method) == NCompendiumBottomButton.MethodName.SetLocalization || (ref method) == NCompendiumBottomButton.MethodName._Notification || (ref method) == NCompendiumBottomButton.MethodName.RefreshLabels || (ref method) == NCompendiumBottomButton.MethodName.OnEnable || (ref method) == NCompendiumBottomButton.MethodName.OnDisable || (ref method) == NCompendiumBottomButton.MethodName.OnFocus || (ref method) == NCompendiumBottomButton.MethodName.OnUnfocus || (ref method) == NCompendiumBottomButton.MethodName.OnPress || (ref method) == NCompendiumBottomButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060029C9 RID: 10697 RVA: 0x0012358C File Offset: 0x0012178C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCompendiumBottomButton.PropertyName._bgPanel)
			{
				this._bgPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._locKeyPrefix)
			{
				this._locKeyPrefix = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._defaultV)
			{
				this._defaultV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._focusV)
			{
				this._focusV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._pressV)
			{
				this._pressV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060029CA RID: 10698 RVA: 0x00123694 File Offset: 0x00121894
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCompendiumBottomButton.PropertyName._bgPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgPanel);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._locKeyPrefix)
			{
				value = VariantUtils.CreateFrom<string>(ref this._locKeyPrefix);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._defaultV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._defaultV);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._focusV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._focusV);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._pressV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pressV);
				return true;
			}
			if ((ref name) == NCompendiumBottomButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029CB RID: 10699 RVA: 0x001237CC File Offset: 0x001219CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCompendiumBottomButton.PropertyName._bgPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumBottomButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumBottomButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(4L, NCompendiumBottomButton.PropertyName._locKeyPrefix, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumBottomButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCompendiumBottomButton.PropertyName._defaultV, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCompendiumBottomButton.PropertyName._focusV, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCompendiumBottomButton.PropertyName._pressV, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumBottomButton.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029CC RID: 10700 RVA: 0x00123908 File Offset: 0x00121B08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCompendiumBottomButton.PropertyName._bgPanel, Variant.From<Control>(ref this._bgPanel));
			info.AddProperty(NCompendiumBottomButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NCompendiumBottomButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NCompendiumBottomButton.PropertyName._locKeyPrefix, Variant.From<string>(ref this._locKeyPrefix));
			info.AddProperty(NCompendiumBottomButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCompendiumBottomButton.PropertyName._defaultV, Variant.From<float>(ref this._defaultV));
			info.AddProperty(NCompendiumBottomButton.PropertyName._focusV, Variant.From<float>(ref this._focusV));
			info.AddProperty(NCompendiumBottomButton.PropertyName._pressV, Variant.From<float>(ref this._pressV));
			info.AddProperty(NCompendiumBottomButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060029CD RID: 10701 RVA: 0x001239E4 File Offset: 0x00121BE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._bgPanel, ref variant))
			{
				this._bgPanel = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._locKeyPrefix, ref variant4))
			{
				this._locKeyPrefix = variant4.As<string>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._defaultV, ref variant6))
			{
				this._defaultV = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._focusV, ref variant7))
			{
				this._focusV = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._pressV, ref variant8))
			{
				this._pressV = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCompendiumBottomButton.PropertyName._tween, ref variant9))
			{
				this._tween = variant9.As<Tween>();
			}
		}

		// Token: 0x04000F99 RID: 3993
		[Nullable(1)]
		private Control _bgPanel;

		// Token: 0x04000F9A RID: 3994
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000F9B RID: 3995
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000F9C RID: 3996
		[Nullable(2)]
		private string _locKeyPrefix;

		// Token: 0x04000F9D RID: 3997
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04000F9E RID: 3998
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000F9F RID: 3999
		private float _defaultV;

		// Token: 0x04000FA0 RID: 4000
		private float _focusV;

		// Token: 0x04000FA1 RID: 4001
		private float _pressV;

		// Token: 0x04000FA2 RID: 4002
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D4 RID: 5332
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F5F RID: 20319
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004F60 RID: 20320
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'SetLocalization' method.
			/// </summary>
			// Token: 0x04004F61 RID: 20321
			public static readonly StringName SetLocalization = "SetLocalization";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04004F62 RID: 20322
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'RefreshLabels' method.
			/// </summary>
			// Token: 0x04004F63 RID: 20323
			public static readonly StringName RefreshLabels = "RefreshLabels";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004F64 RID: 20324
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004F65 RID: 20325
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004F66 RID: 20326
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004F67 RID: 20327
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004F68 RID: 20328
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04004F69 RID: 20329
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D5 RID: 5333
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bgPanel' field.
			/// </summary>
			// Token: 0x04004F6A RID: 20330
			public static readonly StringName _bgPanel = "_bgPanel";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004F6B RID: 20331
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004F6C RID: 20332
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_locKeyPrefix' field.
			/// </summary>
			// Token: 0x04004F6D RID: 20333
			public static readonly StringName _locKeyPrefix = "_locKeyPrefix";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004F6E RID: 20334
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_defaultV' field.
			/// </summary>
			// Token: 0x04004F6F RID: 20335
			public static readonly StringName _defaultV = "_defaultV";

			/// <summary>
			/// Cached name for the '_focusV' field.
			/// </summary>
			// Token: 0x04004F70 RID: 20336
			public static readonly StringName _focusV = "_focusV";

			/// <summary>
			/// Cached name for the '_pressV' field.
			/// </summary>
			// Token: 0x04004F71 RID: 20337
			public static readonly StringName _pressV = "_pressV";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004F72 RID: 20338
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D6 RID: 5334
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
