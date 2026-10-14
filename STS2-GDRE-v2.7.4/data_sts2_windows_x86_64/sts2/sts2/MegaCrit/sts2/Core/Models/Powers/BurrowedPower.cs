using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063C RID: 1596
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BurrowedPower : PowerModel
	{
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x0024C17D File Offset: 0x0024A37D
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06005706 RID: 22278 RVA: 0x0024C180 File Offset: 0x0024A380
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x0024C183 File Offset: 0x0024A383
		public override bool ShouldClearBlock(Creature creature)
		{
			return base.Owner != creature;
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x0024C194 File Offset: 0x0024A394
		public override async Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, [Nullable(2)] Creature breaker)
		{
			if (target == base.Owner)
			{
				MonsterModel monster = target.Monster;
				Tunneler tunneler = monster as Tunneler;
				if (tunneler != null)
				{
					await tunneler.GetStunned();
					await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(tunneler.StillDizzyMove), "BITE_MOVE");
					await PowerCmd.Remove<BurrowedPower>(base.Owner);
				}
			}
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x0024C1E0 File Offset: 0x0024A3E0
		public override async Task AfterRemoved(Creature oldOwner)
		{
			await CreatureCmd.LoseBlock(new BlockingPlayerChoiceContext(), oldOwner, 999999999m, null);
		}
	}
}
