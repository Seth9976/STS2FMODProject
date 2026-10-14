using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069D RID: 1693
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfestedPower : PowerModel
	{
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06005926 RID: 22822 RVA: 0x0024FF75 File Offset: 0x0024E175
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06005927 RID: 22823 RVA: 0x0024FF78 File Offset: 0x0024E178
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x0024FF7C File Offset: 0x0024E17C
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (base.Owner == target)
				{
					List<Creature> wrigglers = new List<Creature>();
					for (int i = 0; i < 4; i++)
					{
						Wriggler wriggler = (Wriggler)ModelDb.Monster<Wriggler>().ToMutable();
						wriggler.StartStunned = true;
						Creature creature = await CreatureCmd.Add(wriggler, base.CombatState, base.Owner.Side, PhrogParasiteElite.GetWrigglerSlotName(i));
						Creature creature2 = creature;
						creature2.SetNodeVisible(false);
						wrigglers.Add(creature2);
					}
					TaskHelper.RunSafely(InfestedPower.RevealWrigglersAfterDeathAnim(wrigglers, deathAnimLength));
				}
			}
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x0024FFD8 File Offset: 0x0024E1D8
		private static async Task RevealWrigglersAfterDeathAnim(List<Creature> wrigglers, float deathAnimLength)
		{
			await Cmd.CustomScaledWait(deathAnimLength, deathAnimLength, false, default(CancellationToken));
			if (TestMode.IsOff)
			{
				NRunMusicController.Instance.TriggerEliteSecondPhase();
			}
			foreach (Creature creature in wrigglers)
			{
				creature.SetNodeVisible(true);
			}
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x00250023 File Offset: 0x0024E223
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}
	}
}
