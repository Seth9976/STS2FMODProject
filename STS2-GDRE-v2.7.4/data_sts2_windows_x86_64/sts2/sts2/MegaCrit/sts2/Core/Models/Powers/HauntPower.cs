using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200068F RID: 1679
	public sealed class HauntPower : PowerModel
	{
		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x060058D1 RID: 22737 RVA: 0x0024F4DF File Offset: 0x0024D6DF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x060058D2 RID: 22738 RVA: 0x0024F4E2 File Offset: 0x0024D6E2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x0024F4E8 File Offset: 0x0024D6E8
		[NullableContext(1)]
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card is Soul)
			{
				if (cardPlay.Card.Owner.Creature == base.Owner)
				{
					IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
					if (hittableEnemies.Count != 0)
					{
						Creature creature = base.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>(hittableEnemies);
						await CreatureCmd.Damage(choiceContext, new <>z__ReadOnlySingleElementList<Creature>(creature), base.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
					}
				}
			}
		}
	}
}
