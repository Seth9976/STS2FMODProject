using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE1 RID: 2785
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpoilsMap : CardModel
	{
		// Token: 0x060077CA RID: 30666 RVA: 0x00298619 File Offset: 0x00296819
		public SpoilsMap()
			: base(-1, CardType.Quest, CardRarity.Quest, TargetType.Self, true)
		{
		}

		// Token: 0x1700208B RID: 8331
		// (get) Token: 0x060077CB RID: 30667 RVA: 0x0029862E File Offset: 0x0029682E
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700208C RID: 8332
		// (get) Token: 0x060077CC RID: 30668 RVA: 0x00298631 File Offset: 0x00296831
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(600));
			}
		}

		// Token: 0x1700208D RID: 8333
		// (get) Token: 0x060077CD RID: 30669 RVA: 0x00298642 File Offset: 0x00296842
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x1700208E RID: 8334
		// (get) Token: 0x060077CE RID: 30670 RVA: 0x0029864A File Offset: 0x0029684A
		// (set) Token: 0x060077CF RID: 30671 RVA: 0x00298652 File Offset: 0x00296852
		[SavedProperty]
		public int SpoilsActIndex
		{
			get
			{
				return this._spoilsActIndex;
			}
			set
			{
				base.AssertMutable();
				this._spoilsActIndex = value;
			}
		}

		// Token: 0x1700208F RID: 8335
		// (get) Token: 0x060077D0 RID: 30672 RVA: 0x00298661 File Offset: 0x00296861
		// (set) Token: 0x060077D1 RID: 30673 RVA: 0x00298669 File Offset: 0x00296869
		public MapCoord? SpoilsCoord { get; private set; }

		// Token: 0x060077D2 RID: 30674 RVA: 0x00298672 File Offset: 0x00296872
		public override void AfterCreated()
		{
			this.SpoilsActIndex = 1;
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x0029867B File Offset: 0x0029687B
		public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
		{
			if (actIndex != this.SpoilsActIndex)
			{
				return map;
			}
			CardPile pile = base.Pile;
			if (pile == null || pile.Type != PileType.Deck)
			{
				return map;
			}
			return new SpoilsActMap(runState, null);
		}

		// Token: 0x060077D4 RID: 30676 RVA: 0x002986AC File Offset: 0x002968AC
		public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
		{
			if (actIndex != this.SpoilsActIndex)
			{
				return map;
			}
			CardPile pile = base.Pile;
			if (pile == null || pile.Type != PileType.Deck)
			{
				return map;
			}
			MapPoint mapPoint = map.GetAllMapPoints().FirstOrDefault<MapPoint>((MapPoint p) => p.PointType == MapPointType.Treasure);
			if (mapPoint != null)
			{
				this.SpoilsCoord = new MapCoord?(mapPoint.coord);
			}
			return map;
		}

		// Token: 0x060077D5 RID: 30677 RVA: 0x00298720 File Offset: 0x00296920
		public override Task AfterMapGenerated(ActMap map, int actIndex)
		{
			CardPile pile = base.Pile;
			if (pile == null || pile.Type != PileType.Deck)
			{
				return Task.CompletedTask;
			}
			if (actIndex != this.SpoilsActIndex)
			{
				return Task.CompletedTask;
			}
			if (this.SpoilsCoord != null && map.HasPoint(this.SpoilsCoord.Value))
			{
				MapPoint point = map.GetPoint(this.SpoilsCoord.Value);
				if (point != null)
				{
					point.AddQuest(this);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x002987A8 File Offset: 0x002969A8
		public override Task BeforeCardRemoved(CardModel card)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			if (this.SpoilsActIndex != base.Owner.RunState.CurrentActIndex)
			{
				return Task.CompletedTask;
			}
			if (this.SpoilsCoord == null)
			{
				return Task.CompletedTask;
			}
			MapPoint point = base.Owner.RunState.Map.GetPoint(this.SpoilsCoord.Value);
			if (point != null)
			{
				point.RemoveQuest(this);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x00298828 File Offset: 0x00296A28
		public async Task<int> OnQuestComplete()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			PlayerCmd.CompleteQuest(this);
			await CardPileCmd.RemoveFromDeck(this, true);
			return base.DynamicVars.Gold.IntValue;
		}

		// Token: 0x040028C6 RID: 10438
		private int _spoilsActIndex = -1;
	}
}
