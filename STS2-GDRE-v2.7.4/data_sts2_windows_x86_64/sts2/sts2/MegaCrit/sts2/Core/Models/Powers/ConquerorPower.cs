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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000648 RID: 1608
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConquerorPower : PowerModel
	{
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x0600574E RID: 22350 RVA: 0x0024CA5E File Offset: 0x0024AC5E
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x0600574F RID: 22351 RVA: 0x0024CA61 File Offset: 0x0024AC61
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x06005750 RID: 22352 RVA: 0x0024CA64 File Offset: 0x0024AC64
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>(false);
			}
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x0024CA6C File Offset: 0x0024AC6C
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!(cardSource is SovereignBlade))
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (target != base.Owner)
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x0024CAA0 File Offset: 0x0024ACA0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
