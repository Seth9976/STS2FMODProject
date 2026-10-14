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
	// Token: 0x02000762 RID: 1890
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GhostInAJar : PotionModel
	{
		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06005DF1 RID: 24049 RVA: 0x00258727 File Offset: 0x00256927
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06005DF2 RID: 24050 RVA: 0x0025872A File Offset: 0x0025692A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06005DF3 RID: 24051 RVA: 0x0025872D File Offset: 0x0025692D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06005DF4 RID: 24052 RVA: 0x00258730 File Offset: 0x00256930
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<IntangiblePower>(1m));
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06005DF5 RID: 24053 RVA: 0x00258744 File Offset: 0x00256944
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<IntangiblePower>(null));
			}
		}

		// Token: 0x06005DF6 RID: 24054 RVA: 0x00258764 File Offset: 0x00256964
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<IntangiblePower>(choiceContext, target, base.DynamicVars["IntangiblePower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
