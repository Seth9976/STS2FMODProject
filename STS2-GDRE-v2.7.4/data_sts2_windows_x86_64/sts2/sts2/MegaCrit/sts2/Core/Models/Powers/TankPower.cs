using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000713 RID: 1811
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TankPower : PowerModel
	{
		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06005BEC RID: 23532 RVA: 0x002553C2 File Offset: 0x002535C2
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x002553C5 File Offset: 0x002535C5
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x002553C8 File Offset: 0x002535C8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("DamageIncrease", 1.5m),
					new DynamicVar("DamageDecrease", 0.5m)
				});
			}
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x00255410 File Offset: 0x00253610
		[NullableContext(2)]
		[return: Nullable(1)]
		public override async Task AfterApplied(Creature applier, CardModel cardSource)
		{
			foreach (Creature creature in base.CombatState.GetTeammatesOf(base.Owner))
			{
				if (creature.IsAlive && creature.IsPlayer && creature != base.Owner)
				{
					await PowerCmd.Apply<GuardedPower>(new ThrowingPlayerChoiceContext(), creature, base.Amount, base.Owner, null, false);
				}
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x00255453 File Offset: 0x00253653
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
			return base.DynamicVars["DamageIncrease"].BaseValue;
		}

		// Token: 0x04002521 RID: 9505
		private const string _damageIncreaseKey = "DamageIncrease";

		// Token: 0x04002522 RID: 9506
		private const string _damageDecreaseKey = "DamageDecrease";

		// Token: 0x04002523 RID: 9507
		public const decimal damageIncrease = 1.5m;

		// Token: 0x04002524 RID: 9508
		public const decimal damageDecrease = 0.5m;
	}
}
