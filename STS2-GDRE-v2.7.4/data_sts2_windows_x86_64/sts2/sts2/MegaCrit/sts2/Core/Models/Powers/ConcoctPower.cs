using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000646 RID: 1606
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConcoctPower : PowerModel
	{
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x0024C89F File Offset: 0x0024AA9F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x0024C8A2 File Offset: 0x0024AAA2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x0024C8A8 File Offset: 0x0024AAA8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x0024C8C8 File Offset: 0x0024AAC8
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer == base.Owner)
			{
				if (props.IsPoweredAttack())
				{
					if (result.UnblockedDamage > 0)
					{
						await PowerCmd.Apply<PoisonPower>(choiceContext, target, base.Amount, base.Owner, null, false);
					}
				}
			}
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x0024C938 File Offset: 0x0024AB38
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (base.Owner.Side != side)
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
