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
	// Token: 0x02000725 RID: 1829
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnderworldPower : PowerModel
	{
		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x06005C68 RID: 23656 RVA: 0x00256357 File Offset: 0x00254557
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x0025635A File Offset: 0x0025455A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x06005C6A RID: 23658 RVA: 0x00256360 File Offset: 0x00254560
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x00256380 File Offset: 0x00254580
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer != null)
			{
				if (dealer.Side == base.Owner.Side)
				{
					if (dealer != base.Owner)
					{
						if (dealer.PetOwner != base.Owner.Player)
						{
							if (props.IsPoweredAttack())
							{
								if (result.TotalDamage > 0)
								{
									await PowerCmd.Apply<DoomPower>(choiceContext, target, result.TotalDamage * base.Amount, base.Owner, null, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x002563F0 File Offset: 0x002545F0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
