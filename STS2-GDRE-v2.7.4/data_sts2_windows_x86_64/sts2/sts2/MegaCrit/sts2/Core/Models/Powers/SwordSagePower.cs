using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070E RID: 1806
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwordSagePower : PowerModel
	{
		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06005BCD RID: 23501 RVA: 0x00254EDF File Offset: 0x002530DF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06005BCE RID: 23502 RVA: 0x00254EE2 File Offset: 0x002530E2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06005BCF RID: 23503 RVA: 0x00254EE5 File Offset: 0x002530E5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>(false));
				list.Add(HoverTipFactory.Static(StaticHoverTip.ReplayStatic, Array.Empty<DynamicVar>()));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005BD0 RID: 23504 RVA: 0x00254F10 File Offset: 0x00253110
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(power is SwordSagePower))
			{
				return Task.CompletedTask;
			}
			if (power.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			Player player = base.Owner.Player;
			IEnumerable<CardModel> enumerable;
			if (player == null)
			{
				enumerable = null;
			}
			else
			{
				PlayerCombatState playerCombatState = player.PlayerCombatState;
				enumerable = ((playerCombatState != null) ? playerCombatState.AllCards : null);
			}
			IEnumerable<CardModel> enumerable2 = enumerable ?? Array.Empty<CardModel>();
			foreach (CardModel cardModel in enumerable2)
			{
				this.TryAddReplays(cardModel, (int)amount);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005BD1 RID: 23505 RVA: 0x00254FB4 File Offset: 0x002531B4
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.IsClone)
			{
				return Task.CompletedTask;
			}
			this.TryAddReplays(card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x06005BD2 RID: 23506 RVA: 0x00254FD8 File Offset: 0x002531D8
		public override Task AfterRemoved(Creature oldOwner)
		{
			Player player = oldOwner.Player;
			IEnumerable<CardModel> enumerable;
			if (player == null)
			{
				enumerable = null;
			}
			else
			{
				PlayerCombatState playerCombatState = player.PlayerCombatState;
				enumerable = ((playerCombatState != null) ? playerCombatState.AllCards : null);
			}
			IEnumerable<CardModel> enumerable2 = enumerable ?? Array.Empty<CardModel>();
			foreach (CardModel cardModel in enumerable2)
			{
				SovereignBlade sovereignBlade = cardModel as SovereignBlade;
				if (sovereignBlade != null)
				{
					sovereignBlade.BaseReplayCount -= base.Amount;
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005BD3 RID: 23507 RVA: 0x00255064 File Offset: 0x00253264
		private void TryAddReplays(CardModel card, int amount)
		{
			if (card.Owner != base.Owner.Player)
			{
				return;
			}
			SovereignBlade sovereignBlade = card as SovereignBlade;
			if (sovereignBlade == null)
			{
				return;
			}
			sovereignBlade.BaseReplayCount += amount;
		}
	}
}
