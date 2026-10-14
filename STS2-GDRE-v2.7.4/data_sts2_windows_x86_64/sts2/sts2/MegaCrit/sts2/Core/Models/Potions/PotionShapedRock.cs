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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000770 RID: 1904
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionShapedRock : PotionModel
	{
		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x06005E4D RID: 24141 RVA: 0x00258EDB File Offset: 0x002570DB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Token;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06005E4E RID: 24142 RVA: 0x00258EDE File Offset: 0x002570DE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06005E4F RID: 24143 RVA: 0x00258EE1 File Offset: 0x002570E1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06005E50 RID: 24144 RVA: 0x00258EE4 File Offset: 0x002570E4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(15m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005E51 RID: 24145 RVA: 0x00258EF8 File Offset: 0x002570F8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.Damage(choiceContext, target, base.DynamicVars.Damage, base.Owner.Creature, null, null);
		}
	}
}
