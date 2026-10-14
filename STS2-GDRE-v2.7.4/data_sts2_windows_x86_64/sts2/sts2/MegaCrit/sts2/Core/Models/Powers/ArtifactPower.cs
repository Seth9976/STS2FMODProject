using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062E RID: 1582
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArtifactPower : PowerModel
	{
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060056B5 RID: 22197 RVA: 0x0024B9F7 File Offset: 0x00249BF7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060056B6 RID: 22198 RVA: 0x0024B9FA File Offset: 0x00249BFA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x0024B9FD File Offset: 0x00249BFD
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x0024BA00 File Offset: 0x00249C00
		public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, [Nullable(2)] Creature _, out decimal modifiedAmount)
		{
			if (target != base.Owner)
			{
				modifiedAmount = amount;
				return false;
			}
			if (canonicalPower.GetTypeForAmount(amount) != PowerType.Debuff)
			{
				modifiedAmount = amount;
				return false;
			}
			if (!canonicalPower.IsVisible)
			{
				modifiedAmount = amount;
				return false;
			}
			modifiedAmount = 0m;
			return true;
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0024BA50 File Offset: 0x00249C50
		public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
		{
			await PowerCmd.Decrement(this);
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0024BA93 File Offset: 0x00249C93
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return amount + combatState.Players.Count - 1m;
		}
	}
}
