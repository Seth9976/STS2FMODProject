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
	// Token: 0x02000760 RID: 1888
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FyshOil : PotionModel
	{
		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06005DE5 RID: 24037 RVA: 0x002585FB File Offset: 0x002567FB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x06005DE6 RID: 24038 RVA: 0x002585FE File Offset: 0x002567FE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06005DE7 RID: 24039 RVA: 0x00258601 File Offset: 0x00256801
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06005DE8 RID: 24040 RVA: 0x00258604 File Offset: 0x00256804
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(1m),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06005DE9 RID: 24041 RVA: 0x0025862C File Offset: 0x0025682C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x00258668 File Offset: 0x00256868
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<StrengthPower>(choiceContext, target, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
			await PowerCmd.Apply<DexterityPower>(choiceContext, target, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
