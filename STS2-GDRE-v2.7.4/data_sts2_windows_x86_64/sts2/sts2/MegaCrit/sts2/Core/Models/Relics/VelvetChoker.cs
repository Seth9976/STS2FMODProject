using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200060D RID: 1549
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VelvetChoker : RelicModel
	{
		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x00249AB4 File Offset: 0x00247CB4
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060055FE RID: 22014 RVA: 0x00249AB7 File Offset: 0x00247CB7
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060055FF RID: 22015 RVA: 0x00249AC3 File Offset: 0x00247CC3
		public override int DisplayAmount
		{
			get
			{
				if (!base.IsCanonical)
				{
					return this._cardsPlayedThisTurn;
				}
				return 0;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x00249AD5 File Offset: 0x00247CD5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(6),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x00249AF4 File Offset: 0x00247CF4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06005602 RID: 22018 RVA: 0x00249B01 File Offset: 0x00247D01
		private bool ShouldPreventCardPlay
		{
			get
			{
				return this._cardsPlayedThisTurn >= base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x00249B1E File Offset: 0x00247D1E
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.BaseValue;
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x00249B41 File Offset: 0x00247D41
		public override bool ShouldPlay(CardModel card, AutoPlayType _)
		{
			return card.Owner != base.Owner || !this.ShouldPreventCardPlay;
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x00249B5C File Offset: 0x00247D5C
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				this._cardsPlayedThisTurn++;
			}
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x00249B8A File Offset: 0x00247D8A
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn = 0;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x00249BAC File Offset: 0x00247DAC
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this._cardsPlayedThisTurn = 0;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x00249BC0 File Offset: 0x00247DC0
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn = 0;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x040024E2 RID: 9442
		private int _cardsPlayedThisTurn;
	}
}
