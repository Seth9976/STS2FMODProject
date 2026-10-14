using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064F RID: 1615
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CountdownPower : PowerModel
	{
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06005771 RID: 22385 RVA: 0x0024CDDC File Offset: 0x0024AFDC
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06005772 RID: 22386 RVA: 0x0024CDDF File Offset: 0x0024AFDF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x0024CDE4 File Offset: 0x0024AFE4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x0024CE04 File Offset: 0x0024B004
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				Creature creature = base.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>(base.CombatState.HittableEnemies);
				if (creature != null)
				{
					await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), creature, base.Amount, base.Owner, null, false);
				}
			}
		}
	}
}
