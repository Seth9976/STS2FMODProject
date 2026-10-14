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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000711 RID: 1809
	public sealed class TaintedPower : PowerModel
	{
		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x00255173 File Offset: 0x00253373
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06005BDF RID: 23519 RVA: 0x00255176 File Offset: 0x00253376
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x00255179 File Offset: 0x00253379
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 0m;
			}
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.Amount;
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x002551A4 File Offset: 0x002533A4
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				base.Flash();
				await PowerCmd.Remove(this);
			}
		}
	}
}
