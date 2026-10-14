using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200072C RID: 1836
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VulnerablePower : PowerModel
	{
		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x06005C9B RID: 23707 RVA: 0x00256C72 File Offset: 0x00254E72
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x06005C9C RID: 23708 RVA: 0x00256C75 File Offset: 0x00254E75
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x06005C9D RID: 23709 RVA: 0x00256C78 File Offset: 0x00254E78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageIncrease", 1.5m));
			}
		}

		/// <summary>
		/// Multiplies damage received by 1.5x.
		/// </summary>
		// Token: 0x06005C9E RID: 23710 RVA: 0x00256C94 File Offset: 0x00254E94
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			decimal num = base.DynamicVars["DamageIncrease"].BaseValue;
			if (dealer != null)
			{
				Player player = dealer.Player;
				PaperPhrog paperPhrog = ((player != null) ? player.GetRelic<PaperPhrog>() : null);
				if (paperPhrog != null)
				{
					num = paperPhrog.ModifyVulnerableMultiplier(target, num, props, dealer, cardSource);
				}
				CrueltyPower crueltyPower;
				if ((crueltyPower = dealer.GetPower<CrueltyPower>()) == null)
				{
					Player petOwner = dealer.PetOwner;
					crueltyPower = ((petOwner != null) ? petOwner.Creature.GetPower<CrueltyPower>() : null);
				}
				CrueltyPower crueltyPower2 = crueltyPower;
				if (crueltyPower2 != null)
				{
					num = crueltyPower2.ModifyVulnerableMultiplier(target, num, props, dealer, cardSource);
				}
			}
			DebilitatePower power = target.GetPower<DebilitatePower>();
			if (power != null)
			{
				num = power.ModifyVulnerableMultiplier(target, num, props, dealer, cardSource);
			}
			return num;
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x00256D4C File Offset: 0x00254F4C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.TickDownDuration(this);
			}
		}

		// Token: 0x04002526 RID: 9510
		private const string _damageIncrease = "DamageIncrease";
	}
}
