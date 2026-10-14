using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	/// <summary>
	/// Inherits from NCardGrid, with functionality to help perf for the card library.
	/// </summary>
	// Token: 0x02000352 RID: 850
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibraryGrid.cs")]
	public class NCardLibraryGrid : NCardGrid
	{
		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600311A RID: 12570 RVA: 0x0016D2D7 File Offset: 0x0016B4D7
		protected override bool IsCardLibrary
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x0016D2DA File Offset: 0x0016B4DA
		protected override bool CenterGrid
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x0016D2DD File Offset: 0x0016B4DD
		// (set) Token: 0x0600311D RID: 12573 RVA: 0x0016D2E8 File Offset: 0x0016B4E8
		public bool ShowStats
		{
			get
			{
				return this._showStats;
			}
			set
			{
				this._showStats = value;
				foreach (NGridCardHolder ngridCardHolder in this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> r) => r))
				{
					if (ngridCardHolder.CardNode != null)
					{
						NCardLibraryStats node = ngridCardHolder.GetNode<NCardLibraryStats>("CardLibraryStats");
						node.Visible = this._showStats;
					}
				}
			}
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x0016D380 File Offset: 0x0016B580
		public override void _Ready()
		{
			this.ConnectSignals();
			List<CardPoolModel> list = ModelDb.AllCardPools.ToList<CardPoolModel>();
			foreach (CardModel cardModel in ModelDb.AllCards)
			{
				if (cardModel.ShouldShowInCardLibrary)
				{
					this._allCards.Add(cardModel);
				}
			}
			this._allCards.Sort(new NCardLibraryGrid.InitialSorter(list));
			this.RefreshVisibility();
		}

		/// <summary>
		/// Refreshes the seen/unlocked card sets. Call when opening the card library
		/// to pick up any cards discovered since the last visit.
		/// </summary>
		// Token: 0x0600311F RID: 12575 RVA: 0x0016D408 File Offset: 0x0016B608
		public void RefreshVisibility()
		{
			this._seenCards = SaveManager.Instance.Progress.DiscoveredCards.ToHashSet<ModelId>();
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			this._unlockedCards = ModelDb.AllCardPools.Select<CardPoolModel, IEnumerable<CardModel>>((CardPoolModel p) => p.GetUnlockedCards(unlockState, CardMultiplayerConstraint.None)).SelectMany<IEnumerable<CardModel>, CardModel>((IEnumerable<CardModel> c) => c).ToHashSet<CardModel>();
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x0016D48C File Offset: 0x0016B68C
		public unsafe void FilterCards(Func<CardModel, bool> filter)
		{
			int num = 1;
			List<SortingOrders> list = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
			int num2 = 0;
			*span[num2] = SortingOrders.AlphabetAscending;
			this.FilterCards(filter, list);
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0016D4C4 File Offset: 0x0016B6C4
		public void FilterCards(Func<CardModel, bool> filter, List<SortingOrders> sortingPriority)
		{
			List<CardModel> list = this._allCards.Where<CardModel>(filter).ToList<CardModel>();
			this.DisplayCards(list, sortingPriority);
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0016D4EB File Offset: 0x0016B6EB
		private void DisplayCards(List<CardModel> cards, List<SortingOrders> sortingPriority)
		{
			base.SetCards(cards, PileType.None, sortingPriority, Task.CompletedTask);
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x0016D4FC File Offset: 0x0016B6FC
		protected override void InitGrid()
		{
			base.InitGrid();
			foreach (NGridCardHolder ngridCardHolder in this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> r) => r))
			{
				if (ngridCardHolder.CardNode != null)
				{
					CardModel model = ngridCardHolder.CardNode.Model;
					bool flag = this._seenCards.Contains(model.Id);
					ngridCardHolder.EnsureCardLibraryStatsExists();
					NCardLibraryStats cardLibraryStats = ngridCardHolder.CardLibraryStats;
					cardLibraryStats.UpdateStats(ngridCardHolder.CardNode.Model);
					cardLibraryStats.Visible = this.ShowStats;
					ngridCardHolder.Hitbox.MouseDefaultCursorShape = (flag ? 16L : 0L);
				}
			}
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x0016D5D4 File Offset: 0x0016B7D4
		protected override void AssignCardsToRow(List<NGridCardHolder> row, int startIndex)
		{
			base.AssignCardsToRow(row, startIndex);
			foreach (NGridCardHolder ngridCardHolder in row)
			{
				if (ngridCardHolder.CardNode != null)
				{
					CardModel model = ngridCardHolder.CardNode.Model;
					bool flag = this._seenCards.Contains(model.Id);
					ngridCardHolder.CardLibraryStats.UpdateStats(model);
					ngridCardHolder.Hitbox.MouseDefaultCursorShape = (flag ? 16L : 0L);
				}
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x0016D66C File Offset: 0x0016B86C
		protected override ModelVisibility GetCardVisibility(CardModel card)
		{
			if (!this._unlockedCards.Contains(card))
			{
				return ModelVisibility.Locked;
			}
			if (!this._seenCards.Contains(card.Id))
			{
				return ModelVisibility.NotSeen;
			}
			return ModelVisibility.Visible;
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x0016D694 File Offset: 0x0016B894
		public IEnumerable<CardModel> VisibleCards
		{
			get
			{
				return this._cards;
			}
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x0016D69C File Offset: 0x0016B89C
		protected override void UpdateGridNavigation()
		{
			for (int i = 0; i < this._cardRows.Count; i++)
			{
				for (int j = 0; j < this._cardRows[i].Count; j++)
				{
					NCardHolder ncardHolder = this._cardRows[i][j];
					ncardHolder.FocusNeighborLeft = ((j > 0) ? this._cardRows[i][j - 1].GetPath() : null);
					ncardHolder.FocusNeighborRight = ((j < this._cardRows[i].Count - 1) ? this._cardRows[i][j + 1].GetPath() : null);
					ncardHolder.FocusNeighborTop = ((i > 0) ? this._cardRows[i - 1][j].GetPath() : null);
					ncardHolder.FocusNeighborBottom = ((i < this._cardRows.Count - 1 && j < this._cardRows[i + 1].Count) ? this._cardRows[i + 1][j].GetPath() : null);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003128 RID: 12584 RVA: 0x0016D7C8 File Offset: 0x0016B9C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardLibraryGrid.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibraryGrid.MethodName.RefreshVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibraryGrid.MethodName.InitGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardLibraryGrid.MethodName.UpdateGridNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003129 RID: 12585 RVA: 0x0016D89C File Offset: 0x0016BA9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardLibraryGrid.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibraryGrid.MethodName.RefreshVisibility && args.Count == 0)
			{
				this.RefreshVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibraryGrid.MethodName.InitGrid && args.Count == 0)
			{
				this.InitGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardLibraryGrid.MethodName.UpdateGridNavigation && args.Count == 0)
			{
				this.UpdateGridNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600312A RID: 12586 RVA: 0x0016D948 File Offset: 0x0016BB48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardLibraryGrid.MethodName._Ready || (ref method) == NCardLibraryGrid.MethodName.RefreshVisibility || (ref method) == NCardLibraryGrid.MethodName.InitGrid || (ref method) == NCardLibraryGrid.MethodName.UpdateGridNavigation || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600312B RID: 12587 RVA: 0x0016D998 File Offset: 0x0016BB98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardLibraryGrid.PropertyName.ShowStats)
			{
				this.ShowStats = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardLibraryGrid.PropertyName._showStats)
			{
				this._showStats = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600312C RID: 12588 RVA: 0x0016D9D8 File Offset: 0x0016BBD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardLibraryGrid.PropertyName.IsCardLibrary)
			{
				bool flag = this.IsCardLibrary;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardLibraryGrid.PropertyName.CenterGrid)
			{
				bool flag = this.CenterGrid;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardLibraryGrid.PropertyName.ShowStats)
			{
				bool flag = this.ShowStats;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardLibraryGrid.PropertyName._showStats)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._showStats);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600312D RID: 12589 RVA: 0x0016DA78 File Offset: 0x0016BC78
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCardLibraryGrid.PropertyName.IsCardLibrary, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardLibraryGrid.PropertyName.CenterGrid, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardLibraryGrid.PropertyName._showStats, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardLibraryGrid.PropertyName.ShowStats, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600312E RID: 12590 RVA: 0x0016DB0C File Offset: 0x0016BD0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName showStats = NCardLibraryGrid.PropertyName.ShowStats;
			bool showStats2 = this.ShowStats;
			info.AddProperty(showStats, Variant.From<bool>(ref showStats2));
			info.AddProperty(NCardLibraryGrid.PropertyName._showStats, Variant.From<bool>(ref this._showStats));
		}

		/// <inheritdoc />
		// Token: 0x0600312F RID: 12591 RVA: 0x0016DB50 File Offset: 0x0016BD50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardLibraryGrid.PropertyName.ShowStats, ref variant))
			{
				this.ShowStats = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardLibraryGrid.PropertyName._showStats, ref variant2))
			{
				this._showStats = variant2.As<bool>();
			}
		}

		// Token: 0x04001323 RID: 4899
		private readonly List<CardModel> _allCards = new List<CardModel>();

		// Token: 0x04001324 RID: 4900
		private HashSet<ModelId> _seenCards;

		// Token: 0x04001325 RID: 4901
		private HashSet<CardModel> _unlockedCards;

		// Token: 0x04001326 RID: 4902
		private bool _showStats;

		// Token: 0x02001632 RID: 5682
		[Nullable(0)]
		private struct InitialSorter : IComparer<CardModel>
		{
			// Token: 0x0600A442 RID: 42050 RVA: 0x003444C2 File Offset: 0x003426C2
			public InitialSorter(List<CardPoolModel> cardPoolModels)
			{
				this._cardPoolModels = cardPoolModels;
			}

			// Token: 0x0600A443 RID: 42051 RVA: 0x003444CC File Offset: 0x003426CC
			[NullableContext(2)]
			public int Compare(CardModel x, CardModel y)
			{
				if (x == null)
				{
					if (y != null)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (y == null)
					{
						return 1;
					}
					int num = this._cardPoolModels.IndexOf(x.Pool).CompareTo(this._cardPoolModels.IndexOf(y.Pool));
					if (num != 0)
					{
						return num;
					}
					int num2 = x.Rarity.CompareTo(y.Rarity);
					if (num2 != 0)
					{
						return num2;
					}
					return x.Id.CompareTo(y.Id);
				}
			}

			// Token: 0x0400568B RID: 22155
			private List<CardPoolModel> _cardPoolModels;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001633 RID: 5683
		[NullableContext(0)]
		public new class MethodName : NCardGrid.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400568C RID: 22156
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshVisibility' method.
			/// </summary>
			// Token: 0x0400568D RID: 22157
			public static readonly StringName RefreshVisibility = "RefreshVisibility";

			/// <summary>
			/// Cached name for the 'InitGrid' method.
			/// </summary>
			// Token: 0x0400568E RID: 22158
			public new static readonly StringName InitGrid = "InitGrid";

			/// <summary>
			/// Cached name for the 'UpdateGridNavigation' method.
			/// </summary>
			// Token: 0x0400568F RID: 22159
			public new static readonly StringName UpdateGridNavigation = "UpdateGridNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001634 RID: 5684
		[NullableContext(0)]
		public new class PropertyName : NCardGrid.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsCardLibrary' property.
			/// </summary>
			// Token: 0x04005690 RID: 22160
			public new static readonly StringName IsCardLibrary = "IsCardLibrary";

			/// <summary>
			/// Cached name for the 'CenterGrid' property.
			/// </summary>
			// Token: 0x04005691 RID: 22161
			public new static readonly StringName CenterGrid = "CenterGrid";

			/// <summary>
			/// Cached name for the 'ShowStats' property.
			/// </summary>
			// Token: 0x04005692 RID: 22162
			public static readonly StringName ShowStats = "ShowStats";

			/// <summary>
			/// Cached name for the '_showStats' field.
			/// </summary>
			// Token: 0x04005693 RID: 22163
			public static readonly StringName _showStats = "_showStats";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001635 RID: 5685
		[NullableContext(0)]
		public new class SignalName : NCardGrid.SignalName
		{
		}
	}
}
