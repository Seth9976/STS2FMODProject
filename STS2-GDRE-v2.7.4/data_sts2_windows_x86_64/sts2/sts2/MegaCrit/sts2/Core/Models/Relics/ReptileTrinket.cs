using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CD RID: 1485
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReptileTrinket : RelicModel
	{
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06005461 RID: 21601 RVA: 0x0024679D File Offset: 0x0024499D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06005462 RID: 21602 RVA: 0x002467A0 File Offset: 0x002449A0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(3m));
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06005463 RID: 21603 RVA: 0x002467B4 File Offset: 0x002449B4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x002467D4 File Offset: 0x002449D4
		public override async Task AfterPotionUsed(PotionModel potion, [Nullable(2)] Creature target)
		{
			if (potion.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					base.Flash();
					await PowerCmd.Apply<ReptileTrinketPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
