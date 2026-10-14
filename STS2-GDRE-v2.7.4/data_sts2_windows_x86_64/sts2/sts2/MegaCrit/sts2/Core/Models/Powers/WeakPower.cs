using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200072E RID: 1838
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeakPower : PowerModel
	{
		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x06005CA6 RID: 23718 RVA: 0x00256DDD File Offset: 0x00254FDD
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x06005CA7 RID: 23719 RVA: 0x00256DE0 File Offset: 0x00254FE0
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x06005CA8 RID: 23720 RVA: 0x00256DE3 File Offset: 0x00254FE3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 0.75m));
			}
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x00256E00 File Offset: 0x00255000
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (dealer != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			decimal num = base.DynamicVars["DamageDecrease"].BaseValue;
			PaperKrane paperKrane;
			if (target == null)
			{
				paperKrane = null;
			}
			else
			{
				Player player = target.Player;
				paperKrane = ((player != null) ? player.GetRelic<PaperKrane>() : null);
			}
			PaperKrane paperKrane2 = paperKrane;
			if (paperKrane2 != null)
			{
				num = paperKrane2.ModifyWeakMultiplier(target, num, props, dealer, cardSource);
			}
			DebilitatePower power = dealer.GetPower<DebilitatePower>();
			if (power != null)
			{
				num = power.ModifyWeakMultiplier(dealer, num, props, dealer, cardSource);
			}
			return num;
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x00256E88 File Offset: 0x00255088
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.TickDownDuration(this);
			}
		}

		// Token: 0x04002527 RID: 9511
		private const string _damageDecrease = "DamageDecrease";
	}
}
