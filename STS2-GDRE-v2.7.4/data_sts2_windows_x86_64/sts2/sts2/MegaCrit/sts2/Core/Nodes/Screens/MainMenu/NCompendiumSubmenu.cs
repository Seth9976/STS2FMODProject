using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x020002FD RID: 765
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NCompendiumSubmenu.cs")]
	public class NCompendiumSubmenu : NSubmenu
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x00123B0D File Offset: 0x00121D0D
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._cardLibraryButton;
			}
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x00123B15 File Offset: 0x00121D15
		[NullableContext(2)]
		public static NCompendiumSubmenu Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCompendiumSubmenu._scenePath).Instantiate<NCompendiumSubmenu>(0L);
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x00123B38 File Offset: 0x00121D38
		public unsafe override void _Ready()
		{
			this.ConnectSignals();
			this._cardLibraryButton = base.GetNode<NShortSubmenuButton>("%CardLibraryButton");
			this._cardLibraryButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCardLibrary)), 0U);
			this._cardLibraryButton.SetIconAndLocalization("COMPENDIUM_CARD_LIBRARY");
			this._relicCollectionButton = base.GetNode<NShortSubmenuButton>("%RelicCollectionButton");
			this._relicCollectionButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRelicCollection)), 0U);
			this._relicCollectionButton.SetIconAndLocalization("COMPENDIUM_RELIC_COLLECTION");
			this._potionLabButton = base.GetNode<NShortSubmenuButton>("%PotionLabButton");
			this._potionLabButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenPotionLab)), 0U);
			this._potionLabButton.SetIconAndLocalization("COMPENDIUM_POTION_LAB");
			this._bestiaryButton = base.GetNode<NShortSubmenuButton>("%BestiaryButton");
			this._bestiaryButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenBestiary)), 0U);
			this._bestiaryButton.SetIconAndLocalization("COMPENDIUM_BESTIARY");
			this._leaderboardsButton = base.GetNode<NCompendiumBottomButton>("%LeaderboardsButton");
			this._leaderboardsButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenLeaderboards)), 0U);
			this._leaderboardsButton.SetLocalization("LEADERBOARDS");
			this._statisticsButton = base.GetNode<NCompendiumBottomButton>("%StatisticsButton");
			this._statisticsButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenStatistics)), 0U);
			this._statisticsButton.SetLocalization("STATISTICS");
			this._runHistoryButton = base.GetNode<NCompendiumBottomButton>("%RunHistoryButton");
			this._runHistoryButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRunHistory)), 0U);
			this._runHistoryButton.SetLocalization("RUN_HISTORY");
			int num = 4;
			List<Control> list = new List<Control>(num);
			CollectionsMarshal.SetCount<Control>(list, num);
			Span<Control> span = CollectionsMarshal.AsSpan<Control>(list);
			int num2 = 0;
			*span[num2] = this._cardLibraryButton;
			num2++;
			*span[num2] = this._relicCollectionButton;
			num2++;
			*span[num2] = this._potionLabButton;
			num2++;
			*span[num2] = this._bestiaryButton;
			List<Control> list2 = list;
			num2 = 3;
			List<Control> list3 = new List<Control>(num2);
			CollectionsMarshal.SetCount<Control>(list3, num2);
			span = CollectionsMarshal.AsSpan<Control>(list3);
			num = 0;
			*span[num] = this._leaderboardsButton;
			num++;
			*span[num] = this._statisticsButton;
			num++;
			*span[num] = this._runHistoryButton;
			List<Control> list4 = list3;
			for (int i = 0; i < list2.Count; i++)
			{
				list2[i].FocusNeighborTop = list2[i].GetPath();
				list2[i].FocusNeighborLeft = ((i > 0) ? list2[i - 1].GetPath() : list2[i].GetPath());
				list2[i].FocusNeighborRight = ((i < list2.Count - 1) ? list2[i + 1].GetPath() : list2[i].GetPath());
			}
			for (int j = 0; j < list4.Count; j++)
			{
				list4[j].FocusNeighborBottom = list4[j].GetPath();
				list4[j].FocusNeighborLeft = ((j > 0) ? list4[j - 1].GetPath() : list4[j].GetPath());
				list4[j].FocusNeighborRight = ((j < list4.Count - 1) ? list4[j + 1].GetPath() : list4[j].GetPath());
			}
			list2[0].FocusNeighborBottom = list4[0].GetPath();
			list2[1].FocusNeighborBottom = list4[0].GetPath();
			list2[2].FocusNeighborBottom = list4[1].GetPath();
			list2[3].FocusNeighborBottom = list4[2].GetPath();
			list4[0].FocusNeighborTop = list2[1].GetPath();
			list4[1].FocusNeighborTop = list2[2].GetPath();
			list4[2].FocusNeighborTop = list2[3].GetPath();
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x00123FE7 File Offset: 0x001221E7
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._leaderboardsButton.Visible = false;
			this._runHistoryButton.Visible = NRunHistory.CanBeShown();
			this._bestiaryButton.Visible = NBestiary.CanBeShown();
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0012401B File Offset: 0x0012221B
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00124024 File Offset: 0x00122224
		private void OpenCardLibrary(NButton _)
		{
			NCardLibrary submenuType = this._stack.GetSubmenuType<NCardLibrary>();
			submenuType.Initialize(this._runState);
			this._stack.Push(submenuType);
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x00124055 File Offset: 0x00122255
		private void OpenRelicCollection(NButton _)
		{
			this._stack.PushSubmenuType<NRelicCollection>();
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x00124063 File Offset: 0x00122263
		private void OpenPotionLab(NButton _)
		{
			this._stack.PushSubmenuType<NPotionLab>();
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x00124071 File Offset: 0x00122271
		private void OpenBestiary(NButton _)
		{
			this.OpenBestiary();
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x0012407A File Offset: 0x0012227A
		public NBestiary OpenBestiary()
		{
			return this._stack.PushSubmenuType<NBestiary>();
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x00124087 File Offset: 0x00122287
		private void OpenLeaderboards(NButton _)
		{
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x00124089 File Offset: 0x00122289
		private void OpenStatistics(NButton _)
		{
			this._stack.PushSubmenuType<NStatsScreen>();
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x00124097 File Offset: 0x00122297
		private void OpenRunHistory(NButton _)
		{
			this._stack.PushSubmenuType<NRunHistory>();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029DD RID: 10717 RVA: 0x001240A8 File Offset: 0x001222A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NCompendiumSubmenu.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCompendiumSubmenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenCardLibrary, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenRelicCollection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenPotionLab, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenBestiary, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenBestiary, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenLeaderboards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenStatistics, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCompendiumSubmenu.MethodName.OpenRunHistory, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029DE RID: 10718 RVA: 0x0012442C File Offset: 0x0012262C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCompendiumSubmenu.MethodName.Create && args.Count == 0)
			{
				NCompendiumSubmenu ncompendiumSubmenu = NCompendiumSubmenu.Create();
				ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenCardLibrary && args.Count == 1)
			{
				this.OpenCardLibrary(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenRelicCollection && args.Count == 1)
			{
				this.OpenRelicCollection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenPotionLab && args.Count == 1)
			{
				this.OpenPotionLab(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenBestiary && args.Count == 1)
			{
				this.OpenBestiary(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenBestiary && args.Count == 0)
			{
				NBestiary nbestiary = this.OpenBestiary();
				ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenLeaderboards && args.Count == 1)
			{
				this.OpenLeaderboards(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenStatistics && args.Count == 1)
			{
				this.OpenStatistics(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCompendiumSubmenu.MethodName.OpenRunHistory && args.Count == 1)
			{
				this.OpenRunHistory(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x00124648 File Offset: 0x00122848
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCompendiumSubmenu.MethodName.Create && args.Count == 0)
			{
				NCompendiumSubmenu ncompendiumSubmenu = NCompendiumSubmenu.Create();
				ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060029E0 RID: 10720 RVA: 0x00124688 File Offset: 0x00122888
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCompendiumSubmenu.MethodName.Create || (ref method) == NCompendiumSubmenu.MethodName._Ready || (ref method) == NCompendiumSubmenu.MethodName.OnSubmenuOpened || (ref method) == NCompendiumSubmenu.MethodName.OpenCardLibrary || (ref method) == NCompendiumSubmenu.MethodName.OpenRelicCollection || (ref method) == NCompendiumSubmenu.MethodName.OpenPotionLab || (ref method) == NCompendiumSubmenu.MethodName.OpenBestiary || (ref method) == NCompendiumSubmenu.MethodName.OpenLeaderboards || (ref method) == NCompendiumSubmenu.MethodName.OpenStatistics || (ref method) == NCompendiumSubmenu.MethodName.OpenRunHistory || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060029E1 RID: 10721 RVA: 0x00124734 File Offset: 0x00122934
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCompendiumSubmenu.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._cardLibraryButton)
			{
				this._cardLibraryButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._relicCollectionButton)
			{
				this._relicCollectionButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._potionLabButton)
			{
				this._potionLabButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._bestiaryButton)
			{
				this._bestiaryButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._leaderboardsButton)
			{
				this._leaderboardsButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._statisticsButton)
			{
				this._statisticsButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._runHistoryButton)
			{
				this._runHistoryButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060029E2 RID: 10722 RVA: 0x00124824 File Offset: 0x00122A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCompendiumSubmenu.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._cardLibraryButton)
			{
				value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._cardLibraryButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._relicCollectionButton)
			{
				value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._relicCollectionButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._potionLabButton)
			{
				value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._potionLabButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._bestiaryButton)
			{
				value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._bestiaryButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._leaderboardsButton)
			{
				value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._leaderboardsButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._statisticsButton)
			{
				value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._statisticsButton);
				return true;
			}
			if ((ref name) == NCompendiumSubmenu.PropertyName._runHistoryButton)
			{
				value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._runHistoryButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029E3 RID: 10723 RVA: 0x0012495C File Offset: 0x00122B5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._cardLibraryButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._relicCollectionButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._potionLabButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._bestiaryButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._leaderboardsButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._statisticsButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName._runHistoryButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCompendiumSubmenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029E4 RID: 10724 RVA: 0x00124A9C File Offset: 0x00122C9C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCompendiumSubmenu.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._cardLibraryButton, Variant.From<NShortSubmenuButton>(ref this._cardLibraryButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._relicCollectionButton, Variant.From<NShortSubmenuButton>(ref this._relicCollectionButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._potionLabButton, Variant.From<NShortSubmenuButton>(ref this._potionLabButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._bestiaryButton, Variant.From<NShortSubmenuButton>(ref this._bestiaryButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._leaderboardsButton, Variant.From<NCompendiumBottomButton>(ref this._leaderboardsButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._statisticsButton, Variant.From<NCompendiumBottomButton>(ref this._statisticsButton));
			info.AddProperty(NCompendiumSubmenu.PropertyName._runHistoryButton, Variant.From<NCompendiumBottomButton>(ref this._runHistoryButton));
		}

		/// <inheritdoc />
		// Token: 0x060029E5 RID: 10725 RVA: 0x00124B60 File Offset: 0x00122D60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._cardLibraryButton, ref variant2))
			{
				this._cardLibraryButton = variant2.As<NShortSubmenuButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._relicCollectionButton, ref variant3))
			{
				this._relicCollectionButton = variant3.As<NShortSubmenuButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._potionLabButton, ref variant4))
			{
				this._potionLabButton = variant4.As<NShortSubmenuButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._bestiaryButton, ref variant5))
			{
				this._bestiaryButton = variant5.As<NShortSubmenuButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._leaderboardsButton, ref variant6))
			{
				this._leaderboardsButton = variant6.As<NCompendiumBottomButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._statisticsButton, ref variant7))
			{
				this._statisticsButton = variant7.As<NCompendiumBottomButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._runHistoryButton, ref variant8))
			{
				this._runHistoryButton = variant8.As<NCompendiumBottomButton>();
			}
		}

		// Token: 0x04000FA3 RID: 4003
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/compendium_submenu");

		// Token: 0x04000FA4 RID: 4004
		private NButton _confirmButton;

		// Token: 0x04000FA5 RID: 4005
		private NShortSubmenuButton _cardLibraryButton;

		// Token: 0x04000FA6 RID: 4006
		private NShortSubmenuButton _relicCollectionButton;

		// Token: 0x04000FA7 RID: 4007
		private NShortSubmenuButton _potionLabButton;

		// Token: 0x04000FA8 RID: 4008
		private NShortSubmenuButton _bestiaryButton;

		// Token: 0x04000FA9 RID: 4009
		private NCompendiumBottomButton _leaderboardsButton;

		// Token: 0x04000FAA RID: 4010
		private NCompendiumBottomButton _statisticsButton;

		// Token: 0x04000FAB RID: 4011
		private NCompendiumBottomButton _runHistoryButton;

		// Token: 0x04000FAC RID: 4012
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D7 RID: 5335
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004F73 RID: 20339
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F74 RID: 20340
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004F75 RID: 20341
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OpenCardLibrary' method.
			/// </summary>
			// Token: 0x04004F76 RID: 20342
			public static readonly StringName OpenCardLibrary = "OpenCardLibrary";

			/// <summary>
			/// Cached name for the 'OpenRelicCollection' method.
			/// </summary>
			// Token: 0x04004F77 RID: 20343
			public static readonly StringName OpenRelicCollection = "OpenRelicCollection";

			/// <summary>
			/// Cached name for the 'OpenPotionLab' method.
			/// </summary>
			// Token: 0x04004F78 RID: 20344
			public static readonly StringName OpenPotionLab = "OpenPotionLab";

			/// <summary>
			/// Cached name for the 'OpenBestiary' method.
			/// </summary>
			// Token: 0x04004F79 RID: 20345
			public static readonly StringName OpenBestiary = "OpenBestiary";

			/// <summary>
			/// Cached name for the 'OpenLeaderboards' method.
			/// </summary>
			// Token: 0x04004F7A RID: 20346
			public static readonly StringName OpenLeaderboards = "OpenLeaderboards";

			/// <summary>
			/// Cached name for the 'OpenStatistics' method.
			/// </summary>
			// Token: 0x04004F7B RID: 20347
			public static readonly StringName OpenStatistics = "OpenStatistics";

			/// <summary>
			/// Cached name for the 'OpenRunHistory' method.
			/// </summary>
			// Token: 0x04004F7C RID: 20348
			public static readonly StringName OpenRunHistory = "OpenRunHistory";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D8 RID: 5336
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004F7D RID: 20349
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04004F7E RID: 20350
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_cardLibraryButton' field.
			/// </summary>
			// Token: 0x04004F7F RID: 20351
			public static readonly StringName _cardLibraryButton = "_cardLibraryButton";

			/// <summary>
			/// Cached name for the '_relicCollectionButton' field.
			/// </summary>
			// Token: 0x04004F80 RID: 20352
			public static readonly StringName _relicCollectionButton = "_relicCollectionButton";

			/// <summary>
			/// Cached name for the '_potionLabButton' field.
			/// </summary>
			// Token: 0x04004F81 RID: 20353
			public static readonly StringName _potionLabButton = "_potionLabButton";

			/// <summary>
			/// Cached name for the '_bestiaryButton' field.
			/// </summary>
			// Token: 0x04004F82 RID: 20354
			public static readonly StringName _bestiaryButton = "_bestiaryButton";

			/// <summary>
			/// Cached name for the '_leaderboardsButton' field.
			/// </summary>
			// Token: 0x04004F83 RID: 20355
			public static readonly StringName _leaderboardsButton = "_leaderboardsButton";

			/// <summary>
			/// Cached name for the '_statisticsButton' field.
			/// </summary>
			// Token: 0x04004F84 RID: 20356
			public static readonly StringName _statisticsButton = "_statisticsButton";

			/// <summary>
			/// Cached name for the '_runHistoryButton' field.
			/// </summary>
			// Token: 0x04004F85 RID: 20357
			public static readonly StringName _runHistoryButton = "_runHistoryButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D9 RID: 5337
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
