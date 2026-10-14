using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C8 RID: 1736
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhantomBladesPower : PowerModel
	{
		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06005A1B RID: 23067 RVA: 0x00251961 File Offset: 0x0024FB61
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x00251964 File Offset: 0x0024FB64
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x00251967 File Offset: 0x0024FB67
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<Shiv>(false),
					HoverTipFactory.FromKeyword(CardKeyword.Retain)
				});
			}
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x00251988 File Offset: 0x0024FB88
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (!card.Tags.Contains(CardTag.Shiv))
			{
				return Task.CompletedTask;
			}
			if (card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			CardCmd.ApplyKeyword(card, new CardKeyword[] { CardKeyword.Retain });
			return Task.CompletedTask;
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x002519D8 File Offset: 0x0024FBD8
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			foreach (CardModel cardModel in base.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Shiv)))
			{
				CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Retain });
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x00251A68 File Offset: 0x0024FC68
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			if (cardSource == null || !cardSource.Tags.Contains(CardTag.Shiv))
			{
				return 0m;
			}
			if (dealer != base.Owner)
			{
				return 0m;
			}
			int num = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Tags.Contains(CardTag.Shiv) && e.CardPlay.Player == base.Owner.Player);
			if (num > 0)
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
