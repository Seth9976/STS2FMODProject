using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000703 RID: 1795
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StockPower : PowerModel
	{
		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06005B85 RID: 23429 RVA: 0x00254563 File Offset: 0x00252763
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x00254566 File Offset: 0x00252766
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x0025456C File Offset: 0x0025276C
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (target == base.Owner)
				{
					if (base.Amount > 0)
					{
						Axebot axebot = (Axebot)ModelDb.Monster<Axebot>().ToMutable();
						axebot.ShouldPlaySpawnAnimation = true;
						axebot.StockAmount = base.Amount - 1;
						Creature creature = await CreatureCmd.Add(axebot, base.CombatState, base.Owner.Side, base.Owner.SlotName);
						Creature creature2 = creature;
						creature2.SetNodeVisible(false);
						TaskHelper.RunSafely(StockPower.RevealReplacementAfterDeathAnim(creature2, deathAnimLength));
					}
				}
			}
		}

		// Token: 0x06005B88 RID: 23432 RVA: 0x002545C8 File Offset: 0x002527C8
		private static async Task RevealReplacementAfterDeathAnim(Creature creature, float deathAnimLength)
		{
			await Cmd.CustomScaledWait(deathAnimLength, deathAnimLength, false, default(CancellationToken));
			creature.SetNodeVisible(true);
			await CreatureCmd.TriggerAnim(creature, "respawn", 0f);
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x00254613 File Offset: 0x00252813
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}
	}
}
