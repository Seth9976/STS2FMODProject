using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Text;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.RichTextTags;

namespace MegaCrit.Sts2.addons.mega_text
{
	// Token: 0x02000DA0 RID: 3488
	[Tool]
	[ScriptPath("res://addons/mega_text/MegaRichTextLabel.cs")]
	public class MegaRichTextLabel : RichTextLabel
	{
		/// <summary>
		/// Releases the cached TextParagraph to free text server RIDs at exit.
		/// Uses Dispose() (not Free()) because TextParagraph is RefCounted.
		/// Nulled to guard against AdjustFontSize running during Godot's quit frames.
		/// </summary>
		// Token: 0x06008C47 RID: 35911 RVA: 0x002ED91F File Offset: 0x002EBB1F
		public static void DisposeCachedParagraph()
		{
			TextParagraph cachedParagraph = MegaRichTextLabel._cachedParagraph;
			if (cachedParagraph != null)
			{
				cachedParagraph.Dispose();
			}
			MegaRichTextLabel._cachedParagraph = null;
		}

		// Token: 0x1700267C RID: 9852
		// (get) Token: 0x06008C48 RID: 35912 RVA: 0x002ED937 File Offset: 0x002EBB37
		// (set) Token: 0x06008C49 RID: 35913 RVA: 0x002ED93F File Offset: 0x002EBB3F
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool AutoSizeEnabled
		{
			get
			{
				return this._isAutoSizeEnabled;
			}
			set
			{
				if (value && base.FitContent)
				{
					GD.PushWarning("Auto Size is not compatible with Fit Content, disabling Auto Size...");
					this._isAutoSizeEnabled = false;
					return;
				}
				if (this.AutoSizeEnabled == value)
				{
					return;
				}
				this._isAutoSizeEnabled = value;
				if (Engine.IsEditorHint())
				{
					this.AdjustFontSize();
				}
			}
		}

		// Token: 0x1700267D RID: 9853
		// (get) Token: 0x06008C4A RID: 35914 RVA: 0x002ED97C File Offset: 0x002EBB7C
		// (set) Token: 0x06008C4B RID: 35915 RVA: 0x002ED984 File Offset: 0x002EBB84
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public int MinFontSize
		{
			get
			{
				return this._minFontSize;
			}
			set
			{
				if (this._minFontSize == value)
				{
					return;
				}
				this._minFontSize = value;
				if (Engine.IsEditorHint())
				{
					this.AdjustFontSize();
				}
			}
		}

		// Token: 0x1700267E RID: 9854
		// (get) Token: 0x06008C4C RID: 35916 RVA: 0x002ED9A4 File Offset: 0x002EBBA4
		// (set) Token: 0x06008C4D RID: 35917 RVA: 0x002ED9AC File Offset: 0x002EBBAC
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public int MaxFontSize
		{
			get
			{
				return this._maxFontSize;
			}
			set
			{
				if (this._maxFontSize == value)
				{
					return;
				}
				this._maxFontSize = value;
				if (Engine.IsEditorHint())
				{
					this.AdjustFontSize();
				}
			}
		}

		// Token: 0x1700267F RID: 9855
		// (get) Token: 0x06008C4E RID: 35918 RVA: 0x002ED9CC File Offset: 0x002EBBCC
		// (set) Token: 0x06008C4F RID: 35919 RVA: 0x002ED9D4 File Offset: 0x002EBBD4
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool IsVerticallyBound
		{
			get
			{
				return this._isVerticallyBound;
			}
			set
			{
				this._isVerticallyBound = value;
				if (Engine.IsEditorHint())
				{
					this.AdjustFontSize();
				}
			}
		}

		// Token: 0x17002680 RID: 9856
		// (get) Token: 0x06008C50 RID: 35920 RVA: 0x002ED9EA File Offset: 0x002EBBEA
		// (set) Token: 0x06008C51 RID: 35921 RVA: 0x002ED9F2 File Offset: 0x002EBBF2
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool IsHorizontallyBound
		{
			get
			{
				return this._isHorizontallyBound;
			}
			set
			{
				this._isHorizontallyBound = value;
				if (Engine.IsEditorHint())
				{
					this.AdjustFontSize();
				}
			}
		}

