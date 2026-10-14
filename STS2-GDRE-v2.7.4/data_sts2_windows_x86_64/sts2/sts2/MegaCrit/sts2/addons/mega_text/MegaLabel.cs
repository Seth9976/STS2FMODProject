using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization.Fonts;

namespace MegaCrit.Sts2.addons.mega_text
{
	// Token: 0x02000D9E RID: 3486
	[Tool]
	[ScriptPath("res://addons/mega_text/MegaLabel.cs")]
	public class MegaLabel : Label
	{
		/// <summary>
		/// Releases the cached TextParagraph to free text server RIDs at exit.
		/// Uses Dispose() (not Free()) because TextParagraph is RefCounted.
		/// Nulled to guard against AdjustFontSize running during Godot's quit frames.
		/// </summary>
		// Token: 0x06008C28 RID: 35880 RVA: 0x002EC7C9 File Offset: 0x002EA9C9
		public static void DisposeCachedParagraph()
		{
			TextParagraph cachedParagraph = MegaLabel._cachedParagraph;
			if (cachedParagraph != null)
			{
				cachedParagraph.Dispose();
			}
			MegaLabel._cachedParagraph = null;
		}

		// Token: 0x17002679 RID: 9849
		// (get) Token: 0x06008C29 RID: 35881 RVA: 0x002EC7E1 File Offset: 0x002EA9E1
		// (set) Token: 0x06008C2A RID: 35882 RVA: 0x002EC7E9 File Offset: 0x002EA9E9
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool AutoSizeEnabled
		{
			get
			{
				return this._autoSizeEnabled;
			}
			set
			{
				if (this._autoSizeEnabled != value)
				{
					this._autoSizeEnabled = value;
					if (Engine.IsEditorHint())
					{
						this.AdjustFontSize();
					}
				}
			}
		}

		// Token: 0x1700267A RID: 9850
		// (get) Token: 0x06008C2B RID: 35883 RVA: 0x002EC808 File Offset: 0x002EAA08
		// (set) Token: 0x06008C2C RID: 35884 RVA: 0x002EC810 File Offset: 0x002EAA10
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public int MinFontSize
		{
			get
			{
				return this._minFontSize;
			}
			set
			{
				if (this._minFontSize != value)
				{
					this._minFontSize = value;
					if (Engine.IsEditorHint())
					{
						this.AdjustFontSize();
					}
				}
			}
		}

		// Token: 0x1700267B RID: 9851
		// (get) Token: 0x06008C2D RID: 35885 RVA: 0x002EC82F File Offset: 0x002EAA2F
		// (set) Token: 0x06008C2E RID: 35886 RVA: 0x002EC837 File Offset: 0x002EAA37
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public int MaxFontSize
		{
			get
			{
				return this._maxFontSize;
			}
			set
			{
				if (this._maxFontSize != value)
				{
					this._maxFontSize = value;
					if (Engine.IsEditorHint())
					{
						this.AdjustFontSize();
					}
				}
			}
		}

		// Token: 0x06008C2F RID: 35887 RVA: 0x002EC856 File Offset: 0x002EAA56
		public override void _Ready()
		{
			MegaLabelHelper.AssertThemeFontOverride(this, ThemeConstants.Label.Font);
			this.RefreshFont();
			this.AdjustFontSize();
		}

		// Token: 0x06008C30 RID: 35888 RVA: 0x002EC86F File Offset: 0x002EAA6F
		public void RefreshFont()
		{
			this.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
		}

		// Token: 0x06008C31 RID: 35889 RVA: 0x002EC87D File Offset: 0x002EAA7D
		public override void _Notification(int what)
		{
			if ((long)what == 40L)
			{
				if (this._lastAdjustedSize.DistanceSquaredTo(base.Size) < 0.0001f)
				{
					return;
				}
				this.AdjustFontSize();
			}
		}

		/// <summary>
		/// Unfortunately, there's no way to override the setting of text for a Label. So if you want the text size to
		/// automatically adjust after being updated during gameplay, you must use this method instead of setting the
		/// Text property directly.
		/// </summary>
		/// <param name="text"></param>
		// Token: 0x06008C32 RID: 35890 RVA: 0x002EC8A5 File Offset: 0x002EAAA5
		[NullableContext(1)]
		public void SetTextAutoSize(string text)
		{
			if (base.Text == text)
			{
				return;
			}
			base.Text = text;
			this.AdjustFontSize();
		}

