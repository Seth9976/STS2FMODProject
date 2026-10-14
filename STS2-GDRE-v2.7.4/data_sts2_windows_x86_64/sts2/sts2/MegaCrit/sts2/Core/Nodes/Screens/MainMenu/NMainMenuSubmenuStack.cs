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
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Manages spawned submenus in the main menu.
	/// We lazily spawn submenus only when they are requested to reduce loading times.
	/// </summary>
	// Token: 0x0200030A RID: 778
	[NullableContext(2)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuSubmenuStack.cs")]
	public class NMainMenuSubmenuStack : NSubmenuStack
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x0012CEF4 File Offset: 0x0012B0F4
		public override void _Ready()
		{
			this.GetSubmenuType<NSettingsScreen>();
			this.GetSubmenuType<NCharacterSelectScreen>();
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x0012CF04 File Offset: 0x0012B104
		[NullableContext(1)]
		public override T PushSubmenuType<[Nullable(0)] T>()
		{
			return (T)((object)this.PushSubmenuType(typeof(T)));
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0012CF1B File Offset: 0x0012B11B
		[NullableContext(1)]
		public override T GetSubmenuType<[Nullable(0)] T>()
		{
			return (T)((object)this.GetSubmenuType(typeof(T)));
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0012CF34 File Offset: 0x0012B134
		[NullableContext(1)]
		public override NSubmenu PushSubmenuType(Type type)
		{
			NSubmenu submenuType = this.GetSubmenuType(type);
			base.Push(submenuType);
			return submenuType;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x0012CF54 File Offset: 0x0012B154
		[NullableContext(1)]
		public override NSubmenu GetSubmenuType(Type type)
		{
			if (type == typeof(NSingleplayerSubmenu))
			{
				if (this._singleplayerSubmenu == null)
				{
					this._singleplayerSubmenu = NSingleplayerSubmenu.Create();
					this._singleplayerSubmenu.Visible = false;
					this.AddChildSafely(this._singleplayerSubmenu);
				}
				return this._singleplayerSubmenu;
			}
			if (type == typeof(NMultiplayerSubmenu))
			{
				if (this._multiplayerSubmenu == null)
				{
					this._multiplayerSubmenu = NMultiplayerSubmenu.Create();
					this._multiplayerSubmenu.Visible = false;
					this.AddChildSafely(this._multiplayerSubmenu);
				}
				return this._multiplayerSubmenu;
			}
			if (type == typeof(NMultiplayerHostSubmenu))
			{
				if (this._multiplayerHostSubmenu == null)
				{
					this._multiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
					this._multiplayerHostSubmenu.Visible = false;
					this.AddChildSafely(this._multiplayerHostSubmenu);
				}
				return this._multiplayerHostSubmenu;
			}
			if (type == typeof(NJoinFriendScreen))
			{
				if (this._joinFriendSubmenu == null)
				{
					this._joinFriendSubmenu = NJoinFriendScreen.Create();
					this._joinFriendSubmenu.Visible = false;
					this.AddChildSafely(this._joinFriendSubmenu);
				}
				return this._joinFriendSubmenu;
			}
			if (type == typeof(NCharacterSelectScreen))
			{
				if (this._characterSelectSubmenu == null)
				{
					this._characterSelectSubmenu = this._characterSelectScreenScene.Instantiate<NCharacterSelectScreen>(0L);
					this._characterSelectSubmenu.Visible = false;
					this.AddChildSafely(this._characterSelectSubmenu);
				}
				return this._characterSelectSubmenu;
			}
			if (type == typeof(NMultiplayerLoadGameScreen))
			{
				if (this._loadMultiplayerSubmenu == null)
				{
					this._loadMultiplayerSubmenu = NMultiplayerLoadGameScreen.Create();
					this._loadMultiplayerSubmenu.Visible = false;
					this.AddChildSafely(this._loadMultiplayerSubmenu);
				}
				return this._loadMultiplayerSubmenu;
			}
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
			if (type == typeof(NMultiplayerHostSubmenu))
			{
				if (this._multiplayerHostSubmenu == null)
				{
					this._multiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
					this._multiplayerHostSubmenu.Visible = false;
					this.AddChildSafely(this._multiplayerHostSubmenu);
				}
				return this._multiplayerHostSubmenu;
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
				if (this._runHistorySubmenu == null)
				{
					this._runHistorySubmenu = NRunHistory.Create();
					this._runHistorySubmenu.Visible = false;
					this.AddChildSafely(this._runHistorySubmenu);
				}
				return this._runHistorySubmenu;
			}
			if (type == typeof(NStatsScreen))
			{
				if (this._statsScreen == null)
				{
					this._statsScreen = NStatsScreen.Create();
					this._statsScreen.Visible = false;
					this.AddChildSafely(this._statsScreen);
				}
				return this._statsScreen;
			}
			if (type == typeof(NTimelineScreen))
			{
				if (this._timelineScreen == null)
				{
					this._timelineScreen = NTimelineScreen.Create();
					this._timelineScreen.Visible = false;
					this.AddChildSafely(this._timelineScreen);
				}
				return this._timelineScreen;
			}
			if (type == typeof(NSettingsScreen))
			{
				if (this._settingsScreen == null)
				{
					this._settingsScreen = this._settingsScreenScene.Instantiate<NSettingsScreen>(0L);
					this._settingsScreen.SetIsInRun(false);
					this._settingsScreen.Visible = false;
					this.AddChildSafely(this._settingsScreen);
				}
				return this._settingsScreen;
			}
			if (type == typeof(NDailyRunScreen))
			{
				if (this._dailyScreen == null)
				{
					this._dailyScreen = NDailyRunScreen.Create();
					this._dailyScreen.Visible = false;
					this.AddChildSafely(this._dailyScreen);
				}
				return this._dailyScreen;
			}
			if (type == typeof(NDailyRunLoadScreen))
			{
				if (this._dailyLoadScreen == null)
				{
					this._dailyLoadScreen = NDailyRunLoadScreen.Create();
					this._dailyLoadScreen.Visible = false;
					this.AddChildSafely(this._dailyLoadScreen);
				}
				return this._dailyLoadScreen;
			}
			if (type == typeof(NCustomRunScreen))
			{
				if (this._customRunScreen == null)
				{
					this._customRunScreen = NCustomRunScreen.Create();
					this._customRunScreen.Visible = false;
					this.AddChildSafely(this._customRunScreen);
				}
				return this._customRunScreen;
			}
			if (type == typeof(NCustomRunLoadScreen))
			{
				if (this._customRunLoadScreen == null)
				{
					this._customRunLoadScreen = NCustomRunLoadScreen.Create();
					this._customRunLoadScreen.Visible = false;
					this.AddChildSafely(this._customRunLoadScreen);
				}
				return this._customRunLoadScreen;
			}
			if (type == typeof(NModdingScreen))
			{
				if (this._moddingScreen == null)
				{
					this._moddingScreen = NModdingScreen.Create();
					this._moddingScreen.Visible = false;
					this.AddChildSafely(this._moddingScreen);
				}
				return this._moddingScreen;
			}
			if (type == typeof(NProfileScreen))
			{
				if (this._profileScreen == null)
				{
					this._profileScreen = NProfileScreen.Create();
					this._profileScreen.Visible = false;
					this.AddChildSafely(this._profileScreen);
				}
				return this._profileScreen;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("No such submenu ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
			defaultInterpolatedStringHandler.AppendLiteral(" in main menu");
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AE0 RID: 10976 RVA: 0x0012D58C File Offset: 0x0012B78C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMainMenuSubmenuStack.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AE1 RID: 10977 RVA: 0x0012D5D0 File Offset: 0x0012B7D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenuSubmenuStack.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002AE2 RID: 10978 RVA: 0x0012D600 File Offset: 0x0012B800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMainMenuSubmenuStack.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002AE3 RID: 10979 RVA: 0x0012D618 File Offset: 0x0012B818
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._settingsScreenScene)
			{
				this._settingsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene)
			{
				this._characterSelectScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu)
			{
				this._singleplayerSubmenu = VariantUtils.ConvertTo<NSingleplayerSubmenu>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu)
			{
				this._multiplayerSubmenu = VariantUtils.ConvertTo<NMultiplayerSubmenu>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu)
			{
				this._multiplayerHostSubmenu = VariantUtils.ConvertTo<NMultiplayerHostSubmenu>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu)
			{
				this._joinFriendSubmenu = VariantUtils.ConvertTo<NJoinFriendScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu)
			{
				this._characterSelectSubmenu = VariantUtils.ConvertTo<NCharacterSelectScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu)
			{
				this._loadMultiplayerSubmenu = VariantUtils.ConvertTo<NMultiplayerLoadGameScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._compendiumSubmenu)
			{
				this._compendiumSubmenu = VariantUtils.ConvertTo<NCompendiumSubmenu>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._bestiarySubmenu)
			{
				this._bestiarySubmenu = VariantUtils.ConvertTo<NBestiary>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu)
			{
				this._relicCollectionSubmenu = VariantUtils.ConvertTo<NRelicCollection>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._potionLabSubmenu)
			{
				this._potionLabSubmenu = VariantUtils.ConvertTo<NPotionLab>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu)
			{
				this._cardLibrarySubmenu = VariantUtils.ConvertTo<NCardLibrary>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._runHistorySubmenu)
			{
				this._runHistorySubmenu = VariantUtils.ConvertTo<NRunHistory>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._statsScreen)
			{
				this._statsScreen = VariantUtils.ConvertTo<NStatsScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._timelineScreen)
			{
				this._timelineScreen = VariantUtils.ConvertTo<NTimelineScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._dailyScreen)
			{
				this._dailyScreen = VariantUtils.ConvertTo<NDailyRunScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._dailyLoadScreen)
			{
				this._dailyLoadScreen = VariantUtils.ConvertTo<NDailyRunLoadScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._customRunScreen)
			{
				this._customRunScreen = VariantUtils.ConvertTo<NCustomRunScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._customRunLoadScreen)
			{
				this._customRunLoadScreen = VariantUtils.ConvertTo<NCustomRunLoadScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._moddingScreen)
			{
				this._moddingScreen = VariantUtils.ConvertTo<NModdingScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._profileScreen)
			{
				this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002AE4 RID: 10980 RVA: 0x0012D89C File Offset: 0x0012BA9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._settingsScreenScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._settingsScreenScene);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._characterSelectScreenScene);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu)
			{
				value = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref this._singleplayerSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu)
			{
				value = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref this._multiplayerSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu)
			{
				value = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref this._multiplayerHostSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu)
			{
				value = VariantUtils.CreateFrom<NJoinFriendScreen>(ref this._joinFriendSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref this._characterSelectSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu)
			{
				value = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref this._loadMultiplayerSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._compendiumSubmenu)
			{
				value = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref this._compendiumSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._bestiarySubmenu)
			{
				value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiarySubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu)
			{
				value = VariantUtils.CreateFrom<NRelicCollection>(ref this._relicCollectionSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._potionLabSubmenu)
			{
				value = VariantUtils.CreateFrom<NPotionLab>(ref this._potionLabSubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu)
			{
				value = VariantUtils.CreateFrom<NCardLibrary>(ref this._cardLibrarySubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._runHistorySubmenu)
			{
				value = VariantUtils.CreateFrom<NRunHistory>(ref this._runHistorySubmenu);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._statsScreen)
			{
				value = VariantUtils.CreateFrom<NStatsScreen>(ref this._statsScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._timelineScreen)
			{
				value = VariantUtils.CreateFrom<NTimelineScreen>(ref this._timelineScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._settingsScreen)
			{
				value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._dailyScreen)
			{
				value = VariantUtils.CreateFrom<NDailyRunScreen>(ref this._dailyScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._dailyLoadScreen)
			{
				value = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref this._dailyLoadScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._customRunScreen)
			{
				value = VariantUtils.CreateFrom<NCustomRunScreen>(ref this._customRunScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._customRunLoadScreen)
			{
				value = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref this._customRunLoadScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._moddingScreen)
			{
				value = VariantUtils.CreateFrom<NModdingScreen>(ref this._moddingScreen);
				return true;
			}
			if ((ref name) == NMainMenuSubmenuStack.PropertyName._profileScreen)
			{
				value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AE5 RID: 10981 RVA: 0x0012DB94 File Offset: 0x0012BD94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._settingsScreenScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._statsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._timelineScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._settingsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._dailyScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._customRunScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._moddingScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuSubmenuStack.PropertyName._profileScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AE6 RID: 10982 RVA: 0x0012DEA4 File Offset: 0x0012C0A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._settingsScreenScene, Variant.From<PackedScene>(ref this._settingsScreenScene));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, Variant.From<PackedScene>(ref this._characterSelectScreenScene));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, Variant.From<NSingleplayerSubmenu>(ref this._singleplayerSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, Variant.From<NMultiplayerSubmenu>(ref this._multiplayerSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, Variant.From<NMultiplayerHostSubmenu>(ref this._multiplayerHostSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, Variant.From<NJoinFriendScreen>(ref this._joinFriendSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, Variant.From<NCharacterSelectScreen>(ref this._characterSelectSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, Variant.From<NMultiplayerLoadGameScreen>(ref this._loadMultiplayerSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, Variant.From<NCompendiumSubmenu>(ref this._compendiumSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, Variant.From<NBestiary>(ref this._bestiarySubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, Variant.From<NRelicCollection>(ref this._relicCollectionSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, Variant.From<NPotionLab>(ref this._potionLabSubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, Variant.From<NCardLibrary>(ref this._cardLibrarySubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, Variant.From<NRunHistory>(ref this._runHistorySubmenu));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._statsScreen, Variant.From<NStatsScreen>(ref this._statsScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._timelineScreen, Variant.From<NTimelineScreen>(ref this._timelineScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._dailyScreen, Variant.From<NDailyRunScreen>(ref this._dailyScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, Variant.From<NDailyRunLoadScreen>(ref this._dailyLoadScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._customRunScreen, Variant.From<NCustomRunScreen>(ref this._customRunScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, Variant.From<NCustomRunLoadScreen>(ref this._customRunLoadScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._moddingScreen, Variant.From<NModdingScreen>(ref this._moddingScreen));
			info.AddProperty(NMainMenuSubmenuStack.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002AE7 RID: 10983 RVA: 0x0012E0B4 File Offset: 0x0012C2B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._settingsScreenScene, ref variant))
			{
				this._settingsScreenScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, ref variant2))
			{
				this._characterSelectScreenScene = variant2.As<PackedScene>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, ref variant3))
			{
				this._singleplayerSubmenu = variant3.As<NSingleplayerSubmenu>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, ref variant4))
			{
				this._multiplayerSubmenu = variant4.As<NMultiplayerSubmenu>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, ref variant5))
			{
				this._multiplayerHostSubmenu = variant5.As<NMultiplayerHostSubmenu>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, ref variant6))
			{
				this._joinFriendSubmenu = variant6.As<NJoinFriendScreen>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, ref variant7))
			{
				this._characterSelectSubmenu = variant7.As<NCharacterSelectScreen>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, ref variant8))
			{
				this._loadMultiplayerSubmenu = variant8.As<NMultiplayerLoadGameScreen>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, ref variant9))
			{
				this._compendiumSubmenu = variant9.As<NCompendiumSubmenu>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, ref variant10))
			{
				this._bestiarySubmenu = variant10.As<NBestiary>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, ref variant11))
			{
				this._relicCollectionSubmenu = variant11.As<NRelicCollection>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, ref variant12))
			{
				this._potionLabSubmenu = variant12.As<NPotionLab>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, ref variant13))
			{
				this._cardLibrarySubmenu = variant13.As<NCardLibrary>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, ref variant14))
			{
				this._runHistorySubmenu = variant14.As<NRunHistory>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._statsScreen, ref variant15))
			{
				this._statsScreen = variant15.As<NStatsScreen>();
			}
			Variant variant16;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._timelineScreen, ref variant16))
			{
				this._timelineScreen = variant16.As<NTimelineScreen>();
			}
			Variant variant17;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._settingsScreen, ref variant17))
			{
				this._settingsScreen = variant17.As<NSettingsScreen>();
			}
			Variant variant18;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._dailyScreen, ref variant18))
			{
				this._dailyScreen = variant18.As<NDailyRunScreen>();
			}
			Variant variant19;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, ref variant19))
			{
				this._dailyLoadScreen = variant19.As<NDailyRunLoadScreen>();
			}
			Variant variant20;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._customRunScreen, ref variant20))
			{
				this._customRunScreen = variant20.As<NCustomRunScreen>();
			}
			Variant variant21;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, ref variant21))
			{
				this._customRunLoadScreen = variant21.As<NCustomRunLoadScreen>();
			}
			Variant variant22;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._moddingScreen, ref variant22))
			{
				this._moddingScreen = variant22.As<NModdingScreen>();
			}
			Variant variant23;
			if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._profileScreen, ref variant23))
			{
				this._profileScreen = variant23.As<NProfileScreen>();
			}
		}

		// Token: 0x04001008 RID: 4104
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _settingsScreenScene;

		// Token: 0x04001009 RID: 4105
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _characterSelectScreenScene;

		// Token: 0x0400100A RID: 4106
		private NSingleplayerSubmenu _singleplayerSubmenu;

		// Token: 0x0400100B RID: 4107
		private NMultiplayerSubmenu _multiplayerSubmenu;

		// Token: 0x0400100C RID: 4108
		private NMultiplayerHostSubmenu _multiplayerHostSubmenu;

		// Token: 0x0400100D RID: 4109
		private NJoinFriendScreen _joinFriendSubmenu;

		// Token: 0x0400100E RID: 4110
		private NCharacterSelectScreen _characterSelectSubmenu;

		// Token: 0x0400100F RID: 4111
		private NMultiplayerLoadGameScreen _loadMultiplayerSubmenu;

		// Token: 0x04001010 RID: 4112
		private NCompendiumSubmenu _compendiumSubmenu;

		// Token: 0x04001011 RID: 4113
		private NBestiary _bestiarySubmenu;

		// Token: 0x04001012 RID: 4114
		private NRelicCollection _relicCollectionSubmenu;

		// Token: 0x04001013 RID: 4115
		private NPotionLab _potionLabSubmenu;

		// Token: 0x04001014 RID: 4116
		private NCardLibrary _cardLibrarySubmenu;

		// Token: 0x04001015 RID: 4117
		private NRunHistory _runHistorySubmenu;

		// Token: 0x04001016 RID: 4118
		private NStatsScreen _statsScreen;

		// Token: 0x04001017 RID: 4119
		private NTimelineScreen _timelineScreen;

		// Token: 0x04001018 RID: 4120
		private NSettingsScreen _settingsScreen;

		// Token: 0x04001019 RID: 4121
		private NDailyRunScreen _dailyScreen;

		// Token: 0x0400101A RID: 4122
		private NDailyRunLoadScreen _dailyLoadScreen;

		// Token: 0x0400101B RID: 4123
		private NCustomRunScreen _customRunScreen;

		// Token: 0x0400101C RID: 4124
		private NCustomRunLoadScreen _customRunLoadScreen;

		// Token: 0x0400101D RID: 4125
		private NModdingScreen _moddingScreen;

		// Token: 0x0400101E RID: 4126
		private NProfileScreen _profileScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001513 RID: 5395
		[NullableContext(0)]
		public new class MethodName : NSubmenuStack.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400505E RID: 20574
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001514 RID: 5396
		[NullableContext(0)]
		public new class PropertyName : NSubmenuStack.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreenScene' field.
			/// </summary>
			// Token: 0x0400505F RID: 20575
			public static readonly StringName _settingsScreenScene = "_settingsScreenScene";

			/// <summary>
			/// Cached name for the '_characterSelectScreenScene' field.
			/// </summary>
			// Token: 0x04005060 RID: 20576
			public static readonly StringName _characterSelectScreenScene = "_characterSelectScreenScene";

			/// <summary>
			/// Cached name for the '_singleplayerSubmenu' field.
			/// </summary>
			// Token: 0x04005061 RID: 20577
			public static readonly StringName _singleplayerSubmenu = "_singleplayerSubmenu";

			/// <summary>
			/// Cached name for the '_multiplayerSubmenu' field.
			/// </summary>
			// Token: 0x04005062 RID: 20578
			public static readonly StringName _multiplayerSubmenu = "_multiplayerSubmenu";

			/// <summary>
			/// Cached name for the '_multiplayerHostSubmenu' field.
			/// </summary>
			// Token: 0x04005063 RID: 20579
			public static readonly StringName _multiplayerHostSubmenu = "_multiplayerHostSubmenu";

			/// <summary>
			/// Cached name for the '_joinFriendSubmenu' field.
			/// </summary>
			// Token: 0x04005064 RID: 20580
			public static readonly StringName _joinFriendSubmenu = "_joinFriendSubmenu";

			/// <summary>
			/// Cached name for the '_characterSelectSubmenu' field.
			/// </summary>
			// Token: 0x04005065 RID: 20581
			public static readonly StringName _characterSelectSubmenu = "_characterSelectSubmenu";

			/// <summary>
			/// Cached name for the '_loadMultiplayerSubmenu' field.
			/// </summary>
			// Token: 0x04005066 RID: 20582
			public static readonly StringName _loadMultiplayerSubmenu = "_loadMultiplayerSubmenu";

			/// <summary>
			/// Cached name for the '_compendiumSubmenu' field.
			/// </summary>
			// Token: 0x04005067 RID: 20583
			public static readonly StringName _compendiumSubmenu = "_compendiumSubmenu";

			/// <summary>
			/// Cached name for the '_bestiarySubmenu' field.
			/// </summary>
			// Token: 0x04005068 RID: 20584
			public static readonly StringName _bestiarySubmenu = "_bestiarySubmenu";

			/// <summary>
			/// Cached name for the '_relicCollectionSubmenu' field.
			/// </summary>
			// Token: 0x04005069 RID: 20585
			public static readonly StringName _relicCollectionSubmenu = "_relicCollectionSubmenu";

			/// <summary>
			/// Cached name for the '_potionLabSubmenu' field.
			/// </summary>
			// Token: 0x0400506A RID: 20586
			public static readonly StringName _potionLabSubmenu = "_potionLabSubmenu";

			/// <summary>
			/// Cached name for the '_cardLibrarySubmenu' field.
			/// </summary>
			// Token: 0x0400506B RID: 20587
			public static readonly StringName _cardLibrarySubmenu = "_cardLibrarySubmenu";

			/// <summary>
			/// Cached name for the '_runHistorySubmenu' field.
			/// </summary>
			// Token: 0x0400506C RID: 20588
			public static readonly StringName _runHistorySubmenu = "_runHistorySubmenu";

			/// <summary>
			/// Cached name for the '_statsScreen' field.
			/// </summary>
			// Token: 0x0400506D RID: 20589
			public static readonly StringName _statsScreen = "_statsScreen";

			/// <summary>
			/// Cached name for the '_timelineScreen' field.
			/// </summary>
			// Token: 0x0400506E RID: 20590
			public static readonly StringName _timelineScreen = "_timelineScreen";

			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x0400506F RID: 20591
			public static readonly StringName _settingsScreen = "_settingsScreen";

			/// <summary>
			/// Cached name for the '_dailyScreen' field.
			/// </summary>
			// Token: 0x04005070 RID: 20592
			public static readonly StringName _dailyScreen = "_dailyScreen";

			/// <summary>
			/// Cached name for the '_dailyLoadScreen' field.
			/// </summary>
			// Token: 0x04005071 RID: 20593
			public static readonly StringName _dailyLoadScreen = "_dailyLoadScreen";

			/// <summary>
			/// Cached name for the '_customRunScreen' field.
			/// </summary>
			// Token: 0x04005072 RID: 20594
			public static readonly StringName _customRunScreen = "_customRunScreen";

			/// <summary>
			/// Cached name for the '_customRunLoadScreen' field.
			/// </summary>
			// Token: 0x04005073 RID: 20595
			public static readonly StringName _customRunLoadScreen = "_customRunLoadScreen";

			/// <summary>
			/// Cached name for the '_moddingScreen' field.
			/// </summary>
			// Token: 0x04005074 RID: 20596
			public static readonly StringName _moddingScreen = "_moddingScreen";

			/// <summary>
			/// Cached name for the '_profileScreen' field.
			/// </summary>
			// Token: 0x04005075 RID: 20597
			public static readonly StringName _profileScreen = "_profileScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001515 RID: 5397
		[NullableContext(0)]
		public new class SignalName : NSubmenuStack.SignalName
		{
		}
	}
}
