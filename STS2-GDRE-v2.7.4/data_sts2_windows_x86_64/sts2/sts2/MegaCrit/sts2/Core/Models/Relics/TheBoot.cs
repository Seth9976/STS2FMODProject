using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F8 RID: 1528
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheBoot : RelicModel
	{
		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06005567 RID: 21863 RVA: 0x002487F7 File Offset: 0x002469F7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06005568 RID: 21864 RVA: 0x002487FA File Offset: 0x002469FA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("DamageMinimum", 5m),
					new DynamicVar("DamageThreshold", 4m)
				});
			}
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x00248830 File Offset: 0x00246A30
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOstyLate([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (dealer != base.Owner.Creature && dealer != base.Owner.Osty)
			{
				return amount;
			}
			if (target == base.Owner.Creature)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			if (amount < 1m)
			{
				return amount;
			}
			if (amount >= base.DynamicVars["DamageMinimum"].BaseValue)
			{
				return amount;
			}
			return base.DynamicVars["DamageMinimum"].BaseValue;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x002488BA File Offset: 0x00246ABA
		public override Task AfterModifyingHpLostAfterOsty()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x040024CB RID: 9419
		private const int _damageMinimum = 5;

		// Token: 0x040024CC RID: 9420
		private const string _damageMinimumKey = "DamageMinimum";

		// Token: 0x040024CD RID: 9421
		private const string _damageThresholdKey = "DamageThreshold";
	}
}
