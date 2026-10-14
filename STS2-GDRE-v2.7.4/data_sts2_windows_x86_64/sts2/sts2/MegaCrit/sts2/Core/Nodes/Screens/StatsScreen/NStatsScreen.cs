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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000277 RID: 631
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatsScreen.cs")]
	public class NStatsScreen : NSubmenu
	{
		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x000E3340 File Offset: 0x000E1540
		public static string[] AssetPaths
		{
			get
			{
				string scenePath = NStatsScreen._scenePath;
				string[] assetPaths = NGeneralStatsGrid.AssetPaths;
				int num = 0;
				string[] array = new string[1 + assetPaths.Length];
				array[num] = scenePath;
				num++;
				ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(assetPaths);
				readOnlySpan.CopyTo(new Span<string>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				return array;
			}
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000E339F File Offset: 0x000E159F
		[NullableContext(2)]
		public static NStatsScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NStatsScreen._scenePath).Instantiate<NStatsScreen>(0L);
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000E33C0 File Offset: 0x000E15C0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._statsTab = base.GetNode<NSettingsTab>("%StatsTab");
			this._statsTab.SetLabel(new LocString("stats_screen", "TAB_STATS.header").GetFormattedText());
			this._achievementsTab = base.GetNode<NSettingsTab>("%Achievements");
			this._achievementsTab.SetLabel(new LocString("stats_screen", "TAB_ACHIEVEMENT.header").GetFormattedText());
			this._statsTab.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				this.OpenStatsMenu();
			}), 0U);
			this._statsTabManager = base.GetNode<NStatsTabManager>("%Tabs");
			this._statsGrid = base.GetNode<NGeneralStatsGrid>("%StatsGrid");
			base.GetNode<MegaLabel>("%OverallStatsHeader").SetTextAutoSize(new LocString("main_menu_ui", "STATISTICS.OVERALL.title").GetFormattedText());
			base.GetNode<MegaLabel>("%CharacterStatsHeader").SetTextAutoSize(new LocString("main_menu_ui", "STATISTICS.title").GetFormattedText());
			this._achievementsTab.Disable();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000E34EC File Offset: 0x000E16EC
		public override void OnSubmenuOpened()
		{
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween();
			this._screenTween.TweenProperty(this, "modulate:a", 1f, 0.4).From(0f);
			base.Visible = true;
			this.OpenStatsMenu();
			this._statsTabManager.ResetTabs();
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000E3567 File Offset: 0x000E1767
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._statsGrid.DefaultFocusedControl;
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000E3574 File Offset: 0x000E1774
		private void OpenStatsMenu()
		{
			this._statsGrid.Visible = true;
			this._statsGrid.LoadStats();
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000E3597 File Offset: 0x000E1797
		private void OpenAchievementsMenu()
		{
			this._statsGrid.Visible = false;
			ActiveScreenContext.Instance.Update();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002169 RID: 8553 RVA: 0x000E35B0 File Offset: 0x000E17B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NStatsScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NStatsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatsScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatsScreen.MethodName.OpenStatsMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatsScreen.MethodName.OpenAchievementsMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600216A RID: 8554 RVA: 0x000E36BC File Offset: 0x000E18BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStatsScreen.MethodName.Create && args.Count == 0)
			{
				NStatsScreen nstatsScreen = NStatsScreen.Create();
				ret = VariantUtils.CreateFrom<NStatsScreen>(ref nstatsScreen);
				return true;
			}
			if ((ref method) == NStatsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsScreen.MethodName.OpenStatsMenu && args.Count == 0)
			{
				this.OpenStatsMenu();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsScreen.MethodName.OpenAchievementsMenu && args.Count == 0)
			{
				this.OpenAchievementsMenu();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000E3794 File Offset: 0x000E1994
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStatsScreen.MethodName.Create && args.Count == 0)
			{
				NStatsScreen nstatsScreen = NStatsScreen.Create();
				ret = VariantUtils.CreateFrom<NStatsScreen>(ref nstatsScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600216C RID: 8556 RVA: 0x000E37D4 File Offset: 0x000E19D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStatsScreen.MethodName.Create || (ref method) == NStatsScreen.MethodName._Ready || (ref method) == NStatsScreen.MethodName.OnSubmenuOpened || (ref method) == NStatsScreen.MethodName.OpenStatsMenu || (ref method) == NStatsScreen.MethodName.OpenAchievementsMenu || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600216D RID: 8557 RVA: 0x000E3834 File Offset: 0x000E1A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStatsScreen.PropertyName._statsTabManager)
			{
				this._statsTabManager = VariantUtils.ConvertTo<NStatsTabManager>(ref value);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._statsTab)
			{
				this._statsTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._achievementsTab)
			{
				this._achievementsTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._statsGrid)
			{
				this._statsGrid = VariantUtils.ConvertTo<NGeneralStatsGrid>(ref value);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600216E RID: 8558 RVA: 0x000E38D0 File Offset: 0x000E1AD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStatsScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._statsTabManager)
			{
				value = VariantUtils.CreateFrom<NStatsTabManager>(ref this._statsTabManager);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._statsTab)
			{
				value = VariantUtils.CreateFrom<NSettingsTab>(ref this._statsTab);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._achievementsTab)
			{
				value = VariantUtils.CreateFrom<NSettingsTab>(ref this._achievementsTab);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._statsGrid)
			{
				value = VariantUtils.CreateFrom<NGeneralStatsGrid>(ref this._statsGrid);
				return true;
			}
			if ((ref name) == NStatsScreen.PropertyName._screenTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600216F RID: 8559 RVA: 0x000E39A8 File Offset: 0x000E1BA8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStatsScreen.PropertyName._statsTabManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsScreen.PropertyName._statsTab, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsScreen.PropertyName._achievementsTab, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsScreen.PropertyName._statsGrid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsScreen.PropertyName._screenTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002170 RID: 8560 RVA: 0x000E3A84 File Offset: 0x000E1C84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStatsScreen.PropertyName._statsTabManager, Variant.From<NStatsTabManager>(ref this._statsTabManager));
			info.AddProperty(NStatsScreen.PropertyName._statsTab, Variant.From<NSettingsTab>(ref this._statsTab));
			info.AddProperty(NStatsScreen.PropertyName._achievementsTab, Variant.From<NSettingsTab>(ref this._achievementsTab));
			info.AddProperty(NStatsScreen.PropertyName._statsGrid, Variant.From<NGeneralStatsGrid>(ref this._statsGrid));
			info.AddProperty(NStatsScreen.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
		}

		/// <inheritdoc />
		// Token: 0x06002171 RID: 8561 RVA: 0x000E3B08 File Offset: 0x000E1D08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStatsScreen.PropertyName._statsTabManager, ref variant))
			{
				this._statsTabManager = variant.As<NStatsTabManager>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStatsScreen.PropertyName._statsTab, ref variant2))
			{
				this._statsTab = variant2.As<NSettingsTab>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStatsScreen.PropertyName._achievementsTab, ref variant3))
			{
				this._achievementsTab = variant3.As<NSettingsTab>();
			}
			Variant variant4;
			if (info.TryGetProperty(NStatsScreen.PropertyName._statsGrid, ref variant4))
			{
				this._statsGrid = variant4.As<NGeneralStatsGrid>();
			}
			Variant variant5;
			if (info.TryGetProperty(NStatsScreen.PropertyName._screenTween, ref variant5))
			{
				this._screenTween = variant5.As<Tween>();
			}
		}

		// Token: 0x04000D0F RID: 3343
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/stats_screen/stats_screen");

		// Token: 0x04000D10 RID: 3344
		private NStatsTabManager _statsTabManager;

		// Token: 0x04000D11 RID: 3345
		private NSettingsTab _statsTab;

		// Token: 0x04000D12 RID: 3346
		private NSettingsTab _achievementsTab;

		// Token: 0x04000D13 RID: 3347
		private NGeneralStatsGrid _statsGrid;

		// Token: 0x04000D14 RID: 3348
		[Nullable(2)]
		private Tween _screenTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130A RID: 4874
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004A0E RID: 18958
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A0F RID: 18959
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004A10 RID: 18960
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OpenStatsMenu' method.
			/// </summary>
			// Token: 0x04004A11 RID: 18961
			public static readonly StringName OpenStatsMenu = "OpenStatsMenu";

			/// <summary>
			/// Cached name for the 'OpenAchievementsMenu' method.
			/// </summary>
			// Token: 0x04004A12 RID: 18962
			public static readonly StringName OpenAchievementsMenu = "OpenAchievementsMenu";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130B RID: 4875
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004A13 RID: 18963
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_statsTabManager' field.
			/// </summary>
			// Token: 0x04004A14 RID: 18964
			public static readonly StringName _statsTabManager = "_statsTabManager";

			/// <summary>
			/// Cached name for the '_statsTab' field.
			/// </summary>
			// Token: 0x04004A15 RID: 18965
			public static readonly StringName _statsTab = "_statsTab";

			/// <summary>
			/// Cached name for the '_achievementsTab' field.
			/// </summary>
			// Token: 0x04004A16 RID: 18966
			public static readonly StringName _achievementsTab = "_achievementsTab";

			/// <summary>
			/// Cached name for the '_statsGrid' field.
			/// </summary>
			// Token: 0x04004A17 RID: 18967
			public static readonly StringName _statsGrid = "_statsGrid";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x04004A18 RID: 18968
			public static readonly StringName _screenTween = "_screenTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130C RID: 4876
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
