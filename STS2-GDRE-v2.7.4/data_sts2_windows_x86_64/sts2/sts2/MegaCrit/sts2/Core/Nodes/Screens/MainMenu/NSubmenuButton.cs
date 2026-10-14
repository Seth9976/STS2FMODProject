using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// The huge submenu buttons you see in the MainMenu.
	/// </summary>
	// Token: 0x02000314 RID: 788
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenuButton.cs")]
	public class NSubmenuButton : NButton
	{
		// Token: 0x06002BB1 RID: 11185 RVA: 0x00134E39 File Offset: 0x00133039
		public override void _Ready()
		{
			if (base.GetType() != typeof(NSubmenuButton))
			{
				throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x00134E64 File Offset: 0x00133064
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._bgPanel = base.GetNode<Control>("BgPanel");
			this._hsv = (ShaderMaterial)this._bgPanel.Material;
			this._icon = base.GetNode<TextureRect>("Icon");
			this._title = base.GetNode<MegaLabel>("%Title");
			this._description = base.GetNode<MegaRichTextLabel>("%Description");
			this._defaultV = (float)this._hsv.GetShaderParameter(NSubmenuButton._v);
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00134F01 File Offset: 0x00133101
		[NullableContext(1)]
		public void SetIconAndLocalization(string locKeyPrefix)
		{
			this._locKeyPrefix = locKeyPrefix;
			this.RefreshLabels();
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x00134F10 File Offset: 0x00133110
		public override void _Notification(int what)
		{
			if ((long)what == 2010L && this._locKeyPrefix != null && base.IsNodeReady())
			{
				this.RefreshLabels();
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x00134F34 File Offset: 0x00133134
		public void RefreshLabels()
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
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Submenu button ");
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

		// Token: 0x06002BB6 RID: 11190 RVA: 0x00135080 File Offset: 0x00133280
		protected override void OnEnable()
		{
			base.Modulate = Colors.White;
			base.GetNode<TextureRect>("%Lock").Visible = false;
			this._hsv.SetShaderParameter(NSubmenuButton._s, 1f);
			this._icon.Modulate = Colors.White;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001350D8 File Offset: 0x001332D8
		protected override void OnDisable()
		{
			base.Modulate = Colors.DarkGray;
			base.GetNode<TextureRect>("%Lock").Visible = true;
			this._hsv.SetShaderParameter(NSubmenuButton._s, 0f);
			this._icon.Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00135144 File Offset: 0x00133344
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			base.Scale = NSubmenuButton._hoverScale;
			this._hsv.SetShaderParameter(NSubmenuButton._v, 1f);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00135184 File Offset: 0x00133384
		protected override void OnUnfocus()
		{
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._scaleTween = base.CreateTween().SetParallel(true);
			this._scaleTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1f, this._defaultV, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x00135235 File Offset: 0x00133435
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NSubmenuButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BBB RID: 11195 RVA: 0x00135250 File Offset: 0x00133450
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NSubmenuButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.SetIconAndLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locKeyPrefix", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSubmenuButton.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSubmenuButton.MethodName.RefreshLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BBC RID: 11196 RVA: 0x001354AC File Offset: 0x001336AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSubmenuButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.SetIconAndLocalization && args.Count == 1)
			{
				this.SetIconAndLocalization(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.RefreshLabels && args.Count == 0)
			{
				this.RefreshLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002BBD RID: 11197 RVA: 0x00135660 File Offset: 0x00133860
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSubmenuButton.MethodName._Ready || (ref method) == NSubmenuButton.MethodName.ConnectSignals || (ref method) == NSubmenuButton.MethodName.SetIconAndLocalization || (ref method) == NSubmenuButton.MethodName._Notification || (ref method) == NSubmenuButton.MethodName.RefreshLabels || (ref method) == NSubmenuButton.MethodName.OnEnable || (ref method) == NSubmenuButton.MethodName.OnDisable || (ref method) == NSubmenuButton.MethodName.OnFocus || (ref method) == NSubmenuButton.MethodName.OnUnfocus || (ref method) == NSubmenuButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002BBE RID: 11198 RVA: 0x0013570C File Offset: 0x0013390C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSubmenuButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._bgPanel)
			{
				this._bgPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._locKeyPrefix)
			{
				this._locKeyPrefix = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._defaultV)
			{
				this._defaultV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002BBF RID: 11199 RVA: 0x001357FC File Offset: 0x001339FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSubmenuButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._bgPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgPanel);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._locKeyPrefix)
			{
				value = VariantUtils.CreateFrom<string>(ref this._locKeyPrefix);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._defaultV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._defaultV);
				return true;
			}
			if ((ref name) == NSubmenuButton.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BC0 RID: 11200 RVA: 0x00135914 File Offset: 0x00133B14
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSubmenuButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenuButton.PropertyName._bgPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenuButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenuButton.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenuButton.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(4L, NSubmenuButton.PropertyName._locKeyPrefix, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSubmenuButton.PropertyName._defaultV, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenuButton.PropertyName._scaleTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BC1 RID: 11201 RVA: 0x00135A30 File Offset: 0x00133C30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSubmenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NSubmenuButton.PropertyName._bgPanel, Variant.From<Control>(ref this._bgPanel));
			info.AddProperty(NSubmenuButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NSubmenuButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
			info.AddProperty(NSubmenuButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NSubmenuButton.PropertyName._locKeyPrefix, Variant.From<string>(ref this._locKeyPrefix));
			info.AddProperty(NSubmenuButton.PropertyName._defaultV, Variant.From<float>(ref this._defaultV));
			info.AddProperty(NSubmenuButton.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
		}

		/// <inheritdoc />
		// Token: 0x06002BC2 RID: 11202 RVA: 0x00135AF4 File Offset: 0x00133CF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._hsv, ref variant))
			{
				this._hsv = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._bgPanel, ref variant2))
			{
				this._bgPanel = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._title, ref variant4))
			{
				this._title = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._description, ref variant5))
			{
				this._description = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._locKeyPrefix, ref variant6))
			{
				this._locKeyPrefix = variant6.As<string>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._defaultV, ref variant7))
			{
				this._defaultV = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSubmenuButton.PropertyName._scaleTween, ref variant8))
			{
				this._scaleTween = variant8.As<Tween>();
			}
		}

		// Token: 0x0400106A RID: 4202
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400106B RID: 4203
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400106C RID: 4204
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x0400106D RID: 4205
		[Nullable(1)]
		private Control _bgPanel;

		// Token: 0x0400106E RID: 4206
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x0400106F RID: 4207
		[Nullable(1)]
		private MegaLabel _title;

		// Token: 0x04001070 RID: 4208
		[Nullable(1)]
		private MegaRichTextLabel _description;

		// Token: 0x04001071 RID: 4209
		[Nullable(2)]
		private string _locKeyPrefix;

		// Token: 0x04001072 RID: 4210
		private float _defaultV;

		// Token: 0x04001073 RID: 4211
		private const float _hoverV = 1f;

		// Token: 0x04001074 RID: 4212
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x04001075 RID: 4213
		private static readonly Vector2 _hoverScale = Vector2.One * 1.025f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001536 RID: 5430
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400511A RID: 20762
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x0400511B RID: 20763
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'SetIconAndLocalization' method.
			/// </summary>
			// Token: 0x0400511C RID: 20764
			public static readonly StringName SetIconAndLocalization = "SetIconAndLocalization";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x0400511D RID: 20765
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'RefreshLabels' method.
			/// </summary>
			// Token: 0x0400511E RID: 20766
			public static readonly StringName RefreshLabels = "RefreshLabels";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x0400511F RID: 20767
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005120 RID: 20768
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005121 RID: 20769
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005122 RID: 20770
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005123 RID: 20771
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001537 RID: 5431
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005124 RID: 20772
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_bgPanel' field.
			/// </summary>
			// Token: 0x04005125 RID: 20773
			public static readonly StringName _bgPanel = "_bgPanel";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04005126 RID: 20774
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x04005127 RID: 20775
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005128 RID: 20776
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_locKeyPrefix' field.
			/// </summary>
			// Token: 0x04005129 RID: 20777
			public static readonly StringName _locKeyPrefix = "_locKeyPrefix";

			/// <summary>
			/// Cached name for the '_defaultV' field.
			/// </summary>
			// Token: 0x0400512A RID: 20778
			public static readonly StringName _defaultV = "_defaultV";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x0400512B RID: 20779
			public static readonly StringName _scaleTween = "_scaleTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001538 RID: 5432
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
