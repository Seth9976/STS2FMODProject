using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000771 RID: 1905
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotOfGhouls : PotionModel
	{
		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06005E53 RID: 24147 RVA: 0x00258F53 File Offset: 0x00257153
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06005E54 RID: 24148 RVA: 0x00258F56 File Offset: 0x00257156
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06005E55 RID: 24149 RVA: 0x00258F59 File Offset: 0x00257159
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06005E56 RID: 24150 RVA: 0x00258F5C File Offset: 0x0025715C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06005E57 RID: 24151 RVA: 0x00258F69 File Offset: 0x00257169
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x06005E58 RID: 24152 RVA: 0x00258F78 File Offset: 0x00257178
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await Soul.CreateInHand(target.Player, base.DynamicVars.Cards.IntValue, target.CombatState, base.Owner);
		}
	}
}
