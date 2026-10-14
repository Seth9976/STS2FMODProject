using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200075F RID: 1887
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FruitJuice : PotionModel
	{
		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x06005DDE RID: 24030 RVA: 0x00258587 File Offset: 0x00256787
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x06005DDF RID: 24031 RVA: 0x0025858A File Offset: 0x0025678A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.AnyTime;
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x06005DE0 RID: 24032 RVA: 0x0025858D File Offset: 0x0025678D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06005DE1 RID: 24033 RVA: 0x00258590 File Offset: 0x00256790
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06005DE2 RID: 24034 RVA: 0x00258593 File Offset: 0x00256793
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(5m));
			}
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x002585A8 File Offset: 0x002567A8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.GainMaxHp(target, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
