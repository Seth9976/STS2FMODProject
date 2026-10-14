using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000772 RID: 1906
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PowderedDemise : PotionModel
	{
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06005E5A RID: 24154 RVA: 0x00258FCB File Offset: 0x002571CB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06005E5B RID: 24155 RVA: 0x00258FCE File Offset: 0x002571CE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06005E5C RID: 24156 RVA: 0x00258FD1 File Offset: 0x002571D1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06005E5D RID: 24157 RVA: 0x00258FD4 File Offset: 0x002571D4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Demise", 9m));
			}
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x00258FEC File Offset: 0x002571EC
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<DemisePower>(choiceContext, target, base.DynamicVars["Demise"].BaseValue, base.Owner.Creature, null, false);
		}

		// Token: 0x04002533 RID: 9523
		private const string _demiseKey = "Demise";
	}
}
