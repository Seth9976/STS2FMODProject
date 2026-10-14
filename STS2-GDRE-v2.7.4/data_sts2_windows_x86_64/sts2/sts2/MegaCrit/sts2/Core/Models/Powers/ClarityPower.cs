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
	/// Draw an extra card at the beginning of your next N turns.
	/// This is distinct from <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DrawCardsNextTurnPower" /> due to its stacking behavior.
	/// </summary>
	// Token: 0x02000644 RID: 1604
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ClarityPower : PowerModel
	{
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06005735 RID: 22325 RVA: 0x0024C73B File Offset: 0x0024A93B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06005736 RID: 22326 RVA: 0x0024C73E File Offset: 0x0024A93E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0024C741 File Offset: 0x0024A941
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return count + 1m;
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x0024C760 File Offset: 0x0024A960
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
