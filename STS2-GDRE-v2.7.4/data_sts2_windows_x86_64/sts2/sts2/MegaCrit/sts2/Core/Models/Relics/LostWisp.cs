using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057F RID: 1407
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LostWisp : RelicModel
	{
		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06005247 RID: 21063 RVA: 0x0024297F File Offset: 0x00240B7F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06005248 RID: 21064 RVA: 0x00242982 File Offset: 0x00240B82
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x00242998 File Offset: 0x00240B98
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					if (cardPlay.Card.Type == CardType.Power)
					{
						base.Flash();
						await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage.BaseValue, base.DynamicVars.Damage.Props, base.Owner.Creature);
					}
				}
			}
		}
	}
}
