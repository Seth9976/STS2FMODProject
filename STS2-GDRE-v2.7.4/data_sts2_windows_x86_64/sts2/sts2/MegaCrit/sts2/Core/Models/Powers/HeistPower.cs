using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000690 RID: 1680
	public sealed class HeistPower : PowerModel
	{
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x060058D5 RID: 22741 RVA: 0x0024F543 File Offset: 0x0024D743
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x060058D6 RID: 22742 RVA: 0x0024F546 File Offset: 0x0024D746
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x060058D7 RID: 22743 RVA: 0x0024F549 File Offset: 0x0024D749
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x0024F54C File Offset: 0x0024D74C
		[NullableContext(1)]
		public override Task BeforeDeath(Creature target)
		{
			if (base.Owner != target)
			{
				return Task.CompletedTask;
			}
			CombatRoom combatRoom = base.CombatState.RunState.CurrentRoom as CombatRoom;
			if (combatRoom != null)
			{
				combatRoom.AddExtraReward(base.Target.Player, new GoldReward(base.Amount, base.Target.Player, true));
			}
			MapPointHistoryEntry currentMapPointHistoryEntry = base.CombatState.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(base.Target.Player.NetId).MarkLootReturned(base.Amount);
			}
			return Task.CompletedTask;
		}
	}
}
