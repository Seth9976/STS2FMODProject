using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D9 RID: 1497
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScreamingFlagon : RelicModel
	{
		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x0600549D RID: 21661 RVA: 0x00246E20 File Offset: 0x00245020
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x0600549E RID: 21662 RVA: 0x00246E23 File Offset: 0x00245023
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(20m, ValueProp.Unpowered));
			}
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x00246E38 File Offset: 0x00245038
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (PileType.Hand.GetPile(base.Owner).IsEmpty)
				{
					base.Flash();
					await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
				}
			}
		}
	}
}
