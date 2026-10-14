using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
	/// The text buttons seen as a vertical list in the main menu.
	/// Buttons like Singleplayer, Multiplayer, Statistics, Options, Quit, etc
	/// Note that we tween the scale/modulate of the label instead of the entire button because stuff is parented to the
	/// continue button
	/// </summary>
	// Token: 0x0200030B RID: 779
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuTextButton.cs")]
	public class NMainMenuTextButton : NButton
	{
		// Token: 0x06002AE9 RID: 10985 RVA: 0x0012E354 File Offset: 0x0012C554
		public override void _Ready()
		{
			if (base.GetType() != typeof(NMainMenuTextButton))
			{
				throw new InvalidOperationException("Don't call base._Ready()!");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x0012E37E File Offset: 0x0012C57E
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this.label = base.GetChild<MegaLabel>(0, false);
			this.label.AddThemeStyleboxOverride(ThemeConstants.Control.Focus, NMainMenuTextButton._emptyStyleBox);
			this.label.FocusMode = 0L;
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x0012E3B6 File Offset: 0x0012C5B6
		[NullableContext(1)]
		public void SetLocalization(string locKey)
		{
			this._locString = new LocString("main_menu_ui", locKey);
			this.RefreshLabel();
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x0012E3CF File Offset: 0x0012C5CF
		public override void _Notification(int what)
		{
			if ((long)what == 2010L)
			{
				this.RefreshLabel();
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x0012E3E4 File Offset: 0x0012C5E4
		private void RefreshLabel()
		{
			if (this.label == null)
			{
				return;
			}
			if (this._locString == null)
			{
				return;
			}
			this.label.Text = this._locString.GetFormattedText();
			this.label.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
			TaskHelper.RunSafely(this.UpdatePivotOffset());
		}

		/// <summary>
		/// Wait one frame before the text change affects the Control node's size so we can set the pivot accurately.
		/// </summary>
		// Token: 0x06002AEE RID: 10990 RVA: 0x0012E438 File Offset: 0x0012C638
		[NullableContext(1)]
		private async Task UpdatePivotOffset()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			if (this.label != null)
			{
				this.label.PivotOffset = this.label.Size * 0.5f;
			}
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x0012E47B File Offset: 0x0012C67B
		protected override void OnPress()
		{
			base.OnPress();
			this.AnimPressDown();
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x0012E489 File Offset: 0x0012C689
		protected override void OnRelease()
		{
			this.AnimRelease();
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0012E494 File Offset: 0x0012C694
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this.label, "scale", NMainMenuTextButton._hoverScale, 0.05);
			this._tween.TweenProperty(this.label, "self_modulate", this._hoveredColor, 0.05);
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x0012E529 File Offset: 0x0012C729
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this.AnimUnhover();
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x0012E538 File Offset: 0x0012C738
		private void AnimUnhover()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			if (this.label == null)
			{
				return;
			}
			this.label.SelfModulate = this._defaultColor;
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this.label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x0012E5BC File Offset: 0x0012C7BC
		private void AnimPressDown()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			if (this.label == null)
			{
				return;
			}
			this.label.SelfModulate = this._hoveredColor;
			this.label.Scale = NMainMenuTextButton._hoverScale;
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this.label, "scale", NMainMenuTextButton._downScale, 0.2).SetTrans(7L).SetEase(1L);
			this._tween.TweenProperty(this.label, "self_modulate", this._downColor, 0.2).SetTrans(7L).SetEase(1L);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x0012E68C File Offset: 0x0012C88C
		private void AnimRelease()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			if (this.label == null)
			{
				return;
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this.label, "scale", base.IsFocused ? NMainMenuTextButton._hoverScale : Vector2.One, 0.2).SetTrans(7L).SetEase(1L);
			this._tween.TweenProperty(this.label, "self_modulate", this._defaultColor, 0.2).SetTrans(7L).SetEase(1L);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0012E749 File Offset: 0x0012C949
		protected override void OnDisable()
		{
			if (this.label == null)
			{
				return;
			}
			this.label.Modulate = StsColors.quarterTransparentWhite;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0012E764 File Offset: 0x0012C964
		protected override void OnEnable()
		{
			if (this.label == null)
			{
				return;
			}
			this.label.Modulate = Colors.White;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AF8 RID: 11000 RVA: 0x0012E780 File Offset: 0x0012C980
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NMainMenuTextButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.SetLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locKey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenuTextButton.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenuTextButton.MethodName.RefreshLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.AnimUnhover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.AnimPressDown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.AnimRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuTextButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AF9 RID: 11001 RVA: 0x0012EA74 File Offset: 0x0012CC74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenuTextButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.SetLocalization && args.Count == 1)
			{
				this.SetLocalization(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.RefreshLabel && args.Count == 0)
			{
				this.RefreshLabel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.AnimUnhover && args.Count == 0)
			{
				this.AnimUnhover();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.AnimPressDown && args.Count == 0)
			{
				this.AnimPressDown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.AnimRelease && args.Count == 0)
			{
				this.AnimRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuTextButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002AFA RID: 11002 RVA: 0x0012ECAC File Offset: 0x0012CEAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMainMenuTextButton.MethodName._Ready || (ref method) == NMainMenuTextButton.MethodName.ConnectSignals || (ref method) == NMainMenuTextButton.MethodName.SetLocalization || (ref method) == NMainMenuTextButton.MethodName._Notification || (ref method) == NMainMenuTextButton.MethodName.RefreshLabel || (ref method) == NMainMenuTextButton.MethodName.OnPress || (ref method) == NMainMenuTextButton.MethodName.OnRelease || (ref method) == NMainMenuTextButton.MethodName.OnFocus || (ref method) == NMainMenuTextButton.MethodName.OnUnfocus || (ref method) == NMainMenuTextButton.MethodName.AnimUnhover || (ref method) == NMainMenuTextButton.MethodName.AnimPressDown || (ref method) == NMainMenuTextButton.MethodName.AnimRelease || (ref method) == NMainMenuTextButton.MethodName.OnDisable || (ref method) == NMainMenuTextButton.MethodName.OnEnable || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002AFB RID: 11003 RVA: 0x0012ED94 File Offset: 0x0012CF94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMainMenuTextButton.PropertyName.label)
			{
				this.label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._defaultColor)
			{
				this._defaultColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._hoveredColor)
			{
				this._hoveredColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._downColor)
			{
				this._downColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002AFC RID: 11004 RVA: 0x0012EE30 File Offset: 0x0012D030
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMainMenuTextButton.PropertyName.label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this.label);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._defaultColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._defaultColor);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._hoveredColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._hoveredColor);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._downColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._downColor);
				return true;
			}
			if ((ref name) == NMainMenuTextButton.PropertyName._tween)
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
		// Token: 0x06002AFD RID: 11005 RVA: 0x0012EEE8 File Offset: 0x0012D0E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMainMenuTextButton.PropertyName.label, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMainMenuTextButton.PropertyName._defaultColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMainMenuTextButton.PropertyName._hoveredColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMainMenuTextButton.PropertyName._downColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuTextButton.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AFE RID: 11006 RVA: 0x0012EFA4 File Offset: 0x0012D1A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMainMenuTextButton.PropertyName.label, Variant.From<MegaLabel>(ref this.label));
			info.AddProperty(NMainMenuTextButton.PropertyName._defaultColor, Variant.From<Color>(ref this._defaultColor));
			info.AddProperty(NMainMenuTextButton.PropertyName._hoveredColor, Variant.From<Color>(ref this._hoveredColor));
			info.AddProperty(NMainMenuTextButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
			info.AddProperty(NMainMenuTextButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002AFF RID: 11007 RVA: 0x0012F028 File Offset: 0x0012D228
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMainMenuTextButton.PropertyName.label, ref variant))
			{
				this.label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMainMenuTextButton.PropertyName._defaultColor, ref variant2))
			{
				this._defaultColor = variant2.As<Color>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMainMenuTextButton.PropertyName._hoveredColor, ref variant3))
			{
				this._hoveredColor = variant3.As<Color>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMainMenuTextButton.PropertyName._downColor, ref variant4))
			{
				this._downColor = variant4.As<Color>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMainMenuTextButton.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		/// <summary>
		/// This is null at certain points when the scene is being loaded.
		/// </summary>
		// Token: 0x0400101F RID: 4127
		[Nullable(2)]
		public MegaLabel label;

		// Token: 0x04001020 RID: 4128
		private Color _defaultColor = StsColors.cream;

		// Token: 0x04001021 RID: 4129
		private Color _hoveredColor = StsColors.gold;

		// Token: 0x04001022 RID: 4130
		private Color _downColor = StsColors.halfTransparentWhite;

		// Token: 0x04001023 RID: 4131
		private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);

		// Token: 0x04001024 RID: 4132
		private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);

		// Token: 0x04001025 RID: 4133
		[Nullable(1)]
		private static readonly StyleBoxEmpty _emptyStyleBox = new StyleBoxEmpty();

		// Token: 0x04001026 RID: 4134
		private const double _pressDownDur = 0.2;

		// Token: 0x04001027 RID: 4135
		private const double _unhoverAnimDur = 0.5;

		// Token: 0x04001028 RID: 4136
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001029 RID: 4137
		[Nullable(2)]
		private LocString _locString;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001516 RID: 5398
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005076 RID: 20598
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04005077 RID: 20599
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'SetLocalization' method.
			/// </summary>
			// Token: 0x04005078 RID: 20600
			public static readonly StringName SetLocalization = "SetLocalization";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04005079 RID: 20601
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'RefreshLabel' method.
			/// </summary>
			// Token: 0x0400507A RID: 20602
			public static readonly StringName RefreshLabel = "RefreshLabel";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400507B RID: 20603
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400507C RID: 20604
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400507D RID: 20605
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400507E RID: 20606
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'AnimUnhover' method.
			/// </summary>
			// Token: 0x0400507F RID: 20607
			public static readonly StringName AnimUnhover = "AnimUnhover";

			/// <summary>
			/// Cached name for the 'AnimPressDown' method.
			/// </summary>
			// Token: 0x04005080 RID: 20608
			public static readonly StringName AnimPressDown = "AnimPressDown";

			/// <summary>
			/// Cached name for the 'AnimRelease' method.
			/// </summary>
			// Token: 0x04005081 RID: 20609
			public static readonly StringName AnimRelease = "AnimRelease";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005082 RID: 20610
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005083 RID: 20611
			public new static readonly StringName OnEnable = "OnEnable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001517 RID: 5399
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'label' field.
			/// </summary>
			// Token: 0x04005084 RID: 20612
			public static readonly StringName label = "label";

			/// <summary>
			/// Cached name for the '_defaultColor' field.
			/// </summary>
			// Token: 0x04005085 RID: 20613
			public static readonly StringName _defaultColor = "_defaultColor";

			/// <summary>
			/// Cached name for the '_hoveredColor' field.
			/// </summary>
			// Token: 0x04005086 RID: 20614
			public static readonly StringName _hoveredColor = "_hoveredColor";

			/// <summary>
			/// Cached name for the '_downColor' field.
			/// </summary>
			// Token: 0x04005087 RID: 20615
			public static readonly StringName _downColor = "_downColor";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005088 RID: 20616
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001518 RID: 5400
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
