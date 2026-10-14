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
	// Token: 0x0200075B RID: 1883
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlexPotion : PotionModel
	{
		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x06005DC1 RID: 24001 RVA: 0x0025820B File Offset: 0x0025640B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x06005DC2 RID: 24002 RVA: 0x0025820E File Offset: 0x0025640E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x06005DC3 RID: 24003 RVA: 0x00258211 File Offset: 0x00256411
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x00258214 File Offset: 0x00256414
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(5m));
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x06005DC5 RID: 24005 RVA: 0x00258228 File Offset: 0x00256428
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x00258248 File Offset: 0x00256448
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			decimal baseValue = base.DynamicVars.Strength.BaseValue;
			await PowerCmd.Apply<FlexPotionPower>(choiceContext, target, baseValue, base.Owner.Creature, null, false);
		}
	}
}
