using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// Manages spawned submenus during a run.
	/// We lazily spawn submenus only when they are requested to reduce loading times.
	/// </summary>
	// Token: 0x02000250 RID: 592
	[NullableContext(2)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NRunSubmenuStack.cs")]
	public class NRunSubmenuStack : NSubmenuStack
	{
		// Token: 0x06001EA3 RID: 7843 RVA: 0x000CA2B1 File Offset: 0x000C84B1
		public override void _Ready()
		{
			this.GetSubmenuType<NSettingsScreen>();
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000CA2BA File Offset: 0x000C84BA
		[NullableContext(1)]
		public override T PushSubmenuType<[Nullable(0)] T>()
		{
			return (T)((object)this.PushSubmenuType(typeof(T)));
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000CA2D1 File Offset: 0x000C84D1
		[NullableContext(1)]
		public override T GetSubmenuType<[Nullable(0)] T>()
		{
			return (T)((object)this.GetSubmenuType(typeof(T)));
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000CA2E8 File Offset: 0x000C84E8
		[NullableContext(1)]
		public override NSubmenu PushSubmenuType(Type type)
		{
			NSubmenu submenuType = this.GetSubmenuType(type);
			base.Push(submenuType);
			return submenuType;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000CA308 File Offset: 0x000C8508
		[NullableContext(1)]
		public override NSubmenu GetSubmenuType(Type type)
		{
			if (type == typeof(NCompendiumSubmenu))
			{
				if (this._compendiumSubmenu == null)
				{
					this._compendiumSubmenu = NCompendiumSubmenu.Create();
					this._compendiumSubmenu.Visible = false;
					this.AddChildSafely(this._compendiumSubmenu);
				}
				return this._compendiumSubmenu;
			}
			if (type == typeof(NBestiary))
			{
				if (this._bestiarySubmenu == null)
				{
					this._bestiarySubmenu = NBestiary.Create();
					this._bestiarySubmenu.Visible = false;
					this.AddChildSafely(this._bestiarySubmenu);
				}
				return this._bestiarySubmenu;
			}
			if (type == typeof(NRelicCollection))
			{
				if (this._relicCollectionSubmenu == null)
				{
					this._relicCollectionSubmenu = NRelicCollection.Create();
					this._relicCollectionSubmenu.Visible = false;
					this.AddChildSafely(this._relicCollectionSubmenu);
				}
				return this._relicCollectionSubmenu;
			}
			if (type == typeof(NPotionLab))
			{
				if (this._potionLabSubmenu == null)
				{
					this._potionLabSubmenu = NPotionLab.Create();
					this._potionLabSubmenu.Visible = false;
					this.AddChildSafely(this._potionLabSubmenu);
				}
				return this._potionLabSubmenu;
			}
			if (type == typeof(NCardLibrary))
			{
				if (this._cardLibrarySubmenu == null)
				{
					this._cardLibrarySubmenu = NCardLibrary.Create();
					this._cardLibrarySubmenu.Visible = false;
					this.AddChildSafely(this._cardLibrarySubmenu);
				}
				return this._cardLibrarySubmenu;
			}
			if (type == typeof(NRunHistory))
			{
				if (this._runHistoryScreen == null)
				{
					this._runHistoryScreen = this._runHistoryScreenScene.Instantiate<NRunHistory>(0L);
					this._runHistoryScreen.Visible = false;
					this.AddChildSafely(this._runHistoryScreen);
				}
				return this._runHistoryScreen;
			}
			if (type == typeof(NSettingsScreen))
			{
				if (this._settingsScreen == null)
				{
					this._settingsScreen = this._settingsScreenScene.Instantiate<NSettingsScreen>(0L);
					this._settingsScreen.SetIsInRun(true);
					this._settingsScreen.Visible = false;
					this.AddChildSafely(this._settingsScreen);
				}
				return this._settingsScreen;
			}
			if (type == typeof(NStatsScreen))
			{
				if (this._statsScreen == null)
				{
					this._statsScreen = this._statsScreenScene.Instantiate<NStatsScreen>(0L);
					this._statsScreen.Visible = false;
					this.AddChildSafely(this._statsScreen);
				}
				return this._statsScreen;
			}
			if (type == typeof(NPauseMenu))
			{
				if (this._pauseMenu == null)
				{
					this._pauseMenu = this._pauseMenuScene.Instantiate<NPauseMenu>(0L);
					this._pauseMenu.Visible = false;
					this.AddChildSafely(this._pauseMenu);
				}
				return this._pauseMenu;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("No such submenu of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
			defaultInterpolatedStringHandler.AppendLiteral(" in run");
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001EA8 RID: 7848 RVA: 0x000CA5DC File Offset: 0x000C87DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NRunSubmenuStack.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EA9 RID: 7849 RVA: 0x000CA620 File Offset: 0x000C8820
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunSubmenuStack.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001EAA RID: 7850 RVA: 0x000CA650 File Offset: 0x000C8850
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunSubmenuStack.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001EAB RID: 7851 RVA: 0x000CA668 File Offset: 0x000C8868
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunSubmenuStack.PropertyName._settingsScreenScene)
			{
				this._settingsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._pauseMenuScene)
			{
				this._pauseMenuScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._statsScreenScene)
			{
				this._statsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._runHistoryScreenScene)
			{
				this._runHistoryScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._compendiumSubmenu)
			{
				this._compendiumSubmenu = VariantUtils.ConvertTo<NCompendiumSubmenu>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._bestiarySubmenu)
			{
				this._bestiarySubmenu = VariantUtils.ConvertTo<NBestiary>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._relicCollectionSubmenu)
			{
				this._relicCollectionSubmenu = VariantUtils.ConvertTo<NRelicCollection>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._potionLabSubmenu)
			{
				this._potionLabSubmenu = VariantUtils.ConvertTo<NPotionLab>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._cardLibrarySubmenu)
			{
				this._cardLibrarySubmenu = VariantUtils.ConvertTo<NCardLibrary>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._runHistoryScreen)
			{
				this._runHistoryScreen = VariantUtils.ConvertTo<NRunHistory>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._statsScreen)
			{
				this._statsScreen = VariantUtils.ConvertTo<NStatsScreen>(ref value);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._pauseMenu)
			{
				this._pauseMenu = VariantUtils.ConvertTo<NPauseMenu>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001EAC RID: 7852 RVA: 0x000CA7DC File Offset: 0x000C89DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunSubmenuStack.PropertyName._settingsScreenScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._settingsScreenScene);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._pauseMenuScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._pauseMenuScene);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._statsScreenScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._statsScreenScene);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._runHistoryScreenScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._runHistoryScreenScene);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._compendiumSubmenu)
			{
				value = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref this._compendiumSubmenu);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._bestiarySubmenu)
			{
				value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiarySubmenu);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._relicCollectionSubmenu)
			{
				value = VariantUtils.CreateFrom<NRelicCollection>(ref this._relicCollectionSubmenu);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._potionLabSubmenu)
			{
				value = VariantUtils.CreateFrom<NPotionLab>(ref this._potionLabSubmenu);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._cardLibrarySubmenu)
			{
				value = VariantUtils.CreateFrom<NCardLibrary>(ref this._cardLibrarySubmenu);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._runHistoryScreen)
			{
				value = VariantUtils.CreateFrom<NRunHistory>(ref this._runHistoryScreen);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._settingsScreen)
			{
				value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._statsScreen)
			{
				value = VariantUtils.CreateFrom<NStatsScreen>(ref this._statsScreen);
				return true;
			}
			if ((ref name) == NRunSubmenuStack.PropertyName._pauseMenu)
			{
				value = VariantUtils.CreateFrom<NPauseMenu>(ref this._pauseMenu);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001EAD RID: 7853 RVA: 0x000CA994 File Offset: 0x000C8B94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._settingsScreenScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._pauseMenuScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._statsScreenScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._runHistoryScreenScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._compendiumSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._bestiarySubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._relicCollectionSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._potionLabSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._cardLibrarySubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._runHistoryScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._settingsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._statsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSubmenuStack.PropertyName._pauseMenu, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EAE RID: 7854 RVA: 0x000CAB5C File Offset: 0x000C8D5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunSubmenuStack.PropertyName._settingsScreenScene, Variant.From<PackedScene>(ref this._settingsScreenScene));
			info.AddProperty(NRunSubmenuStack.PropertyName._pauseMenuScene, Variant.From<PackedScene>(ref this._pauseMenuScene));
			info.AddProperty(NRunSubmenuStack.PropertyName._statsScreenScene, Variant.From<PackedScene>(ref this._statsScreenScene));
			info.AddProperty(NRunSubmenuStack.PropertyName._runHistoryScreenScene, Variant.From<PackedScene>(ref this._runHistoryScreenScene));
			info.AddProperty(NRunSubmenuStack.PropertyName._compendiumSubmenu, Variant.From<NCompendiumSubmenu>(ref this._compendiumSubmenu));
			info.AddProperty(NRunSubmenuStack.PropertyName._bestiarySubmenu, Variant.From<NBestiary>(ref this._bestiarySubmenu));
			info.AddProperty(NRunSubmenuStack.PropertyName._relicCollectionSubmenu, Variant.From<NRelicCollection>(ref this._relicCollectionSubmenu));
			info.AddProperty(NRunSubmenuStack.PropertyName._potionLabSubmenu, Variant.From<NPotionLab>(ref this._potionLabSubmenu));
			info.AddProperty(NRunSubmenuStack.PropertyName._cardLibrarySubmenu, Variant.From<NCardLibrary>(ref this._cardLibrarySubmenu));
			info.AddProperty(NRunSubmenuStack.PropertyName._runHistoryScreen, Variant.From<NRunHistory>(ref this._runHistoryScreen));
			info.AddProperty(NRunSubmenuStack.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
			info.AddProperty(NRunSubmenuStack.PropertyName._statsScreen, Variant.From<NStatsScreen>(ref this._statsScreen));
			info.AddProperty(NRunSubmenuStack.PropertyName._pauseMenu, Variant.From<NPauseMenu>(ref this._pauseMenu));
		}

		/// <inheritdoc />
		// Token: 0x06001EAF RID: 7855 RVA: 0x000CAC90 File Offset: 0x000C8E90
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._settingsScreenScene, ref variant))
			{
				this._settingsScreenScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._pauseMenuScene, ref variant2))
			{
				this._pauseMenuScene = variant2.As<PackedScene>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._statsScreenScene, ref variant3))
			{
				this._statsScreenScene = variant3.As<PackedScene>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._runHistoryScreenScene, ref variant4))
			{
				this._runHistoryScreenScene = variant4.As<PackedScene>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._compendiumSubmenu, ref variant5))
			{
				this._compendiumSubmenu = variant5.As<NCompendiumSubmenu>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._bestiarySubmenu, ref variant6))
			{
				this._bestiarySubmenu = variant6.As<NBestiary>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._relicCollectionSubmenu, ref variant7))
			{
				this._relicCollectionSubmenu = variant7.As<NRelicCollection>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._potionLabSubmenu, ref variant8))
			{
				this._potionLabSubmenu = variant8.As<NPotionLab>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._cardLibrarySubmenu, ref variant9))
			{
				this._cardLibrarySubmenu = variant9.As<NCardLibrary>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._runHistoryScreen, ref variant10))
			{
				this._runHistoryScreen = variant10.As<NRunHistory>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._settingsScreen, ref variant11))
			{
				this._settingsScreen = variant11.As<NSettingsScreen>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._statsScreen, ref variant12))
			{
				this._statsScreen = variant12.As<NStatsScreen>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRunSubmenuStack.PropertyName._pauseMenu, ref variant13))
			{
				this._pauseMenu = variant13.As<NPauseMenu>();
			}
		}

		// Token: 0x04000BD1 RID: 3025
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _settingsScreenScene;

		// Token: 0x04000BD2 RID: 3026
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _pauseMenuScene;

		// Token: 0x04000BD3 RID: 3027
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _statsScreenScene;

		// Token: 0x04000BD4 RID: 3028
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _runHistoryScreenScene;

		// Token: 0x04000BD5 RID: 3029
		private NCompendiumSubmenu _compendiumSubmenu;

		// Token: 0x04000BD6 RID: 3030
		private NBestiary _bestiarySubmenu;

		// Token: 0x04000BD7 RID: 3031
		private NRelicCollection _relicCollectionSubmenu;

		// Token: 0x04000BD8 RID: 3032
		private NPotionLab _potionLabSubmenu;

		// Token: 0x04000BD9 RID: 3033
		private NCardLibrary _cardLibrarySubmenu;

		// Token: 0x04000BDA RID: 3034
		private NRunHistory _runHistoryScreen;

		// Token: 0x04000BDB RID: 3035
		private NSettingsScreen _settingsScreen;

		// Token: 0x04000BDC RID: 3036
		private NStatsScreen _statsScreen;

		// Token: 0x04000BDD RID: 3037
		private NPauseMenu _pauseMenu;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001270 RID: 4720
		[NullableContext(0)]
		public new class MethodName : NSubmenuStack.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004790 RID: 18320
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001271 RID: 4721
		[NullableContext(0)]
		public new class PropertyName : NSubmenuStack.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreenScene' field.
			/// </summary>
			// Token: 0x04004791 RID: 18321
			public static readonly StringName _settingsScreenScene = "_settingsScreenScene";

			/// <summary>
			/// Cached name for the '_pauseMenuScene' field.
			/// </summary>
			// Token: 0x04004792 RID: 18322
			public static readonly StringName _pauseMenuScene = "_pauseMenuScene";

			/// <summary>
			/// Cached name for the '_statsScreenScene' field.
			/// </summary>
			// Token: 0x04004793 RID: 18323
			public static readonly StringName _statsScreenScene = "_statsScreenScene";

			/// <summary>
			/// Cached name for the '_runHistoryScreenScene' field.
			/// </summary>
			// Token: 0x04004794 RID: 18324
			public static readonly StringName _runHistoryScreenScene = "_runHistoryScreenScene";

			/// <summary>
			/// Cached name for the '_compendiumSubmenu' field.
			/// </summary>
			// Token: 0x04004795 RID: 18325
			public static readonly StringName _compendiumSubmenu = "_compendiumSubmenu";

			/// <summary>
			/// Cached name for the '_bestiarySubmenu' field.
			/// </summary>
			// Token: 0x04004796 RID: 18326
			public static readonly StringName _bestiarySubmenu = "_bestiarySubmenu";

			/// <summary>
			/// Cached name for the '_relicCollectionSubmenu' field.
			/// </summary>
			// Token: 0x04004797 RID: 18327
			public static readonly StringName _relicCollectionSubmenu = "_relicCollectionSubmenu";

			/// <summary>
			/// Cached name for the '_potionLabSubmenu' field.
			/// </summary>
			// Token: 0x04004798 RID: 18328
			public static readonly StringName _potionLabSubmenu = "_potionLabSubmenu";

			/// <summary>
			/// Cached name for the '_cardLibrarySubmenu' field.
			/// </summary>
			// Token: 0x04004799 RID: 18329
			public static readonly StringName _cardLibrarySubmenu = "_cardLibrarySubmenu";

			/// <summary>
			/// Cached name for the '_runHistoryScreen' field.
			/// </summary>
			// Token: 0x0400479A RID: 18330
			public static readonly StringName _runHistoryScreen = "_runHistoryScreen";

			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x0400479B RID: 18331
			public static readonly StringName _settingsScreen = "_settingsScreen";

			/// <summary>
			/// Cached name for the '_statsScreen' field.
			/// </summary>
			// Token: 0x0400479C RID: 18332
			public static readonly StringName _statsScreen = "_statsScreen";

			/// <summary>
			/// Cached name for the '_pauseMenu' field.
			/// </summary>
			// Token: 0x0400479D RID: 18333
			public static readonly StringName _pauseMenu = "_pauseMenu";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001272 RID: 4722
		[NullableContext(0)]
		public new class SignalName : NSubmenuStack.SignalName
		{
		}
	}
}
