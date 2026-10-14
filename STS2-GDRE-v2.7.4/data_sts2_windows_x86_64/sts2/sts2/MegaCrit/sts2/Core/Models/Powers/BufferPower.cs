using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063B RID: 1595
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BufferPower : PowerModel
	{
		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060056FF RID: 22271 RVA: 0x0024C0FB File Offset: 0x0024A2FB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06005700 RID: 22272 RVA: 0x0024C0FE File Offset: 0x0024A2FE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		/// <summary>
		/// We use Late because other effects may reduce damage taken to 0 too, and it's more player-friendly for them to
		/// trigger first so that this power doesn't have to decrement.
		/// </summary>
		// Token: 0x06005701 RID: 22273 RVA: 0x0024C101 File Offset: 0x0024A301
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOstyLate([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return amount;
			}
			return 0m;
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x0024C114 File Offset: 0x0024A314
		public override async Task AfterModifyingHpLostAfterOsty()
		{
			await PowerCmd.Decrement(this);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x0024C157 File Offset: 0x0024A357
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return ((combatState.Players.Count - 1) * 2 + 1) * amount;
		}
	}
}
