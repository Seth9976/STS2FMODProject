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
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200076A RID: 1898
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MazalethsGift : PotionModel
	{
		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x06005E26 RID: 24102 RVA: 0x00258B6F File Offset: 0x00256D6F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x06005E27 RID: 24103 RVA: 0x00258B72 File Offset: 0x00256D72
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x06005E28 RID: 24104 RVA: 0x00258B75 File Offset: 0x00256D75
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x06005E29 RID: 24105 RVA: 0x00258B78 File Offset: 0x00256D78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<RitualPower>(1m));
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06005E2A RID: 24106 RVA: 0x00258B8C File Offset: 0x00256D8C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<RitualPower>(null),
					HoverTipFactory.FromPower<StrengthPower>(null)
				});
			}
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x00258BC8 File Offset: 0x00256DC8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<RitualPower>(choiceContext, target, base.DynamicVars["RitualPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
