using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C2 RID: 1730
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PainfulStabsPower : PowerModel
	{
		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x060059F5 RID: 23029 RVA: 0x002515F3 File Offset: 0x0024F7F3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x060059F6 RID: 23030 RVA: 0x002515F6 File Offset: 0x0024F7F6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x060059F7 RID: 23031 RVA: 0x002515F9 File Offset: 0x0024F7F9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Wound>(false);
			}
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x00251601 File Offset: 0x0024F801
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x00251604 File Offset: 0x0024F804
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x00251614 File Offset: 0x0024F814
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			if (command.Attacker == base.Owner)
			{
				if (command.TargetSide != base.Owner.Side)
				{
					if (command.DamageProps.IsPoweredAttack())
					{
						List<DamageResult> list = command.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).ToList<DamageResult>();
						if (list.Any<DamageResult>((DamageResult r) => r.UnblockedDamage > 0))
						{
							Dictionary<Creature, List<DamageResult>> damageResultsByCreature = new Dictionary<Creature, List<DamageResult>>();
							foreach (DamageResult damageResult in list)
							{
								if (damageResult.Receiver.IsPlayer)
								{
									if (!damageResultsByCreature.ContainsKey(damageResult.Receiver))
									{
										damageResultsByCreature.Add(damageResult.Receiver, new List<DamageResult>());
									}
									damageResultsByCreature[damageResult.Receiver].Add(damageResult);
								}
							}
							bool anyWoundApplied = false;
							foreach (Creature creature in damageResultsByCreature.Keys)
							{
								int num = damageResultsByCreature[creature].Count<DamageResult>((DamageResult r) => r.UnblockedDamage > 0);
								anyWoundApplied |= num > 0;
								await CardPileCmd.AddToCombatAndPreview<Wound>(creature, PileType.Discard, base.Amount * num, null, CardPilePosition.Bottom);
							}
							Dictionary<Creature, List<DamageResult>>.KeyCollection.Enumerator enumerator2 = default(Dictionary<Creature, List<DamageResult>>.KeyCollection.Enumerator);
							if (anyWoundApplied)
							{
								base.Flash();
							}
						}
					}
				}
			}
		}
	}
}
