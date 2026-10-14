using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000271 RID: 625
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NAchievementHolder.cs")]
	public class NAchievementHolder : Control
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x000DF4D5 File Offset: 0x000DD6D5
		// (set) Token: 0x060020FD RID: 8445 RVA: 0x000DF4DD File Offset: 0x000DD6DD
		public bool IsUnlocked { get; private set; }

		// Token: 0x060020FE RID: 8446 RVA: 0x000DF4E8 File Offset: 0x000DD6E8
		[NullableContext(2)]
		public static NAchievementHolder Create(Achievement achievement)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NAchievementHolder nachievementHolder = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("screens/stats_screen/achievement_holder")).Instantiate<NAchievementHolder>(0L);
			nachievementHolder._achievement = achievement;
			nachievementHolder.IsUnlocked = AchievementsUtil.IsUnlocked(achievement);
			return nachievementHolder;
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000DF530 File Offset: 0x000DD730
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._border = base.GetNode<TextureRect>("%Border");
			this._lock = base.GetNode<TextureRect>("%Lock");
			this._borderHsv = (ShaderMaterial)this._border.Material;
			this._iconHsv = (ShaderMaterial)this._icon.Material;
			this._infoLabel = base.GetNode<MegaRichTextLabel>("%InfoText");
			this._date = base.GetNode<MegaLabel>("%DateText");
			this.RefreshUnlocked();
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000DF5E0 File Offset: 0x000DD7E0
		public static string GetPathForAchievement(Enum achievement)
		{
			string text = StringHelper.SnakeCase(achievement.ToString()).ToLowerInvariant();
			return ImageHelper.GetImagePath("packed/achievements/unlocked/" + text + ".png");
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000DF614 File Offset: 0x000DD814
		public void RefreshUnlocked()
		{
			this.IsUnlocked = AchievementsUtil.IsUnlocked(this._achievement);
			string text = StringHelper.SnakeCase(this._achievement.ToString()).ToLowerInvariant();
			this._icon.Texture = PreloadManager.Cache.GetTexture2D(NAchievementHolder.GetPathForAchievement(this._achievement));
			text = text.ToUpperInvariant();
			if (this.IsUnlocked)
			{
				this._infoLabel.Text = "[b][gold]" + new LocString("achievements", text + ".title").GetRawText() + "[/gold][/b]\n" + new LocString("achievements", text + ".description").GetFormattedText();
			}
			else
			{
				this._infoLabel.Text = "[b][red]" + new LocString("achievements", "LOCKED.title").GetRawText() + "[/red][/b]\n" + new LocString("achievements", text + ".description").GetFormattedText();
			}
			this.SetLockVisuals();
			this.SetDateLabel();
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000DF728 File Offset: 0x000DD928
		private void SetLockVisuals()
		{
			this._lock.Visible = !this.IsUnlocked;
			if (this.IsUnlocked)
			{
				this._borderHsv.SetShaderParameter(NAchievementHolder._h, 1f);
				this._borderHsv.SetShaderParameter(NAchievementHolder._s, 1f);
				this._borderHsv.SetShaderParameter(NAchievementHolder._v, 1f);
				this._iconHsv.SetShaderParameter(NAchievementHolder._s, 1f);
				this._iconHsv.SetShaderParameter(NAchievementHolder._v, 1f);
				return;
			}
			this._borderHsv.SetShaderParameter(NAchievementHolder._h, 0.4f);
			this._borderHsv.SetShaderParameter(NAchievementHolder._s, 0.4f);
			this._borderHsv.SetShaderParameter(NAchievementHolder._v, 0.8f);
			this._iconHsv.SetShaderParameter(NAchievementHolder._s, 0.2f);
			this._iconHsv.SetShaderParameter(NAchievementHolder._v, 0.5f);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000DF85C File Offset: 0x000DDA5C
		private void SetDateLabel()
		{
			this._date.Visible = this.IsUnlocked;
			if (!this.IsUnlocked)
			{
				return;
			}
			long num;
			if (!SaveManager.Instance.Progress.UnlockedAchievements.TryGetValue(this._achievement, out num))
			{
				this._date.Visible = false;
				return;
			}
			DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
			DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(num).UtcDateTime, TimeZoneInfo.Local);
			LocString locString = new LocString("achievements", "UNLOCK_DATE.text");
			string text = dateTime.ToString(new LocString("achievements", "UNLOCK_DATE.format").GetRawText(), dateTimeFormat);
			locString.Add("Date", text);
			this._date.SetTextAutoSize(locString.GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002104 RID: 8452 RVA: 0x000DF928 File Offset: 0x000DDB28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAchievementHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "achievement", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAchievementHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementHolder.MethodName.RefreshUnlocked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementHolder.MethodName.SetLockVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementHolder.MethodName.SetDateLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002105 RID: 8453 RVA: 0x000DFA5C File Offset: 0x000DDC5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAchievementHolder.MethodName.Create && args.Count == 1)
			{
				NAchievementHolder nachievementHolder = NAchievementHolder.Create(VariantUtils.ConvertTo<Achievement>(args[0]));
				ret = VariantUtils.CreateFrom<NAchievementHolder>(ref nachievementHolder);
				return true;
			}
			if ((ref method) == NAchievementHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementHolder.MethodName.RefreshUnlocked && args.Count == 0)
			{
				this.RefreshUnlocked();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementHolder.MethodName.SetLockVisuals && args.Count == 0)
			{
				this.SetLockVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementHolder.MethodName.SetDateLabel && args.Count == 0)
			{
				this.SetDateLabel();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000DFB40 File Offset: 0x000DDD40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAchievementHolder.MethodName.Create && args.Count == 1)
			{
				NAchievementHolder nachievementHolder = NAchievementHolder.Create(VariantUtils.ConvertTo<Achievement>(args[0]));
				ret = VariantUtils.CreateFrom<NAchievementHolder>(ref nachievementHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002107 RID: 8455 RVA: 0x000DFB90 File Offset: 0x000DDD90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAchievementHolder.MethodName.Create || (ref method) == NAchievementHolder.MethodName._Ready || (ref method) == NAchievementHolder.MethodName.RefreshUnlocked || (ref method) == NAchievementHolder.MethodName.SetLockVisuals || (ref method) == NAchievementHolder.MethodName.SetDateLabel || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002108 RID: 8456 RVA: 0x000DFBF0 File Offset: 0x000DDDF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAchievementHolder.PropertyName.IsUnlocked)
			{
				this.IsUnlocked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._border)
			{
				this._border = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._lock)
			{
				this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._iconHsv)
			{
				this._iconHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._borderHsv)
			{
				this._borderHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._date)
			{
				this._date = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._achievement)
			{
				this._achievement = VariantUtils.ConvertTo<Achievement>(ref value);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002109 RID: 8457 RVA: 0x000DFD14 File Offset: 0x000DDF14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAchievementHolder.PropertyName.IsUnlocked)
			{
				bool isUnlocked = this.IsUnlocked;
				value = VariantUtils.CreateFrom<bool>(ref isUnlocked);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._border)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._border);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._lock)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._iconHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._iconHsv);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._borderHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._borderHsv);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._date)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._date);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._achievement)
			{
				value = VariantUtils.CreateFrom<Achievement>(ref this._achievement);
				return true;
			}
			if ((ref name) == NAchievementHolder.PropertyName._tween)
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
		// Token: 0x0600210A RID: 8458 RVA: 0x000DFE6C File Offset: 0x000DE06C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAchievementHolder.PropertyName._border, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._lock, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._iconHsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._borderHsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._date, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAchievementHolder.PropertyName.IsUnlocked, 0L, "", 4096L, false),
				new PropertyInfo(2L, NAchievementHolder.PropertyName._achievement, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementHolder.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600210B RID: 8459 RVA: 0x000DFFC8 File Offset: 0x000DE1C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isUnlocked = NAchievementHolder.PropertyName.IsUnlocked;
			bool isUnlocked2 = this.IsUnlocked;
			info.AddProperty(isUnlocked, Variant.From<bool>(ref isUnlocked2));
			info.AddProperty(NAchievementHolder.PropertyName._border, Variant.From<TextureRect>(ref this._border));
			info.AddProperty(NAchievementHolder.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NAchievementHolder.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
			info.AddProperty(NAchievementHolder.PropertyName._iconHsv, Variant.From<ShaderMaterial>(ref this._iconHsv));
			info.AddProperty(NAchievementHolder.PropertyName._borderHsv, Variant.From<ShaderMaterial>(ref this._borderHsv));
			info.AddProperty(NAchievementHolder.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NAchievementHolder.PropertyName._date, Variant.From<MegaLabel>(ref this._date));
			info.AddProperty(NAchievementHolder.PropertyName._achievement, Variant.From<Achievement>(ref this._achievement));
			info.AddProperty(NAchievementHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600210C RID: 8460 RVA: 0x000E00BC File Offset: 0x000DE2BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAchievementHolder.PropertyName.IsUnlocked, ref variant))
			{
				this.IsUnlocked = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._border, ref variant2))
			{
				this._border = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._lock, ref variant4))
			{
				this._lock = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._iconHsv, ref variant5))
			{
				this._iconHsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._borderHsv, ref variant6))
			{
				this._borderHsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._infoLabel, ref variant7))
			{
				this._infoLabel = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._date, ref variant8))
			{
				this._date = variant8.As<MegaLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._achievement, ref variant9))
			{
				this._achievement = variant9.As<Achievement>();
			}
			Variant variant10;
			if (info.TryGetProperty(NAchievementHolder.PropertyName._tween, ref variant10))
			{
				this._tween = variant10.As<Tween>();
			}
		}

		// Token: 0x04000CD3 RID: 3283
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000CD4 RID: 3284
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000CD5 RID: 3285
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04000CD6 RID: 3286
		private const string _scenePath = "screens/stats_screen/achievement_holder";

		// Token: 0x04000CD7 RID: 3287
		private TextureRect _border;

		// Token: 0x04000CD8 RID: 3288
		private TextureRect _icon;

		// Token: 0x04000CD9 RID: 3289
		private TextureRect _lock;

		// Token: 0x04000CDA RID: 3290
		private ShaderMaterial _iconHsv;

		// Token: 0x04000CDB RID: 3291
		private ShaderMaterial _borderHsv;

		// Token: 0x04000CDC RID: 3292
		private MegaRichTextLabel _infoLabel;

		// Token: 0x04000CDD RID: 3293
		private MegaLabel _date;

		// Token: 0x04000CDF RID: 3295
		private Achievement _achievement;

		// Token: 0x04000CE0 RID: 3296
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F4 RID: 4852
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040049C0 RID: 18880
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049C1 RID: 18881
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshUnlocked' method.
			/// </summary>
			// Token: 0x040049C2 RID: 18882
			public static readonly StringName RefreshUnlocked = "RefreshUnlocked";

			/// <summary>
			/// Cached name for the 'SetLockVisuals' method.
			/// </summary>
			// Token: 0x040049C3 RID: 18883
			public static readonly StringName SetLockVisuals = "SetLockVisuals";

			/// <summary>
			/// Cached name for the 'SetDateLabel' method.
			/// </summary>
			// Token: 0x040049C4 RID: 18884
			public static readonly StringName SetDateLabel = "SetDateLabel";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F5 RID: 4853
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsUnlocked' property.
			/// </summary>
			// Token: 0x040049C5 RID: 18885
			public static readonly StringName IsUnlocked = "IsUnlocked";

			/// <summary>
			/// Cached name for the '_border' field.
			/// </summary>
			// Token: 0x040049C6 RID: 18886
			public static readonly StringName _border = "_border";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040049C7 RID: 18887
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_lock' field.
			/// </summary>
			// Token: 0x040049C8 RID: 18888
			public static readonly StringName _lock = "_lock";

			/// <summary>
			/// Cached name for the '_iconHsv' field.
			/// </summary>
			// Token: 0x040049C9 RID: 18889
			public static readonly StringName _iconHsv = "_iconHsv";

			/// <summary>
			/// Cached name for the '_borderHsv' field.
			/// </summary>
			// Token: 0x040049CA RID: 18890
			public static readonly StringName _borderHsv = "_borderHsv";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x040049CB RID: 18891
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_date' field.
			/// </summary>
			// Token: 0x040049CC RID: 18892
			public static readonly StringName _date = "_date";

			/// <summary>
			/// Cached name for the '_achievement' field.
			/// </summary>
			// Token: 0x040049CD RID: 18893
			public static readonly StringName _achievement = "_achievement";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040049CE RID: 18894
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F6 RID: 4854
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
