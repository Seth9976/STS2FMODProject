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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200029C RID: 668
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageButton.cs")]
	public class NLanguageButton : NButton
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000F1470 File Offset: 0x000EF670
		// (set) Token: 0x06002375 RID: 9077 RVA: 0x000F1478 File Offset: 0x000EF678
		public bool IsSelected { get; private set; }

		// Token: 0x06002376 RID: 9078 RVA: 0x000F1484 File Offset: 0x000EF684
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("%ButtonImage");
			this._outline = base.GetNode<Control>("%Outline");
			this._flag = base.GetNode<TextureRect>("%FlagImage");
			this._label = base.GetNode<MegaLabel>("%Label");
		}

		/// <summary>
		/// Given a 3 character ISO 639-2 "like" code, initialize the language button's asset and label.
		/// </summary>
		/// <param name="languageIsoCode"></param>
		// Token: 0x06002377 RID: 9079 RVA: 0x000F14F0 File Offset: 0x000EF6F0
		[NullableContext(1)]
		public void Init(string languageIsoCode)
		{
			Log.Info("Adding Language: " + languageIsoCode, 2);
			this.isoCode = languageIsoCode;
			this._label.SetTextAutoSize(new LocString("settings_ui", "LANGUAGE_" + languageIsoCode.ToUpperInvariant()).GetRawText());
			this._flag.Texture = ResourceLoader.Load<Texture2D>("res://images/ui/settings_screen/flag_" + languageIsoCode + ".png", null, 1L);
			this.IsSelected = SaveManager.Instance.SettingsSave.Language == this.isoCode;
			this._outline.Visible = this.IsSelected;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000F1594 File Offset: 0x000EF794
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "self_modulate", new Color("26373d"), 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "modulate", Colors.White, 0.05);
			this._tween.TweenProperty(this._image, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "modulate", StsColors.cream, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000F16B8 File Offset: 0x000EF8B8
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "self_modulate", new Color("235161"), 0.03);
			this._tween.TweenProperty(this._image, "modulate", Colors.White, 0.05);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 1.05f, 0.03);
			this._tween.TweenProperty(this._label, "modulate", StsColors.gold, 0.03);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000F17BC File Offset: 0x000EF9BC
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "modulate", Colors.White, 0.5);
			this._tween.TweenProperty(this._image, "self_modulate", new Color("26373d"), 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "modulate", StsColors.cream, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000F18E0 File Offset: 0x000EFAE0
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "self_modulate", new Color("26373d"), 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "modulate", StsColors.lightGray, 0.05);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 0.95f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000F19CE File Offset: 0x000EFBCE
		public void SetAsSelected()
		{
			this.IsSelected = true;
			this._outline.Visible = true;
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000F19E3 File Offset: 0x000EFBE3
		public void SetAsDeselected()
		{
			this.IsSelected = false;
			this._outline.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600237E RID: 9086 RVA: 0x000F19F8 File Offset: 0x000EFBF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NLanguageButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "languageIsoCode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLanguageButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.SetAsSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageButton.MethodName.SetAsDeselected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600237F RID: 9087 RVA: 0x000F1BAC File Offset: 0x000EFDAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLanguageButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.SetAsSelected && args.Count == 0)
			{
				this.SetAsSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageButton.MethodName.SetAsDeselected && args.Count == 0)
			{
				this.SetAsDeselected();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002380 RID: 9088 RVA: 0x000F1CF8 File Offset: 0x000EFEF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLanguageButton.MethodName._Ready || (ref method) == NLanguageButton.MethodName.Init || (ref method) == NLanguageButton.MethodName.OnRelease || (ref method) == NLanguageButton.MethodName.OnFocus || (ref method) == NLanguageButton.MethodName.OnUnfocus || (ref method) == NLanguageButton.MethodName.OnPress || (ref method) == NLanguageButton.MethodName.SetAsSelected || (ref method) == NLanguageButton.MethodName.SetAsDeselected || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002381 RID: 9089 RVA: 0x000F1D84 File Offset: 0x000EFF84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLanguageButton.PropertyName.IsSelected)
			{
				this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._flag)
			{
				this._flag = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName.isoCode)
			{
				this.isoCode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002382 RID: 9090 RVA: 0x000F1E58 File Offset: 0x000F0058
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLanguageButton.PropertyName.IsSelected)
			{
				bool isSelected = this.IsSelected;
				value = VariantUtils.CreateFrom<bool>(ref isSelected);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._flag)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._flag);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NLanguageButton.PropertyName.isoCode)
			{
				value = VariantUtils.CreateFrom<string>(ref this.isoCode);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002383 RID: 9091 RVA: 0x000F1F50 File Offset: 0x000F0150
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLanguageButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLanguageButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLanguageButton.PropertyName._flag, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLanguageButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLanguageButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(4L, NLanguageButton.PropertyName.isoCode, 0L, "", 4096L, false),
				new PropertyInfo(1L, NLanguageButton.PropertyName.IsSelected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002384 RID: 9092 RVA: 0x000F204C File Offset: 0x000F024C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isSelected = NLanguageButton.PropertyName.IsSelected;
			bool isSelected2 = this.IsSelected;
			info.AddProperty(isSelected, Variant.From<bool>(ref isSelected2));
			info.AddProperty(NLanguageButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NLanguageButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NLanguageButton.PropertyName._flag, Variant.From<TextureRect>(ref this._flag));
			info.AddProperty(NLanguageButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NLanguageButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NLanguageButton.PropertyName.isoCode, Variant.From<string>(ref this.isoCode));
		}

		/// <inheritdoc />
		// Token: 0x06002385 RID: 9093 RVA: 0x000F2100 File Offset: 0x000F0300
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLanguageButton.PropertyName.IsSelected, ref variant))
			{
				this.IsSelected = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLanguageButton.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLanguageButton.PropertyName._outline, ref variant3))
			{
				this._outline = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLanguageButton.PropertyName._flag, ref variant4))
			{
				this._flag = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLanguageButton.PropertyName._label, ref variant5))
			{
				this._label = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLanguageButton.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NLanguageButton.PropertyName.isoCode, ref variant7))
			{
				this.isoCode = variant7.As<string>();
			}
		}

		// Token: 0x04000D89 RID: 3465
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000D8A RID: 3466
		[Nullable(1)]
		private Control _outline;

		// Token: 0x04000D8B RID: 3467
		[Nullable(1)]
		private TextureRect _flag;

		// Token: 0x04000D8C RID: 3468
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000D8D RID: 3469
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000D8E RID: 3470
		[Nullable(1)]
		public string isoCode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138A RID: 5002
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B4C RID: 19276
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x04004B4D RID: 19277
			public static readonly StringName Init = "Init";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004B4E RID: 19278
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004B4F RID: 19279
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004B50 RID: 19280
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004B51 RID: 19281
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'SetAsSelected' method.
			/// </summary>
			// Token: 0x04004B52 RID: 19282
			public static readonly StringName SetAsSelected = "SetAsSelected";

			/// <summary>
			/// Cached name for the 'SetAsDeselected' method.
			/// </summary>
			// Token: 0x04004B53 RID: 19283
			public static readonly StringName SetAsDeselected = "SetAsDeselected";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138B RID: 5003
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsSelected' property.
			/// </summary>
			// Token: 0x04004B54 RID: 19284
			public static readonly StringName IsSelected = "IsSelected";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004B55 RID: 19285
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04004B56 RID: 19286
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_flag' field.
			/// </summary>
			// Token: 0x04004B57 RID: 19287
			public static readonly StringName _flag = "_flag";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004B58 RID: 19288
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004B59 RID: 19289
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the 'isoCode' field.
			/// </summary>
			// Token: 0x04004B5A RID: 19290
			public static readonly StringName isoCode = "isoCode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138C RID: 5004
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
