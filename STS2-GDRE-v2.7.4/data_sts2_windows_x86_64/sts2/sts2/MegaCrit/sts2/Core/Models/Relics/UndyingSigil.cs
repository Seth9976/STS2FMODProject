using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	/// <summary>
	/// This relic doesn't actually _do_ anything; Doom checks for its existence and makes enemy Doom trigger at the
	/// start of the enemy's turn instead of the end of the enemy's turn (so enemies die before they can attack).
	/// </summary>
	// Token: 0x02000608 RID: 1544
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UndyingSigil : RelicModel
	{
		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x060055D7 RID: 21975 RVA: 0x0024957E File Offset: 0x0024777E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060055D8 RID: 21976 RVA: 0x00249581 File Offset: 0x00247781
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 0.5m));
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060055D9 RID: 21977 RVA: 0x0024959C File Offset: 0x0024779C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x002495BC File Offset: 0x002477BC
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (dealer == null)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (target != base.Owner.Creature)
			{
				return 1m;
			}
			if (dealer == base.Owner.Creature)
			{
				return 1m;
			}
			if (dealer.CurrentHp > dealer.GetPowerAmount<DoomPower>())
			{
				return 1m;
			}
			return base.DynamicVars["DamageDecrease"].BaseValue;
		}

		// Token: 0x040024DC RID: 9436
		private const string _damageDecrease = "DamageDecrease";
	}
}