		// Token: 0x06008C33 RID: 35891 RVA: 0x002EC8C3 File Offset: 0x002EAAC3
		private void SetFontSize(int size)
		{
			if (this._lastSetSize == size)
			{
				return;
			}
			this._lastSetSize = size;
			if (base.HasThemeFont(ThemeConstants.Label.Font, null))
			{
				base.AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, size);
			}
		}

		// Token: 0x06008C34 RID: 35892 RVA: 0x002EC8F0 File Offset: 0x002EAAF0
		private void AdjustFontSize()
		{
			TextParagraph cachedParagraph = MegaLabel._cachedParagraph;
			if (!this.AutoSizeEnabled || cachedParagraph == null)
			{
				return;
			}
			this._lastAdjustedSize = base.Size;
			Font themeFont = base.GetThemeFont(ThemeConstants.Label.Font, "Label");
			float num = (float)base.GetThemeConstant(ThemeConstants.Label.LineSpacing, "Label");
			Vector2 size = base.GetRect().Size;
			bool flag = base.AutowrapMode > 0L;
			if (!MegaLabelHelper.IsTooBig(cachedParagraph, base.Text, themeFont, this.MaxFontSize, num, flag, size))
			{
				this.SetFontSize(this.MaxFontSize);
				return;
			}
			if (this._lastSetSize >= this.MinFontSize && this._lastSetSize < this.MaxFontSize && !MegaLabelHelper.IsTooBig(cachedParagraph, base.Text, themeFont, this._lastSetSize, num, flag, size) && MegaLabelHelper.IsTooBig(cachedParagraph, base.Text, themeFont, this._lastSetSize + 1, num, flag, size))
			{
				this.SetFontSize(this._lastSetSize);
				return;
			}
			int num2 = this.MinFontSize;
			int i = this.MaxFontSize;
			while (i >= num2)
			{
				int num3 = num2 + (i - num2) / 2;
				bool flag2 = num3 == this.MaxFontSize || MegaLabelHelper.IsTooBig(cachedParagraph, base.Text, themeFont, num3, num, flag, size);
				if (flag2)
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008C35 RID: 35893 RVA: 0x002ECA50 File Offset: 0x002EAC50
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(MegaLabel.MethodName.DisposeCachedParagraph, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(MegaLabel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaLabel.MethodName.RefreshFont, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(MegaLabel.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(MegaLabel.MethodName.SetTextAutoSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(MegaLabel.MethodName.SetFontSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "size", 0L, "", 6L, false)
				}, null),
				new MethodInfo(MegaLabel.MethodName.AdjustFontSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008C36 RID: 35894 RVA: 0x002ECC20 File Offset: 0x002EAE20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == MegaLabel.MethodName.DisposeCachedParagraph && args.Count == 0)
			{
				MegaLabel.DisposeCachedParagraph();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName.RefreshFont && args.Count == 0)
			{
				this.RefreshFont();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName.SetTextAutoSize && args.Count == 1)
			{
				this.SetTextAutoSize(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName.SetFontSize && args.Count == 1)
			{
				this.SetFontSize(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == MegaLabel.MethodName.AdjustFontSize && args.Count == 0)
			{
				this.AdjustFontSize();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06008C37 RID: 35895 RVA: 0x002ECD62 File Offset: 0x002EAF62
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == MegaLabel.MethodName.DisposeCachedParagraph && args.Count == 0)
			{
				MegaLabel.DisposeCachedParagraph();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06008C38 RID: 35896 RVA: 0x002ECD90 File Offset: 0x002EAF90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == MegaLabel.MethodName.DisposeCachedParagraph || (ref method) == MegaLabel.MethodName._Ready || (ref method) == MegaLabel.MethodName.RefreshFont || (ref method) == MegaLabel.MethodName._Notification || (ref method) == MegaLabel.MethodName.SetTextAutoSize || (ref method) == MegaLabel.MethodName.SetFontSize || (ref method) == MegaLabel.MethodName.AdjustFontSize || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06008C39 RID: 35897 RVA: 0x002ECE10 File Offset: 0x002EB010
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == MegaLabel.PropertyName.AutoSizeEnabled)
			{
				this.AutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName.MinFontSize)
			{
				this.MinFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName.MaxFontSize)
			{
				this.MaxFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._autoSizeEnabled)
			{
				this._autoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._minFontSize)
			{
				this._minFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._maxFontSize)
			{
				this._maxFontSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._lastSetSize)
			{
				this._lastSetSize = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._lastAdjustedSize)
			{
				this._lastAdjustedSize = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06008C3A RID: 35898 RVA: 0x002ECF00 File Offset: 0x002EB100
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == MegaLabel.PropertyName.AutoSizeEnabled)
			{
				bool autoSizeEnabled = this.AutoSizeEnabled;
				value = VariantUtils.CreateFrom<bool>(ref autoSizeEnabled);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName.MinFontSize)
			{
				int num = this.MinFontSize;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName.MaxFontSize)
			{
				int num = this.MaxFontSize;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._autoSizeEnabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._autoSizeEnabled);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._minFontSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._minFontSize);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._maxFontSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxFontSize);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._lastSetSize)
			{
				value = VariantUtils.CreateFrom<int>(ref this._lastSetSize);
				return true;
			}
			if ((ref name) == MegaLabel.PropertyName._lastAdjustedSize)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._lastAdjustedSize);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008C3B RID: 35899 RVA: 0x002ED020 File Offset: 0x002EB220
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, MegaLabel.PropertyName._autoSizeEnabled, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaLabel.PropertyName._minFontSize, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaLabel.PropertyName._maxFontSize, 0L, "", 4096L, false),
				new PropertyInfo(2L, MegaLabel.PropertyName._lastSetSize, 0L, "", 4096L, false),
				new PropertyInfo(5L, MegaLabel.PropertyName._lastAdjustedSize, 0L, "", 4096L, false),
				new PropertyInfo(1L, MegaLabel.PropertyName.AutoSizeEnabled, 0L, "", 4102L, true),
				new PropertyInfo(2L, MegaLabel.PropertyName.MinFontSize, 0L, "", 4102L, true),
				new PropertyInfo(2L, MegaLabel.PropertyName.MaxFontSize, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008C3C RID: 35900 RVA: 0x002ED134 File Offset: 0x002EB334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName autoSizeEnabled = MegaLabel.PropertyName.AutoSizeEnabled;
			bool autoSizeEnabled2 = this.AutoSizeEnabled;
			info.AddProperty(autoSizeEnabled, Variant.From<bool>(ref autoSizeEnabled2));
			StringName minFontSize = MegaLabel.PropertyName.MinFontSize;
			int num = this.MinFontSize;
			info.AddProperty(minFontSize, Variant.From<int>(ref num));
			StringName maxFontSize = MegaLabel.PropertyName.MaxFontSize;
			num = this.MaxFontSize;
			info.AddProperty(maxFontSize, Variant.From<int>(ref num));
			info.AddProperty(MegaLabel.PropertyName._autoSizeEnabled, Variant.From<bool>(ref this._autoSizeEnabled));
			info.AddProperty(MegaLabel.PropertyName._minFontSize, Variant.From<int>(ref this._minFontSize));
			info.AddProperty(MegaLabel.PropertyName._maxFontSize, Variant.From<int>(ref this._maxFontSize));
			info.AddProperty(MegaLabel.PropertyName._lastSetSize, Variant.From<int>(ref this._lastSetSize));
			info.AddProperty(MegaLabel.PropertyName._lastAdjustedSize, Variant.From<Vector2>(ref this._lastAdjustedSize));
		}

		/// <inheritdoc />
		// Token: 0x06008C3D RID: 35901 RVA: 0x002ED204 File Offset: 0x002EB404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(MegaLabel.PropertyName.AutoSizeEnabled, ref variant))
			{
				this.AutoSizeEnabled = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(MegaLabel.PropertyName.MinFontSize, ref variant2))
			{
				this.MinFontSize = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(MegaLabel.PropertyName.MaxFontSize, ref variant3))
			{
				this.MaxFontSize = variant3.As<int>();
			}
			Variant variant4;
			if (info.TryGetProperty(MegaLabel.PropertyName._autoSizeEnabled, ref variant4))
			{
				this._autoSizeEnabled = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(MegaLabel.PropertyName._minFontSize, ref variant5))
			{
				this._minFontSize = variant5.As<int>();
			}
			Variant variant6;
			if (info.TryGetProperty(MegaLabel.PropertyName._maxFontSize, ref variant6))
			{
				this._maxFontSize = variant6.As<int>();
			}
			Variant variant7;
			if (info.TryGetProperty(MegaLabel.PropertyName._lastSetSize, ref variant7))
			{
				this._lastSetSize = variant7.As<int>();
			}
			Variant variant8;
			if (info.TryGetProperty(MegaLabel.PropertyName._lastAdjustedSize, ref variant8))
			{
				this._lastAdjustedSize = variant8.As<Vector2>();
			}
		}