		// Token: 0x06008C52 RID: 35922 RVA: 0x002EDA08 File Offset: 0x002EBC08
		public override void _Ready()
		{
			MegaLabelHelper.AssertThemeFontOverride(this, ThemeConstants.RichTextLabel.NormalFont);
			this.RefreshFont();
			this.InstallEffectsIfNeeded();
			this.AdjustFontSize();
			base.ParseBbcode(this.Text);
		}

		// Token: 0x06008C53 RID: 35923 RVA: 0x002EDA33 File Offset: 0x002EBC33
		public void RefreshFont()
		{
			this.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.RichTextLabel.NormalFont);
			this.ApplyLocaleFontSubstitution(FontType.Bold, ThemeConstants.RichTextLabel.BoldFont);
			this.ApplyLocaleFontSubstitution(FontType.Italic, ThemeConstants.RichTextLabel.ItalicsFont);
		}

		// Token: 0x06008C54 RID: 35924 RVA: 0x002EDA5C File Offset: 0x002EBC5C
		public override void _Notification(int what)
		{
			if (what != 40)
			{
				if (what == 9001)
				{
					base.CustomEffects.Clear();
					return;
				}
				if (what != 9002)
				{
					return;
				}
				this.InstallEffectsIfNeeded();
			}
			else
			{
				if (this._lastAdjustedSize.DistanceSquaredTo(base.Size) < 0.0001f)
				{
					return;
				}
				if (this.AutoSizeEnabled)
				{
					this._needsResize = true;
					this.AdjustFontSize();
					return;
				}
			}
		}

		// Token: 0x06008C55 RID: 35925 RVA: 0x002EDAC0 File Offset: 0x002EBCC0
		private void InstallEffectsIfNeeded()
		{
			if (this._effectsInstalled && base.CustomEffects.Count > 0)
			{
				return;
			}
			if (!base.BbcodeEnabled)
			{
				return;
			}
			Array array = new Array();
			foreach (AbstractMegaRichTextEffect abstractMegaRichTextEffect in MegaRichTextLabel._textEffects)
			{
				array.Add(abstractMegaRichTextEffect);
			}
			base.CustomEffects = array;
			this._effectsInstalled = true;
		}

		// Token: 0x06008C56 RID: 35926 RVA: 0x002EDB25 File Offset: 0x002EBD25
		[NullableContext(1)]
		private bool HasEffect(AbstractMegaRichTextEffect effect)
		{
			return base.CustomEffects.Contains(effect);
		}

		/// <summary>
		/// Unfortunately, there's no way to override the setting of text for a Label. So if you want the text size to
		/// automatically adjust after being updated during gameplay, you must use this method instead of setting the
		/// Text property directly.
		/// </summary>
		/// <param name="text"></param>
		// Token: 0x06008C57 RID: 35927 RVA: 0x002EDB38 File Offset: 0x002EBD38
		[NullableContext(1)]
		public void SetTextAutoSize(string text)
		{
			if (base.Text == text)
			{
				return;
			}
			base.Text = text;
			this.InstallEffectsIfNeeded();
			if (this.AutoSizeEnabled)
			{
				this._needsResize = true;
				base.CallDeferred("AdjustFontSize", Array.Empty<Variant>());
			}
		}

		// Token: 0x17002681 RID: 9857
		// (get) Token: 0x06008C58 RID: 35928 RVA: 0x002EDB86 File Offset: 0x002EBD86
		// (set) Token: 0x06008C59 RID: 35929 RVA: 0x002EDB8E File Offset: 0x002EBD8E
		[Nullable(1)]
		public string Text
		{
			[NullableContext(1)]
			get
			{
				return base.Text;
			}
			[NullableContext(1)]
			set
			{
				this.SetTextAutoSize(value);
			}
		}

