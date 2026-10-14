using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.RichTextTags;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes
{
	/// <summary>
	/// This VFX dramatically shows the name and epithet of an Ancient when you enter their room.
	/// </summary>
	// Token: 0x02000175 RID: 373
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/NAncientNameBanner.cs")]
	public class NAncientNameBanner : Control
	{
		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00065641 File Offset: 0x00063841
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NAncientNameBanner._path);
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00065650 File Offset: 0x00063850
		[return: Nullable(2)]
		public static NAncientNameBanner Create(AncientEventModel ancient)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NAncientNameBanner nancientNameBanner = PreloadManager.Cache.GetScene(NAncientNameBanner._path).Instantiate<NAncientNameBanner>(0L);
			nancientNameBanner._ancient = ancient;
			return nancientNameBanner;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00065688 File Offset: 0x00063888
		public override void _Ready()
		{
			this._titleLabel = base.GetNode<MegaRichTextLabel>("%Title");
			string text = this._ancient.Title.GetFormattedText().ToUpper();
			this._ancientBannerEffect = new RichTextAncientBanner();
			this._ancientBannerEffect.CenterCharacter = this.GetTextCenterGlyphIndex(text, this._titleLabel.GetThemeFont(ThemeConstants.RichTextLabel.NormalFont, "RichTextLabel"), this._titleLabel.GetThemeFontSize(ThemeConstants.RichTextLabel.NormalFontSize, "RichTextLabel"));
			this._titleLabel.InstallEffect(this._ancientBannerEffect);
			this._titleLabel.BbcodeEnabled = true;
			this._titleLabel.Text = "[ancient_banner]" + text + "[/ancient_banner]";
			this._epithetLabel = base.GetNode<MegaLabel>("%Epithet");
			this._epithetLabel.SetTextAutoSize(this._ancient.Epithet.GetFormattedText());
			TaskHelper.RunSafely(this.AnimateVfx());
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0006578C File Offset: 0x0006398C
		private async Task AnimateVfx()
		{
			this._epithetLabel.Position = new Vector2(0f, 18f);
			this._epithetLabel.Modulate = Colors.Transparent;
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "position:y", -100f, 4.0).SetEase(1L).SetTrans(8L);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateGlyphSpace)), 1f, 0f, 3.0).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateTransform)), 0f, 1f, 3.0).SetEase(1L).SetTrans(11L);
			this._tween.TweenProperty(this._epithetLabel, "position:y", 42f, 2.0).SetEase(1L).SetTrans(7L)
				.SetDelay(1.0);
			this._tween.TweenProperty(this._epithetLabel, "modulate:a", 1f, 1.0).SetDelay(1.5);
			this._tween.Chain();
			this._tween.TweenInterval(1.5);
			this._tween.Chain();
			this._tween.TweenProperty(this._titleLabel, "modulate", Colors.Red, 1.0).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._titleLabel, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._epithetLabel, "modulate", Colors.Red, 1.0).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._epithetLabel, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				if (this.IsValid())
				{
					this._moveTween.Kill();
					this._moveTween = base.CreateTween().SetParallel(true);
					base.Position = new Vector2(0f, -80f);
					this._titleLabel.Modulate = Colors.White;
					this._titleLabel.HorizontalAlignment = 0L;
					this._titleLabel.VerticalAlignment = 2L;
					this._titleLabel.Position = Vector2.Zero;
					this._titleLabel.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize, 54);
					this._titleLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor, Colors.Transparent);
					this._titleLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.FontShadowColor, Colors.Transparent);
					this._titleLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.cream);
					this._epithetLabel.HorizontalAlignment = 0L;
					this._epithetLabel.VerticalAlignment = 2L;
					this._epithetLabel.Modulate = new Color(1f, 1f, 1f, 0f);
					this._epithetLabel.AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, 18);
					this._epithetLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, Colors.Transparent);
					this._epithetLabel.AddThemeColorOverride(ThemeConstants.Label.FontShadowColor, Colors.Transparent);
					this._epithetLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
					this._moveTween.TweenProperty(this._epithetLabel, "modulate:a", 0.5f, 2.0).SetEase(1L).SetTrans(8L);
					this._moveTween.TweenProperty(this, "position:x", 48f, 2.0).SetEase(1L).From(0)
						.SetTrans(8L);
				}
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000657CF File Offset: 0x000639CF
		private void UpdateTransform(float obj)
		{
			this._ancientBannerEffect.Rotation = obj;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000657DD File Offset: 0x000639DD
		private void UpdateGlyphSpace(float spacing)
		{
			this._ancientBannerEffect.Spacing = spacing * 1000f;
		}

		/// <summary>
		/// Returns the index of the glyph in the center of the text.
		/// Returns a fractional value if the midline is in the middle of a glyph (which it usually is).
		/// </summary>
		// Token: 0x0600116B RID: 4459 RVA: 0x000657F4 File Offset: 0x000639F4
		private float GetTextCenterGlyphIndex(string text, Font font, int fontSize)
		{
			float num6;
			using (TextParagraph textParagraph = new TextParagraph())
			{
				textParagraph.AddString(text, font, fontSize, "", default(Variant));
				float num = 0f;
				TextServer primaryInterface = TextServerManager.Singleton.GetPrimaryInterface();
				Array<Dictionary> array = primaryInterface.ShapedTextGetGlyphs(textParagraph.GetLineRid(0));
				foreach (Dictionary dictionary in array)
				{
					float num2 = dictionary.GetValueOrDefault("advance").AsSingle();
					num += num2;
				}
				float num3 = 0f;
				int num4 = 0;
				foreach (Dictionary dictionary2 in array)
				{
					float num5 = dictionary2.GetValueOrDefault("advance").AsSingle();
					num3 += num5;
					if (num3 > num * 0.5f)
					{
						return (float)num4 + (num * 0.5f - (num3 - num5)) / num5;
					}
					num4++;
				}
				num6 = 0f;
			}
			return num6;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0006596C File Offset: 0x00063B6C
		public override void _ExitTree()
		{
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._titleLabel.RemoveThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize);
			this._titleLabel.RemoveThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor);
			this._titleLabel.RemoveThemeColorOverride(ThemeConstants.RichTextLabel.FontShadowColor);
			this._titleLabel.RemoveThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor);
			this._epithetLabel.RemoveThemeFontSizeOverride(ThemeConstants.Label.FontSize);
			this._epithetLabel.RemoveThemeColorOverride(ThemeConstants.Label.FontOutlineColor);
			this._epithetLabel.RemoveThemeColorOverride(ThemeConstants.Label.FontShadowColor);
			this._epithetLabel.RemoveThemeColorOverride(ThemeConstants.Label.FontColor);
			base._ExitTree();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600116D RID: 4461 RVA: 0x00065A24 File Offset: 0x00063C24
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAncientNameBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientNameBanner.MethodName.UpdateTransform, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "obj", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientNameBanner.MethodName.UpdateGlyphSpace, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "spacing", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientNameBanner.MethodName.GetTextCenterGlyphIndex, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false),
					new PropertyInfo(24L, "font", 0L, "", 6L, new StringName("Font"), false),
					new PropertyInfo(2L, "fontSize", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientNameBanner.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600116E RID: 4462 RVA: 0x00065BE0 File Offset: 0x00063DE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientNameBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientNameBanner.MethodName.UpdateTransform && args.Count == 1)
			{
				this.UpdateTransform(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientNameBanner.MethodName.UpdateGlyphSpace && args.Count == 1)
			{
				this.UpdateGlyphSpace(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientNameBanner.MethodName.GetTextCenterGlyphIndex && args.Count == 3)
			{
				float textCenterGlyphIndex = this.GetTextCenterGlyphIndex(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<Font>(args[1]), VariantUtils.ConvertTo<int>(args[2]));
				ret = VariantUtils.CreateFrom<float>(ref textCenterGlyphIndex);
				return true;
			}
			if ((ref method) == NAncientNameBanner.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600116F RID: 4463 RVA: 0x00065CFC File Offset: 0x00063EFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientNameBanner.MethodName._Ready || (ref method) == NAncientNameBanner.MethodName.UpdateTransform || (ref method) == NAncientNameBanner.MethodName.UpdateGlyphSpace || (ref method) == NAncientNameBanner.MethodName.GetTextCenterGlyphIndex || (ref method) == NAncientNameBanner.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001170 RID: 4464 RVA: 0x00065D5C File Offset: 0x00063F5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientNameBanner.PropertyName._titleLabel)
			{
				this._titleLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._ancientBannerEffect)
			{
				this._ancientBannerEffect = VariantUtils.ConvertTo<RichTextAncientBanner>(ref value);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._epithetLabel)
			{
				this._epithetLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._moveTween)
			{
				this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001171 RID: 4465 RVA: 0x00065DF8 File Offset: 0x00063FF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientNameBanner.PropertyName._titleLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._titleLabel);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._ancientBannerEffect)
			{
				value = VariantUtils.CreateFrom<RichTextAncientBanner>(ref this._ancientBannerEffect);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._epithetLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._epithetLabel);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._moveTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
				return true;
			}
			if ((ref name) == NAncientNameBanner.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001172 RID: 4466 RVA: 0x00065EB0 File Offset: 0x000640B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAncientNameBanner.PropertyName._titleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientNameBanner.PropertyName._ancientBannerEffect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientNameBanner.PropertyName._epithetLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientNameBanner.PropertyName._moveTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientNameBanner.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001173 RID: 4467 RVA: 0x00065F6C File Offset: 0x0006416C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientNameBanner.PropertyName._titleLabel, Variant.From<MegaRichTextLabel>(ref this._titleLabel));
			info.AddProperty(NAncientNameBanner.PropertyName._ancientBannerEffect, Variant.From<RichTextAncientBanner>(ref this._ancientBannerEffect));
			info.AddProperty(NAncientNameBanner.PropertyName._epithetLabel, Variant.From<MegaLabel>(ref this._epithetLabel));
			info.AddProperty(NAncientNameBanner.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
			info.AddProperty(NAncientNameBanner.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001174 RID: 4468 RVA: 0x00065FF0 File Offset: 0x000641F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientNameBanner.PropertyName._titleLabel, ref variant))
			{
				this._titleLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientNameBanner.PropertyName._ancientBannerEffect, ref variant2))
			{
				this._ancientBannerEffect = variant2.As<RichTextAncientBanner>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientNameBanner.PropertyName._epithetLabel, ref variant3))
			{
				this._epithetLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAncientNameBanner.PropertyName._moveTween, ref variant4))
			{
				this._moveTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAncientNameBanner.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		// Token: 0x04000633 RID: 1587
		private MegaRichTextLabel _titleLabel;

		// Token: 0x04000634 RID: 1588
		private RichTextAncientBanner _ancientBannerEffect;

		// Token: 0x04000635 RID: 1589
		private MegaLabel _epithetLabel;

		// Token: 0x04000636 RID: 1590
		private static readonly string _path = SceneHelper.GetScenePath("ui/ancient_name_banner");

		// Token: 0x04000637 RID: 1591
		private AncientEventModel _ancient;

		// Token: 0x04000638 RID: 1592
		[Nullable(2)]
		private Tween _moveTween;

		// Token: 0x04000639 RID: 1593
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F54 RID: 3924
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003CF7 RID: 15607
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateTransform' method.
			/// </summary>
			// Token: 0x04003CF8 RID: 15608
			public static readonly StringName UpdateTransform = "UpdateTransform";

			/// <summary>
			/// Cached name for the 'UpdateGlyphSpace' method.
			/// </summary>
			// Token: 0x04003CF9 RID: 15609
			public static readonly StringName UpdateGlyphSpace = "UpdateGlyphSpace";

			/// <summary>
			/// Cached name for the 'GetTextCenterGlyphIndex' method.
			/// </summary>
			// Token: 0x04003CFA RID: 15610
			public static readonly StringName GetTextCenterGlyphIndex = "GetTextCenterGlyphIndex";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003CFB RID: 15611
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F55 RID: 3925
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_titleLabel' field.
			/// </summary>
			// Token: 0x04003CFC RID: 15612
			public static readonly StringName _titleLabel = "_titleLabel";

			/// <summary>
			/// Cached name for the '_ancientBannerEffect' field.
			/// </summary>
			// Token: 0x04003CFD RID: 15613
			public static readonly StringName _ancientBannerEffect = "_ancientBannerEffect";

			/// <summary>
			/// Cached name for the '_epithetLabel' field.
			/// </summary>
			// Token: 0x04003CFE RID: 15614
			public static readonly StringName _epithetLabel = "_epithetLabel";

			/// <summary>
			/// Cached name for the '_moveTween' field.
			/// </summary>
			// Token: 0x04003CFF RID: 15615
			public static readonly StringName _moveTween = "_moveTween";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003D00 RID: 15616
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F56 RID: 3926
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
