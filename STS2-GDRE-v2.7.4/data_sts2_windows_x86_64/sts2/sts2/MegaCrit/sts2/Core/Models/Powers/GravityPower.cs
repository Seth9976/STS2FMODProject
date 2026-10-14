using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000687 RID: 1671
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GravityPower : PowerModel
	{
		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x0600589F RID: 22687 RVA: 0x0024EFAF File Offset: 0x0024D1AF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x060058A0 RID: 22688 RVA: 0x0024EFB2 File Offset: 0x0024D1B2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x0024EFB5 File Offset: 0x0024D1B5
		protected override object InitInternalData()
		{
			return new GravityPower.Data();
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x0024EFBC File Offset: 0x0024D1BC
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<GravityPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x0024F008 File Offset: 0x0024D208
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num;
			base.GetInternalData<GravityPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num);
			if (num > 0)
			{
				await CreatureCmd.Damage(choiceContext, base.Owner.CombatState.HittableEnemies, num, ValueProp.Unpowered, base.Owner);
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x0024F05C File Offset: 0x0024D25C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x02001BD8 RID: 7128
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets Gravity avoid triggering on cards that started play before it was applied, and avoid applying
			/// extra damage on multiple plays of Gravity Well.
			/// </summary>
			// Token: 0x04006F07 RID: 28423
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
