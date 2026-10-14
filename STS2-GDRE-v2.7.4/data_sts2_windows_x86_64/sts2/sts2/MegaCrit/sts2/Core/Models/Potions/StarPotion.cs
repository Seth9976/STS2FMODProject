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

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200077D RID: 1917
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StarPotion : PotionModel
	{
		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06005EA4 RID: 24228 RVA: 0x00259617 File Offset: 0x00257817
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06005EA5 RID: 24229 RVA: 0x0025961A File Offset: 0x0025781A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06005EA6 RID: 24230 RVA: 0x0025961D File Offset: 0x0025781D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06005EA7 RID: 24231 RVA: 0x00259620 File Offset: 0x00257820
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(3));
			}
		}

		// Token: 0x06005EA8 RID: 24232 RVA: 0x00259630 File Offset: 0x00257830
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, target.Player);
		}
	}
}
