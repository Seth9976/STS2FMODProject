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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B2 RID: 1714
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MonologuePower : PowerModel
	{
		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06005994 RID: 22932 RVA: 0x00250AEB File Offset: 0x0024ECEB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06005995 RID: 22933 RVA: 0x00250AEE File Offset: 0x0024ECEE
		public override PowerStackType StackType
		{
			get
			{
				if (base.DynamicVars["StrengthApplied"].IntValue != 0)
				{
					return PowerStackType.Counter;
				}
				return PowerStackType.None;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06005996 RID: 22934 RVA: 0x00250B0A File Offset: 0x0024ED0A
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars["StrengthApplied"].IntValue;
			}
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06005997 RID: 22935 RVA: 0x00250B21 File Offset: 0x0024ED21
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06005998 RID: 22936 RVA: 0x00250B24 File Offset: 0x0024ED24
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new DynamicVar("StrengthApplied", 0m)
				});
			}
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x00250B50 File Offset: 0x0024ED50
		protected override object InitInternalData()
		{
			return new MonologuePower.Data();
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x0600599A RID: 22938 RVA: 0x00250B58 File Offset: 0x0024ED58
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x00250B78 File Offset: 0x0024ED78
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<MonologuePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.DynamicVars.Strength.IntValue);
			return Task.CompletedTask;
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x00250BD0 File Offset: 0x0024EDD0
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				int num;
				if (base.GetInternalData<MonologuePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num))
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, num, base.Owner, null, true);
					base.DynamicVars["StrengthApplied"].BaseValue += base.DynamicVars.Strength.IntValue;
					base.InvokeDisplayAmountChanged();
				}
			}
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x00250C24 File Offset: 0x0024EE24
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -base.DynamicVars["StrengthApplied"].BaseValue, base.Owner, null, true);
			}
		}

		// Token: 0x04002506 RID: 9478
		public const string strengthAppliedKey = "StrengthApplied";

		// Token: 0x02001C09 RID: 7177
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets Monologue avoid triggering on cards that started play before it was applied, and avoid gaining
			/// extra block on multiple plays of Monologue.
			/// </summary>
			// Token: 0x04006FF4 RID: 28660
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
