using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070D RID: 1805
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwipePower : PowerModel
	{
		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06005BC4 RID: 23492 RVA: 0x00254D74 File Offset: 0x00252F74
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06005BC5 RID: 23493 RVA: 0x00254D77 File Offset: 0x00252F77
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06005BC6 RID: 23494 RVA: 0x00254D7A File Offset: 0x00252F7A
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06005BC7 RID: 23495 RVA: 0x00254D7D File Offset: 0x00252F7D
		// (set) Token: 0x06005BC8 RID: 23496 RVA: 0x00254D85 File Offset: 0x00252F85
		[Nullable(2)]
		public CardModel StolenCard
		{
			[NullableContext(2)]
			get
			{
				return this._stolenCard;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._stolenCard = value;
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06005BC9 RID: 23497 RVA: 0x00254D94 File Offset: 0x00252F94
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				if (this.StolenCard == null)
				{
					return Array.Empty<IHoverTip>();
				}
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(this.StolenCard, false));
			}
		}

		// Token: 0x06005BCA RID: 23498 RVA: 0x00254DB8 File Offset: 0x00252FB8
		public override Task BeforeDeath(Creature target)
		{
			if (base.Owner != target)
			{
				return Task.CompletedTask;
			}
			CardModel stolenCard = this.StolenCard;
			if (((stolenCard != null) ? stolenCard.DeckVersion : null) == null)
			{
				return Task.CompletedTask;
			}
			IRunState runState = base.CombatState.RunState;
			runState.AddCard(this.StolenCard.DeckVersion, base.Target.Player);
			SpecialCardReward specialCardReward = new SpecialCardReward(this.StolenCard.DeckVersion, base.Target.Player);
			specialCardReward.SetCustomDescriptionEncounterSource(ModelDb.Encounter<ThievingHopperWeak>().Id);
			((CombatRoom)runState.CurrentRoom).AddExtraReward(base.Target.Player, specialCardReward);
			MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(base.Target.Player.NetId).MarkLootReturned(1);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x00254E8C File Offset: 0x0025308C
		public async Task Steal(CardModel card)
		{
			base.Target = card.Owner.Creature;
			this.StolenCard = card;
			if (card.DeckVersion != null)
			{
				await CardPileCmd.RemoveFromDeck(card.DeckVersion, false);
				MapPointHistoryEntry currentMapPointHistoryEntry = card.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(card.Owner.NetId).MarkLootStolen(1);
				}
			}
		}

		// Token: 0x04002520 RID: 9504
		[Nullable(2)]
		private CardModel _stolenCard;
	}
}
