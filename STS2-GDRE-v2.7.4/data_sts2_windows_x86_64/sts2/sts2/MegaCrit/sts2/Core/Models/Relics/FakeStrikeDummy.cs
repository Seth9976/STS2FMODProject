using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000546 RID: 1350
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeStrikeDummy : RelicModel
	{
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060050BC RID: 20668 RVA: 0x0023F997 File Offset: 0x0023DB97
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x0023F99A File Offset: 0x0023DB9A
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060050BE RID: 20670 RVA: 0x0023F99E File Offset: 0x0023DB9E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraDamage", 1m));
			}
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x0023F9B4 File Offset: 0x0023DBB4
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			if (cardSource == null)
			{
				return 0m;
			}
			if (!cardSource.Tags.Contains(CardTag.Strike))
			{
				return 0m;
			}
			if (dealer != base.Owner.Creature && cardSource.Owner != base.Owner)
			{
				return 0m;
			}
			return base.DynamicVars["ExtraDamage"].BaseValue;
		}

		// Token: 0x04002447 RID: 9287
		private const string _extraDamageKey = "ExtraDamage";
	}
}
