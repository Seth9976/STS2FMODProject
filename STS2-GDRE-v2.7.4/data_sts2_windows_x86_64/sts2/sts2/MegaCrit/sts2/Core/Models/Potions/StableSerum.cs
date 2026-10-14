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
	// Token: 0x0200077C RID: 1916
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StableSerum : PotionModel
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06005E9D RID: 24221 RVA: 0x00259597 File Offset: 0x00257797
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06005E9E RID: 24222 RVA: 0x0025959A File Offset: 0x0025779A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06005E9F RID: 24223 RVA: 0x0025959D File Offset: 0x0025779D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x06005EA0 RID: 24224 RVA: 0x002595A0 File Offset: 0x002577A0
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x06005EA1 RID: 24225 RVA: 0x002595AD File Offset: 0x002577AD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(2));
			}
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x002595BC File Offset: 0x002577BC
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<RetainHandPower>(choiceContext, target, base.DynamicVars.Repeat.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