		// Token: 0x0400310E RID: 12558
		[Nullable(2)]
		private static TextParagraph _cachedParagraph = new TextParagraph();

		// Token: 0x0400310F RID: 12559
		private const float _sizeComparisonEpsilon = 0.01f;

		// Token: 0x04003110 RID: 12560
		private bool _autoSizeEnabled = true;

		// Token: 0x04003111 RID: 12561
		private int _minFontSize = 8;

		// Token: 0x04003112 RID: 12562
		private int _maxFontSize = 100;

		// Token: 0x04003113 RID: 12563
		private int _lastSetSize;

		// Token: 0x04003114 RID: 12564
		private Vector2 _lastAdjustedSize;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002573 RID: 9587
		public class MethodName : Label.MethodName
		{
			/// <summary>
			/// Cached name for the 'DisposeCachedParagraph' method.
			/// </summary>
			// Token: 0x0400A048 RID: 41032
			public static readonly StringName DisposeCachedParagraph = "DisposeCachedParagraph";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400A049 RID: 41033
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshFont' method.
			/// </summary>
			// Token: 0x0400A04A RID: 41034
			public static readonly StringName RefreshFont = "RefreshFont";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x0400A04B RID: 41035
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'SetTextAutoSize' method.
			/// </summary>
			// Token: 0x0400A04C RID: 41036
			public static readonly StringName SetTextAutoSize = "SetTextAutoSize";

