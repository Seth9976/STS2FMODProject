using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E6 RID: 1766
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SerpentFormPower : PowerModel
	{
		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002533CF File Offset: 0x002515CF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x002533D2 File Offset: 0x002515D2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x002533D5 File Offset: 0x002515D5
		protected override object InitInternalData()
		{
			return new SerpentFormPower.Data();
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x002533DC File Offset: 0x002515DC
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<SerpentFormPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x00253428 File Offset: 0x00251628
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				int damage;
				if (base.GetInternalData<SerpentFormPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out damage))
				{
					if (damage > 0)
					{
						await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
						Rng combatTargets = base.Owner.Player.RunState.Rng.CombatTargets;
						ICombatState combatState = base.Owner.CombatState;
						Creature creature = combatTargets.NextItem<Creature>(((combatState != null) ? combatState.HittableEnemies : null) ?? Array.Empty<Creature>());
						if (creature != null)
						{
							VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_blunt");
							await CreatureCmd.Damage(choiceContext, creature, damage, ValueProp.Unpowered, base.Owner);
						}
					}
				}
			}
		}

		// Token: 0x02001C58 RID: 7256
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets Serpent Form avoid triggering on cards that started play before it was applied, and avoid
			/// dealing extra damage on multiple plays of Serpent Form.
			/// </summary>
			// Token: 0x04007191 RID: 29073
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
