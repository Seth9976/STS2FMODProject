using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000683 RID: 1667
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FurnacePower : PowerModel
	{
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06005887 RID: 22663 RVA: 0x0024EC85 File Offset: 0x0024CE85
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06005888 RID: 22664 RVA: 0x0024EC88 File Offset: 0x0024CE88
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06005889 RID: 22665 RVA: 0x0024EC8B File Offset: 0x0024CE8B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x0024EC94 File Offset: 0x0024CE94
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await ForgeCmd.Forge(base.Amount, base.Owner.Player, this);
			}
		}
	}
}
