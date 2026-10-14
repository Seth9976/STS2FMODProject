using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000702 RID: 1794
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SteamEruptionPower : PowerModel
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x002544EB File Offset: 0x002526EB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06005B7F RID: 23423 RVA: 0x002544EE File Offset: 0x002526EE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B80 RID: 23424 RVA: 0x002544F4 File Offset: 0x002526F4
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					await ((WaterfallGiant)creature.Monster).TriggerAboutToBlowState();
				}
			}
		}

		// Token: 0x06005B81 RID: 23425 RVA: 0x00254547 File Offset: 0x00252747
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x0025454A File Offset: 0x0025274A
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x06005B83 RID: 23427 RVA: 0x00254558 File Offset: 0x00252758
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}
	}
}
