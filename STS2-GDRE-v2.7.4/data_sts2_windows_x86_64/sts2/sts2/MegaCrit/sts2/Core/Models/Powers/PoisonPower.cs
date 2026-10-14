using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006CD RID: 1741
	public sealed class PoisonPower : PowerModel
	{
		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06005A3D RID: 23101 RVA: 0x00251E27 File Offset: 0x00250027
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06005A3E RID: 23102 RVA: 0x00251E2A File Offset: 0x0025002A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06005A3F RID: 23103 RVA: 0x00251E2D File Offset: 0x0025002D
		public override Color AmountLabelColor
		{
			get
			{
				return PowerModel._normalAmountLabelColor;
			}
		}

		/// <summary>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Powers.AccelerantPower" /> makes Poison trigger extra times, but it should never trigger more times than the
		/// amount of Poison we have.
		/// Since the power is applied to players, enemy creatures need to look to them (their opponents) to figure out the
		/// trigger count.
		/// </summary>
		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06005A40 RID: 23104 RVA: 0x00251E34 File Offset: 0x00250034
		private int TriggerCount
		{
			get
			{
				IEnumerable<Creature> enumerable = from c in base.Owner.CombatState.GetOpponentsOf(base.Owner)
					where c.IsAlive
					select c;
				return Math.Min(base.Amount, 1 + enumerable.Sum<Creature>((Creature a) => a.GetPowerAmount<AccelerantPower>()));
			}
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x00251EB0 File Offset: 0x002500B0
		public int CalculateTotalDamageNextTurn()
		{
			decimal num = 0m;
			int num2 = Math.Min(base.Amount, this.TriggerCount);
			for (int i = 0; i < num2; i++)
			{
				decimal num3 = base.Amount - i;
				IEnumerable<AbstractModel> enumerable;
				num3 = Hook.ModifyDamage(base.Owner.CombatState.RunState, base.Owner.CombatState, base.Owner, null, num3, ValueProp.Unblockable | ValueProp.Unpowered, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out enumerable);
				num += num3;
			}
			return (int)num;
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x00251F30 File Offset: 0x00250130
		[NullableContext(1)]
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				int iterations = this.TriggerCount;
				for (int i = 0; i < iterations; i++)
				{
					await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
					if (base.Owner.IsAlive)
					{
						await PowerCmd.Decrement(this);
					}
					else
					{
						await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
					}
				}
			}
		}
	}
}
