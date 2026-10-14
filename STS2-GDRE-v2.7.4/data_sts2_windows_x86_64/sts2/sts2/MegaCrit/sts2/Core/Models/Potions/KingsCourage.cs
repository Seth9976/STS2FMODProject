using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000766 RID: 1894
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KingsCourage : PotionModel
	{
		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06005E0C RID: 24076 RVA: 0x00258967 File Offset: 0x00256B67
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06005E0D RID: 24077 RVA: 0x0025896A File Offset: 0x00256B6A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06005E0E RID: 24078 RVA: 0x0025896D File Offset: 0x00256B6D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x06005E0F RID: 24079 RVA: 0x00258970 File Offset: 0x00256B70
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(15));
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x06005E10 RID: 24080 RVA: 0x0025897E File Offset: 0x00256B7E
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x00258988 File Offset: 0x00256B88
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, target.Player, this);
		}
	}
}
