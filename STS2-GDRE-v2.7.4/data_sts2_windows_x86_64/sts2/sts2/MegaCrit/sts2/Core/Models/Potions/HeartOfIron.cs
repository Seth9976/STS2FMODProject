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
	// Token: 0x02000765 RID: 1893
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HeartOfIron : PotionModel
	{
		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06005E05 RID: 24069 RVA: 0x002588B7 File Offset: 0x00256AB7
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06005E06 RID: 24070 RVA: 0x002588BA File Offset: 0x00256ABA
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06005E07 RID: 24071 RVA: 0x002588BD File Offset: 0x00256ABD
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06005E08 RID: 24072 RVA: 0x002588C0 File Offset: 0x00256AC0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PlatingPower>(7m));
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06005E09 RID: 24073 RVA: 0x002588D4 File Offset: 0x00256AD4
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<PlatingPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x0025890C File Offset: 0x00256B0C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<PlatingPower>(choiceContext, target, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