			/// <summary>
			/// Cached name for the 'SetFontSize' method.
			/// </summary>
			// Token: 0x0400A04D RID: 41037
			public static readonly StringName SetFontSize = "SetFontSize";

			/// <summary>
			/// Cached name for the 'AdjustFontSize' method.
			/// </summary>
			// Token: 0x0400A04E RID: 41038
			public static readonly StringName AdjustFontSize = "AdjustFontSize";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002574 RID: 9588
		public class PropertyName : Label.PropertyName
		{
			/// <summary>
			/// Cached name for the 'AutoSizeEnabled' property.
			/// </summary>
			// Token: 0x0400A04F RID: 41039
			public static readonly StringName AutoSizeEnabled = "AutoSizeEnabled";

			/// <summary>
			/// Cached name for the 'MinFontSize' property.
			/// </summary>
			// Token: 0x0400A050 RID: 41040
			public static readonly StringName MinFontSize = "MinFontSize";

			/// <summary>
			/// Cached name for the 'MaxFontSize' property.
			/// </summary>
			// Token: 0x0400A051 RID: 41041
			public static readonly StringName MaxFontSize = "MaxFontSize";

			/// <summary>
			/// Cached name for the '_autoSizeEnabled' field.
			/// </summary>
			// Token: 0x0400A052 RID: 41042
			public static readonly StringName _autoSizeEnabled = "_autoSizeEnabled";

			/// <summary>
			/// Cached name for the '_minFontSize' field.
			/// </summary>
			// Token: 0x0400A053 RID: 41043
			public static readonly StringName _minFontSize = "_minFontSize";

			/// <summary>
			/// Cached name for the '_maxFontSize' field.
			/// </summary>
			// Token: 0x0400A054 RID: 41044
			public static readonly StringName _maxFontSize = "_maxFontSize";

			/// <summary>
			/// Cached name for the '_lastSetSize' field.
			/// </summary>
			// Token: 0x0400A055 RID: 41045
			public static readonly StringName _lastSetSize = "_lastSetSize";

			/// <summary>
			/// Cached name for the '_lastAdjustedSize' field.
			/// </summary>
			// Token: 0x0400A056 RID: 41046
			public static readonly StringName _lastAdjustedSize = "_lastAdjustedSize";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002575 RID: 9589
		public class SignalName : Label.SignalName
		{
		}
	}
}
