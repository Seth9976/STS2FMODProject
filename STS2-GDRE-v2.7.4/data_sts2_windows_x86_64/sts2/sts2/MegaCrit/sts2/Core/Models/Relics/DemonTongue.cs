using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200052B RID: 1323
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DemonTongue : RelicModel
	{
		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x0023E9DF File Offset: 0x0023CBDF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x0023E9E4 File Offset: 0x0023CBE4
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (base.Owner.Creature.CombatState != null)
			{
				if (base.Owner.Creature.CombatState.CurrentSide == base.Owner.Creature.Side)
				{
					if (target == base.Owner.Creature)
					{
						if (result.UnblockedDamage > 0)
						{
							if (!this._triggeredThisTurn)
							{
								this._triggeredThisTurn = true;
								base.Flash();
								await CreatureCmd.Heal(base.Owner.Creature, result.UnblockedDamage, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x0023EA37 File Offset: 0x0023CC37
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this._triggeredThisTurn = false;
			return Task.CompletedTask;
		}

		// Token: 0x0400243B RID: 9275
		private bool _triggeredThisTurn;
	}
}
