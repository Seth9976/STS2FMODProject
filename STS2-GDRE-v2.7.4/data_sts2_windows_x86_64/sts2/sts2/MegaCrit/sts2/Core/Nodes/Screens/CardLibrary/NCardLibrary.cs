using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	// Token: 0x02000351 RID: 849
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibrary.cs")]
	public sealed class NCardLibrary : NSubmenu
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x00169692 File Offset: 0x00167892
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NCardLibrary._scenePath };
			}
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x001696A2 File Offset: 0x001678A2
		[NullableContext(2)]
		public static NCardLibrary Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCardLibrary._scenePath).Instantiate<NCardLibrary>(0L);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x001696C4 File Offset: 0x001678C4
		public override void _Ready()
		{
			this.ConnectSignals();
			this._grid = base.GetNode<NCardLibraryGrid>("%CardGrid");
			this._grid.Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ShowCardDetail)), 0U);
			this._grid.Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ShowCardDetail)), 0U);
			this._cardCountLabel = base.GetNode<MegaRichTextLabel>("%CardCountLabel");
			this._noResultsLabel = base.GetNode<MegaRichTextLabel>("%NoResultsLabel");
			this._noResultsLabel.Text = this._noResultsLocString.GetFormattedText();
			this._searchBar = base.GetNode<NSearchBar>("%SearchBar");
			this._searchBar.Connect(NSearchBar.SignalName.QueryChanged, Callable.From<string>(new Action<string>(this.SearchBarQueryChanged)), 0U);
			this._searchBar.Connect(NSearchBar.SignalName.QuerySubmitted, Callable.From<string>(new Action<string>(this.SearchBarQuerySubmitted)), 0U);
			this._ironcladFilter = base.GetNode<NCardPoolFilter>("%IroncladPool");
			this._silentFilter = base.GetNode<NCardPoolFilter>("%SilentPool");
			this._defectFilter = base.GetNode<NCardPoolFilter>("%DefectPool");
			this._regentFilter = base.GetNode<NCardPoolFilter>("%RegentPool");
			this._necrobinderFilter = base.GetNode<NCardPoolFilter>("%NecrobinderPool");
			this._colorlessFilter = base.GetNode<NCardPoolFilter>("%ColorlessPool");
			this._ancientsFilter = base.GetNode<NCardPoolFilter>("%AncientsPool");
			this._miscPoolFilter = base.GetNode<NCardPoolFilter>("%MiscPool");
			Callable callable = Callable.From<NCardPoolFilter>(new Action<NCardPoolFilter>(this.UpdateCardPoolFilter));
			this._ironcladFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._silentFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._defectFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._regentFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._necrobinderFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._colorlessFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._ancientsFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._miscPoolFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable, 0U);
			this._poolFilters.Add(this._ironcladFilter, (CardModel c) => c.Pool is IroncladCardPool);
			this._poolFilters.Add(this._silentFilter, (CardModel c) => c.Pool is SilentCardPool);
			this._poolFilters.Add(this._defectFilter, (CardModel c) => c.Pool is DefectCardPool);
			this._poolFilters.Add(this._regentFilter, (CardModel c) => c.Pool is RegentCardPool);
			this._poolFilters.Add(this._necrobinderFilter, (CardModel c) => c.Pool is NecrobinderCardPool);
			this._poolFilters.Add(this._colorlessFilter, (CardModel c) => c.Pool is ColorlessCardPool);
			this._poolFilters.Add(this._ancientsFilter, (CardModel c) => c.Rarity == CardRarity.Ancient);
			this._poolFilters.Add(this._miscPoolFilter, delegate(CardModel c)
			{
				CardRarity rarity = c.Rarity;
				return rarity - CardRarity.Event <= 4;
			});
			this._typeSorter = base.GetNode<NCardViewSortButton>("%CardTypeSorter");
			this._typeSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCardTypeSort)), 0U);
			this._attackFilter = base.GetNode<NCardTypeTickbox>("%AttackType");
			this._skillFilter = base.GetNode<NCardTypeTickbox>("%SkillType");
			this._powerFilter = base.GetNode<NCardTypeTickbox>("%PowerType");
			this._otherTypeFilter = base.GetNode<NCardTypeTickbox>("%OtherType");
			Callable callable2 = Callable.From<NCardTypeTickbox>(new Action<NCardTypeTickbox>(this.UpdateTypeFilter));
			this._attackFilter.Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
			this._skillFilter.Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
			this._powerFilter.Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
			this._otherTypeFilter.Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
			this._cardTypeFilters.Add(this._attackFilter, (CardModel c) => c.Type == CardType.Attack);
			this._cardTypeFilters.Add(this._skillFilter, (CardModel c) => c.Type == CardType.Skill);
			this._cardTypeFilters.Add(this._powerFilter, (CardModel c) => c.Type == CardType.Power);
			this._cardTypeFilters.Add(this._otherTypeFilter, delegate(CardModel c)
			{
				CardType type = c.Type;
				bool flag = type - CardType.Attack <= 2;
				return !flag;
			});
			this._raritySorter = base.GetNode<NCardViewSortButton>("%RaritySorter");
			this._raritySorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRaritySort)), 0U);
			this._commonFilter = base.GetNode<NCardRarityTickbox>("%CommonRarity");
			this._uncommonFilter = base.GetNode<NCardRarityTickbox>("%UncommonRarity");
			this._rareFilter = base.GetNode<NCardRarityTickbox>("%RareRarity");
			this._otherFilter = base.GetNode<NCardRarityTickbox>("%OtherRarity");
			Callable callable3 = Callable.From<NTickbox>(new Action<NTickbox>(this.UpdateRarityFilter));
			this._commonFilter.Connect(NTickbox.SignalName.Toggled, callable3, 0U);
			this._uncommonFilter.Connect(NTickbox.SignalName.Toggled, callable3, 0U);
			this._rareFilter.Connect(NTickbox.SignalName.Toggled, callable3, 0U);
			this._otherFilter.Connect(NTickbox.SignalName.Toggled, callable3, 0U);
			this._rarityFilters.Add(this._commonFilter, (CardModel c) => c.Rarity == CardRarity.Common);
			this._rarityFilters.Add(this._uncommonFilter, (CardModel c) => c.Rarity == CardRarity.Uncommon);
			this._rarityFilters.Add(this._rareFilter, (CardModel c) => c.Rarity == CardRarity.Rare);
			this._rarityFilters.Add(this._otherFilter, delegate(CardModel c)
			{
				CardRarity rarity2 = c.Rarity;
				bool flag2 = rarity2 - CardRarity.Common <= 2;
				return !flag2;
			});
			this._costSorter = base.GetNode<NCardViewSortButton>("%CostSorter");
			this._costSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCostSort)), 0U);
			this._zeroFilter = base.GetNode<NCardCostTickbox>("%Cost0");
			this._oneFilter = base.GetNode<NCardCostTickbox>("%Cost1");
			this._twoFilter = base.GetNode<NCardCostTickbox>("%Cost2");
			this._threePlusFilter = base.GetNode<NCardCostTickbox>("%Cost3+");
			this._xFilter = base.GetNode<NCardCostTickbox>("%CostX");
			Callable callable4 = Callable.From<NCardCostTickbox>(new Action<NCardCostTickbox>(this.UpdateCostFilter));
			this._zeroFilter.Connect(NClickableControl.SignalName.Released, callable4, 0U);
			this._oneFilter.Connect(NClickableControl.SignalName.Released, callable4, 0U);
			this._twoFilter.Connect(NClickableControl.SignalName.Released, callable4, 0U);
			this._threePlusFilter.Connect(NClickableControl.SignalName.Released, callable4, 0U);
			this._xFilter.Connect(NClickableControl.SignalName.Released, callable4, 0U);
			this._costFilters.Add(this._zeroFilter, delegate(CardModel c)
			{
				CardEnergyCost energyCost = c.EnergyCost;
				return energyCost != null && energyCost.Canonical <= 0 && !energyCost.CostsX;
			});
			this._costFilters.Add(this._oneFilter, (CardModel c) => c.EnergyCost.Canonical == 1);
			this._costFilters.Add(this._twoFilter, (CardModel c) => c.EnergyCost.Canonical == 2);
			this._costFilters.Add(this._threePlusFilter, (CardModel c) => c.EnergyCost.Canonical >= 3);
			this._costFilters.Add(this._xFilter, (CardModel c) => c.EnergyCost.CostsX || c.HasStarCostX);
			this._alphabetSorter = base.GetNode<NCardViewSortButton>("%AlphabetSorter");
			this._alphabetSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAlphabetSort)), 0U);
			this._viewStats = base.GetNode<NLibraryStatTickbox>("%Stats");
			this._viewUpgrades = base.GetNode<NLibraryStatTickbox>("%Upgrades");
			this._viewMultiplayerCards = base.GetNode<NLibraryStatTickbox>("%MultiplayerCards");
			this._viewStats.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowStats)), 0U);
			this._viewUpgrades.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
			this._viewMultiplayerCards.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleFilterMultiplayerCards)), 0U);
			this._typeSorter.SetLabel(new LocString("gameplay_ui", "SORT_TYPE").GetRawText());
			this._raritySorter.SetLabel(new LocString("gameplay_ui", "SORT_RARITY").GetRawText());
			this._costSorter.SetLabel(new LocString("gameplay_ui", "SORT_COST").GetRawText());
			this._alphabetSorter.SetLabel(new LocString("gameplay_ui", "SORT_ALPHABET").GetRawText());
			this._commonFilter.SetLabel(new LocString("card_library", "RARITY_COMMON").GetRawText());
			this._uncommonFilter.SetLabel(new LocString("card_library", "RARITY_UNCOMMON").GetRawText());
			this._rareFilter.SetLabel(new LocString("card_library", "RARITY_RARE").GetRawText());
			this._otherFilter.SetLabel(new LocString("card_library", "RARITY_OTHER").GetRawText());
			this._viewStats.SetLabel(new LocString("card_library", "VIEW_STATS").GetRawText());
			this._viewUpgrades.SetLabel(new LocString("card_library", "VIEW_UPGRADES").GetRawText());
			this._viewMultiplayerCards.SetLabel(new LocString("card_library", "VIEW_MULTIPLAYER_CARDS").GetRawText());
			this._colorlessFilter.Loc = new LocString("card_library", "POOL_COLORLESS_TIP");
			this._ancientsFilter.Loc = new LocString("card_library", "POOL_ANCIENT_TIP");
			this._miscPoolFilter.Loc = new LocString("card_library", "POOL_MISC_TIP");
			this._attackFilter.Loc = new LocString("card_library", "TYPE_ATTACK_TIP");
			this._skillFilter.Loc = new LocString("card_library", "TYPE_SKILL_TIP");
			this._powerFilter.Loc = new LocString("card_library", "TYPE_POWER_TIP");
			this._otherTypeFilter.Loc = new LocString("card_library", "TYPE_OTHER_TIP");
			this._commonFilter.Loc = new LocString("card_library", "RARITY_COMMON_TIP");
			this._uncommonFilter.Loc = new LocString("card_library", "RARITY_UNCOMMON_TIP");
			this._rareFilter.Loc = new LocString("card_library", "RARITY_RARE_TIP");
			this._otherFilter.Loc = new LocString("card_library", "RARITY_OTHER_TIP");
			this._zeroFilter.Loc = new LocString("card_library", "COST_ZERO_TIP");
			this._oneFilter.Loc = new LocString("card_library", "COST_ONE_TIP");
			this._twoFilter.Loc = new LocString("card_library", "COST_TWO_TIP");
			this._threePlusFilter.Loc = new LocString("card_library", "COST_THREE_TIP");
			this._xFilter.Loc = new LocString("card_library", "COST_X_TIP");
			this._cardPoolFilters.Add(ModelDb.Character<Ironclad>(), this._ironcladFilter);
			this._cardPoolFilters.Add(ModelDb.Character<Silent>(), this._silentFilter);
			this._cardPoolFilters.Add(ModelDb.Character<Defect>(), this._defectFilter);
			this._cardPoolFilters.Add(ModelDb.Character<Necrobinder>(), this._necrobinderFilter);
			this._cardPoolFilters.Add(ModelDb.Character<Regent>(), this._regentFilter);
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			foreach (KeyValuePair<CharacterModel, NCardPoolFilter> keyValuePair in this._cardPoolFilters)
			{
				keyValuePair.Value.Visible = unlockState.Characters.Contains(keyValuePair.Key);
			}
			CardRarity[] values = Enum.GetValues<CardRarity>();
			for (int i = 0; i < values.Length; i++)
			{
				CardRarity keyword = values[i];
				this._specialSearchbarKeywords.Add(keyword.ToString().ToLowerInvariant(), (CardModel c) => c.Rarity == keyword);
			}
			this._ironcladFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._ironcladFilter;
			}), 0U);
			this._silentFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._silentFilter;
			}), 0U);
			this._defectFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._defectFilter;
			}), 0U);
			this._regentFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._regentFilter;
			}), 0U);
			this._necrobinderFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._necrobinderFilter;
			}), 0U);
			this._colorlessFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._colorlessFilter;
			}), 0U);
			this._ancientsFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._ancientsFilter;
			}), 0U);
			this._miscPoolFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._miscPoolFilter;
			}), 0U);
			this._attackFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._attackFilter;
			}), 0U);
			this._skillFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._skillFilter;
			}), 0U);
			this._powerFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._powerFilter;
			}), 0U);
			this._otherTypeFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._otherTypeFilter;
			}), 0U);
			this._commonFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._commonFilter;
			}), 0U);
			this._uncommonFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._uncommonFilter;
			}), 0U);
			this._rareFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._rareFilter;
			}), 0U);
			this._otherFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._otherFilter;
			}), 0U);
			this._zeroFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._zeroFilter;
			}), 0U);
			this._oneFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._oneFilter;
			}), 0U);
			this._twoFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._twoFilter;
			}), 0U);
			this._threePlusFilter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._threePlusFilter;
			}), 0U);
			this._alphabetSorter.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._alphabetSorter;
			}), 0U);
			this._viewUpgrades.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._lastHoveredControl = this._viewUpgrades;
			}), 0U);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x0016A824 File Offset: 0x00168A24
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x0016A830 File Offset: 0x00168A30
		private void OnCardTypeSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.TypeAscending);
			this._sortingPriority.Remove(SortingOrders.TypeDescending);
			if (this._typeSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.TypeDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.TypeAscending);
			}
			TaskHelper.RunSafely(this.DisplayCards());
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x0016A88C File Offset: 0x00168A8C
		private void OnRaritySort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.RarityAscending);
			this._sortingPriority.Remove(SortingOrders.RarityDescending);
			if (this._raritySorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.RarityAscending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.RarityDescending);
			}
			TaskHelper.RunSafely(this.DisplayCards());
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x0016A8E8 File Offset: 0x00168AE8
		private void OnCostSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.CostAscending);
			this._sortingPriority.Remove(SortingOrders.CostDescending);
			if (this._costSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.CostDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.CostAscending);
			}
			TaskHelper.RunSafely(this.DisplayCards());
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x0016A944 File Offset: 0x00168B44
		private void OnAlphabetSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.AlphabetAscending);
			this._sortingPriority.Remove(SortingOrders.AlphabetDescending);
			if (this._alphabetSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.AlphabetDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.AlphabetAscending);
			}
			TaskHelper.RunSafely(this.DisplayCards());
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x0016A9A0 File Offset: 0x00168BA0
		public override void OnSubmenuOpened()
		{
			this._grid.RefreshVisibility();
			Player me = LocalContext.GetMe(this._runState);
			CharacterModel characterModel = ((me != null) ? me.Character : null);
			this._searchBar.ClearText();
			if (characterModel != null)
			{
				using (Dictionary<NCardPoolFilter, Func<CardModel, bool>>.KeyCollection.Enumerator enumerator = this._poolFilters.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NCardPoolFilter ncardPoolFilter = enumerator.Current;
						ncardPoolFilter.IsSelected = this._cardPoolFilters[characterModel] == ncardPoolFilter;
					}
					goto IL_00C0;
				}
			}
			foreach (NCardPoolFilter ncardPoolFilter2 in this._poolFilters.Keys)
			{
				ncardPoolFilter2.IsSelected = ncardPoolFilter2 == this._ironcladFilter;
			}
			IL_00C0:
			foreach (NCardTypeTickbox ncardTypeTickbox in this._cardTypeFilters.Keys)
			{
				ncardTypeTickbox.IsTicked = false;
			}
			foreach (NCardRarityTickbox ncardRarityTickbox in this._rarityFilters.Keys)
			{
				ncardRarityTickbox.IsTicked = false;
			}
			foreach (NCardCostTickbox ncardCostTickbox in this._costFilters.Keys)
			{
				ncardCostTickbox.IsTicked = false;
			}
			this._typeSorter.IsDescending = true;
			this._raritySorter.IsDescending = true;
			this._costSorter.IsDescending = true;
			this._alphabetSorter.IsDescending = true;
			this._viewUpgrades.IsTicked = false;
			this._viewStats.IsTicked = false;
			this._viewMultiplayerCards.IsTicked = true;
			this.ToggleShowStats(this._viewStats);
			this.ToggleShowUpgrades(this._viewUpgrades);
			this.UpdateFilter(false);
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x0016ABDC File Offset: 0x00168DDC
		public override void OnSubmenuClosed()
		{
			this._grid.ClearGrid();
		}

		/// <summary>
		/// Called when the player changes the text in the search box.
		/// If this was already called recently, then the previous one is cancelled and a new delay starts.
		/// </summary>
		// Token: 0x060030EB RID: 12523 RVA: 0x0016ABEC File Offset: 0x00168DEC
		private async Task DisplayCardsAfterShortDelay()
		{
			if (this._displayCardsShortDelayCancelToken != null)
			{
				await this._displayCardsShortDelayCancelToken.CancelAsync();
			}
			if (!this._grid.IsAnimatingOut)
			{
				TaskHelper.RunSafely(this._grid.AnimateOut());
			}
			CancellationTokenSource cancelToken = new CancellationTokenSource();
			this._displayCardsShortDelayCancelToken = cancelToken;
			await Task.Delay(250, cancelToken.Token);
			if (!cancelToken.IsCancellationRequested)
			{
				await this.DisplayCards();
			}
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x0016AC30 File Offset: 0x00168E30
		private async Task DisplayCards()
		{
			if (this._displayCardsShortDelayCancelToken != null)
			{
				await this._displayCardsShortDelayCancelToken.CancelAsync();
			}
			await Task.Yield();
			this._grid.FilterCards(this._filter, this._sortingPriority);
			this._cardCountLocString.Add("Amount", this._grid.VisibleCards.Count<CardModel>());
			this._cardCountLabel.Text = "[center]" + this._cardCountLocString.GetFormattedText() + "[/center]";
			this._noResultsLabel.Visible = !this._grid.VisibleCards.Any<CardModel>();
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x0016AC73 File Offset: 0x00168E73
		private void ToggleShowStats(NTickbox tickbox)
		{
			this._grid.ShowStats = tickbox.IsTicked;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x0016AC86 File Offset: 0x00168E86
		private void ToggleShowUpgrades(NTickbox tickbox)
		{
			this._grid.IsShowingUpgrades = tickbox.IsTicked;
			if (!string.IsNullOrWhiteSpace(this._searchBar.Text))
			{
				this.UpdateFilter(false);
			}
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x0016ACB2 File Offset: 0x00168EB2
		private void ToggleFilterMultiplayerCards(NTickbox tickbox)
		{
			this.UpdateFilter(false);
		}

		/// <summary>
		/// The only filter in this screen that acts like radio buttons rather than tickboxes.
		/// </summary>
		/// <param name="filter"></param>
		// Token: 0x060030F0 RID: 12528 RVA: 0x0016ACBC File Offset: 0x00168EBC
		private void UpdateCardPoolFilter(NCardPoolFilter filter)
		{
			if (filter.IsSelected)
			{
				foreach (NCardPoolFilter ncardPoolFilter in this._poolFilters.Keys)
				{
					if (ncardPoolFilter != filter)
					{
						ncardPoolFilter.IsSelected = false;
					}
				}
			}
			bool flag = true;
			foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> keyValuePair in this._poolFilters)
			{
				NCardPoolFilter key = keyValuePair.Key;
				if (key.IsSelected && key != this._miscPoolFilter && key != this._ancientsFilter)
				{
					flag = false;
					break;
				}
			}
			foreach (NCardRarityTickbox ncardRarityTickbox in this._rarityFilters.Keys)
			{
				if (flag)
				{
					ncardRarityTickbox.Disable();
				}
				else
				{
					ncardRarityTickbox.Enable();
				}
			}
			this.UpdateFilter(false);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x0016ADE4 File Offset: 0x00168FE4
		private void UpdateTypeFilter(NCardTypeTickbox tickbox)
		{
			this.UpdateFilter(false);
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x0016ADED File Offset: 0x00168FED
		private void UpdateRarityFilter(NTickbox tickbox)
		{
			this.UpdateFilter(false);
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x0016ADF6 File Offset: 0x00168FF6
		private void UpdateCostFilter(NCardCostTickbox tickbox)
		{
			this.UpdateFilter(false);
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x0016ADFF File Offset: 0x00168FFF
		private void SearchBarQueryChanged(string _ = "")
		{
			this.UpdateFilter(true);
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x0016AE08 File Offset: 0x00169008
		private void SearchBarQuerySubmitted(string _ = "")
		{
			this.UpdateFilter(false);
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x0016AE14 File Offset: 0x00169014
		private void UpdateFilter(bool isTextInput = false)
		{
			List<Func<CardModel, bool>> activeRarityFilters = new List<Func<CardModel, bool>>();
			bool flag = true;
			foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> keyValuePair in this._poolFilters)
			{
				if (keyValuePair.Key.IsSelected && keyValuePair.Key != this._miscPoolFilter && keyValuePair.Key != this._ancientsFilter)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				foreach (KeyValuePair<NCardRarityTickbox, Func<CardModel, bool>> keyValuePair2 in this._rarityFilters)
				{
					NCardRarityTickbox ncardRarityTickbox;
					Func<CardModel, bool> func;
					keyValuePair2.Deconstruct(out ncardRarityTickbox, out func);
					NTickbox ntickbox = ncardRarityTickbox;
					Func<CardModel, bool> func2 = func;
					if (ntickbox.IsTicked)
					{
						activeRarityFilters.Add(func2);
					}
				}
			}
			if (activeRarityFilters.Count == 0)
			{
				activeRarityFilters.Add((CardModel _) => true);
			}
			List<Func<CardModel, bool>> activeCardTypeFilter = new List<Func<CardModel, bool>>();
			foreach (KeyValuePair<NCardTypeTickbox, Func<CardModel, bool>> keyValuePair3 in this._cardTypeFilters)
			{
				Func<CardModel, bool> func;
				NCardTypeTickbox ncardTypeTickbox;
				keyValuePair3.Deconstruct(out ncardTypeTickbox, out func);
				NCardTypeTickbox ncardTypeTickbox2 = ncardTypeTickbox;
				Func<CardModel, bool> func3 = func;
				if (ncardTypeTickbox2.IsTicked)
				{
					activeCardTypeFilter.Add(func3);
				}
			}
			if (activeCardTypeFilter.Count == 0)
			{
				activeCardTypeFilter.Add((CardModel _) => true);
			}
			List<Func<CardModel, bool>> poolFilter = new List<Func<CardModel, bool>>();
			foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> keyValuePair4 in this._poolFilters)
			{
				if (keyValuePair4.Key.IsSelected)
				{
					poolFilter.Add(keyValuePair4.Value);
				}
			}
			List<Func<CardModel, bool>> activeCostFilter = new List<Func<CardModel, bool>>();
			foreach (KeyValuePair<NCardCostTickbox, Func<CardModel, bool>> keyValuePair5 in this._costFilters)
			{
				Func<CardModel, bool> func;
				NCardCostTickbox ncardCostTickbox;
				keyValuePair5.Deconstruct(out ncardCostTickbox, out func);
				NCardCostTickbox ncardCostTickbox2 = ncardCostTickbox;
				Func<CardModel, bool> func4 = func;
				if (ncardCostTickbox2.IsTicked)
				{
					activeCostFilter.Add(func4);
				}
			}
			if (activeCostFilter.Count == 0)
			{
				activeCostFilter.Add((CardModel _) => true);
			}
			Func<CardModel, bool> multiplayerCardFilter = (CardModel c) => true;
			if (!this._viewMultiplayerCards.IsTicked)
			{
				multiplayerCardFilter = (CardModel c) => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly;
			}
			this._filter = (CardModel c) => activeCostFilter.Any<Func<CardModel, bool>>((Func<CardModel, bool> filter) => filter(c)) && activeRarityFilters.Any<Func<CardModel, bool>>((Func<CardModel, bool> filter) => filter(c)) && activeCardTypeFilter.Any<Func<CardModel, bool>>((Func<CardModel, bool> filter) => filter(c)) && poolFilter.Any<Func<CardModel, bool>>((Func<CardModel, bool> filter) => filter(c)) && base.<UpdateFilter>g__TextFilter|6(c) && multiplayerCardFilter(c);
			Task task;
			if (isTextInput)
			{
				task = this.DisplayCardsAfterShortDelay();
			}
			else
			{
				task = this.DisplayCards();
			}
			TaskHelper.RunSafely(task);
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x0016B188 File Offset: 0x00169388
		private void ShowCardDetail(NCardHolder holder)
		{
			if (!SaveManager.Instance.Progress.DiscoveredCards.Contains(holder.CardModel.Id))
			{
				return;
			}
			this._lastHoveredControl = holder;
			List<CardModel> list = this._grid.VisibleCards.Where<CardModel>((CardModel c) => SaveManager.Instance.Progress.DiscoveredCards.Contains(c.Id)).ToList<CardModel>();
			NGame.Instance.GetInspectCardScreen().Open(list, list.IndexOf(holder.CardModel), this._viewUpgrades.IsTicked);
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x0016B21A File Offset: 0x0016941A
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._lastHoveredControl ?? this._ironcladFilter;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030F9 RID: 12537 RVA: 0x0016B22C File Offset: 0x0016942C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(19)
			{
				new MethodInfo(NCardLibrary.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCardLibrary.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibrary.MethodName.OnCardTypeSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.OnRaritySort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.OnCostSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.OnAlphabetSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibrary.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibrary.MethodName.ToggleShowStats, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.ToggleShowUpgrades, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.ToggleFilterMultiplayerCards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.UpdateCardPoolFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "filter", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.UpdateTypeFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.UpdateRarityFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.UpdateCostFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.SearchBarQueryChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.SearchBarQuerySubmitted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.UpdateFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isTextInput", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardLibrary.MethodName.ShowCardDetail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030FA RID: 12538 RVA: 0x0016B87C File Offset: 0x00169A7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardLibrary.MethodName.Create && args.Count == 0)
			{
				NCardLibrary ncardLibrary = NCardLibrary.Create();
				ret = VariantUtils.CreateFrom<NCardLibrary>(ref ncardLibrary);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnCardTypeSort && args.Count == 1)
			{
				this.OnCardTypeSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnRaritySort && args.Count == 1)
			{
				this.OnRaritySort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnCostSort && args.Count == 1)
			{
				this.OnCostSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnAlphabetSort && args.Count == 1)
			{
				this.OnAlphabetSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.ToggleShowStats && args.Count == 1)
			{
				this.ToggleShowStats(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.ToggleShowUpgrades && args.Count == 1)
			{
				this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.ToggleFilterMultiplayerCards && args.Count == 1)
			{
				this.ToggleFilterMultiplayerCards(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.UpdateCardPoolFilter && args.Count == 1)
			{
				this.UpdateCardPoolFilter(VariantUtils.ConvertTo<NCardPoolFilter>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.UpdateTypeFilter && args.Count == 1)
			{
				this.UpdateTypeFilter(VariantUtils.ConvertTo<NCardTypeTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.UpdateRarityFilter && args.Count == 1)
			{
				this.UpdateRarityFilter(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.UpdateCostFilter && args.Count == 1)
			{
				this.UpdateCostFilter(VariantUtils.ConvertTo<NCardCostTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.SearchBarQueryChanged && args.Count == 1)
			{
				this.SearchBarQueryChanged(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.SearchBarQuerySubmitted && args.Count == 1)
			{
				this.SearchBarQuerySubmitted(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.UpdateFilter && args.Count == 1)
			{
				this.UpdateFilter(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibrary.MethodName.ShowCardDetail && args.Count == 1)
			{
				this.ShowCardDetail(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x0016BC2C File Offset: 0x00169E2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardLibrary.MethodName.Create && args.Count == 0)
			{
				NCardLibrary ncardLibrary = NCardLibrary.Create();
				ret = VariantUtils.CreateFrom<NCardLibrary>(ref ncardLibrary);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060030FC RID: 12540 RVA: 0x0016BC6C File Offset: 0x00169E6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardLibrary.MethodName.Create || (ref method) == NCardLibrary.MethodName._Ready || (ref method) == NCardLibrary.MethodName.OnCardTypeSort || (ref method) == NCardLibrary.MethodName.OnRaritySort || (ref method) == NCardLibrary.MethodName.OnCostSort || (ref method) == NCardLibrary.MethodName.OnAlphabetSort || (ref method) == NCardLibrary.MethodName.OnSubmenuOpened || (ref method) == NCardLibrary.MethodName.OnSubmenuClosed || (ref method) == NCardLibrary.MethodName.ToggleShowStats || (ref method) == NCardLibrary.MethodName.ToggleShowUpgrades || (ref method) == NCardLibrary.MethodName.ToggleFilterMultiplayerCards || (ref method) == NCardLibrary.MethodName.UpdateCardPoolFilter || (ref method) == NCardLibrary.MethodName.UpdateTypeFilter || (ref method) == NCardLibrary.MethodName.UpdateRarityFilter || (ref method) == NCardLibrary.MethodName.UpdateCostFilter || (ref method) == NCardLibrary.MethodName.SearchBarQueryChanged || (ref method) == NCardLibrary.MethodName.SearchBarQuerySubmitted || (ref method) == NCardLibrary.MethodName.UpdateFilter || (ref method) == NCardLibrary.MethodName.ShowCardDetail || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060030FD RID: 12541 RVA: 0x0016BDA0 File Offset: 0x00169FA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardLibrary.PropertyName._grid)
			{
				this._grid = VariantUtils.ConvertTo<NCardLibraryGrid>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._searchBar)
			{
				this._searchBar = VariantUtils.ConvertTo<NSearchBar>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._ironcladFilter)
			{
				this._ironcladFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._silentFilter)
			{
				this._silentFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._defectFilter)
			{
				this._defectFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._regentFilter)
			{
				this._regentFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._necrobinderFilter)
			{
				this._necrobinderFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._colorlessFilter)
			{
				this._colorlessFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._ancientsFilter)
			{
				this._ancientsFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._miscPoolFilter)
			{
				this._miscPoolFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._typeSorter)
			{
				this._typeSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._attackFilter)
			{
				this._attackFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._skillFilter)
			{
				this._skillFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._powerFilter)
			{
				this._powerFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._otherTypeFilter)
			{
				this._otherTypeFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._raritySorter)
			{
				this._raritySorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._commonFilter)
			{
				this._commonFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._uncommonFilter)
			{
				this._uncommonFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._rareFilter)
			{
				this._rareFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._otherFilter)
			{
				this._otherFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._costSorter)
			{
				this._costSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._zeroFilter)
			{
				this._zeroFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._oneFilter)
			{
				this._oneFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._twoFilter)
			{
				this._twoFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._threePlusFilter)
			{
				this._threePlusFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._xFilter)
			{
				this._xFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._alphabetSorter)
			{
				this._alphabetSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewMultiplayerCards)
			{
				this._viewMultiplayerCards = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewStats)
			{
				this._viewStats = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewUpgrades)
			{
				this._viewUpgrades = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._cardCountLabel)
			{
				this._cardCountLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._noResultsLabel)
			{
				this._noResultsLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._lastHoveredControl)
			{
				this._lastHoveredControl = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060030FE RID: 12542 RVA: 0x0016C130 File Offset: 0x0016A330
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardLibrary.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._grid)
			{
				value = VariantUtils.CreateFrom<NCardLibraryGrid>(ref this._grid);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._searchBar)
			{
				value = VariantUtils.CreateFrom<NSearchBar>(ref this._searchBar);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._ironcladFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._ironcladFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._silentFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._silentFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._defectFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._defectFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._regentFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._regentFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._necrobinderFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._necrobinderFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._colorlessFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._colorlessFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._ancientsFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._ancientsFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._miscPoolFilter)
			{
				value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._miscPoolFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._typeSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._typeSorter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._attackFilter)
			{
				value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._attackFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._skillFilter)
			{
				value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._skillFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._powerFilter)
			{
				value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._powerFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._otherTypeFilter)
			{
				value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._otherTypeFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._raritySorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._raritySorter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._commonFilter)
			{
				value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._commonFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._uncommonFilter)
			{
				value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._uncommonFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._rareFilter)
			{
				value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._rareFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._otherFilter)
			{
				value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._otherFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._costSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._costSorter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._zeroFilter)
			{
				value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._zeroFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._oneFilter)
			{
				value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._oneFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._twoFilter)
			{
				value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._twoFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._threePlusFilter)
			{
				value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._threePlusFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._xFilter)
			{
				value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._xFilter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._alphabetSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._alphabetSorter);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewMultiplayerCards)
			{
				value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewMultiplayerCards);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewStats)
			{
				value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewStats);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._viewUpgrades)
			{
				value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewUpgrades);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._cardCountLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardCountLabel);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._noResultsLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._noResultsLabel);
				return true;
			}
			if ((ref name) == NCardLibrary.PropertyName._lastHoveredControl)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lastHoveredControl);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030FF RID: 12543 RVA: 0x0016C588 File Offset: 0x0016A788
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardLibrary.PropertyName._grid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._searchBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._ironcladFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._silentFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._defectFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._regentFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._necrobinderFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._colorlessFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._ancientsFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._miscPoolFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._typeSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._attackFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._skillFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._powerFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._otherTypeFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._raritySorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._commonFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._uncommonFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._rareFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._otherFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._costSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._zeroFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._oneFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._twoFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._threePlusFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._xFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._alphabetSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._viewMultiplayerCards, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._viewStats, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._viewUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._cardCountLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._noResultsLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName._lastHoveredControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardLibrary.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003100 RID: 12544 RVA: 0x0016CA00 File Offset: 0x0016AC00
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardLibrary.PropertyName._grid, Variant.From<NCardLibraryGrid>(ref this._grid));
			info.AddProperty(NCardLibrary.PropertyName._searchBar, Variant.From<NSearchBar>(ref this._searchBar));
			info.AddProperty(NCardLibrary.PropertyName._ironcladFilter, Variant.From<NCardPoolFilter>(ref this._ironcladFilter));
			info.AddProperty(NCardLibrary.PropertyName._silentFilter, Variant.From<NCardPoolFilter>(ref this._silentFilter));
			info.AddProperty(NCardLibrary.PropertyName._defectFilter, Variant.From<NCardPoolFilter>(ref this._defectFilter));
			info.AddProperty(NCardLibrary.PropertyName._regentFilter, Variant.From<NCardPoolFilter>(ref this._regentFilter));
			info.AddProperty(NCardLibrary.PropertyName._necrobinderFilter, Variant.From<NCardPoolFilter>(ref this._necrobinderFilter));
			info.AddProperty(NCardLibrary.PropertyName._colorlessFilter, Variant.From<NCardPoolFilter>(ref this._colorlessFilter));
			info.AddProperty(NCardLibrary.PropertyName._ancientsFilter, Variant.From<NCardPoolFilter>(ref this._ancientsFilter));
			info.AddProperty(NCardLibrary.PropertyName._miscPoolFilter, Variant.From<NCardPoolFilter>(ref this._miscPoolFilter));
			info.AddProperty(NCardLibrary.PropertyName._typeSorter, Variant.From<NCardViewSortButton>(ref this._typeSorter));
			info.AddProperty(NCardLibrary.PropertyName._attackFilter, Variant.From<NCardTypeTickbox>(ref this._attackFilter));
			info.AddProperty(NCardLibrary.PropertyName._skillFilter, Variant.From<NCardTypeTickbox>(ref this._skillFilter));
			info.AddProperty(NCardLibrary.PropertyName._powerFilter, Variant.From<NCardTypeTickbox>(ref this._powerFilter));
			info.AddProperty(NCardLibrary.PropertyName._otherTypeFilter, Variant.From<NCardTypeTickbox>(ref this._otherTypeFilter));
			info.AddProperty(NCardLibrary.PropertyName._raritySorter, Variant.From<NCardViewSortButton>(ref this._raritySorter));
			info.AddProperty(NCardLibrary.PropertyName._commonFilter, Variant.From<NCardRarityTickbox>(ref this._commonFilter));
			info.AddProperty(NCardLibrary.PropertyName._uncommonFilter, Variant.From<NCardRarityTickbox>(ref this._uncommonFilter));
			info.AddProperty(NCardLibrary.PropertyName._rareFilter, Variant.From<NCardRarityTickbox>(ref this._rareFilter));
			info.AddProperty(NCardLibrary.PropertyName._otherFilter, Variant.From<NCardRarityTickbox>(ref this._otherFilter));
			info.AddProperty(NCardLibrary.PropertyName._costSorter, Variant.From<NCardViewSortButton>(ref this._costSorter));
			info.AddProperty(NCardLibrary.PropertyName._zeroFilter, Variant.From<NCardCostTickbox>(ref this._zeroFilter));
			info.AddProperty(NCardLibrary.PropertyName._oneFilter, Variant.From<NCardCostTickbox>(ref this._oneFilter));
			info.AddProperty(NCardLibrary.PropertyName._twoFilter, Variant.From<NCardCostTickbox>(ref this._twoFilter));
			info.AddProperty(NCardLibrary.PropertyName._threePlusFilter, Variant.From<NCardCostTickbox>(ref this._threePlusFilter));
			info.AddProperty(NCardLibrary.PropertyName._xFilter, Variant.From<NCardCostTickbox>(ref this._xFilter));
			info.AddProperty(NCardLibrary.PropertyName._alphabetSorter, Variant.From<NCardViewSortButton>(ref this._alphabetSorter));
			info.AddProperty(NCardLibrary.PropertyName._viewMultiplayerCards, Variant.From<NLibraryStatTickbox>(ref this._viewMultiplayerCards));
			info.AddProperty(NCardLibrary.PropertyName._viewStats, Variant.From<NLibraryStatTickbox>(ref this._viewStats));
			info.AddProperty(NCardLibrary.PropertyName._viewUpgrades, Variant.From<NLibraryStatTickbox>(ref this._viewUpgrades));
			info.AddProperty(NCardLibrary.PropertyName._cardCountLabel, Variant.From<MegaRichTextLabel>(ref this._cardCountLabel));
			info.AddProperty(NCardLibrary.PropertyName._noResultsLabel, Variant.From<MegaRichTextLabel>(ref this._noResultsLabel));
			info.AddProperty(NCardLibrary.PropertyName._lastHoveredControl, Variant.From<Control>(ref this._lastHoveredControl));
		}

		/// <inheritdoc />
		// Token: 0x06003101 RID: 12545 RVA: 0x0016CCEC File Offset: 0x0016AEEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardLibrary.PropertyName._grid, ref variant))
			{
				this._grid = variant.As<NCardLibraryGrid>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardLibrary.PropertyName._searchBar, ref variant2))
			{
				this._searchBar = variant2.As<NSearchBar>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardLibrary.PropertyName._ironcladFilter, ref variant3))
			{
				this._ironcladFilter = variant3.As<NCardPoolFilter>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardLibrary.PropertyName._silentFilter, ref variant4))
			{
				this._silentFilter = variant4.As<NCardPoolFilter>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardLibrary.PropertyName._defectFilter, ref variant5))
			{
				this._defectFilter = variant5.As<NCardPoolFilter>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardLibrary.PropertyName._regentFilter, ref variant6))
			{
				this._regentFilter = variant6.As<NCardPoolFilter>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardLibrary.PropertyName._necrobinderFilter, ref variant7))
			{
				this._necrobinderFilter = variant7.As<NCardPoolFilter>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardLibrary.PropertyName._colorlessFilter, ref variant8))
			{
				this._colorlessFilter = variant8.As<NCardPoolFilter>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardLibrary.PropertyName._ancientsFilter, ref variant9))
			{
				this._ancientsFilter = variant9.As<NCardPoolFilter>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardLibrary.PropertyName._miscPoolFilter, ref variant10))
			{
				this._miscPoolFilter = variant10.As<NCardPoolFilter>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardLibrary.PropertyName._typeSorter, ref variant11))
			{
				this._typeSorter = variant11.As<NCardViewSortButton>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCardLibrary.PropertyName._attackFilter, ref variant12))
			{
				this._attackFilter = variant12.As<NCardTypeTickbox>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCardLibrary.PropertyName._skillFilter, ref variant13))
			{
				this._skillFilter = variant13.As<NCardTypeTickbox>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCardLibrary.PropertyName._powerFilter, ref variant14))
			{
				this._powerFilter = variant14.As<NCardTypeTickbox>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCardLibrary.PropertyName._otherTypeFilter, ref variant15))
			{
				this._otherTypeFilter = variant15.As<NCardTypeTickbox>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCardLibrary.PropertyName._raritySorter, ref variant16))
			{
				this._raritySorter = variant16.As<NCardViewSortButton>();
			}
			Variant variant17;
			if (info.TryGetProperty(NCardLibrary.PropertyName._commonFilter, ref variant17))
			{
				this._commonFilter = variant17.As<NCardRarityTickbox>();
			}
			Variant variant18;
			if (info.TryGetProperty(NCardLibrary.PropertyName._uncommonFilter, ref variant18))
			{
				this._uncommonFilter = variant18.As<NCardRarityTickbox>();
			}
			Variant variant19;
			if (info.TryGetProperty(NCardLibrary.PropertyName._rareFilter, ref variant19))
			{
				this._rareFilter = variant19.As<NCardRarityTickbox>();
			}
			Variant variant20;
			if (info.TryGetProperty(NCardLibrary.PropertyName._otherFilter, ref variant20))
			{
				this._otherFilter = variant20.As<NCardRarityTickbox>();
			}
			Variant variant21;
			if (info.TryGetProperty(NCardLibrary.PropertyName._costSorter, ref variant21))
			{
				this._costSorter = variant21.As<NCardViewSortButton>();
			}
			Variant variant22;
			if (info.TryGetProperty(NCardLibrary.PropertyName._zeroFilter, ref variant22))
			{
				this._zeroFilter = variant22.As<NCardCostTickbox>();
			}
			Variant variant23;
			if (info.TryGetProperty(NCardLibrary.PropertyName._oneFilter, ref variant23))
			{
				this._oneFilter = variant23.As<NCardCostTickbox>();
			}
			Variant variant24;
			if (info.TryGetProperty(NCardLibrary.PropertyName._twoFilter, ref variant24))
			{
				this._twoFilter = variant24.As<NCardCostTickbox>();
			}
			Variant variant25;
			if (info.TryGetProperty(NCardLibrary.PropertyName._threePlusFilter, ref variant25))
			{
				this._threePlusFilter = variant25.As<NCardCostTickbox>();
			}
			Variant variant26;
			if (info.TryGetProperty(NCardLibrary.PropertyName._xFilter, ref variant26))
			{
				this._xFilter = variant26.As<NCardCostTickbox>();
			}
			Variant variant27;
			if (info.TryGetProperty(NCardLibrary.PropertyName._alphabetSorter, ref variant27))
			{
				this._alphabetSorter = variant27.As<NCardViewSortButton>();
			}
			Variant variant28;
			if (info.TryGetProperty(NCardLibrary.PropertyName._viewMultiplayerCards, ref variant28))
			{
				this._viewMultiplayerCards = variant28.As<NLibraryStatTickbox>();
			}
			Variant variant29;
			if (info.TryGetProperty(NCardLibrary.PropertyName._viewStats, ref variant29))
			{
				this._viewStats = variant29.As<NLibraryStatTickbox>();
			}
			Variant variant30;
			if (info.TryGetProperty(NCardLibrary.PropertyName._viewUpgrades, ref variant30))
			{
				this._viewUpgrades = variant30.As<NLibraryStatTickbox>();
			}
			Variant variant31;
			if (info.TryGetProperty(NCardLibrary.PropertyName._cardCountLabel, ref variant31))
			{
				this._cardCountLabel = variant31.As<MegaRichTextLabel>();
			}
			Variant variant32;
			if (info.TryGetProperty(NCardLibrary.PropertyName._noResultsLabel, ref variant32))
			{
				this._noResultsLabel = variant32.As<MegaRichTextLabel>();
			}
			Variant variant33;
			if (info.TryGetProperty(NCardLibrary.PropertyName._lastHoveredControl, ref variant33))
			{
				this._lastHoveredControl = variant33.As<Control>();
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x0016D09C File Offset: 0x0016B29C
		public unsafe NCardLibrary()
		{
			int num = 4;
			List<SortingOrders> list = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
			int num2 = 0;
			*span[num2] = SortingOrders.RarityAscending;
			num2++;
			*span[num2] = SortingOrders.TypeAscending;
			num2++;
			*span[num2] = SortingOrders.CostAscending;
			num2++;
			*span[num2] = SortingOrders.AlphabetAscending;
			this._sortingPriority = list;
			this._filter = (CardModel _) => true;
			base..ctor();
		}

		// Token: 0x040012F4 RID: 4852
		private const int _delayAfterTextFilterChangedMsec = 250;

		// Token: 0x040012F5 RID: 4853
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/card_library/card_library");

		// Token: 0x040012F6 RID: 4854
		private readonly LocString _cardCountLocString = new LocString("card_library", "CARD_COUNT");

		// Token: 0x040012F7 RID: 4855
		private readonly LocString _noResultsLocString = new LocString("card_library", "NO_RESULTS");

		// Token: 0x040012F8 RID: 4856
		[Nullable(2)]
		private IRunState _runState;

		// Token: 0x040012F9 RID: 4857
		private NCardLibraryGrid _grid;

		// Token: 0x040012FA RID: 4858
		private NSearchBar _searchBar;

		// Token: 0x040012FB RID: 4859
		private readonly Dictionary<string, Func<CardModel, bool>> _specialSearchbarKeywords = new Dictionary<string, Func<CardModel, bool>>();

		// Token: 0x040012FC RID: 4860
		private readonly Dictionary<CharacterModel, NCardPoolFilter> _cardPoolFilters = new Dictionary<CharacterModel, NCardPoolFilter>();

		// Token: 0x040012FD RID: 4861
		private NCardPoolFilter _ironcladFilter;

		// Token: 0x040012FE RID: 4862
		private NCardPoolFilter _silentFilter;

		// Token: 0x040012FF RID: 4863
		private NCardPoolFilter _defectFilter;

		// Token: 0x04001300 RID: 4864
		private NCardPoolFilter _regentFilter;

		// Token: 0x04001301 RID: 4865
		private NCardPoolFilter _necrobinderFilter;

		// Token: 0x04001302 RID: 4866
		private NCardPoolFilter _colorlessFilter;

		// Token: 0x04001303 RID: 4867
		private NCardPoolFilter _ancientsFilter;

		// Token: 0x04001304 RID: 4868
		private NCardPoolFilter _miscPoolFilter;

		// Token: 0x04001305 RID: 4869
		private readonly Dictionary<NCardPoolFilter, Func<CardModel, bool>> _poolFilters = new Dictionary<NCardPoolFilter, Func<CardModel, bool>>();

		// Token: 0x04001306 RID: 4870
		private NCardViewSortButton _typeSorter;

		// Token: 0x04001307 RID: 4871
		private NCardTypeTickbox _attackFilter;

		// Token: 0x04001308 RID: 4872
		private NCardTypeTickbox _skillFilter;

		// Token: 0x04001309 RID: 4873
		private NCardTypeTickbox _powerFilter;

		// Token: 0x0400130A RID: 4874
		private NCardTypeTickbox _otherTypeFilter;

		// Token: 0x0400130B RID: 4875
		private readonly Dictionary<NCardTypeTickbox, Func<CardModel, bool>> _cardTypeFilters = new Dictionary<NCardTypeTickbox, Func<CardModel, bool>>();

		// Token: 0x0400130C RID: 4876
		private NCardViewSortButton _raritySorter;

		// Token: 0x0400130D RID: 4877
		private NCardRarityTickbox _commonFilter;

		// Token: 0x0400130E RID: 4878
		private NCardRarityTickbox _uncommonFilter;

		// Token: 0x0400130F RID: 4879
		private NCardRarityTickbox _rareFilter;

		// Token: 0x04001310 RID: 4880
		private NCardRarityTickbox _otherFilter;

		// Token: 0x04001311 RID: 4881
		private readonly Dictionary<NCardRarityTickbox, Func<CardModel, bool>> _rarityFilters = new Dictionary<NCardRarityTickbox, Func<CardModel, bool>>();

		// Token: 0x04001312 RID: 4882
		private NCardViewSortButton _costSorter;

		// Token: 0x04001313 RID: 4883
		private NCardCostTickbox _zeroFilter;

		// Token: 0x04001314 RID: 4884
		private NCardCostTickbox _oneFilter;

		// Token: 0x04001315 RID: 4885
		private NCardCostTickbox _twoFilter;

		// Token: 0x04001316 RID: 4886
		private NCardCostTickbox _threePlusFilter;

		// Token: 0x04001317 RID: 4887
		private NCardCostTickbox _xFilter;

		// Token: 0x04001318 RID: 4888
		private readonly Dictionary<NCardCostTickbox, Func<CardModel, bool>> _costFilters = new Dictionary<NCardCostTickbox, Func<CardModel, bool>>();

		// Token: 0x04001319 RID: 4889
		private NCardViewSortButton _alphabetSorter;

		// Token: 0x0400131A RID: 4890
		private NLibraryStatTickbox _viewMultiplayerCards;

		// Token: 0x0400131B RID: 4891
		private NLibraryStatTickbox _viewStats;

		// Token: 0x0400131C RID: 4892
		private NLibraryStatTickbox _viewUpgrades;

		// Token: 0x0400131D RID: 4893
		private MegaRichTextLabel _cardCountLabel;

		// Token: 0x0400131E RID: 4894
		private MegaRichTextLabel _noResultsLabel;

		// Token: 0x0400131F RID: 4895
		[Nullable(2)]
		private CancellationTokenSource _displayCardsShortDelayCancelToken;

		// Token: 0x04001320 RID: 4896
		private readonly List<SortingOrders> _sortingPriority;

		// Token: 0x04001321 RID: 4897
		private Func<CardModel, bool> _filter;

		// Token: 0x04001322 RID: 4898
		[Nullable(2)]
		private Control _lastHoveredControl;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001629 RID: 5673
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005627 RID: 22055
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005628 RID: 22056
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnCardTypeSort' method.
			/// </summary>
			// Token: 0x04005629 RID: 22057
			public static readonly StringName OnCardTypeSort = "OnCardTypeSort";

			/// <summary>
			/// Cached name for the 'OnRaritySort' method.
			/// </summary>
			// Token: 0x0400562A RID: 22058
			public static readonly StringName OnRaritySort = "OnRaritySort";

			/// <summary>
			/// Cached name for the 'OnCostSort' method.
			/// </summary>
			// Token: 0x0400562B RID: 22059
			public static readonly StringName OnCostSort = "OnCostSort";

			/// <summary>
			/// Cached name for the 'OnAlphabetSort' method.
			/// </summary>
			// Token: 0x0400562C RID: 22060
			public static readonly StringName OnAlphabetSort = "OnAlphabetSort";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x0400562D RID: 22061
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x0400562E RID: 22062
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'ToggleShowStats' method.
			/// </summary>
			// Token: 0x0400562F RID: 22063
			public static readonly StringName ToggleShowStats = "ToggleShowStats";

			/// <summary>
			/// Cached name for the 'ToggleShowUpgrades' method.
			/// </summary>
			// Token: 0x04005630 RID: 22064
			public static readonly StringName ToggleShowUpgrades = "ToggleShowUpgrades";

			/// <summary>
			/// Cached name for the 'ToggleFilterMultiplayerCards' method.
			/// </summary>
			// Token: 0x04005631 RID: 22065
			public static readonly StringName ToggleFilterMultiplayerCards = "ToggleFilterMultiplayerCards";

			/// <summary>
			/// Cached name for the 'UpdateCardPoolFilter' method.
			/// </summary>
			// Token: 0x04005632 RID: 22066
			public static readonly StringName UpdateCardPoolFilter = "UpdateCardPoolFilter";

			/// <summary>
			/// Cached name for the 'UpdateTypeFilter' method.
			/// </summary>
			// Token: 0x04005633 RID: 22067
			public static readonly StringName UpdateTypeFilter = "UpdateTypeFilter";

			/// <summary>
			/// Cached name for the 'UpdateRarityFilter' method.
			/// </summary>
			// Token: 0x04005634 RID: 22068
			public static readonly StringName UpdateRarityFilter = "UpdateRarityFilter";

			/// <summary>
			/// Cached name for the 'UpdateCostFilter' method.
			/// </summary>
			// Token: 0x04005635 RID: 22069
			public static readonly StringName UpdateCostFilter = "UpdateCostFilter";

			/// <summary>
			/// Cached name for the 'SearchBarQueryChanged' method.
			/// </summary>
			// Token: 0x04005636 RID: 22070
			public static readonly StringName SearchBarQueryChanged = "SearchBarQueryChanged";

			/// <summary>
			/// Cached name for the 'SearchBarQuerySubmitted' method.
			/// </summary>
			// Token: 0x04005637 RID: 22071
			public static readonly StringName SearchBarQuerySubmitted = "SearchBarQuerySubmitted";

			/// <summary>
			/// Cached name for the 'UpdateFilter' method.
			/// </summary>
			// Token: 0x04005638 RID: 22072
			public static readonly StringName UpdateFilter = "UpdateFilter";

			/// <summary>
			/// Cached name for the 'ShowCardDetail' method.
			/// </summary>
			// Token: 0x04005639 RID: 22073
			public static readonly StringName ShowCardDetail = "ShowCardDetail";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200162A RID: 5674
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x0400563A RID: 22074
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_grid' field.
			/// </summary>
			// Token: 0x0400563B RID: 22075
			public static readonly StringName _grid = "_grid";

			/// <summary>
			/// Cached name for the '_searchBar' field.
			/// </summary>
			// Token: 0x0400563C RID: 22076
			public static readonly StringName _searchBar = "_searchBar";

			/// <summary>
			/// Cached name for the '_ironcladFilter' field.
			/// </summary>
			// Token: 0x0400563D RID: 22077
			public static readonly StringName _ironcladFilter = "_ironcladFilter";

			/// <summary>
			/// Cached name for the '_silentFilter' field.
			/// </summary>
			// Token: 0x0400563E RID: 22078
			public static readonly StringName _silentFilter = "_silentFilter";

			/// <summary>
			/// Cached name for the '_defectFilter' field.
			/// </summary>
			// Token: 0x0400563F RID: 22079
			public static readonly StringName _defectFilter = "_defectFilter";

			/// <summary>
			/// Cached name for the '_regentFilter' field.
			/// </summary>
			// Token: 0x04005640 RID: 22080
			public static readonly StringName _regentFilter = "_regentFilter";

			/// <summary>
			/// Cached name for the '_necrobinderFilter' field.
			/// </summary>
			// Token: 0x04005641 RID: 22081
			public static readonly StringName _necrobinderFilter = "_necrobinderFilter";

			/// <summary>
			/// Cached name for the '_colorlessFilter' field.
			/// </summary>
			// Token: 0x04005642 RID: 22082
			public static readonly StringName _colorlessFilter = "_colorlessFilter";

			/// <summary>
			/// Cached name for the '_ancientsFilter' field.
			/// </summary>
			// Token: 0x04005643 RID: 22083
			public static readonly StringName _ancientsFilter = "_ancientsFilter";

			/// <summary>
			/// Cached name for the '_miscPoolFilter' field.
			/// </summary>
			// Token: 0x04005644 RID: 22084
			public static readonly StringName _miscPoolFilter = "_miscPoolFilter";

			/// <summary>
			/// Cached name for the '_typeSorter' field.
			/// </summary>
			// Token: 0x04005645 RID: 22085
			public static readonly StringName _typeSorter = "_typeSorter";

			/// <summary>
			/// Cached name for the '_attackFilter' field.
			/// </summary>
			// Token: 0x04005646 RID: 22086
			public static readonly StringName _attackFilter = "_attackFilter";

			/// <summary>
			/// Cached name for the '_skillFilter' field.
			/// </summary>
			// Token: 0x04005647 RID: 22087
			public static readonly StringName _skillFilter = "_skillFilter";

			/// <summary>
			/// Cached name for the '_powerFilter' field.
			/// </summary>
			// Token: 0x04005648 RID: 22088
			public static readonly StringName _powerFilter = "_powerFilter";

			/// <summary>
			/// Cached name for the '_otherTypeFilter' field.
			/// </summary>
			// Token: 0x04005649 RID: 22089
			public static readonly StringName _otherTypeFilter = "_otherTypeFilter";

			/// <summary>
			/// Cached name for the '_raritySorter' field.
			/// </summary>
			// Token: 0x0400564A RID: 22090
			public static readonly StringName _raritySorter = "_raritySorter";

			/// <summary>
			/// Cached name for the '_commonFilter' field.
			/// </summary>
			// Token: 0x0400564B RID: 22091
			public static readonly StringName _commonFilter = "_commonFilter";

			/// <summary>
			/// Cached name for the '_uncommonFilter' field.
			/// </summary>
			// Token: 0x0400564C RID: 22092
			public static readonly StringName _uncommonFilter = "_uncommonFilter";

			/// <summary>
			/// Cached name for the '_rareFilter' field.
			/// </summary>
			// Token: 0x0400564D RID: 22093
			public static readonly StringName _rareFilter = "_rareFilter";

			/// <summary>
			/// Cached name for the '_otherFilter' field.
			/// </summary>
			// Token: 0x0400564E RID: 22094
			public static readonly StringName _otherFilter = "_otherFilter";

			/// <summary>
			/// Cached name for the '_costSorter' field.
			/// </summary>
			// Token: 0x0400564F RID: 22095
			public static readonly StringName _costSorter = "_costSorter";

			/// <summary>
			/// Cached name for the '_zeroFilter' field.
			/// </summary>
			// Token: 0x04005650 RID: 22096
			public static readonly StringName _zeroFilter = "_zeroFilter";

			/// <summary>
			/// Cached name for the '_oneFilter' field.
			/// </summary>
			// Token: 0x04005651 RID: 22097
			public static readonly StringName _oneFilter = "_oneFilter";

			/// <summary>
			/// Cached name for the '_twoFilter' field.
			/// </summary>
			// Token: 0x04005652 RID: 22098
			public static readonly StringName _twoFilter = "_twoFilter";

			/// <summary>
			/// Cached name for the '_threePlusFilter' field.
			/// </summary>
			// Token: 0x04005653 RID: 22099
			public static readonly StringName _threePlusFilter = "_threePlusFilter";

			/// <summary>
			/// Cached name for the '_xFilter' field.
			/// </summary>
			// Token: 0x04005654 RID: 22100
			public static readonly StringName _xFilter = "_xFilter";

			/// <summary>
			/// Cached name for the '_alphabetSorter' field.
			/// </summary>
			// Token: 0x04005655 RID: 22101
			public static readonly StringName _alphabetSorter = "_alphabetSorter";

			/// <summary>
			/// Cached name for the '_viewMultiplayerCards' field.
			/// </summary>
			// Token: 0x04005656 RID: 22102
			public static readonly StringName _viewMultiplayerCards = "_viewMultiplayerCards";

			/// <summary>
			/// Cached name for the '_viewStats' field.
			/// </summary>
			// Token: 0x04005657 RID: 22103
			public static readonly StringName _viewStats = "_viewStats";

			/// <summary>
			/// Cached name for the '_viewUpgrades' field.
			/// </summary>
			// Token: 0x04005658 RID: 22104
			public static readonly StringName _viewUpgrades = "_viewUpgrades";

			/// <summary>
			/// Cached name for the '_cardCountLabel' field.
			/// </summary>
			// Token: 0x04005659 RID: 22105
			public static readonly StringName _cardCountLabel = "_cardCountLabel";

			/// <summary>
			/// Cached name for the '_noResultsLabel' field.
			/// </summary>
			// Token: 0x0400565A RID: 22106
			public static readonly StringName _noResultsLabel = "_noResultsLabel";

			/// <summary>
			/// Cached name for the '_lastHoveredControl' field.
			/// </summary>
			// Token: 0x0400565B RID: 22107
			public static readonly StringName _lastHoveredControl = "_lastHoveredControl";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200162B RID: 5675
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
