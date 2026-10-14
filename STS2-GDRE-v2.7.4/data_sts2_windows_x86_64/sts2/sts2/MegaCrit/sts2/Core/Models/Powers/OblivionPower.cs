using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006BC RID: 1724
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OblivionPower : PowerModel
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x002511BB File Offset: 0x0024F3BB
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x060059D1 RID: 22993 RVA: 0x002511BE File Offset: 0x0024F3BE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x060059D2 RID: 22994 RVA: 0x002511C1 File Offset: 0x0024F3C1
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.InstancedPerApplier;
			}
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x002511C4 File Offset: 0x0024F3C4
		protected override object InitInternalData()
		{
			return new OblivionPower.Data();
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x002511CC File Offset: 0x0024F3CC
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			Creature applier = base.Applier;
			if (((applier != null) ? applier.Player : null) == null)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Applier.Player)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<OblivionPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x00251234 File Offset: 0x0024F434
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num;
			if (base.GetInternalData<OblivionPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num))
			{
				base.Flash();
				await PowerCmd.Apply<DoomPower>(choiceContext, base.Owner, num, base.Applier, null, false);
			}
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x00251288 File Offset: 0x0024F488
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Player)
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x02001C15 RID: 7189
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets Oblivion avoid triggering on itself.
			/// </summary>
			// Token: 0x04007032 RID: 28722
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