		// Token: 0x06008C5A RID: 35930 RVA: 0x002EDB98 File Offset: 0x002EBD98
		private void AdjustFontSize()
		{
			TextParagraph cachedParagraph = MegaRichTextLabel._cachedParagraph;
			if (!this.AutoSizeEnabled || this._isAutoSizing || cachedParagraph == null)
			{
				return;
			}
			if (!this._needsResize)
			{
				return;
			}
			this._isAutoSizing = true;
			try
			{
				this._needsResize = true;
				this._lastAdjustedSize = base.Size;
				Font themeFont = base.GetThemeFont(ThemeConstants.RichTextLabel.NormalFont, "RichTextLabel");
				float num = (float)base.GetThemeConstant(ThemeConstants.RichTextLabel.LineSpacing, "RichTextLabel");
				Vector2 size = base.GetRect().Size;
				List<BbcodeObject> list = MegaLabelHelper.ParseBbcode(this.Text);
				if (!MegaLabelHelper.IsTooBig(cachedParagraph, list, themeFont, this.MaxFontSize, num, size, this._isHorizontallyBound, this._isVerticallyBound))
				{
					this.SetFontSize(this.MaxFontSize);
				}
				else if (this._lastSetSize >= this.MinFontSize && this._lastSetSize < this.MaxFontSize && !MegaLabelHelper.IsTooBig(cachedParagraph, list, themeFont, this._lastSetSize, num, size, this._isHorizontallyBound, this._isVerticallyBound) && MegaLabelHelper.IsTooBig(cachedParagraph, list, themeFont, this._lastSetSize + 1, num, size, this._isHorizontallyBound, this._isVerticallyBound))
				{
					this.SetFontSize(this._lastSetSize);
				}
				else
				{
					int num2 = this.MinFontSize;
					int i = this.MaxFontSize;
					while (i >= num2)
					{
						int num3 = num2 + (i - num2) / 2;
						bool flag = MegaLabelHelper.IsTooBig(cachedParagraph, list, themeFont, num3, num, size, this._isHorizontallyBound, this._isVerticallyBound);
						if (flag)
						{
							i = num3 - 1;
						}
						else
						{
							num2 = num3 + 1;
						}
					}
					this.SetFontSize(Math.Min(num2, i));
				}
			}
			finally
			{
				this._isAutoSizing = false;
			}
		}

