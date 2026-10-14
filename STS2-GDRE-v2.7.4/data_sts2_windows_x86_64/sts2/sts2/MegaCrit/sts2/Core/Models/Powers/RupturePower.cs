using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E1 RID: 1761
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RupturePower : PowerModel
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06005ABC RID: 23228 RVA: 0x00252E93 File Offset: 0x00251093
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06005ABD RID: 23229 RVA: 0x00252E96 File Offset: 0x00251096
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06005ABE RID: 23230 RVA: 0x00252E9C File Offset: 0x0025109C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x00252EBC File Offset: 0x002510BC
		protected override object InitInternalData()
		{
			return new RupturePower.Data();
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x00252EC4 File Offset: 0x002510C4
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (base.CombatState.CurrentSide != base.Owner.Side)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<RupturePower.Data>().playedCards.Add(cardPlay.Card, 0);
			return Task.CompletedTask;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x00252F2C File Offset: 0x0025112C
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage > 0)
				{
					if (base.CombatState.CurrentSide == base.Owner.Side)
					{
						if (cardSource == null || !base.GetInternalData<RupturePower.Data>().playedCards.ContainsKey(cardSource))
						{
							await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
						}
						else
						{
							base.GetInternalData<RupturePower.Data>().playedCards[cardSource] += base.Amount;
						}
					}
				}
			}
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x00252F90 File Offset: 0x00251190
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner)
			{
				int num;
				if (base.GetInternalData<RupturePower.Data>().playedCards.Remove(cardPlay.Card, out num))
				{
					await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, num, base.Owner, null, false);
				}
			}
		}

		// Token: 0x02001C4C RID: 7244
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04007153 RID: 29011
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> playedCards = new Dictionary<CardModel, int>();
		}
	}
}
