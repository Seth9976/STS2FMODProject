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
	// Token: 0x02000769 RID: 1897
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LuckyTonic : PotionModel
	{
		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06005E1F RID: 24095 RVA: 0x00258AD7 File Offset: 0x00256CD7
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06005E20 RID: 24096 RVA: 0x00258ADA File Offset: 0x00256CDA
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06005E21 RID: 24097 RVA: 0x00258ADD File Offset: 0x00256CDD
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06005E22 RID: 24098 RVA: 0x00258AE0 File Offset: 0x00256CE0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<BufferPower>(1m));
			}
		}

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06005E23 RID: 24099 RVA: 0x00258AF4 File Offset: 0x00256CF4
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<BufferPower>(null));
			}
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x00258B14 File Offset: 0x00256D14
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<BufferPower>(choiceContext, target, base.DynamicVars["BufferPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
