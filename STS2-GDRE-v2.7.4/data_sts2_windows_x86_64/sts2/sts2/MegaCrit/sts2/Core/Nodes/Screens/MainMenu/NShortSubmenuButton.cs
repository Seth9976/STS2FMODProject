using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// The huge submenu buttons you see in the MainMenu.
	/// </summary>
	// Token: 0x02000311 RID: 785
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NShortSubmenuButton.cs")]
	public class NShortSubmenuButton : NButton
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x00132AEC File Offset: 0x00130CEC
		[return: Nullable(2)]
		public static string GetImagePath(string key)
		{
			string imagePath = ImageHelper.GetImagePath("packed/main_menu/submenu_icon_" + key.ToLowerInvariant() + ".png");
			if (ResourceLoader.Exists(imagePath, ""))
			{
				return imagePath;
			}
			return null;
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x00132B24 File Offset: 0x00130D24
		public override void _Ready()
		{
			if (base.GetType() != typeof(NShortSubmenuButton))
			{
				throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x00132B50 File Offset: 0x00130D50
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._bgPanel = base.GetNode<Control>("BgPanel");
			this._hsv = (ShaderMaterial)this._bgPanel.Material;
			this._icon = base.GetNode<TextureRect>("Icon");
			this._title = base.GetNode<MegaLabel>("%Title");
			this._description = base.GetNode<MegaRichTextLabel>("%Description");
			this._defaultV = (float)this._hsv.GetShaderParameter(NShortSubmenuButton._v);
			this._focusV = this._defaultV + 0.2f;
			this._pressV = this._defaultV - 0.2f;
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x00132C14 File Offset: 0x00130E14
		public void SetIconAndLocalization(string locKeyPrefix)
		{
			this._locKeyPrefix = locKeyPrefix;
			this.RefreshLabels();
			string imagePath = NShortSubmenuButton.GetImagePath(locKeyPrefix);
			if (imagePath != null)
			{
				this._icon.Texture = PreloadManager.Cache.GetCompressedTexture2D(imagePath);
			}
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x00132C4E File Offset: 0x00130E4E
		public override void _Notification(int what)
		{
			if ((long)what == 2010L && this._locKeyPrefix != null && base.IsNodeReady())
			{
				this.RefreshLabels();
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x00132C70 File Offset: 0x00130E70
		private void RefreshLabels()
		{
			LocString locString = new LocString("main_menu_ui", this._locKeyPrefix + ".title");
			this._title.SetTextAutoSize(locString.GetFormattedText());
			this._title.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
			LocString locString2;
			if (base.IsEnabled)
			{
				locString2 = new LocString("main_menu_ui", this._locKeyPrefix + ".description");
			}
			else
			{
				locString2 = new LocString("main_menu_ui", this._locKeyPrefix + ".LOCKED.description");
				if (!locString2.Exists())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Short submenu button ");
					defaultInterpolatedStringHandler.AppendFormatted<StringName>(base.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" tried to find locked description for ");
					defaultInterpolatedStringHandler.AppendFormatted(this._locKeyPrefix);
					defaultInterpolatedStringHandler.AppendLiteral(" but couldn't");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					locString2 = new LocString("main_menu_ui", this._locKeyPrefix + ".description");
				}
			}
			this._description.Text = locString2.GetFormattedText();
			this._description.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.RichTextLabel.NormalFont);
			this._description.ApplyLocaleFontSubstitution(FontType.Bold, ThemeConstants.RichTextLabel.BoldFont);
			this._description.ApplyLocaleFontSubstitution(FontType.Italic, ThemeConstants.RichTextLabel.ItalicsFont);
		}

		/// <summary>
		/// Used when this button is LOCKED from the player for whatever reason.
		/// </summary>
		// Token: 0x06002B76 RID: 11126 RVA: 0x00132DBC File Offset: 0x00130FBC
		protected override void OnEnable()
		{
			base.Modulate = Colors.White;
			base.GetNode<Control>("%Lock").Visible = false;
			this._hsv.SetShaderParameter(NShortSubmenuButton._s, 1f);
			this._icon.Modulate = Colors.White;
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x00132E14 File Offset: 0x00131014
		protected override void OnDisable()
		{
			base.Modulate = Colors.DarkGray;
			base.GetNode<Control>("%Lock").Visible = true;
			this._hsv.SetShaderParameter(NShortSubmenuButton._s, 0f);
			this._icon.Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x00132E80 File Offset: 0x00131080
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.02f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NShortSubmenuButton._v), this._focusV, 0.05);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x00132F2C File Offset: 0x0013112C
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NShortSubmenuButton._v), this._defaultV, 0.3);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.3);
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x00133000 File Offset: 0x00131200
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.98f, 0.2).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this, "modulate", StsColors.lightGray, 0.2);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x001330A2 File Offset: 0x001312A2
		protected override void OnRelease()
		{
			this.OnUnfocus();
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x001330AA File Offset: 0x001312AA
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NShortSubmenuButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B7D RID: 11133 RVA: 0x001330C4 File Offset: 0x001312C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NShortSubmenuButton.MethodName.GetImagePath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "key", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShortSubmenuButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.SetIconAndLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locKeyPrefix", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShortSubmenuButton.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShortSubmenuButton.MethodName.RefreshLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShortSubmenuButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B7E RID: 11134 RVA: 0x001333D4 File Offset: 0x001315D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShortSubmenuButton.MethodName.GetImagePath && args.Count == 1)
			{
				string imagePath = NShortSubmenuButton.GetImagePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref imagePath);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.SetIconAndLocalization && args.Count == 1)
			{
				this.SetIconAndLocalization(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.RefreshLabels && args.Count == 0)
			{
				this.RefreshLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShortSubmenuButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x0013360C File Offset: 0x0013180C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShortSubmenuButton.MethodName.GetImagePath && args.Count == 1)
			{
				string imagePath = NShortSubmenuButton.GetImagePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref imagePath);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002B80 RID: 11136 RVA: 0x0013365C File Offset: 0x0013185C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NShortSubmenuButton.MethodName.GetImagePath || (ref method) == NShortSubmenuButton.MethodName._Ready || (ref method) == NShortSubmenuButton.MethodName.ConnectSignals || (ref method) == NShortSubmenuButton.MethodName.SetIconAndLocalization || (ref method) == NShortSubmenuButton.MethodName._Notification || (ref method) == NShortSubmenuButton.MethodName.RefreshLabels || (ref method) == NShortSubmenuButton.MethodName.OnEnable || (ref method) == NShortSubmenuButton.MethodName.OnDisable || (ref method) == NShortSubmenuButton.MethodName.OnFocus || (ref method) == NShortSubmenuButton.MethodName.OnUnfocus || (ref method) == NShortSubmenuButton.MethodName.OnPress || (ref method) == NShortSubmenuButton.MethodName.OnRelease || (ref method) == NShortSubmenuButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B81 RID: 11137 RVA: 0x00133734 File Offset: 0x00131934
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NShortSubmenuButton.PropertyName._bgPanel)
			{
				this._bgPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._locKeyPrefix)
			{
				this._locKeyPrefix = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._defaultV)
			{
				this._defaultV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._focusV)
			{
				this._focusV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._pressV)
			{
				this._pressV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B82 RID: 11138 RVA: 0x00133858 File Offset: 0x00131A58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NShortSubmenuButton.PropertyName._bgPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgPanel);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._locKeyPrefix)
			{
				value = VariantUtils.CreateFrom<string>(ref this._locKeyPrefix);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._defaultV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._defaultV);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._focusV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._focusV);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._pressV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pressV);
				return true;
			}
			if ((ref name) == NShortSubmenuButton.PropertyName._tween)
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
		// Token: 0x06002B83 RID: 11139 RVA: 0x001339B0 File Offset: 0x00131BB0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._bgPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(4L, NShortSubmenuButton.PropertyName._locKeyPrefix, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(3L, NShortSubmenuButton.PropertyName._defaultV, 0L, "", 4096L, false),
				new PropertyInfo(3L, NShortSubmenuButton.PropertyName._focusV, 0L, "", 4096L, false),
				new PropertyInfo(3L, NShortSubmenuButton.PropertyName._pressV, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShortSubmenuButton.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B84 RID: 11140 RVA: 0x00133B0C File Offset: 0x00131D0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NShortSubmenuButton.PropertyName._bgPanel, Variant.From<Control>(ref this._bgPanel));
			info.AddProperty(NShortSubmenuButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NShortSubmenuButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
			info.AddProperty(NShortSubmenuButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NShortSubmenuButton.PropertyName._locKeyPrefix, Variant.From<string>(ref this._locKeyPrefix));
			info.AddProperty(NShortSubmenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NShortSubmenuButton.PropertyName._defaultV, Variant.From<float>(ref this._defaultV));
			info.AddProperty(NShortSubmenuButton.PropertyName._focusV, Variant.From<float>(ref this._focusV));
			info.AddProperty(NShortSubmenuButton.PropertyName._pressV, Variant.From<float>(ref this._pressV));
			info.AddProperty(NShortSubmenuButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002B85 RID: 11141 RVA: 0x00133BFC File Offset: 0x00131DFC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._bgPanel, ref variant))
			{
				this._bgPanel = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._title, ref variant3))
			{
				this._title = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._description, ref variant4))
			{
				this._description = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._locKeyPrefix, ref variant5))
			{
				this._locKeyPrefix = variant5.As<string>();
			}
			Variant variant6;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._hsv, ref variant6))
			{
				this._hsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._defaultV, ref variant7))
			{
				this._defaultV = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._focusV, ref variant8))
			{
				this._focusV = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._pressV, ref variant9))
			{
				this._pressV = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NShortSubmenuButton.PropertyName._tween, ref variant10))
			{
				this._tween = variant10.As<Tween>();
			}
		}

		// Token: 0x04001054 RID: 4180
		private Control _bgPanel;

		// Token: 0x04001055 RID: 4181
		private TextureRect _icon;

		// Token: 0x04001056 RID: 4182
		private MegaLabel _title;

		// Token: 0x04001057 RID: 4183
		private MegaRichTextLabel _description;

		// Token: 0x04001058 RID: 4184
		[Nullable(2)]
		private string _locKeyPrefix;

		// Token: 0x04001059 RID: 4185
		private ShaderMaterial _hsv;

		// Token: 0x0400105A RID: 4186
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400105B RID: 4187
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400105C RID: 4188
		private float _defaultV;

		// Token: 0x0400105D RID: 4189
		private float _focusV;

		// Token: 0x0400105E RID: 4190
		private float _pressV;

		// Token: 0x0400105F RID: 4191
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200152D RID: 5421
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'GetImagePath' method.
			/// </summary>
			// Token: 0x040050EA RID: 20714
			public static readonly StringName GetImagePath = "GetImagePath";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040050EB RID: 20715
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x040050EC RID: 20716
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'SetIconAndLocalization' method.
			/// </summary>
			// Token: 0x040050ED RID: 20717
			public static readonly StringName SetIconAndLocalization = "SetIconAndLocalization";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x040050EE RID: 20718
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'RefreshLabels' method.
			/// </summary>
			// Token: 0x040050EF RID: 20719
			public static readonly StringName RefreshLabels = "RefreshLabels";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040050F0 RID: 20720
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040050F1 RID: 20721
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040050F2 RID: 20722
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040050F3 RID: 20723
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040050F4 RID: 20724
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040050F5 RID: 20725
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x040050F6 RID: 20726
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200152E RID: 5422
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bgPanel' field.
			/// </summary>
			// Token: 0x040050F7 RID: 20727
			public static readonly StringName _bgPanel = "_bgPanel";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040050F8 RID: 20728
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x040050F9 RID: 20729
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x040050FA RID: 20730
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_locKeyPrefix' field.
			/// </summary>
			// Token: 0x040050FB RID: 20731
			public static readonly StringName _locKeyPrefix = "_locKeyPrefix";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040050FC RID: 20732
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_defaultV' field.
			/// </summary>
			// Token: 0x040050FD RID: 20733
			public static readonly StringName _defaultV = "_defaultV";

			/// <summary>
			/// Cached name for the '_focusV' field.
			/// </summary>
			// Token: 0x040050FE RID: 20734
			public static readonly StringName _focusV = "_focusV";

			/// <summary>
			/// Cached name for the '_pressV' field.
			/// </summary>
			// Token: 0x040050FF RID: 20735
			public static readonly StringName _pressV = "_pressV";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005100 RID: 20736
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200152F RID: 5423
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
