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
	// Token: 0x02000705 RID: 1797
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StranglePower : PowerModel
	{
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06005B92 RID: 23442 RVA: 0x0025470B File Offset: 0x0025290B
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x06005B93 RID: 23443 RVA: 0x0025470E File Offset: 0x0025290E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06005B94 RID: 23444 RVA: 0x00254711 File Offset: 0x00252911
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.InstancedPerApplier;
			}
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x00254714 File Offset: 0x00252914
		protected override object InitInternalData()
		{
			return new StranglePower.Data();
		}

		// Token: 0x06005B96 RID: 23446 RVA: 0x0025471C File Offset: 0x0025291C
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
			base.GetInternalData<StranglePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x00254784 File Offset: 0x00252984
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num;
			if (base.GetInternalData<StranglePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num))
			{
				base.Flash();
				await CreatureCmd.Damage(choiceContext, base.Owner, num, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			}
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x002547D8 File Offset: 0x002529D8
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x02001C7C RID: 7292
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets Strangle avoid triggering on itself, especially when stacking.
			/// </summary>
			// Token: 0x04007250 RID: 29264
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
