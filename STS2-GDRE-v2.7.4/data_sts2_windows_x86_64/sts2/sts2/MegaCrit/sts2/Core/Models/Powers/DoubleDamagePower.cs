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
	// Token: 0x02000665 RID: 1637
	public sealed class DoubleDamagePower : PowerModel
	{
		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x060057F7 RID: 22519 RVA: 0x0024DDF3 File Offset: 0x0024BFF3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x060057F8 RID: 22520 RVA: 0x0024DDF6 File Offset: 0x0024BFF6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x0024DDFC File Offset: 0x0024BFFC
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (dealer != base.Owner && !base.Owner.Pets.Contains(dealer))
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x0024DE4C File Offset: 0x0024C04C
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
