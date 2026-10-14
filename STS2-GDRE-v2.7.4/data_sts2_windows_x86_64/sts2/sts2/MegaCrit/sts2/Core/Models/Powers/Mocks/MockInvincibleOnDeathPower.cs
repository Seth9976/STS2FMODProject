using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000737 RID: 1847
	[NullableContext(1)]
	[Nullable(0)]
	public class MockInvincibleOnDeathPower : PowerModel
	{
		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x06005CD8 RID: 23768 RVA: 0x002571BB File Offset: 0x002553BB
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x06005CD9 RID: 23769 RVA: 0x002571BE File Offset: 0x002553BE
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x06005CDA RID: 23770 RVA: 0x002571C1 File Offset: 0x002553C1
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x002571C4 File Offset: 0x002553C4
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					await CreatureCmd.SetMaxAndCurrentHp(base.Owner, 999999999m);
				}
			}
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x00257217 File Offset: 0x00255417
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x0025721A File Offset: 0x0025541A
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x00257228 File Offset: 0x00255428
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}
	}
}
