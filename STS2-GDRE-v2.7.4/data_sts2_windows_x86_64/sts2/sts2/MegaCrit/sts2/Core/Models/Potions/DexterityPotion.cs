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
	// Token: 0x02000751 RID: 1873
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DexterityPotion : PotionModel
	{
		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06005D84 RID: 23940 RVA: 0x00257D54 File Offset: 0x00255F54
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x06005D85 RID: 23941 RVA: 0x00257D57 File Offset: 0x00255F57
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x06005D86 RID: 23942 RVA: 0x00257D5A File Offset: 0x00255F5A
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06005D87 RID: 23943 RVA: 0x00257D5D File Offset: 0x00255F5D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(2m));
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06005D88 RID: 23944 RVA: 0x00257D70 File Offset: 0x00255F70
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x00257D90 File Offset: 0x00255F90
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<DexterityPower>(choiceContext, target, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
