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
	// Token: 0x0200077B RID: 1915
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpeedPotion : PotionModel
	{
		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x06005E96 RID: 24214 RVA: 0x00259500 File Offset: 0x00257700
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06005E97 RID: 24215 RVA: 0x00259503 File Offset: 0x00257703
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06005E98 RID: 24216 RVA: 0x00259506 File Offset: 0x00257706
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06005E99 RID: 24217 RVA: 0x00259509 File Offset: 0x00257709
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(5m));
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06005E9A RID: 24218 RVA: 0x0025951C File Offset: 0x0025771C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x0025953C File Offset: 0x0025773C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			decimal baseValue = base.DynamicVars.Dexterity.BaseValue;
			await PowerCmd.Apply<SpeedPotionPower>(choiceContext, target, baseValue, base.Owner.Creature, null, false);
		}
	}
}
