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
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	// Token: 0x0200034A RID: 842
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCombatPileCardSelectScreen.cs")]
	public sealed class NCombatPileCardSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x00162123 File Offset: 0x00160323
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/combat_pile_card_select_screen");
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x0016212F File Offset: 0x0016032F
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCombatPileCardSelectScreen.ScenePath);
			}
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x0016213C File Offset: 0x0016033C
		public static NCombatPileCardSelectScreen Create(CardPile pile, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter)
		{
			NCombatPileCardSelectScreen ncombatPileCardSelectScreen = PreloadManager.Cache.GetScene(NCombatPileCardSelectScreen.ScenePath).Instantiate<NCombatPileCardSelectScreen>(0L);
			ncombatPileCardSelectScreen.Name = "NCombatPileCardSelectScreen";
			ncombatPileCardSelectScreen._cardResults = null;
			ncombatPileCardSelectScreen._prefs = prefs;
			ncombatPileCardSelectScreen._cards = Array.Empty<CardModel>();
			ncombatPileCardSelectScreen._pile = pile;
			ncombatPileCardSelectScreen._filter = filter;
			return ncombatPileCardSelectScreen;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x00162198 File Offset: 0x00160398
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._confirmButton = base.GetNode<NConfirmButton>("%Confirm");
			this._bottomTextContainer = base.GetNode<Control>("%BottomText");
			this._infoLabel = this._bottomTextContainer.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
			this.UpdatePileContents();
			if (this._prefs.MinSelect == 0)
			{
				this._confirmButton.Enable();
			}
			else
			{
				this._confirmButton.Disable();
			}
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.CompleteSelection();
			}), 0U);
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x0016225B File Offset: 0x0016045B
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
			this._pile.ContentsChanged += this.UpdatePileContents;
			this._isSubscribedToPile = true;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x00162286 File Offset: 0x00160486
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cts.Cancel();
			this.UnsubscribeFromPile();
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x001622A0 File Offset: 0x001604A0
		protected override void ConnectSignalsAndInitGrid()
		{
			base.ConnectSignalsAndInitGrid();
			this._combatPiles = base.GetNode<NCombatPilesContainer>("%CombatPiles");
			if (CombatManager.Instance.IsInProgress)
			{
				this._combatPiles.Initialize(this._pile.Cards.First<CardModel>().Owner);
			}
			this._combatPiles.Disable();
			this._combatPiles.SetVisible(false);
			this._peekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(delegate(NPeekButton _)
			{
				if (this._peekButton.IsPeeking)
				{
					this._combatPiles.Enable();
					this._combatPiles.SetVisible(true);
					return;
				}
				this._combatPiles.Disable();
				this._combatPiles.SetVisible(false);
			}), 0U);
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x0016232F File Offset: 0x0016052F
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<Control>(this._bottomTextContainer);
			}
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x0016233C File Offset: 0x0016053C
		public override void AfterOverlayOpened()
		{
			base.AfterOverlayOpened();
			TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x00162350 File Offset: 0x00160550
		private async Task FlashRelicsOnModifiedCards()
		{
			if (this._cardResults != null)
			{
				await this.AwaitProcessFrame(this._cts.Token);
				using (List<CardCreationResult>.Enumerator enumerator = this._cardResults.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardCreationResult result = enumerator.Current;
						NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((NGridCardHolder h) => h.CardModel == result.Card);
						if (ngridCardHolder != null && result.HasBeenModified)
						{
							foreach (RelicModel relicModel in result.ModifyingRelics)
							{
								relicModel.Flash();
								NCard cardNode = ngridCardHolder.CardNode;
								if (cardNode != null)
								{
									cardNode.FlashRelicOnCard(relicModel);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x00162394 File Offset: 0x00160594
		protected override void OnCardClicked(CardModel card)
		{
			if (this._selectedCards.Contains(card))
			{
				this._grid.UnhighlightCard(card);
				this._selectedCards.Remove(card);
			}
			else
			{
				if (this._selectedCards.Count < this._prefs.MaxSelect)
				{
					this._grid.HighlightCard(card);
					this._selectedCards.Add(card);
				}
				if (!this._prefs.RequireManualConfirmation)
				{
					this.CheckIfSelectionComplete();
				}
			}
			this.UpdateConfirmButton();
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x00162414 File Offset: 0x00160614
		private void UpdateConfirmButton()
		{
			int num = Mathf.Min(this._prefs.MinSelect, this._grid.CurrentlyDisplayedCards.Count<CardModel>());
			if (this._selectedCards.Count >= num && this._prefs.RequireManualConfirmation)
			{
				this._confirmButton.Enable();
				return;
			}
			this._confirmButton.Disable();
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x00162474 File Offset: 0x00160674
		private void CheckIfSelectionComplete()
		{
			int num = Mathf.Min(this._prefs.MaxSelect, this._grid.CurrentlyDisplayedCards.Count<CardModel>());
			if (this._selectedCards.Count >= num)
			{
				this.CompleteSelection();
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x001624B6 File Offset: 0x001606B6
		private void CompleteSelection()
		{
			this.UnsubscribeFromPile();
			this._completionSource.SetResult(this._selectedCards);
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x001624DA File Offset: 0x001606DA
		private void UnsubscribeFromPile()
		{
			if (this._isSubscribedToPile)
			{
				this._pile.ContentsChanged -= this.UpdatePileContents;
				this._isSubscribedToPile = false;
			}
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00162504 File Offset: 0x00160704
		private unsafe void UpdatePileContents()
		{
			NCombatPileCardSelectScreen.<>c__DisplayClass29_0 CS$<>8__locals1 = new NCombatPileCardSelectScreen.<>c__DisplayClass29_0();
			NCombatPileCardSelectScreen.<>c__DisplayClass29_0 CS$<>8__locals2 = CS$<>8__locals1;
			IReadOnlyList<CardModel> readOnlyList;
			if (this._filter == null)
			{
				readOnlyList = this._pile.Cards;
			}
			else
			{
				IReadOnlyList<CardModel> readOnlyList2 = this._pile.Cards.Where<CardModel>(this._filter).ToList<CardModel>();
				readOnlyList = readOnlyList2;
			}
			CS$<>8__locals2.validPileCards = readOnlyList;
			this._selectedCards = this._selectedCards.Where<CardModel>((CardModel c) => CS$<>8__locals1.validPileCards.Contains(c)).ToHashSet<CardModel>();
			if (CS$<>8__locals1.validPileCards.Count == 0)
			{
				this._selectedCards.Clear();
				this.CompleteSelection();
				return;
			}
			if (CS$<>8__locals1.validPileCards.Count == this._selectedCards.Count)
			{
				this.CompleteSelection();
				return;
			}
			List<SortingOrders> list2;
			if (this._pile.Type != PileType.Draw)
			{
				int num = 1;
				List<SortingOrders> list = new List<SortingOrders>(num);
				CollectionsMarshal.SetCount<SortingOrders>(list, num);
				list2 = list;
				Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
				int num2 = 0;
				*span[num2] = SortingOrders.Ascending;
			}
			else
			{
				int num2 = 2;
				List<SortingOrders> list3 = new List<SortingOrders>(num2);
				CollectionsMarshal.SetCount<SortingOrders>(list3, num2);
				list2 = list3;
				Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list3);
				int num = 0;
				*span[num] = SortingOrders.RarityAscending;
				num++;
				*span[num] = SortingOrders.AlphabetAscending;
			}
			List<SortingOrders> list4 = list2;
			this._grid.SetCards(CS$<>8__locals1.validPileCards, this._pile.Type, list4, null);
			this.UpdateConfirmButton();
			foreach (CardModel cardModel in this._selectedCards)
			{
				this._grid.HighlightCard(cardModel);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003041 RID: 12353 RVA: 0x0016268C File Offset: 0x0016088C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NCombatPileCardSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.CompleteSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPileCardSelectScreen.MethodName.UpdatePileContents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003042 RID: 12354 RVA: 0x00162878 File Offset: 0x00160A78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatPileCardSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid && args.Count == 0)
			{
				this.ConnectSignalsAndInitGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton && args.Count == 0)
			{
				this.UpdateConfirmButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.CompleteSelection && args.Count == 0)
			{
				this.CompleteSelection();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile && args.Count == 0)
			{
				this.UnsubscribeFromPile();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPileCardSelectScreen.MethodName.UpdatePileContents && args.Count == 0)
			{
				this.UpdatePileContents();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003043 RID: 12355 RVA: 0x00162A00 File Offset: 0x00160C00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatPileCardSelectScreen.MethodName._Ready || (ref method) == NCombatPileCardSelectScreen.MethodName._EnterTree || (ref method) == NCombatPileCardSelectScreen.MethodName._ExitTree || (ref method) == NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid || (ref method) == NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened || (ref method) == NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton || (ref method) == NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete || (ref method) == NCombatPileCardSelectScreen.MethodName.CompleteSelection || (ref method) == NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile || (ref method) == NCombatPileCardSelectScreen.MethodName.UpdatePileContents || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003044 RID: 12356 RVA: 0x00162AAC File Offset: 0x00160CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._bottomTextContainer)
			{
				this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._combatPiles)
			{
				this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile)
			{
				this._isSubscribedToPile = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003045 RID: 12357 RVA: 0x00162B48 File Offset: 0x00160D48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._bottomTextContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._combatPiles)
			{
				value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
				return true;
			}
			if ((ref name) == NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSubscribedToPile);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003046 RID: 12358 RVA: 0x00162C00 File Offset: 0x00160E00
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPileCardSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPileCardSelectScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPileCardSelectScreen.PropertyName._combatPiles, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003047 RID: 12359 RVA: 0x00162CB8 File Offset: 0x00160EB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
			info.AddProperty(NCombatPileCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NCombatPileCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NCombatPileCardSelectScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
			info.AddProperty(NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, Variant.From<bool>(ref this._isSubscribedToPile));
		}

		/// <inheritdoc />
		// Token: 0x06003048 RID: 12360 RVA: 0x00162D3C File Offset: 0x00160F3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, ref variant))
			{
				this._bottomTextContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._infoLabel, ref variant2))
			{
				this._infoLabel = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._confirmButton, ref variant3))
			{
				this._confirmButton = variant3.As<NConfirmButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._combatPiles, ref variant4))
			{
				this._combatPiles = variant4.As<NCombatPilesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, ref variant5))
			{
				this._isSubscribedToPile = variant5.As<bool>();
			}
		}

		// Token: 0x0400129B RID: 4763
		private Control _bottomTextContainer;

		// Token: 0x0400129C RID: 4764
		private MegaRichTextLabel _infoLabel;

		// Token: 0x0400129D RID: 4765
		private NConfirmButton _confirmButton;

		// Token: 0x0400129E RID: 4766
		private NCombatPilesContainer _combatPiles;

		// Token: 0x0400129F RID: 4767
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040012A0 RID: 4768
		private HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012A1 RID: 4769
		private CardSelectorPrefs _prefs;

		// Token: 0x040012A2 RID: 4770
		private CardPile _pile;

		// Token: 0x040012A3 RID: 4771
		[Nullable(new byte[] { 2, 1 })]
		private Func<CardModel, bool> _filter;

		// Token: 0x040012A4 RID: 4772
		[Nullable(new byte[] { 2, 1 })]
		private List<CardCreationResult> _cardResults;

		// Token: 0x040012A5 RID: 4773
		private bool _isSubscribedToPile;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200160D RID: 5645
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005594 RID: 21908
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005595 RID: 21909
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005596 RID: 21910
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectSignalsAndInitGrid' method.
			/// </summary>
			// Token: 0x04005597 RID: 21911
			public new static readonly StringName ConnectSignalsAndInitGrid = "ConnectSignalsAndInitGrid";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005598 RID: 21912
			public new static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'UpdateConfirmButton' method.
			/// </summary>
			// Token: 0x04005599 RID: 21913
			public static readonly StringName UpdateConfirmButton = "UpdateConfirmButton";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x0400559A RID: 21914
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";

			/// <summary>
			/// Cached name for the 'CompleteSelection' method.
			/// </summary>
			// Token: 0x0400559B RID: 21915
			public static readonly StringName CompleteSelection = "CompleteSelection";

			/// <summary>
			/// Cached name for the 'UnsubscribeFromPile' method.
			/// </summary>
			// Token: 0x0400559C RID: 21916
			public static readonly StringName UnsubscribeFromPile = "UnsubscribeFromPile";

			/// <summary>
			/// Cached name for the 'UpdatePileContents' method.
			/// </summary>
			// Token: 0x0400559D RID: 21917
			public static readonly StringName UpdatePileContents = "UpdatePileContents";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200160E RID: 5646
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bottomTextContainer' field.
			/// </summary>
			// Token: 0x0400559E RID: 21918
			public static readonly StringName _bottomTextContainer = "_bottomTextContainer";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x0400559F RID: 21919
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040055A0 RID: 21920
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_combatPiles' field.
			/// </summary>
			// Token: 0x040055A1 RID: 21921
			public static readonly StringName _combatPiles = "_combatPiles";

			/// <summary>
			/// Cached name for the '_isSubscribedToPile' field.
			/// </summary>
			// Token: 0x040055A2 RID: 21922
			public static readonly StringName _isSubscribedToPile = "_isSubscribedToPile";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200160F RID: 5647
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
