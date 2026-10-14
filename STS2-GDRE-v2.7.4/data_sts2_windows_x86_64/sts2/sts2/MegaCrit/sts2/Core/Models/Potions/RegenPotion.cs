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
	// Token: 0x02000775 RID: 1909
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegenPotion : PotionModel
	{
		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x00259143 File Offset: 0x00257343
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06005E6D RID: 24173 RVA: 0x00259146 File Offset: 0x00257346
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06005E6E RID: 24174 RVA: 0x00259149 File Offset: 0x00257349
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06005E6F RID: 24175 RVA: 0x0025914C File Offset: 0x0025734C
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06005E70 RID: 24176 RVA: 0x0025914F File Offset: 0x0025734F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<RegenPower>(5m));
			}
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x06005E71 RID: 24177 RVA: 0x00259164 File Offset: 0x00257364
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<RegenPower>(null));
			}
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x00259184 File Offset: 0x00257384
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<RegenPower>(choiceContext, target, base.DynamicVars["RegenPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