		// Token: 0x06008C5B RID: 35931 RVA: 0x002EDD4C File Offset: 0x002EBF4C
		private void SetFontSize(int size)
		{
			if (this._lastSetSize == size)
			{
				return;
			}
			this._lastSetSize = size;
			base.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize, size);
			if (base.BbcodeEnabled)
			{
				base.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.BoldFontSize, size);
				base.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.BoldItalicsFontSize, size);
				base.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.ItalicsFontSize, size);
				base.AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.MonoFontSize, size);
				base.ParseBbcode(this.Text);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008C5C RID: 35932 RVA: 0x002EDDBC File Offset: 0x002EBFBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(MegaRichTextLabel.MethodName.DisposeCachedParagraph, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(MegaRichTextLabel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaRichTextLabel.MethodName.RefreshFont, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaRichTextLabel.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(MegaRichTextLabel.MethodName.InstallEffectsIfNeeded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaRichTextLabel.MethodName.HasEffect, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "effect", 0L, "", 6L, new StringName("RichTextEffect"), false)
				}, null),
				new MethodInfo(MegaRichTextLabel.MethodName.SetTextAutoSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(MegaRichTextLabel.MethodName.AdjustFontSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaRichTextLabel.MethodName.SetFontSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "size", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008C5D RID: 35933 RVA: 0x002EE01C File Offset: 0x002EC21C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == MegaRichTextLabel.MethodName.DisposeCachedParagraph && args.Count == 0)
			{
				MegaRichTextLabel.DisposeCachedParagraph();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.RefreshFont && args.Count == 0)
			{
				this.RefreshFont();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.InstallEffectsIfNeeded && args.Count == 0)
			{
				this.InstallEffectsIfNeeded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.HasEffect && args.Count == 1)
			{
				bool flag = this.HasEffect(VariantUtils.ConvertTo<AbstractMegaRichTextEffect>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.SetTextAutoSize && args.Count == 1)
			{
				this.SetTextAutoSize(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.AdjustFontSize && args.Count == 0)
			{
				this.AdjustFontSize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaRichTextLabel.MethodName.SetFontSize && args.Count == 1)
			{
				this.SetFontSize(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06008C5E RID: 35934 RVA: 0x002EE1BD File Offset: 0x002EC3BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == MegaRichTextLabel.MethodName.DisposeCachedParagraph && args.Count == 0)
			{
				MegaRichTextLabel.DisposeCachedParagraph();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06008C5F RID: 35935 RVA: 0x002EE1EC File Offset: 0x002EC3EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == MegaRichTextLabel.MethodName.DisposeCachedParagraph || (ref method) == MegaRichTextLabel.MethodName._Ready || (ref method) == MegaRichTextLabel.MethodName.RefreshFont || (ref method) == MegaRichTextLabel.MethodName._Notification || (ref method) == MegaRichTextLabel.MethodName.InstallEffectsIfNeeded || (ref method) == MegaRichTextLabel.MethodName.HasEffect || (ref method) == MegaRichTextLabel.MethodName.SetTextAutoSize || (ref method) == MegaRichTextLabel.MethodName.AdjustFontSize || (ref method) == MegaRichTextLabel.MethodName.SetFontSize || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06008C60 RID: 35936 RVA: 0x002EE288 File Offset: 0x002EC488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == MegaRichTextLabel.PropertyName.AutoSizeEnabled)
			{
				this.AutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.MinFontSize)
			{
				this.MinFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.MaxFontSize)
			{
				this.MaxFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.IsVerticallyBound)
			{
				this.IsVerticallyBound = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.IsHorizontallyBound)
			{
				this.IsHorizontallyBound = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.Text)
			{
				this.Text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isAutoSizeEnabled)
			{
				this._isAutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._minFontSize)
			{
				this._minFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._maxFontSize)
			{
				this._maxFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._lastSetSize)
			{
				this._lastSetSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isVerticallyBound)
			{
				this._isVerticallyBound = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isHorizontallyBound)
			{
				this._isHorizontallyBound = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._needsResize)
			{
				this._needsResize = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._effectsInstalled)
			{
				this._effectsInstalled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._lastAdjustedSize)
			{
				this._lastAdjustedSize = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isAutoSizing)
			{
				this._isAutoSizing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06008C61 RID: 35937 RVA: 0x002EE450 File Offset: 0x002EC650
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == MegaRichTextLabel.PropertyName.AutoSizeEnabled)
			{
				bool flag = this.AutoSizeEnabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.MinFontSize)
			{
				int num = this.MinFontSize;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.MaxFontSize)
			{
				int num = this.MaxFontSize;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.IsVerticallyBound)
			{
				bool flag = this.IsVerticallyBound;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.IsHorizontallyBound)
			{
				bool flag = this.IsHorizontallyBound;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName.Text)
			{
				string text = this.Text;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isAutoSizeEnabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAutoSizeEnabled);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._minFontSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._minFontSize);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._maxFontSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxFontSize);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._lastSetSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._lastSetSize);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isVerticallyBound)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isVerticallyBound);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isHorizontallyBound)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHorizontallyBound);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._needsResize)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._needsResize);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._effectsInstalled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._effectsInstalled);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._lastAdjustedSize)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._lastAdjustedSize);
				return true;
			}
			if ((ref name) == MegaRichTextLabel.PropertyName._isAutoSizing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAutoSizing);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008C62 RID: 35938 RVA: 0x002EE678 File Offset: 0x002EC878
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._isAutoSizeEnabled, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaRichTextLabel.PropertyName._minFontSize, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaRichTextLabel.PropertyName._maxFontSize, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaRichTextLabel.PropertyName._lastSetSize, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._isVerticallyBound, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._isHorizontallyBound, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._needsResize, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._effectsInstalled, 0L, "", 4096L, false),
				new PropertyInfo(5L, MegaRichTextLabel.PropertyName._lastAdjustedSize, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName.AutoSizeEnabled, 0L, "", 4102L, true),
				new PropertyInfo(2L, MegaRichTextLabel.PropertyName.MinFontSize, 0L, "", 4102L, true),
				new PropertyInfo(2L, MegaRichTextLabel.PropertyName.MaxFontSize, 0L, "", 4102L, true),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName.IsVerticallyBound, 0L, "", 4102L, true),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName.IsHorizontallyBound, 0L, "", 4102L, true),
				new PropertyInfo(1L, MegaRichTextLabel.PropertyName._isAutoSizing, 0L, "", 4096L, false),
				new PropertyInfo(4L, MegaRichTextLabel.PropertyName.Text, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008C63 RID: 35939 RVA: 0x002EE88C File Offset: 0x002ECA8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName autoSizeEnabled = MegaRichTextLabel.PropertyName.AutoSizeEnabled;
			bool flag = this.AutoSizeEnabled;
			info.AddProperty(autoSizeEnabled, Variant.From<bool>(ref flag));
			StringName minFontSize = MegaRichTextLabel.PropertyName.MinFontSize;
			int num = this.MinFontSize;
			info.AddProperty(minFontSize, Variant.From<int>(ref num));
			StringName maxFontSize = MegaRichTextLabel.PropertyName.MaxFontSize;
			num = this.MaxFontSize;
			info.AddProperty(maxFontSize, Variant.From<int>(ref num));
			StringName isVerticallyBound = MegaRichTextLabel.PropertyName.IsVerticallyBound;
			flag = this.IsVerticallyBound;
			info.AddProperty(isVerticallyBound, Variant.From<bool>(ref flag));
			StringName isHorizontallyBound = MegaRichTextLabel.PropertyName.IsHorizontallyBound;
			flag = this.IsHorizontallyBound;
			info.AddProperty(isHorizontallyBound, Variant.From<bool>(ref flag));
			StringName text = MegaRichTextLabel.PropertyName.Text;
			string text2 = this.Text;
			info.AddProperty(text, Variant.From<string>(ref text2));
			info.AddProperty(MegaRichTextLabel.PropertyName._isAutoSizeEnabled, Variant.From<bool>(ref this._isAutoSizeEnabled));
			info.AddProperty(MegaRichTextLabel.PropertyName._minFontSize, Variant.From<int>(ref this._minFontSize));
			info.AddProperty(MegaRichTextLabel.PropertyName._maxFontSize, Variant.From<int>(ref this._maxFontSize));
			info.AddProperty(MegaRichTextLabel.PropertyName._lastSetSize, Variant.From<int>(ref this._lastSetSize));
			info.AddProperty(MegaRichTextLabel.PropertyName._isVerticallyBound, Variant.From<bool>(ref this._isVerticallyBound));
			info.AddProperty(MegaRichTextLabel.PropertyName._isHorizontallyBound, Variant.From<bool>(ref this._isHorizontallyBound));
			info.AddProperty(MegaRichTextLabel.PropertyName._needsResize, Variant.From<bool>(ref this._needsResize));
			info.AddProperty(MegaRichTextLabel.PropertyName._effectsInstalled, Variant.From<bool>(ref this._effectsInstalled));
			info.AddProperty(MegaRichTextLabel.PropertyName._lastAdjustedSize, Variant.From<Vector2>(ref this._lastAdjustedSize));
			info.AddProperty(MegaRichTextLabel.PropertyName._isAutoSizing, Variant.From<bool>(ref this._isAutoSizing));
		}

		/// <inheritdoc />
		// Token: 0x06008C64 RID: 35940 RVA: 0x002EEA14 File Offset: 0x002ECC14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.AutoSizeEnabled, ref variant))
			{
				this.AutoSizeEnabled = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.MinFontSize, ref variant2))
			{
				this.MinFontSize = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.MaxFontSize, ref variant3))
			{
				this.MaxFontSize = variant3.As<int>();
			}
			Variant variant4;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.IsVerticallyBound, ref variant4))
			{
				this.IsVerticallyBound = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.IsHorizontallyBound, ref variant5))
			{
				this.IsHorizontallyBound = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName.Text, ref variant6))
			{
				this.Text = variant6.As<string>();
			}
			Variant variant7;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isAutoSizeEnabled, ref variant7))
			{
				this._isAutoSizeEnabled = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._minFontSize, ref variant8))
			{
				this._minFontSize = variant8.As<int>();
			}
			Variant variant9;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._maxFontSize, ref variant9))
			{
				this._maxFontSize = variant9.As<int>();
			}
			Variant variant10;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._lastSetSize, ref variant10))
			{
				this._lastSetSize = variant10.As<int>();
			}
			Variant variant11;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isVerticallyBound, ref variant11))
			{
				this._isVerticallyBound = variant11.As<bool>();
			}
			Variant variant12;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isHorizontallyBound, ref variant12))
			{
				this._isHorizontallyBound = variant12.As<bool>();
			}
			Variant variant13;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._needsResize, ref variant13))
			{
				this._needsResize = variant13.As<bool>();
			}
			Variant variant14;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._effectsInstalled, ref variant14))
			{
				this._effectsInstalled = variant14.As<bool>();
			}
			Variant variant15;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._lastAdjustedSize, ref variant15))
			{
				this._lastAdjustedSize = variant15.As<Vector2>();
			}
			Variant variant16;
			if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isAutoSizing, ref variant16))
			{
				this._isAutoSizing = variant16.As<bool>();
			}
		}

		// Token: 0x04003117 RID: 12567
		[Nullable(2)]
		private static TextParagraph _cachedParagraph = new TextParagraph();

		// Token: 0x04003118 RID: 12568
		private const float _sizeComparisonEpsilon = 0.01f;

		// Token: 0x04003119 RID: 12569
		private bool _isAutoSizeEnabled = true;

		// Token: 0x0400311A RID: 12570
		private int _minFontSize = 8;

		// Token: 0x0400311B RID: 12571
		private int _maxFontSize = 100;

		// Token: 0x0400311C RID: 12572
		private int _lastSetSize;

		// Token: 0x0400311D RID: 12573
		private bool _isVerticallyBound = true;

		// Token: 0x0400311E RID: 12574
		private bool _isHorizontallyBound;

		// Token: 0x0400311F RID: 12575
		private bool _needsResize = true;

		// Token: 0x04003120 RID: 12576
		private bool _effectsInstalled;

		// Token: 0x04003121 RID: 12577
		private Vector2 _lastAdjustedSize;

		// Token: 0x04003122 RID: 12578
		[Nullable(1)]
		private static readonly AbstractMegaRichTextEffect[] _textEffects = new AbstractMegaRichTextEffect[]
		{
			new RichTextAqua(),
			new RichTextBlue(),
			new RichTextFadeIn(),
			new RichTextFlyIn(),
			new RichTextGold(),
			new RichTextGreen(),
			new RichTextJitter(),
			new RichTextOrange(),
			new RichTextPink(),
			new RichTextPurple(),
			new RichTextRed(),
			new RichTextScramble(),
			new RichTextSine(),
			new RichTextThinkyDots()
		};

		// Token: 0x04003123 RID: 12579
		private bool _isAutoSizing;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002577 RID: 9591
		public class MethodName : RichTextLabel.MethodName
		{
			/// <summary>
			/// Cached name for the 'DisposeCachedParagraph' method.
			/// </summary>
			// Token: 0x0400A05C RID: 41052
			public static readonly StringName DisposeCachedParagraph = "DisposeCachedParagraph";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400A05D RID: 41053
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshFont' method.
			/// </summary>
			// Token: 0x0400A05E RID: 41054
			public static readonly StringName RefreshFont = "RefreshFont";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x0400A05F RID: 41055
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'InstallEffectsIfNeeded' method.
			/// </summary>
			// Token: 0x0400A060 RID: 41056
			public static readonly StringName InstallEffectsIfNeeded = "InstallEffectsIfNeeded";

			/// <summary>
			/// Cached name for the 'HasEffect' method.
			/// </summary>
			// Token: 0x0400A061 RID: 41057
			public static readonly StringName HasEffect = "HasEffect";

			/// <summary>
			/// Cached name for the 'SetTextAutoSize' method.
			/// </summary>
			// Token: 0x0400A062 RID: 41058
			public static readonly StringName SetTextAutoSize = "SetTextAutoSize";

			/// <summary>
			/// Cached name for the 'AdjustFontSize' method.
			/// </summary>
			// Token: 0x0400A063 RID: 41059
			public static readonly StringName AdjustFontSize = "AdjustFontSize";

			/// <summary>
			/// Cached name for the 'SetFontSize' method.
			/// </summary>
			// Token: 0x0400A064 RID: 41060
			public static readonly StringName SetFontSize = "SetFontSize";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002578 RID: 9592
		public class PropertyName : RichTextLabel.PropertyName
		{
			/// <summary>
			/// Cached name for the 'AutoSizeEnabled' property.
			/// </summary>
			// Token: 0x0400A065 RID: 41061
			public static readonly StringName AutoSizeEnabled = "AutoSizeEnabled";

			/// <summary>
			/// Cached name for the 'MinFontSize' property.
			/// </summary>
			// Token: 0x0400A066 RID: 41062
			public static readonly StringName MinFontSize = "MinFontSize";

			/// <summary>
			/// Cached name for the 'MaxFontSize' property.
			/// </summary>
			// Token: 0x0400A067 RID: 41063
			public static readonly StringName MaxFontSize = "MaxFontSize";

			/// <summary>
			/// Cached name for the 'IsVerticallyBound' property.
			/// </summary>
			// Token: 0x0400A068 RID: 41064
			public static readonly StringName IsVerticallyBound = "IsVerticallyBound";

			/// <summary>
			/// Cached name for the 'IsHorizontallyBound' property.
			/// </summary>
			// Token: 0x0400A069 RID: 41065
			public static readonly StringName IsHorizontallyBound = "IsHorizontallyBound";

			/// <summary>
			/// Cached name for the 'Text' property.
			/// </summary>
			// Token: 0x0400A06A RID: 41066
			public static readonly StringName Text = "Text";

			/// <summary>
			/// Cached name for the '_isAutoSizeEnabled' field.
			/// </summary>
			// Token: 0x0400A06B RID: 41067
			public static readonly StringName _isAutoSizeEnabled = "_isAutoSizeEnabled";

			/// <summary>
			/// Cached name for the '_minFontSize' field.
			/// </summary>
			// Token: 0x0400A06C RID: 41068
			public static readonly StringName _minFontSize = "_minFontSize";

			/// <summary>
			/// Cached name for the '_maxFontSize' field.
			/// </summary>
			// Token: 0x0400A06D RID: 41069
			public static readonly StringName _maxFontSize = "_maxFontSize";

			/// <summary>
			/// Cached name for the '_lastSetSize' field.
			/// </summary>
			// Token: 0x0400A06E RID: 41070
			public static readonly StringName _lastSetSize = "_lastSetSize";

			/// <summary>
			/// Cached name for the '_isVerticallyBound' field.
			/// </summary>
			// Token: 0x0400A06F RID: 41071
			public static readonly StringName _isVerticallyBound = "_isVerticallyBound";

			/// <summary>
			/// Cached name for the '_isHorizontallyBound' field.
			/// </summary>
			// Token: 0x0400A070 RID: 41072
			public static readonly StringName _isHorizontallyBound = "_isHorizontallyBound";

			/// <summary>
			/// Cached name for the '_needsResize' field.
			/// </summary>
			// Token: 0x0400A071 RID: 41073
			public static readonly StringName _needsResize = "_needsResize";

			/// <summary>
			/// Cached name for the '_effectsInstalled' field.
			/// </summary>
			// Token: 0x0400A072 RID: 41074
			public static readonly StringName _effectsInstalled = "_effectsInstalled";

			/// <summary>
			/// Cached name for the '_lastAdjustedSize' field.
			/// </summary>
			// Token: 0x0400A073 RID: 41075
			public static readonly StringName _lastAdjustedSize = "_lastAdjustedSize";

			/// <summary>
			/// Cached name for the '_isAutoSizing' field.
			/// </summary>
			// Token: 0x0400A074 RID: 41076
			public static readonly StringName _isAutoSizing = "_isAutoSizing";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002579 RID: 9593
		public class SignalName : RichTextLabel.SignalName
		{
		}
	}
}
