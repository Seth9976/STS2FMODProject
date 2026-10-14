using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070B RID: 1803
	public sealed class SurprisePower : PowerModel
	{
		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06005BB3 RID: 23475 RVA: 0x00254A83 File Offset: 0x00252C83
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06005BB4 RID: 23476 RVA: 0x00254A86 File Offset: 0x00252C86
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x00254A8C File Offset: 0x00252C8C
		[NullableContext(1)]
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (base.Owner == target)
				{
					Creature fatGremlin = base.CombatState.CreateCreature(ModelDb.Monster<FatGremlin>().ToMutable(), CombatSide.Enemy, "fat");
					int totalStolen = 0;
					foreach (ThieveryPower thieveryPower in base.Owner.GetPowerInstances<ThieveryPower>())
					{
						int intValue = thieveryPower.DynamicVars.Gold.IntValue;
						totalStolen += intValue;
						HeistPower heistPower = (HeistPower)ModelDb.Power<HeistPower>().ToMutable(0);
						heistPower.Target = thieveryPower.Target;
						await PowerCmd.Apply(choiceContext, heistPower, fatGremlin, intValue, base.Owner, null, false);
					}
					IEnumerator<ThieveryPower> enumerator = null;
					await CreatureCmd.Add<SneakyGremlin>(base.CombatState, "sneaky");
					await CreatureCmd.Add(fatGremlin);
					if (totalStolen > 0)
					{
						GremlinMercNormal gremlinMercNormal = base.CombatState.Encounter as GremlinMercNormal;
						if (gremlinMercNormal != null)
						{
							gremlinMercNormal.MarkGoldStolen();
						}
					}
				}
			}
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x00254AE7 File Offset: 0x00252CE7
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}
	}
}
