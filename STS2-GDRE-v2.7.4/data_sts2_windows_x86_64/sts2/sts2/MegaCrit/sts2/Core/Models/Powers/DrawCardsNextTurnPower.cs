using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// Draw an extra N cards at the beginning of your next turn.
	/// This is distinct from <see cref="T:MegaCrit.Sts2.Core.Models.Powers.ClarityPower" /> due to its stacking behavior.
	/// </summary>
	// Token: 0x02000666 RID: 1638
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DrawCardsNextTurnPower : PowerModel
	{
		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x060057FC RID: 22524 RVA: 0x0024DE9F File Offset: 0x0024C09F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x060057FD RID: 22525 RVA: 0x0024DEA2 File Offset: 0x0024C0A2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x0024DEA5 File Offset: 0x0024C0A5
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			if (base.AmountOnTurnStart == 0)
			{
				return count;
			}
			return count + base.Amount;
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x0024DED4 File Offset: 0x0024C0D4
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.AmountOnTurnStart != 0)
				{
					await PowerCmd.Remove(this);
				}
			}
		}
	}
}
