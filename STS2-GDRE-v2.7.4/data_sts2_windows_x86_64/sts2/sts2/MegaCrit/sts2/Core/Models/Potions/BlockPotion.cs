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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000747 RID: 1863
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlockPotion : PotionModel
	{
		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x06005D47 RID: 23879 RVA: 0x002578DC File Offset: 0x00255ADC
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x06005D48 RID: 23880 RVA: 0x002578DF File Offset: 0x00255ADF
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x06005D49 RID: 23881 RVA: 0x002578E2 File Offset: 0x00255AE2
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x06005D4A RID: 23882 RVA: 0x002578E5 File Offset: 0x00255AE5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(12m, ValueProp.Unpowered));
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x06005D4B RID: 23883 RVA: 0x002578F9 File Offset: 0x00255AF9
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x0025790C File Offset: 0x00255B0C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.GainBlock(target, base.DynamicVars.Block, null, false);
		}
	}
}
